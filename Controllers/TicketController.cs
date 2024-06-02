using APITicket.BLL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace APITicket.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TicketController : Controller
    {
        public IConfiguration configuration;
        private Ticket ticket;
        public TicketController(IConfiguration configuration)
        {
            this.configuration = configuration;

            ticket = new(this.configuration);
        }

        [Authorize]
        [HttpGet("listar")]
        public IActionResult Listar()
        {

            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();

            // Leer el token JWT
            var jwtToken = handler.ReadToken(token) as System.IdentityModel.Tokens.Jwt.JwtSecurityToken;

            // Obtener el claim 'sub' (identificador del sujeto)
            var userIdClaim = jwtToken?.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

            if (userIdClaim != null)
            {
                string userName = userIdClaim;

                var respuesta = ticket.Listar(userName);

                return Ok(respuesta);

            }
            return Unauthorized();

        }

        [Authorize]
        [HttpPost("agregar")]
        public IActionResult Agregar([FromBody] JsonElement resultado)
        {
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();

            // Leer el token JWT
            var jwtToken = handler.ReadToken(token) as System.IdentityModel.Tokens.Jwt.JwtSecurityToken;

            // Obtener el claim 'sub' (identificador del sujeto)
            var userIdClaim = jwtToken?.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

            if (userIdClaim != null)
            {
                string userName = userIdClaim;

                var respuesta = ticket.Agregar(resultado, userName);

                return Ok(respuesta);
            }
            return Unauthorized();


        }
        [Authorize]
        [HttpPut("modificar/{id}")]
        public IActionResult Modificar(int id, [FromBody] JsonElement resultado)
        {
            var respuesta = ticket.Modificar(id, resultado);

            return Ok(respuesta);

        }

        [Authorize]
        [HttpPost("agregar-registro")]
        public IActionResult AgregarRegistro([FromBody] JsonElement resultado)
        {
            var respuesta = ticket.AgregarRegistro(resultado);

            return Ok(respuesta);

        }

        [Authorize]
        [HttpGet("registro/{ticketID}")]
        public IActionResult BuscarRegistro(long ticketID)
        {
            var respuesta = ticket.BuscarRegistro(ticketID);

            return Ok(respuesta);

        }
    }
}

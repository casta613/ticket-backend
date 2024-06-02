using APITicket.BLL;
using APITicket.MOD;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;

namespace APITicket.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccesoController : Controller
    {
        private readonly IConfiguration configuration;
        private Acceso Acceso;
        public AccesoController(IConfiguration configuration)
        {
            this.configuration = configuration;

            Acceso = new(this.configuration);
        }

        [HttpPost]
        [Route("acceder")]
        public IActionResult ValidarAcceso([FromBody] ReqAccesoMOD request)
        {

            (var response, int estatus) = Acceso.ValidarAcceso(request);
            return StatusCode(estatus, response);

        }
        [Authorize(AuthenticationSchemes = "Codigo")]
        [HttpGet]
        [Route("validar/{codigo}")]
        public IActionResult ValidarCodigo(string codigo)
        {
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();

            // Leer el token JWT
            var jwtToken = handler.ReadToken(token) as System.IdentityModel.Tokens.Jwt.JwtSecurityToken;

            // Obtener el claim 'sub' (identificador del sujeto)
            var userIdClaim = jwtToken?.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

            if (userIdClaim != null)
            {
                string usuario = userIdClaim;
                (var response ,int estatus)= Acceso.ValidarCodigo(codigo, usuario);
                return StatusCode(estatus, response);
            }
            return Unauthorized();



        }

        [Authorize]
        [HttpGet]
        [Route("validar-permiso")]
        public IActionResult ValidarPermiso()
        {
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();

            // Leer el token JWT
            var jwtToken = handler.ReadToken(token) as System.IdentityModel.Tokens.Jwt.JwtSecurityToken;

            // Obtener el claim 'sub' (identificador del sujeto)
            var userIdClaim = jwtToken?.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

            if (userIdClaim != null)
            {
                string usuario = userIdClaim;
                var response = Acceso.ValidarPermiso(usuario);
                return Ok(response);
            }
            return Unauthorized();



        }
    }
}

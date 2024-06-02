using APITicket.BLL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace APITicket.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AtributosController : Controller
    {
        public IConfiguration configuration;
        private Atributos atributos;
        public AtributosController(IConfiguration configuration)
        {
            this.configuration = configuration;

            atributos = new(this.configuration);
        }
        [Authorize]
        [HttpGet("estados")]
        public IActionResult ListarEstados()
        {

            var respuesta = atributos.ListaEstados();

            return Ok(respuesta);

        }
        [Authorize]
        [HttpGet("prioridad")]
        public IActionResult ListarPrioridad()
        {

            var respuesta = atributos.ListaPrioridad();

            return Ok(respuesta);

        }

    }
}

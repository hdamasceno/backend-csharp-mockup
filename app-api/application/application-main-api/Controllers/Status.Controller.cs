using Biblioteca;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using application_domain.Interfaces;
using application_domain.Objects;

namespace application_main_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        private readonly ILogger<StatusController> _logger;

        public StatusController(ILogger<StatusController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "status")]
        public IResposta GetStatus()
        {
            var objetoResposta = new RespostaAPI();

            var objeto = new
            {
                Id = Guid.NewGuid(),
                DataHoje = DateTime.Today,
                DataHojeBR = DateTime.Today.ToString("dd/MM/yyyy"),
                DataHora = DateTime.Now,
                DataHoraBR = DateTime.Now.ToString("dd/M/yyyy HH:mm:ss"),
                DataHoraUTC = DateTime.Now.ToUniversalTime(),
                AnoMes = DateTime.Today.ToString("yyyy-MM")
            };                        

            objetoResposta.ComandoExecutadoComSucesso(objeto);

            return objetoResposta;
        }
    }
}

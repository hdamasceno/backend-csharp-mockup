using Biblioteca;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using application_domain.Interfaces;
using application_data_models.Models.Account;
using application_service.Services;

namespace application_main_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<StatusController> _logger;
        private readonly AccountService _serviceController;
        public AccountController(ILogger<StatusController> logger, IServiceBase<AccountModel> serviceBase)
        {
            _logger = logger;
            _serviceController = (AccountService)serviceBase;
        }

        [HttpPost]
        [Route("/Autenticar")]
        // api retorna um IActionResult, isso aqui nao existe de IResposta, kill forever!!!!!
        // o nome da rota so entra se nao for os 4 metodos basicos: get (all), post, put, delete
        // o desenho dessa REST esta errado
        // sua api deve tratar o http status, nao espere milagre do .NET e nem do http :D
        // alem disso nao tem asincronismo, ou seja, vai ficar pendurado ate terminar de processar tudo
        // os browsers e clients junto com a thread do pc tratam isso da melhor forma, melhora o processamento e alivia gargalo

        public IResposta Autenticar(string email, string senha)
        {
            return _serviceController.Autenticar(email, senha);
        }

        
        // exemplo correto
        // alem de ter de tratar ProblemDetails, que é uma abordagem de REST
        [HttpGet]
        [Route("{guid:id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var result = _serviceController.GetById(id); // aqui deve ser um metodo async

                return result is null ? NotFound() : Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpGet]
        [Route("/GetFromId")]
        public IResposta GetFromId(Guid id)
        {
            return _serviceController.GetById(id);
        }

        [HttpPost]
        [Route("/GetAll")]
        public IResposta GetAll()
        {
            return _serviceController.GetAll();
        }

        [HttpPost]
        [Route("/Save")]
        public IResposta Save([FromBody] AccountModel objModel)
        {
            return _serviceController.Save(objModel);
        }

        [HttpDelete]
        [Route("/Delete")]
        public IResposta Delete(Guid id)
        {
            return _serviceController.Delete(id);
        }
    }
}

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
        public IResposta Autenticar(string email, string senha)
        {
            return _serviceController.Autenticar(email, senha);
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using application_domain.Interfaces;
using application_domain.Objects;
using application_infra_shared.Contexts;
using Microsoft.Extensions.Configuration;
using Biblioteca;
using application_data_models.Models.Account;
using application_infra_data.Repository;
using application_infra_shared;

namespace application_service.Services
{
    public class AccountService : IServiceBase<AccountModel>
    {
        private readonly AccountRepository _repository;
        private readonly NotificationContext _notificationContext;

        public AccountService(IConfiguration configuration)
        {
            AccountRepository repository = new AccountRepository(configuration);

            _repository = repository;
            _notificationContext = new NotificationContext();
        }

        public AccountService(AccountRepository repository, NotificationContext notificationContext)
        {
            _repository = repository;
            _notificationContext = notificationContext;
        }

        public IResposta Delete(Guid id)
        {
            var objetoResposta = new RespostaAPI();

            try
            {
                var objetoEntity = _repository.GetById(id);

                if (objetoEntity == null)
                    return objetoResposta.ComandoExecutadoComErro($"{nameof(objetoEntity)} não localizado na base de dados do aplicativo.");

                if (objetoEntity?.IsValid == false)
                {
                    return objetoResposta.ComandoExecutadoComErro
                        (
                            $"{nameof(objetoEntity)} is invalid." + Environment.NewLine + objetoEntity?.GetValidationErrors()
                        );
                }

                return _repository.Delete(id);
            }
            catch (Exception ex)
            {
                objetoResposta.ComandoExecutadoComErro(ex);
            }

            return objetoResposta;
        }

        public IResposta GetAll()
        {
            var objetoResposta = new RespostaAPI();

            try
            {
                var objetoTeste = _repository.GetAll();
            }
            catch (Exception ex)
            {
                objetoResposta.ComandoExecutadoComErro(ex);
            }

            return objetoResposta;
        }

        public IResposta GetById(Guid id)
        {
            var objetoResposta = new RespostaAPI();

            try
            {
                var objetoEntity = _repository.GetById(id);

                if (objetoEntity?.IsValid == false)
                {
                    return objetoResposta.ComandoExecutadoComErro
                        (
                            $"{nameof(objetoEntity)} is invalid." + Environment.NewLine + objetoEntity?.GetValidationErrors()
                        );
                }

                if (objetoEntity == null)
                    return objetoResposta.ComandoExecutadoComErro($"{nameof(objetoEntity)} não localizado na base de dados do aplicativo.");
                else
                {
                    var objetoR = new
                    {
                        record = AccountMapper.ConvertToModel(objetoEntity)
                    };

                    objetoResposta.ComandoExecutadoComSucesso(objetoR);
                }
            }
            catch (Exception ex)
            {
                objetoResposta.ComandoExecutadoComErro(ex);
            }

            return objetoResposta;
        }

        public IResposta Save(AccountModel obj)
        {
            var objetoResposta = new RespostaAPI();

            try
            {
                var objetoEntity = _repository.GetById(FuncoesEspeciais.ToGuid(obj.Id));

                if (objetoEntity?.IsValid != true)
                    objetoEntity = AccountMapper.ConvertToEntity(obj);
                else
                    objetoEntity.Load(obj);

                if (objetoEntity?.IsValid == false)
                {
                    return objetoResposta.ComandoExecutadoComErro
                        (
                            $"{nameof(objetoEntity)} is invalid." + Environment.NewLine + objetoEntity?.GetValidationErrors()
                        );
                }

                if (objetoEntity == null)
                    return objetoResposta.ComandoExecutadoComErro($"{nameof(objetoEntity)} não localizado na base de dados do aplicativo.");
                else
                {
                    var objAccountTemp = _repository.GetByEmail(obj.Email);

                    if (objAccountTemp?.IsValid == true)
                    {
                        if (objAccountTemp.Id.ToString() != objetoEntity.Id.ToString())
                            return objetoResposta.ComandoExecutadoComErro($"Já existe uma conta criada para o e-mail informado.");
                    }

                    return _repository.Save(objetoEntity);
                }
            }
            catch (Exception ex)
            {
                objetoResposta.ComandoExecutadoComErro(ex);
            }

            return objetoResposta;
        }

        public IResposta Autenticar(string email, string senha)
        {
            var objetoResposta = new RespostaAPI();

            try
            {

            }
            catch (Exception ex)
            {
                objetoResposta.ComandoExecutadoComErro(ex);
            }

            return objetoResposta;
        }

        public IResposta AutenticarWithApple(string email)
        {
            var objetoResposta = new RespostaAPI();

            try
            {

            }
            catch (Exception ex)
            {
                objetoResposta.ComandoExecutadoComErro(ex);
            }

            return objetoResposta;
        }

        public IResposta AutenticarWithGoogle(string email)
        {
            var objetoResposta = new RespostaAPI();

            try
            {

            }
            catch (Exception ex)
            {
                objetoResposta.ComandoExecutadoComErro(ex);
            }

            return objetoResposta;
        }
    }
}

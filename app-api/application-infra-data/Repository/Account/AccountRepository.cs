using Microsoft.Extensions.Configuration;
using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using application_domain.Interfaces;
using application_domain.Objects;
using application_domain.Types.Values;
using application_data_entities;
using application_infra_shared;
using application_data_models.Models.Account;

namespace application_infra_data.Repository
{
    public class AccountRepository : BaseRepositoryRaven<Account, Key>, IRepository<Account>
    {
        public IQueryOption<Account> QueryOptions { get; set; } = new RavenQueryOptions<Account>() { TakeNumber = 10, SkipNumber = 0 };

        public AccountRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public AccountRepository(
                string dataBaseEndPointUrl,
                string dataBaseName,
                string certificateThumbPrint
            ) :
            base(
                dataBaseEndPoint: dataBaseEndPointUrl,
                dataBaseName: dataBaseName,
                certificateThumbPrint: certificateThumbPrint
            )
        {
            QueryOptions = new RavenQueryOptions<Account>() { TakeNumber = 10, SkipNumber = 0 };
        }

        public IResposta Save(Account entity)
        {
            var objetoResposta = new RespostaAPI();

            try
            {
                var objModel = AccountMapper.ConvertToModel(entity);

                using (IDocumentSession Session = DocumentStore.OpenSession())
                {
                    Session.Store(objModel);
                    Session.SaveChanges();
                }

                objetoResposta.ComandoExecutadoComSucesso();
            }
            catch (Exception ex)
            {
                objetoResposta.ComandoExecutadoComErro(ex);
            }

            return objetoResposta;
        }

        public IResposta Delete(Guid id)
        {
            return DeleteDocument(id);
        }

        public List<Account> GetAll()
        {
            using (IDocumentSession Session = DocumentStore.OpenSession())
            {
                var objModelList = Session
                    .Query<AccountModel>()
                    .OrderByDescending(p => p.CadastradoDataHora)
                    .Skip(QueryOptions.SkipNumber)
                    .Take(QueryOptions.TakeNumber)
                    .ToList();

                return objModelList.ConvertToEntityList();
            }
        }
        public Account GetById(Guid id)
        {
            using (IDocumentSession Session = DocumentStore.OpenSession())
            {
                var objModel = Session
                    .Query<AccountModel>()
                    .FirstOrDefault(p => p.Id.Equals(id)); // aqui é .FirstOrDefault(p => p.Id == id) pois esta comparando valores e nao objetos

                if (objModel == null)
                    return null;
                else // n precisa de else ou use o operador ternario
                    return objModel.ConvertToEntity();

                //return objModel is null ? null : objModel.ConvertToEntity();

                // ou melhor: return objModel?.ConvertToEntity();
            }
        }

        public Account GetByEmail(string email)
        {
            using (IDocumentSession Session = DocumentStore.OpenSession())
            {
                var objModel = Session
                    .Query<AccountModel>()
                    .FirstOrDefault(p => p.Email.Equals(email));

                if (objModel == null)
                    return null;
                else
                    return objModel.ConvertToEntity();
            }
        }

        public Account GetByEmailAndSenha(string email, string senha)
        {
            using (IDocumentSession Session = DocumentStore.OpenSession())
            {
                var objModel = Session
                    .Query<AccountModel>()
                    .FirstOrDefault(p => p.Email == email);

                if (objModel == null)
                    return null;
                else
                {
                    var objEntity = objModel.ConvertToEntity();

                    if (objEntity.IsValid)
                    {
                        if (objEntity.Senha.Equals(senha) == false)
                            objEntity.AddNotification("Senha", "Senha Incorreta");
                    }

                    return objEntity;
                }
            }
        }
    }
}

using Biblioteca;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using application_data_entities;
using application_data_models.Models.Account;

namespace application_infra_shared // kill
{

    // quem faz o mapeamento n é a infra e sim a camada que recebe o dado do repositorio
    // remember: repositorio nao retorna objeto mapeado, traz o dado como esta do banco
    // cabe a um "serviço" fazer esse trabalho de conversao para o tipo xpto
    // neste caso seria o application_service.Services o responsavel
    public static class AccountMapper
    {
        public static Account ConvertToEntity
            (this AccountModel accountModel) => new Account(accountModel.ToDynamic());

        public static List<Account> ConvertToEntityList
            (this List<AccountModel> accountModelList) =>
                new List<Account>
                    (
                        accountModelList.Select(item => new Account(item.ToDynamic()))
                    );

        public static AccountModel ConvertToModel
            (this Account accountEntity) => new AccountModel(accountEntity.ToJSON<Account>());
    }
}

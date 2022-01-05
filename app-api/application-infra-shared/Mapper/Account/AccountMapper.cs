using Biblioteca;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using application_data_entities;
using application_data_models.Models.Account;

namespace application_infra_shared
{
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

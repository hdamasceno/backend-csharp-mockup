using System;
using System.ComponentModel.DataAnnotations.Schema;
using Biblioteca;
using application_data_models.Models._Base;

namespace application_data_models.Models.Account
{
    [Table("Account")]
    public class AccountModel : BaseModel
    {
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public bool Cancelado { get; set; }
        public DateTime? CanceladoDataHora { get; set; }
        public bool Bloqueado { get; set; }
        public DateTime? BloqueadoDataHora { get; set; }

        public AccountModel() : base()
        {
        }

        public AccountModel(string json) : base()
        {
            LoadFromJSON<AccountModel>(this, json);
        }
    }
}
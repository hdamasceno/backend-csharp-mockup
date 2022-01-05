using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using application_data_models.Models._Base;

namespace application_data_models.Models.Account
{
    [Table("AccountAssinaturaAtivacao")]
    public class AccountAssinaturaAtivacaoModel : BaseModel
    {
        public Guid AccountId { get; set; }
        public Guid AccountAssinaturaId { get; set; }
        public DateTime AtivacaoDataHora { get; set; }
        public Guid AtivacaoDeviceId { get; set; }
        public string AtivacaoIp { get; set; } = string.Empty;
        public bool IsAtivacaoFinanceira { get; set; }
        public DateTime ValidadeData { get; set; }
        public DateTime ValidadeCarenciaData { get; set; }

        public AccountAssinaturaAtivacaoModel() : base()
        {
        }

        public AccountAssinaturaAtivacaoModel(string json) : base()
        {
            LoadFromJSON<AccountAssinaturaAtivacaoModel>(this, json);
        }
    }
}

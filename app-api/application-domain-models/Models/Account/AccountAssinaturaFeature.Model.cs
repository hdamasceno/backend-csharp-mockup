using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using application_data_models.Models._Base;

namespace application_data_models.Models.Account
{
    [Table("AccountAssinaturaFeature")]
    public class AccountAssinaturaFeature : BaseModel
    {
        public Guid AccountId { get; set; }
        public Guid AccountAssinaturaId { get; set; }
        public Guid FeatureId { get; set; }
        public DateTime ValidadeInicialDataHora { get; set; }
        public DateTime? ValidadeFinalDataHora { get; set; }
        public bool Cancelado { get; set; }
        public DateTime? CanceladoDataHora { get; set; }

        public AccountAssinaturaFeature() : base()
        {
        }

        public AccountAssinaturaFeature(string json) : base()
        {
            LoadFromJSON<AccountAssinaturaFeature>(this, json);
        }
    }
}

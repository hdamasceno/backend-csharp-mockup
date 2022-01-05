using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Biblioteca;
using application_data_models.Models._Base;

namespace application_data_models.Models.PessoaJuridica
{
    [Table("RegimeTributario")]
    public class RegimeTributarioModel : BaseModel
    {
        public string Nome { get; set; } = string.Empty;
        public decimal NFeCodigo { get; set; } = 0;
        public decimal NFCeCodigo { get; set; } = 0;
        public decimal SatCodigo { get; set; } = 0;
        public decimal SpedFiscalCodigo { get; set; } = 0;
        public decimal SpedContribuicaoCodigo { get; set; } = 0;
        public decimal SintegraCodigo { get; set; } = 0;

        public RegimeTributarioModel() : base() { }

        public RegimeTributarioModel(string json) : base()
        {
            LoadFromJSON<RegimeTributarioModel>(this, json);
        }
    }
}

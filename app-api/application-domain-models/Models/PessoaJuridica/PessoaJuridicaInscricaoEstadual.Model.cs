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
    [Table("PessoaJuridicaInscricaoEstadual")]
    public class PessoaJuridicaInscricaoEstadualModel : BaseModel
    {
        public Guid AccountId { get; set; }
        public Guid PessoaJuridicaId { get; set; }
        public string DocumentoInscricaoEstadual { get; set; } = string.Empty;
        public Guid EstadoId { get; set; }

        public PessoaJuridicaInscricaoEstadualModel() : base() { }

        public PessoaJuridicaInscricaoEstadualModel(string json) : base()
        {
            LoadFromJSON<PessoaJuridicaInscricaoEstadualModel>(this, json);
        }
    }
}

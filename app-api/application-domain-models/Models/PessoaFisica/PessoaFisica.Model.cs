using System.ComponentModel.DataAnnotations.Schema;
using Biblioteca;
using application_data_models.Models._Base;

namespace application_data_models.Models.PessoaFisica
{
    [Table("PessoaFisica")]
    public class PessoaFisicaModel : BaseModel
    {
        public Guid AccountId { get; set; }
        public string DocumentoCPF { get; set; } = string.Empty;
        public string DocumentoIdentidade { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public DateTime? NascimentoData { get; set; }
        public Guid? EstadoCivilId { get; set; }
        public Guid? SexoId { get; set; }

        public PessoaFisicaModel() : base()
        {
        }

        public PessoaFisicaModel(string json)
        {
            LoadFromJSON<PessoaFisicaModel>(this, json);
        }
    }
}
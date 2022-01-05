using System.ComponentModel.DataAnnotations.Schema;
using Biblioteca;
using application_data_models.Models._Base;

namespace application_data_models.Models.PessoaJuridica
{
    [Table("PessoaJuridica")]
    public class PessoaJuridicaModel : BaseModel
    {
        public Guid AccountId { get; set; }
        public Guid? RegimeTributarioId { get; set; }
        public string DocumentoCNPJ { get; set; } = string.Empty;
        public string DocumentoInscricaoMunicipal { get; set; } = string.Empty;
        public string CNAE { get; set; } = string.Empty;
        public string RazaoSocial { get; set; } = string.Empty;
        public string NomeFantasia { get; set; } = string.Empty;
        public DateTime? AberturaData { get; set; } = null;

        public PessoaJuridicaModel() : base()
        { }

        public PessoaJuridicaModel(string json)
        {
            LoadFromJSON<PessoaJuridicaModel>(this, json);
        }
    }
}
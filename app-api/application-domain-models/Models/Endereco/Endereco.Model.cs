using System.ComponentModel.DataAnnotations.Schema;
using Biblioteca;
using application_data_models.Models._Base;

namespace application_data_models.Models.Endereco
{
    [Table("Endereco")]
    public class EnderecoModel : BaseModel
    {
        public Guid AccountId { get; set; }
        public Guid EstadoId { get; set; }
        public Guid CidadeId { get; set; }
        public string Logradouro { get; set; } = string.Empty;
        public string Numero { get; set; } = string.Empty;
        public string Complemento { get; set; } = string.Empty;
        public string Bairro { get; set; } = string.Empty;
        public string CEP { get; set; } = string.Empty;

        public EnderecoModel() : base() { }

        public EnderecoModel(string json)
        {
            LoadFromJSON<EnderecoModel>(this, json);
        }
    }
}
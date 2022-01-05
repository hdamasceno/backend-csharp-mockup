using System.ComponentModel.DataAnnotations.Schema;
using Biblioteca;
using application_data_models.Models._Base;

namespace application_data_models.Models.Endereco
{
    [Table("Cidade")]
    public class CidadeModel : BaseModel
    {
        public Guid EstadoId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public decimal IbgeCodigo { get; set; }

        public CidadeModel() : base() { }

        public CidadeModel(string json)
        {
            LoadFromJSON<CidadeModel>(this, json);
        }
    }
}
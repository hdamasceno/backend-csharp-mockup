using System.ComponentModel.DataAnnotations.Schema;
using Biblioteca;
using application_data_models.Models._Base;

namespace application_data_models.Models.Endereco
{
    [Table("Estado")]
    public class EstadoModel : BaseModel
    {
        public string Nome { get; set; } = string.Empty;
        public string UF { get; set; } = string.Empty;

        public EstadoModel() : base() { }

        public EstadoModel(string json)
        {
            LoadFromJSON<EstadoModel>(this, json);
        }
    }
}
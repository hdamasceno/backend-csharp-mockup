using System.ComponentModel.DataAnnotations.Schema;
using Biblioteca;
using application_data_models.Models._Base;

namespace application_data_models.Models.PessoaFisica
{
    [Table("EstadoCivil")]
    public class EstadoCivilModel : BaseModel
    {
        public string Nome { get; set; } = string.Empty;

        public EstadoCivilModel() : base() { }

        public EstadoCivilModel(string json) : base()
        {
            LoadFromJSON<EstadoCivilModel>(this, json);
        }
    }
}
using System.ComponentModel.DataAnnotations.Schema;
using Biblioteca;
using application_data_models.Models._Base;

namespace application_data_models.Models.PessoaFisica
{
    [Table("Sexo")]
    public class SexoModel : BaseModel
    {
        public string Nome { get; set; } = string.Empty;
        public string Sigla { get; set; } = string.Empty;

        public SexoModel() : base() { }

        public SexoModel(string json)
        {
            LoadFromJSON<SexoModel>(this, json);
        }
    }
}
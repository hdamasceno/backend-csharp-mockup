using System.ComponentModel.DataAnnotations.Schema;
using Biblioteca;
using application_data_models.Models._Base;

namespace application_data_models.Models.PessoaFisica
{
    [Table("PessoaFisicaEndereco")]
    public class PessoaFisicaEnderecoModel : BaseModel
    {
        public Guid AccountId { get; set; }
        public Guid PessoaFisicaId { get; set; }
        public Guid EnderecoId { get; set; }
        public bool IsPrincipal { get; set; }

        public PessoaFisicaEnderecoModel() : base() { }

        public PessoaFisicaEnderecoModel(string json)
        {
            LoadFromJSON<PessoaFisicaEnderecoModel>(this, json);
        }
    }
}
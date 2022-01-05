using System.ComponentModel.DataAnnotations.Schema;
using Biblioteca;
using application_data_models.Models._Base;

namespace application_data_models.Models.PessoaJuridica
{
    [Table("PessoaJuridicaEndereco")]
    public class PessoaJuridicaEnderecoModel : BaseModel
    {
        public Guid AccountId { get; set; }
        public Guid PessoaJuridicaId { get; set; }
        public Guid EnderecoId { get; set; }
        public bool IsPrincipal { get; set; }

        public PessoaJuridicaEnderecoModel() : base() { }

        public PessoaJuridicaEnderecoModel(string json)
        {
            LoadFromJSON<PessoaJuridicaEnderecoModel>(this, json);
        }
    }
}
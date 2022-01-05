using System;
using System.ComponentModel.DataAnnotations.Schema;
using Biblioteca;
using application_data_models.Models._Base;

namespace application_data_models.Models.Account
{
    [Table("AccountAssinatura")]
    public class AccountAssinaturaModel : BaseModel
    {
        public Guid AccountId { get; set; }
        public Guid ProdutoId { get; set; }
        public DateTime AssinaturaDataHora { get; set; }
        public DateTime ValidadeInicialDatHora { get; set; }
        public DateTime ValidadeFinalDataHora { get; set; }
        public bool RenovacaoAutomatica { get; set; }
        public bool Bloqueado { get; set; }
        public DateTime? BloqueadoDataHora { get; set; }
        public string? BloqueadoMotivo { get; set; }
        public bool Cancelado { get; set; }
        public DateTime? CanceladoDataHora { get; set; }
        public string? CanceladoMotivo { get; set; }
        public Guid? PessoaFisicaId { get; set; }
        public Guid? PessoaJuridicaId { get; set; }
        public Guid? EnderecoId { get; set; }

        public AccountAssinaturaModel() : base()
        {
        }

        public AccountAssinaturaModel(string json) : base()
        {
            LoadFromJSON<AccountAssinaturaModel>(this, json);
        }
    }
}
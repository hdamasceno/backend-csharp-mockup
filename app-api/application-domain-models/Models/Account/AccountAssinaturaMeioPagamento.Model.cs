using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using application_data_models.Models._Base;

namespace application_data_models.Models.Account
{
    [Table("AccountAssinaturaMeioPagamento")]
    public class AccountAssinaturaMeioPagamento : BaseModel
    {
        public Guid AccountId { get; set; }
        public Guid AccountAssinaturaId { get; set; }
        public Guid? PessoaFisicaId { get; set; }
        public Guid? PessoaJuridicaId { get; set; }
        public Guid? EnderecoId { get; set; }
        public string? CartaoToken { get; set; } = string.Empty;
        public bool Cancelado { get; set; }
        public DateTime? CanceladoDataHora { get; set; }
        public string? CanceladoMotivo { get; set; }
        public bool Bloqueado { get; set; }
        public DateTime? BloqueadoDataHora { get; set; }
        public string? BloqueadoMotivo { get; set; }

        public AccountAssinaturaMeioPagamento() : base()
        {
        }

        public AccountAssinaturaMeioPagamento(string json) : base()
        {
            LoadFromJSON<AccountAssinaturaMeioPagamento>(this, json);
        }
    }
}

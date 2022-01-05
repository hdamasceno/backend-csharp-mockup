using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using application_data_models.Models._Base;

namespace application_data_models.Models.Account
{
    [Table("AccountAssinaturaMeioPagamentoItem")]
    public class AccountAssinaturaMeioPagamentoItem : BaseModel
    {
        public Guid AccountId { get; set; }
        public Guid AccountAssinaturaId { get; set; }
        public Guid AccountAssinaturaMeioPagamentoId { get; set; }
        public DateTime PagamentoDataHora { get; set; }
        public decimal PagamentoValor { get; set; }
        public string? CartaoToken { get; set; }
        public string TransactionId { get; set; } = string.Empty;
        public string TransactionNSUNumber { get; set; } = string.Empty;
        public string TransactionGatewayId { get; set; } = string.Empty;

        public AccountAssinaturaMeioPagamentoItem() : base()
        {
        }

        public AccountAssinaturaMeioPagamentoItem(string json) : base()
        {
            LoadFromJSON<AccountAssinaturaMeioPagamentoItem>(this, json);
        }
    }
}

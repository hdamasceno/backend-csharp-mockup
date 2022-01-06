using Biblioteca;
using application_domain.Abstracts;
using application_domain.Interfaces;
using application_domain.Types.Values;

namespace application_data_entities
{
    public class AccountAssinaturaMeioPagamentoItem : BaseEntity<Key>, IEntidadeBase
    {
        public DataHora CadastradoDataHora { get; private set; }
        public DataHora? AlteradoDataHora { get; private set; }
        public Key AccountId { get; private set; }
        public virtual IEntidade? Account { get; private set; }
        public Key AccountFinanceiroId { get; private set; }
        public virtual IEntidadeBase? AccountFinanceiro { get; private set; }
        public DataHora PagamentoDataHora { get; private set; }
        public DecimalPositive PagamentoValor { get; private set; }
        public string CartaoToken { get; private set; } = string.Empty;
        public string TransactionId { get; private set; } = string.Empty;
        public string TransactionNSUNumber { get; private set; } = string.Empty;
        public Key TransactionGatewayId { get; private set; }

        public AccountAssinaturaMeioPagamentoItem(dynamic objetoDynamic) : base(
                id: (Key)FuncoesEspeciais.ToGuid(objetoDynamic?.Id != null ? objetoDynamic?.Id : Guid.NewGuid())
            )
        {
            Load(objetoDynamic);
        }

        public void Load(dynamic objetoDynamic)
        {
            if (objetoDynamic == null)
            {
                AddNotification($"{GetType().Name}.Load", $"{GetType().Name} - JSON inválido.");

                return;
            }

            LoadFromDynamic<AccountAssinaturaMeioPagamentoItem>(this, objetoDynamic);

            if (IsValid)
            {
                AddNotifications(
                    AccountId.contract,
                    CadastradoDataHora.contract,
                    AccountFinanceiroId.contract,
                    PagamentoDataHora.contract,
                    PagamentoValor.contract,
                    TransactionGatewayId.contract);

                if (AlteradoDataHora.HasValue)
                    AddNotifications(AlteradoDataHora?.contract);
            }
        }
    }
}
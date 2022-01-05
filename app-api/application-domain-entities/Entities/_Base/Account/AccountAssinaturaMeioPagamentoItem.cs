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
                AddNotification($"{GetType().Name}.LoadFromDynamic", $"{GetType().Name} - JSON inválido.");

                return;
            }

            DataHora cadastradoDataHora = FuncoesEspeciais.ToDateTime(objetoDynamic?.CadastradoDataHora, false, false, true);
            DataHora? alteradoDataHora = FuncoesEspeciais.ToDateTimeNull(objetoDynamic?.AlteradoDataHora, false, false, true);
            Key accountId = FuncoesEspeciais.ToGuid(objetoDynamic?.AccountId);
            Key accountFinanceiroId = FuncoesEspeciais.ToGuid(objetoDynamic?.AccountFinanceiroId);

            DataHora pagamentoDataHora = FuncoesEspeciais.ToDateTime(objetoDynamic?.PagamentoDataHora, false, false, true);
            DecimalPositive pagamentoValor = FuncoesEspeciais.ToDecimal(objetoDynamic?.PagamentoValor, true);
            string cartaoToken = FuncoesEspeciais.ToString(objetoDynamic?.CartaoToken, false, false, false);
            string transactionId = FuncoesEspeciais.ToString(objetoDynamic?.TransactionId, false, false, false);
            string transactionNSUNumber = FuncoesEspeciais.ToString(objetoDynamic?.TransactionNSUNNumber, false, false, false);
            Key transactionGatewayId = FuncoesEspeciais.ToGuid(objetoDynamic?.TransactionGatewayId);

            AddNotifications(
                accountId.contract,
                cadastradoDataHora.contract,
                accountFinanceiroId.contract,
                pagamentoDataHora.contract,
                pagamentoValor.contract,
                transactionGatewayId.contract);

            if (alteradoDataHora.HasValue)
                AddNotifications(alteradoDataHora?.contract);

            if (IsValid)
            {
                CadastradoDataHora = cadastradoDataHora;
                AlteradoDataHora = alteradoDataHora;
                AccountId = accountId;
                AccountFinanceiroId = accountFinanceiroId;
                CartaoToken = cartaoToken;
                PagamentoDataHora = pagamentoDataHora;
                PagamentoValor = pagamentoValor;
                TransactionId = transactionId;
                TransactionNSUNumber = transactionNSUNumber;
                TransactionGatewayId = transactionGatewayId;
            }
        }
    }
}
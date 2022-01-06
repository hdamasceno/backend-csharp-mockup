using Biblioteca;
using application_domain.Abstracts;
using application_domain.Interfaces;
using application_domain.Types.Values;

namespace application_data_entities
{
    public class AccountAssinaturaAtivacao : BaseEntity<Key>, IEntidadeBase
    {
        public DataHora CadastradoDataHora { get; private set; }
        public DataHora? AlteradoDataHora { get; private set; }
        public Key AccountId { get; private set; }
        public virtual IEntidade? Account { get; private set; }
        public Key AccountAssinaturaId { get; private set; }
        public virtual IEntidadeBase? AccountAssinatura { get; private set; }
        public DataHora AtivacaoDataHora { get; private set; }
        public Key AtivacaoDeviceId { get; private set; }
        public virtual IEntidadeBase? AtivacaoDevice { get; private set; }
        public string AtivacaoIp { get; private set; } = string.Empty;
        public bool IsAtivacaoFinanceira { get; private set; }
        public Data ValidadeData { get; private set; }
        public Data ValidadeCarenciaData { get; private set; }

        public AccountAssinaturaAtivacao(dynamic objetoDynamic) :
            base(id: (Key)FuncoesEspeciais.ToGuid(objetoDynamic?.Id != null ? objetoDynamic?.Id : Guid.NewGuid()))
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

            LoadFromDynamic<AccountAssinaturaAtivacao>(this, objetoDynamic);            

            if (IsValid)
            {
                AddNotifications(
                    AccountId.contract,
                    AccountAssinaturaId.contract,
                    AtivacaoDataHora.contract,
                    AtivacaoDeviceId.contract,
                    ValidadeData.contract,
                    ValidadeCarenciaData.contract
                );
            }
        }
    }
}
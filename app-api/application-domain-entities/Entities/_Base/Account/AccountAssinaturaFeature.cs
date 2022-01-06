using Biblioteca;
using application_domain.Abstracts;
using application_domain.Interfaces;
using application_domain.Types.Values;

namespace application_data_entities
{
    public class AccountAssinaturaFeature : BaseEntity<Key>, IEntidadeBase
    {
        public DataHora CadastradoDataHora { get; private set; }
        public DataHora? AlteradoDataHora { get; private set; }
        public Key AccountId { get; private set; }
        public virtual IEntidade? Account { get; private set; }
        public Key AccountAssinaturaId { get; private set; }
        public virtual IEntidadeBase? AccountAssinatura { get; private set; }
        public Key FeatureId { get; private set; }
        public DataHora ValidadeInicialDataHora { get; private set; }
        public DataHora? ValidadeFinalDataHora { get; private set; }
        public bool Cancelado { get; private set; }
        public DataHora? CanceladoDataHora { get; private set; }

        public AccountAssinaturaFeature(dynamic objetoDynamic) :
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

            LoadFromDynamic<AccountAssinaturaFeature>(this, objetoDynamic);

            if (IsValid)
            {
                AddNotifications(
                    CadastradoDataHora.contract,
                    AccountId.contract,
                    AccountAssinaturaId.contract,
                    FeatureId.contract,
                    ValidadeInicialDataHora.contract                    
                );

                if (CanceladoDataHora.HasValue)
                    AddNotifications(CanceladoDataHora?.contract);

                if (ValidadeFinalDataHora.HasValue)
                    AddNotifications(ValidadeFinalDataHora?.contract);
            }
        }
    }
}
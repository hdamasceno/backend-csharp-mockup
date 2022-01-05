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
                AddNotification($"{GetType().Name}.LoadFromDynamic", $"{GetType().Name} - JSON inválido.");

                return;
            }

            DataHora cadastradoDataHora = FuncoesEspeciais.ToDateTime(objetoDynamic.CadastradoDataHora, false, false, true);
            DataHora? alteradoDataHora = FuncoesEspeciais.ToDateTimeNull(objetoDynamic.AlteradoDataHora, false, false, true);
            Key accountId = FuncoesEspeciais.ToGuid(objetoDynamic.AccountId);
            Key accountAssinaturaId = FuncoesEspeciais.ToGuid(objetoDynamic.AccountAssinaturaId);
            Key featureId = FuncoesEspeciais.ToGuid(objetoDynamic.FeatureId);
            DataHora validadeInicialDataHora = FuncoesEspeciais.ToDateTime(objetoDynamic.ValidadeInicialDataHora, false, false, true);
            DataHora validadeFinalDataHora = FuncoesEspeciais.ToDateTime(objetoDynamic.ValidadeFinalDataHora, false, false, true);
            bool cancelado = FuncoesEspeciais.ToString(objetoDynamic.Cancelado) == "TRUE";
            DataHora? canceladoDataHora = FuncoesEspeciais.ToDateTimeNull(objetoDynamic.CanceladoDataHora, false, false, true);

            AddNotifications(
                cadastradoDataHora.contract,
                accountId.contract,
                accountAssinaturaId.contract,
                featureId.contract,
                validadeInicialDataHora.contract,
                validadeFinalDataHora.contract
            );

            if (canceladoDataHora.HasValue)
                AddNotifications(canceladoDataHora?.contract);

            if (IsValid)
            {
                CadastradoDataHora = cadastradoDataHora;
                AlteradoDataHora = alteradoDataHora;
                AccountId = accountId;
                AccountAssinaturaId = accountAssinaturaId;
                FeatureId = featureId;
                ValidadeInicialDataHora = validadeInicialDataHora;
                ValidadeFinalDataHora = validadeFinalDataHora;
                Cancelado = cancelado;
                CanceladoDataHora = canceladoDataHora;
            }
        }
    }
}
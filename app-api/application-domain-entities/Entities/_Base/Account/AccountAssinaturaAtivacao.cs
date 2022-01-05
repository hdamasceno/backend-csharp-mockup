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
                AddNotification($"{GetType().Name}.LoadFromDynamic", $"{GetType().Name} - JSON inválido.");

                return;
            }

            DataHora cadastradoDataHora = FuncoesEspeciais.ToDateTime(objetoDynamic.CadastradoDataHora, false, false, true);
            DataHora? alteradoDataHora = FuncoesEspeciais.ToDateTimeNull(objetoDynamic.AlteradoDataHora, false, false, true);
            Key accountId = FuncoesEspeciais.ToGuid(objetoDynamic.AccountId);
            Key accountAssinaturaId = FuncoesEspeciais.ToGuid(objetoDynamic?.AccountAssinaturaId);
            DataHora ativacaoDataHora = FuncoesEspeciais.ToDateTime(objetoDynamic?.AtivacaoDataHora, false, false, true);
            Key ativacaoDeviceId = FuncoesEspeciais.ToGuid(objetoDynamic?.AtivacaoDeviceId);
            string ativacaoIp = FuncoesEspeciais.ToString(objetoDynamic?.AtivacaoIp);
            bool isAtivacaoFinanceira = FuncoesEspeciais.ToString(objetoDynamic?.IsAtivacaoFinanceira) == "TRUE";
            Data validadeData = FuncoesEspeciais.ToDateTime(objetoDynamic?.ValidadeData, true, false, true);
            Data validadeCarenciaData = FuncoesEspeciais.ToDateTime(objetoDynamic?.ValidadeCarenciaData, true, false, true);

            AddNotifications(
                accountId.contract,
                accountAssinaturaId.contract,
                ativacaoDataHora.contract,
                ativacaoDeviceId.contract,
                ValidadeData.contract,
                validadeCarenciaData.contract
            );

            if (IsValid)
            {
                CadastradoDataHora = cadastradoDataHora;
                AlteradoDataHora = alteradoDataHora;
                AccountId = accountId;
                AccountAssinaturaId = accountAssinaturaId;
                AtivacaoDataHora = ativacaoDataHora;
                AtivacaoDeviceId = ativacaoDeviceId;
                AtivacaoIp = ativacaoIp;
                IsAtivacaoFinanceira = isAtivacaoFinanceira;
                ValidadeData = validadeData;
                ValidadeCarenciaData = validadeCarenciaData;
            }
        }
    }
}
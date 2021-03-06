using Biblioteca;
using System.Reflection;
using application_domain.Abstracts;
using application_domain.Interfaces;
using application_domain.Types.Values;

namespace application_data_entities
{
    public class AccountAssinatura : BaseEntity<Key>, IEntidadeBase
    {
        public DataHora CadastradoDataHora { get; private set; }
        public DataHora? AlteradoDataHora { get; private set; }
        public Key AccountId { get; private set; }
        public virtual IEntidade? Account { get; private set; }
        public Key ProdutoId { get; private set; }
        public DataHora AssinaturaDataHora { get; private set; }
        public DataHora ValidadeInicialDataHora { get; private set; }
        public DataHora ValidadeFinalDataHora { get; private set; }
        public bool RenovacaoAutomatica { get; private set; }
        public bool Bloqueado { get; private set; }
        public DataHora? BloqueadoDataHora { get; private set; }
        public string BloqueadoMotivo { get; private set; } = string.Empty;
        public bool Cancelado { get; private set; }
        public DataHora? CanceladoDataHora { get; private set; }
        public string? CanceladoMotivo { get; private set; }
        public virtual List<IEntidadeBase> MeiosDePagamentos { get; private set; } = new List<IEntidadeBase>();
        public virtual List<IEntidadeBase> Features { get; private set; } = new List<IEntidadeBase>();
        public virtual List<IEntidadeBase> Ativacoes { get; private set; } = new List<IEntidadeBase>();

        public AccountAssinatura(dynamic objetoDynamic) :
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

            LoadFromDynamic<AccountAssinatura>(this, objetoDynamic);            

            if (IsValid)
            {
                AddNotifications(
                    CadastradoDataHora.contract,
                    AccountId.contract,
                    ProdutoId.contract,
                    AssinaturaDataHora.contract,
                    ValidadeInicialDataHora.contract,
                    ValidadeFinalDataHora.contract
                );

                if (CanceladoDataHora.HasValue)
                    AddNotifications(CanceladoDataHora?.contract);

                if (BloqueadoDataHora.HasValue)
                    AddNotifications(BloqueadoDataHora?.contract);
            }
        }

        public void Cancelar(DateTime cancelamentoDataHora)
        {
            if (Cancelado)
                AdicionarNotificacao("Account já está cancelada.");

            CanceladoDataHora = cancelamentoDataHora;

            if (CanceladoDataHora == null)
                AdicionarNotificacao(GetType().Name, MethodBase.GetCurrentMethod()?.Name, "CanceladoDataHora is null or with contract invalid.");

            AddNotifications(CanceladoDataHora?.contract);

            if (IsValid)
                Cancelado = true;
        }

        public void Bloquear(DateTime bloqueioDataHora)
        {
            if (Bloqueado)
                AdicionarNotificacao("Account já está bloqueada.");

            BloqueadoDataHora = bloqueioDataHora;

            if (BloqueadoDataHora == null)
                AdicionarNotificacao(GetType().Name, MethodBase.GetCurrentMethod()?.Name, "CanceladoDataHora is null or with contract invalid.");

            AddNotifications(BloqueadoDataHora?.contract);

            if (IsValid)
                Bloqueado = true;
        }

        public void MeioPagamentoAdicionar(AccountAssinaturaMeioPagamento objEntity)
        {
            if (objEntity == null)
                AddNotification($"{objEntity?.GetType().Name}.MeioPagamentoAdicionar", $"{nameof(AccountAssinaturaMeioPagamento)} : is null.");

            if (objEntity?.IsValid == false)
                AddNotifications(objEntity.Notifications);

            if (objEntity != null)
                MeiosDePagamentos.Add(objEntity);
        }

        public void FeaturesAdicionar(AccountAssinaturaFeature objEntity)
        {
            if (objEntity == null)
                AddNotification($"{objEntity?.GetType().Name}.FeaturesAdicionar", $"{nameof(AccountAssinaturaFeature)} : is null.");

            if (objEntity?.IsValid == false)
                AddNotifications(objEntity.Notifications);

            if (objEntity != null)
                Features.Add(objEntity);
        }

        public void AtivacoesAdicionar(AccountAssinaturaAtivacao objEntity)
        {
            if (objEntity == null)
                AddNotification($"{objEntity?.GetType().Name}.AtivacoesAdicionar", $"{nameof(AccountAssinaturaAtivacao)} : is null.");

            if (objEntity?.IsValid == false)
                AddNotifications(objEntity.Notifications);

            if (objEntity != null)
                Ativacoes.Add(objEntity);
        }
    }
}
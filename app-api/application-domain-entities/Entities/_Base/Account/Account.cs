using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Biblioteca;
using application_domain.Types.Values;
using application_domain.Abstracts;
using application_domain.Interfaces;
using System.Reflection;
using application_domain.Objects;

namespace application_data_entities
{
    public class Account : BaseEntity<Key>, IEntidade
    {
        public DataHora CadastradoDataHora { get; private set; }
        public DataHora? AlteradoDataHora { get; private set; }
        public Email Email { get; private set; }
        public Password Senha { get; private set; }
        public bool Cancelado { get; private set; }
        public DataHora? CanceladoDataHora { get; private set; }
        public bool Bloqueado { get; private set; }
        public DataHora? BloqueadoDataHora { get; private set; }
        public virtual List<AccountAssinatura> Assinaturas { get; private set; } = new List<AccountAssinatura>();

        public Account(dynamic objetoDynamic) : base(
                id: (Key)FuncoesEspeciais.ToGuid(FuncoesEspeciais.IsFieldExist(objetoDynamic, "Id") ? objetoDynamic?.Id : Guid.NewGuid())
            )
        {
            Load(objetoDynamic);
        }

        public Account(Email email, Password senha) : base(id: Guid.NewGuid())
        {
            AddNotifications(email.contract, senha.contract);

            if (IsValid)
            {
                CadastradoDataHora = DateTime.Now.ConverteDataAzureBrasil();
                Email = email;
                Senha = senha.Encriptar();
                Bloqueado = false;
                Cancelado = false;
            }
        }

        public void Load(dynamic objetoDynamic)
        {
            if (objetoDynamic == null)
            {
                AddNotification($"{GetType().Name}.Load", $"{GetType().Name} - JSON inválido.");

                return;
            }

            LoadFromDynamic<Account>(this, objetoDynamic);

            if (IsValid)
            {
                AddNotifications
                    (
                        Id.contract,
                        CadastradoDataHora.contract,
                        Email.contract,
                        Senha.contract
                    );

                if (AlteradoDataHora.HasValue)
                    AddNotifications(AlteradoDataHora?.contract);

                if (CanceladoDataHora.HasValue)
                    AddNotifications(CanceladoDataHora?.contract);

                if (BloqueadoDataHora.HasValue)
                    AddNotifications(BloqueadoDataHora?.contract);
            }
        }

        public void SenhaEncriptar()
        {
            if (IsValid)
                AddNotifications(Senha.contract);

            if (IsValid)
            {
                Senha = Senha.Encriptar();
            }
        }

        public void SenhaModificar(string senha)
        {
            Senha = senha;

            AddNotifications(Senha.contract);

            if (IsValid)
                SenhaEncriptar();
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

        public void AssinaturaAdicionar(AccountAssinatura objetoAssinatura)
        {
            if (objetoAssinatura == null)
                AddNotification($"{nameof(AccountAssinatura)}.AssinaturaAdicionar", $"{nameof(AccountAssinatura)} : is null.");

            if (objetoAssinatura?.IsValid == false)
                AddNotifications(objetoAssinatura.Notifications);

            if (objetoAssinatura != null)
                Assinaturas.Add(objetoAssinatura);
        }
    }
}

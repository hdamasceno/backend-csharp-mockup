using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using application_domain.Types.Values;
using application_domain.Abstracts;
using application_domain.Interfaces;
using Biblioteca;

namespace application_data_entities
{
    public class AccountAssinaturaMeioPagamento : BaseEntity<Key>, IEntidadeBase
    {
        public DataHora CadastradoDataHora { get; private set; }
        public DataHora? AlteradoDataHora { get; private set; }
        public Key AccountId { get; private set; }
        public virtual IEntidade? Account { get; private set; }
        public Key AccountAssinaturaId { get; private set; }
        public virtual IEntidadeBase? AccountAssinatura { get; private set; }
        public Key? PessoaFisicaId { get; private set; }
        public virtual IEntidadeBase? PessoaFisica { get; private set; }
        public Key? PessoaJuridicaId { get; private set; }
        public virtual IEntidadeBase? PessoaJuridica { get; private set; }
        public Key? EnderecoId { get; private set; }
        public virtual IEntidadeBase? Endereco { get; private set; }
        public string CartaoToken { get; private set; } = string.Empty;
        public virtual List<IEntidadeBase> Pagamentos { get; private set; } = new List<IEntidadeBase>();

        public AccountAssinaturaMeioPagamento(dynamic objetoDynamic) : base(
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

            LoadFromDynamic<AccountAssinaturaMeioPagamento>(this, objetoDynamic);

            if (IsValid)
            {
                AddNotifications(AccountId.contract, CadastradoDataHora.contract, AccountAssinaturaId.contract);

                if (AlteradoDataHora.HasValue)
                    AddNotifications(AlteradoDataHora?.contract);

                if (PessoaFisicaId.HasValue)
                    AddNotifications(PessoaFisicaId?.contract);

                if (PessoaJuridicaId.HasValue)
                    AddNotifications(PessoaJuridicaId?.contract);

                if (EnderecoId.HasValue)
                    AddNotifications(EnderecoId?.contract);
            }
        }

        public void PagamentoAdicionar(AccountAssinaturaMeioPagamentoItem objEntity)
        {
            if (objEntity == null)
                AddNotification($"{objEntity?.GetType().Name}.PagamentoAdicionar", $"{nameof(AccountAssinaturaMeioPagamentoItem)} : is null.");

            if (objEntity?.IsValid == false)
                AddNotifications(objEntity.Notifications);

            if (objEntity != null)
                Pagamentos.Add(objEntity);
        }
    }
}

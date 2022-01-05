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
        public Key AccoutAssinaturaId { get; private set; }
        public virtual IEntidadeBase? AccoutAssinatura { get; private set; }
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
                AddNotification($"{GetType().Name}.LoadFromDynamic", $"{GetType().Name} - JSON inválido.");

                return;
            }

            DataHora cadastradoDataHora = FuncoesEspeciais.ToDateTime(objetoDynamic?.CadastradoDataHora, false, false, true);
            DataHora? alteradoDataHora = FuncoesEspeciais.ToDateTimeNull(objetoDynamic?.AlteradoDataHora, false, false, true);
            Key accountId = FuncoesEspeciais.ToGuid(objetoDynamic?.AccountId);
            Key accountAssinaturaId = FuncoesEspeciais.ToGuid(objetoDynamic?.AccountAssinaturaId);
            Key? pessoaJuridicaId = FuncoesEspeciais.ToGuidOrNull(objetoDynamic?.PessoaJuridicaId);
            Key? pessoaFisicaId = FuncoesEspeciais.ToGuidOrNull(objetoDynamic?.PessoaFisicaId);
            Key? enderecoId = FuncoesEspeciais.ToGuidOrNull(objetoDynamic?.EnderecoId);
            string cartaoToken = FuncoesEspeciais.ToString(objetoDynamic?.CartaoToken, false, false, false);

            AddNotifications(accountId.contract, cadastradoDataHora.contract, accountAssinaturaId.contract);

            if (alteradoDataHora.HasValue)
                AddNotifications(alteradoDataHora?.contract);

            if (pessoaFisicaId.HasValue)
                AddNotifications(pessoaFisicaId?.contract);

            if (pessoaJuridicaId.HasValue)
                AddNotifications(pessoaJuridicaId?.contract);

            if (enderecoId.HasValue)
                AddNotifications(enderecoId?.contract);

            if (IsValid)
            {
                CadastradoDataHora = cadastradoDataHora;
                AlteradoDataHora = alteradoDataHora;
                AccountId = accountId;
                PessoaFisicaId = pessoaFisicaId;
                PessoaJuridicaId = pessoaJuridicaId;
                EnderecoId = enderecoId;
                CartaoToken = cartaoToken;
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

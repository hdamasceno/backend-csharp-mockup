using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using application_domain.Abstracts;
using application_domain.Interfaces;
using application_domain.Types.Values;
using Biblioteca;

namespace application_data_entities
{
    public class PessoaFisicaEndereco : BaseEntity<Key>, IEntidadeBase
    {
        public DataHora CadastradoDataHora { get; private set; }
        public DataHora? AlteradoDataHora { get; private set; }
        public Key AccountId { get; private set; }
        public IEntidade? Account { get; private set; }
        public Key PessoaFisicaId { get; set; }
        public virtual IEntidadeBase? PessoaFisica { get; set; }
        public Key EnderecoId { get; set; }
        public virtual IEntidadeBase? Endereco { get; set; }
        public bool IsPrincipal { get; set; }

        public PessoaFisicaEndereco(dynamic objetoDynamic) : base(
                id: (Key)(objetoDynamic?.Id != null ? FuncoesEspeciais.ToGuid(objetoDynamic.Id) : Guid.NewGuid())
            )
        {
            Load(objetoDynamic);
        }

        public void LoadRelationShips()
        {
        }

        public void Load(dynamic objetoDynamic)
        {
            if (objetoDynamic == null)
            {
                AddNotification($"{GetType().Name}.LoadFromDynamic", $"{GetType().Name} - JSON invalido.");

                return;
            }

            DataHora cadastradoDataHora = FuncoesEspeciais.ToDateTime(objetoDynamic?.CadastradoDataHora, false, false, true);
            DataHora? alteradoDataHora = FuncoesEspeciais.ToDateTime(objetoDynamic?.AlteradoDataHora, false, false, true);

            Key accountId = FuncoesEspeciais.ToGuid(objetoDynamic?.AccountId);
            Key pessoaFisicaId = FuncoesEspeciais.ToGuid(objetoDynamic?.PessoaFisicaId);
            Key enderecoId = FuncoesEspeciais.ToGuid(objetoDynamic?.EnderecoId);
            bool isPrincipal = FuncoesEspeciais.ToString(objetoDynamic?.IsPrincipal) == "TRUE";

            AddNotifications(accountId.contract, cadastradoDataHora.contract, pessoaFisicaId.contract, enderecoId.contract);

            if (alteradoDataHora.HasValue)
                AddNotifications(alteradoDataHora?.contract);

            if (IsValid)
            {
                AccountId = accountId;
                CadastradoDataHora = cadastradoDataHora;
                AlteradoDataHora = alteradoDataHora;
                PessoaFisicaId = pessoaFisicaId;
                EnderecoId = enderecoId;
                IsPrincipal = isPrincipal;
            }
        }

    }
}

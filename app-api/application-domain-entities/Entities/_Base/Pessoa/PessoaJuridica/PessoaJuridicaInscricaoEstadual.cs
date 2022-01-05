using Biblioteca;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using application_domain.Abstracts;
using application_domain.Interfaces;
using application_domain.Types.Values;

namespace application_data_entities
{
    public class PessoaJuridicaInscricaoEstadual : BaseEntity<Key>, IEntidadeBase
    {
        public DataHora CadastradoDataHora { get; private set; }
        public DataHora? AlteradoDataHora { get; private set; }
        public Key AccountId { get; private set; }
        public IEntidade? Account { get; private set; }
        public InscricaoEstadual DocumentoInscricaoEstadual { get; set; }
        public Uf UF { get; set; }

        public PessoaJuridicaInscricaoEstadual(dynamic objetoDynamic) : base(
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
            InscricaoEstadual inscricaoEstadual = FuncoesEspeciais.ToString(objetoDynamic?.InscricaoEstadual);
            Uf uf = FuncoesEspeciais.ToString(objetoDynamic?.UF);

            Key accountId = FuncoesEspeciais.ToGuid(objetoDynamic?.AccountId);

            AddNotifications(accountId.contract, cadastradoDataHora.contract, inscricaoEstadual.contract, uf.contract);

            if (alteradoDataHora.HasValue)
                AddNotifications(alteradoDataHora?.contract);

            if (IsValid)
            {
                AccountId = accountId;
                CadastradoDataHora = cadastradoDataHora;
                AlteradoDataHora = alteradoDataHora;
                DocumentoInscricaoEstadual = inscricaoEstadual;
                UF = uf;
            }
        }
    }
}

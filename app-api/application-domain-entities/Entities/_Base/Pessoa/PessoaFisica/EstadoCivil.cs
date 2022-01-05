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
    public class EstadoCivil : BaseEntity<Key>, IEntidade
    {
        public DataHora CadastradoDataHora { get; private set; }
        public DataHora? AlteradoDataHora { get; private set; }
        public Name Nome { get; set; }

        public EstadoCivil(dynamic objetoDynamic) : base(
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
            Name nome = FuncoesEspeciais.ToString(objetoDynamic?.Nome);

            AddNotifications(cadastradoDataHora.contract, nome.contract);

            if (alteradoDataHora.HasValue)
                AddNotifications(alteradoDataHora?.contract);

            if (IsValid)
            {
                CadastradoDataHora = cadastradoDataHora;
                AlteradoDataHora = alteradoDataHora;
                Nome = nome;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Biblioteca;
using application_domain.Types.Values;
using application_domain.Abstracts;
using application_domain.Interfaces;

namespace application_data_entities
{
    public class Cidade : BaseEntity<Key>, IEntidade
    {
        public DataHora CadastradoDataHora { get; set; }
        public DataHora? AlteradoDataHora { get; set; }
        public DecimalPositive IbgeCodigo { get; set; }
        public Key EstadoId { get; set; }
        public virtual Estado? Estado { get; set; }
        public Name Nome { get; set; }

        public Cidade(dynamic objetoDynamic) : base(id: (Key)FuncoesEspeciais.ToGuid(objetoDynamic?.Id != null ? objetoDynamic?.Id : Guid.NewGuid()))
        {
            Load(objetoDynamic);
        }

        public Cidade(Key estadoId, DecimalPositive ibgeCodigo, Name nome) : base(id: Guid.NewGuid())
        {
            AddNotifications(ibgeCodigo.contract, estadoId.contract, nome.contract);

            if (IsValid)
            {
                CadastradoDataHora = DateTime.Now;
                EstadoId = estadoId;
                IbgeCodigo = ibgeCodigo;
                Nome = nome;
            }
        }

        public void Load(dynamic objetoDynamic)
        {
            if (objetoDynamic == null)
            {
                AddNotification($"{GetType().Name}.Load", $"{GetType().Name} - JSON inválido.");

                return;
            }

            LoadFromDynamic<Cidade>(this, objetoDynamic);

            if (IsValid)
            {
                AddNotifications(
                    CadastradoDataHora.contract,
                    AlteradoDataHora?.contract,
                    IbgeCodigo.contract,
                    EstadoId.contract,
                    Nome.contract);
            }
        }
    }
}

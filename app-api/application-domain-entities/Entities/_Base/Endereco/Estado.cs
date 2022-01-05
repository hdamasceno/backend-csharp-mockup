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
    public class Estado : BaseEntity<Key>, IEntidade
    {
        public DataHora CadastradoDataHora { get; set; }
        public DataHora? AlteradoDataHora { get; set; }
        public Name Nome { get; set; }
        public Uf UF { get; set; }
        public DecimalPositive IbgeCodigo { get; set; }

        public Estado(dynamic objetoDynamic) : base(id: (Key)FuncoesEspeciais.ToGuid(objetoDynamic?.Id != null ? objetoDynamic?.Id : Guid.NewGuid()))
        {
            Load(objetoDynamic);
        }

        public Estado(DecimalPositive ibgeCodigo, Uf uf, Name nome) : base(id: Guid.NewGuid())
        {
            AddNotifications(IbgeCodigo.contract, uf.contract, nome.contract);

            if (IsValid)
            {
                CadastradoDataHora = DateTime.Now;
                IbgeCodigo = ibgeCodigo;
                Nome = nome;
                UF = uf;
            }
        }

        public void LoadRelationShips()
        {
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
            Name nome = FuncoesEspeciais.ToString(objetoDynamic?.Nome);
            Uf uf = FuncoesEspeciais.ToString(objetoDynamic?.UF);
            DecimalPositive ibgeCodigo = FuncoesEspeciais.ToDecimal(objetoDynamic?.IbgeCodigo);

            AddNotifications(cadastradoDataHora.contract, nome.contract, uf.contract, IbgeCodigo.contract);

            if (alteradoDataHora.HasValue)
                AddNotifications(alteradoDataHora?.contract);

            if (IsValid)
            {
                CadastradoDataHora = cadastradoDataHora;
                AlteradoDataHora = alteradoDataHora;
                UF = uf;
                IbgeCodigo = ibgeCodigo;
                Nome = nome;
            }
        }
    }
}

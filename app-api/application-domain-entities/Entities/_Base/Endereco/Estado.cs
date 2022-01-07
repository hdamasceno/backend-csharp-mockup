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
    // ja pensou em consumir o endereço de um serviço de ceps? :D quem guarda endereço em tabela?? isso nem existe mais
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

        public void Load(dynamic objetoDynamic)
        {
            if (objetoDynamic == null)
            {
                AddNotification($"{GetType().Name}.Load", $"{GetType().Name} - JSON inválido.");

                return;
            }

            LoadFromDynamic<Estado>(this, objetoDynamic);            

            if (IsValid)
            {
                AddNotifications(CadastradoDataHora.contract, Nome.contract, UF.contract, IbgeCodigo.contract);

                if (AlteradoDataHora.HasValue)
                    AddNotifications(AlteradoDataHora?.contract);
            }
        }
    }
}

using Biblioteca;
using application_domain.Abstracts;
using application_domain.Interfaces;
using application_domain.Types.Values;

namespace application_data_entities
{
    public abstract class Pessoa : BaseEntity<Key>, IEntidadeBase
    {
        public DataHora CadastradoDataHora { get; set; }
        public DataHora? AlteradoDataHora { get; set; }
        public Key AccountId { get; set; }
        public virtual IEntidade? Account { get; set; }

        public Pessoa() : base(id: Guid.NewGuid())
        {
            CadastradoDataHora = DateTime.Now.ConverteDataAzureBrasil();
        }

        public Pessoa(dynamic objetoDynamic) : base(
                id: (Key)(objetoDynamic?.Id != null ? FuncoesEspeciais.ToGuid(objetoDynamic.Id) : Guid.NewGuid())
            )
        {
            Load(objetoDynamic);
        }

        public virtual void Load(dynamic objetoDynamic)
        {
        }
    }
}
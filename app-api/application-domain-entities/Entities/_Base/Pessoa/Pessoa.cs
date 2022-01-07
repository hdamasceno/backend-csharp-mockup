using Biblioteca;
using application_domain.Abstracts;
using application_domain.Interfaces;
using application_domain.Types.Values;

namespace application_data_entities
{
    // hehe faz muitos anos que n via esse negocio de Pessoa
    // o conceito ideal é vc criar objetos de identidade para cada caso (pj e pf) somente, o resto repete mesmo
    // pq? pq cada um é uma coisa, n é uma pessoa, sao entidades, entidades tem uma identidade
    public abstract class Pessoa : BaseEntity<Key>, IEntidadeBase
    {
        public DataHora CadastradoDataHora { get; set; }
        public DataHora? AlteradoDataHora { get; set; }
        public Key AccountId { get; set; }
        public virtual IEntidade? Account { get; set; }
        public Name Nome { get; set; } = string.Empty;

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
            if (objetoDynamic == null)
            {
                AddNotification($"{GetType().Name}.Load", $"{GetType().Name} - JSON invalido.");

                return;
            }

            LoadFromDynamic<RegimeTributario>(this, objetoDynamic);

            if (IsValid)
            {
                AddNotifications(
                    CadastradoDataHora.contract,
                    Nome.contract);

                if (AlteradoDataHora.HasValue)
                    AddNotifications(AlteradoDataHora?.contract);
            }
        }
    }
}
using Biblioteca;
using application_domain.Abstracts;
using application_domain.Interfaces;
using application_domain.Types.Values;

namespace application_data_entities
{
    public class PessoaJuridicaFilial : BaseEntity<Key>, IEntidadeBase
    {
        public DataHora CadastradoDataHora { get; private set; }
        public DataHora? AlteradoDataHora { get; private set; }
        public Key AccountId { get; private set; }
        public virtual IEntidade? Account { get; private set; }
        public Key PessoaJuridicaMatrizId { get; private set; }
        public virtual IEntidadeBase? PessoaJuridicaQuandoMatriz { get; private set; }
        public Key PessoaJuridicaFilialId { get; private set; }
        public virtual IEntidadeBase? PessoaJuridicaQuandoFilial { get; private set; }

        public PessoaJuridicaFilial(dynamic objetoDynamic) :
            base(id: (Key)(objetoDynamic?.Id != null ? FuncoesEspeciais.ToGuid(objetoDynamic.Id) : Guid.NewGuid()))
        {
            Load(objetoDynamic);
        }

        public void Load(dynamic objetoDynamic)
        {

        }
    }
}
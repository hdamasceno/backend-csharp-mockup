using Biblioteca;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using application_domain.Types.Values;
using application_domain.Abstracts;
using application_domain.Interfaces;

namespace application_data_entities
{
    public class Endereco : BaseEntity<Key>, IEntidadeBase
    {
        public Key AccountId { get; private set; }
        public virtual IEntidade? Account { get; set; }
        public DataHora CadastradoDataHora { get; private set; }
        public DataHora? AlteradoDataHora { get; private set; }
        public Key EstadoId { get; private set; }
        public virtual Estado? Estado { get; private set; }
        public Key CidadeId { get; private set; }
        public virtual Cidade? Cidade { get; private set; }
        public Logradouro Logradouro { get; private set; }
        public string? Numero { get; private set; }
        public string? Complemento { get; private set; }
        public Cep Cep { get; private set; }

        public Endereco(dynamic objetoDynamic) : base(id: (Key)FuncoesEspeciais.ToGuid(objetoDynamic?.Id != null ? objetoDynamic?.Id : Guid.NewGuid()))
        {
            Load(objetoDynamic);
        }

        public Endereco(
            Key estadoId,
            Key cidadeId,
            Cep cep,
            Logradouro logradouro,
            string? numero,
            string? complemento) : base(id: Guid.NewGuid())
        {
            AddNotifications
                (
                    estadoId.contract,
                    cidadeId.contract,
                    cep.contract,
                    logradouro.contract
                );

            if (IsValid)
            {
                CadastradoDataHora = DateTime.Now;
                EstadoId = estadoId;
                CidadeId = cidadeId;
                Cep = cep;
                Logradouro = logradouro;
                Numero = numero;
                Complemento = complemento;
            }
        }

        public void Load(dynamic objetoDynamic)
        {
            if (objetoDynamic == null)
            {
                AddNotification($"{GetType().Name}.Load", $"{GetType().Name} - JSON inválido.");

                return;
            }

            LoadFromDynamic<Endereco>(this, objetoDynamic);

            if (IsValid)
            {
                AddNotifications
                (
                    CadastradoDataHora.contract,                    
                    EstadoId.contract,
                    CidadeId.contract,
                    Logradouro.contract,
                    Cep.contract
                );
            }
        }
    }
}

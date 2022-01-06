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
    public class PessoaFisicaDocumento : BaseEntity<Key>, IEntidadeBase
    {
        public DataHora CadastradoDataHora { get; private set; }
        public DataHora? AlteradoDataHora { get; private set; }
        public Key AccountId { get; private set; }
        public IEntidade? Account { get; private set; }
        public Key PessoaFisicaId { get; set; }
        public virtual IEntidadeBase? PessoaFisica { get; set; }
        public Key DocumentoTipoId { get; set; }
        public virtual IEntidade? DocumentoTipo { get; set; }
        public string? DocumentoNumero { get; set; }
        public Data? DocumentoEmissaoData { get; set; }
        public Key? DocumentoEmissaoEstadoId { get; set; }
        public virtual IEntidade? DocumentoEmissaoEstado { get; set; }
        public Key? DocumentoEmissaoOrgaoExpedidorId { get; set; }
        public virtual IEntidade? DocumentoEmissaoOrgaoExpedidor { get; set; }
        public Data? DocumentoValidadeData { get; set; }

        public PessoaFisicaDocumento(dynamic objetoDynamic) : base(
                id: (Key)(objetoDynamic?.Id != null ? FuncoesEspeciais.ToGuid(objetoDynamic.Id) : Guid.NewGuid())
            )
        {
            Load(objetoDynamic);
        }

        public void Load(dynamic objetoDynamic)
        {
            if (objetoDynamic == null)
            {
                AddNotification($"{GetType().Name}.Load", $"{GetType().Name} - JSON invalido.");

                return;
            }

            LoadFromDynamic<PessoaFisicaDocumento>(this, objetoDynamic);            

            if (IsValid)
            {
                AddNotifications(AccountId.contract, CadastradoDataHora.contract, PessoaFisicaId.contract);

                if (AlteradoDataHora.HasValue)
                    AddNotifications(AlteradoDataHora?.contract);

                if (DocumentoEmissaoData.HasValue)
                    AddNotifications(DocumentoEmissaoData?.contract);

                if (DocumentoValidadeData.HasValue)
                    AddNotifications(DocumentoValidadeData?.contract);

                if (DocumentoEmissaoEstadoId.HasValue)
                    AddNotifications(DocumentoEmissaoEstadoId?.contract);

                if (DocumentoEmissaoOrgaoExpedidorId.HasValue)
                    AddNotifications(DocumentoEmissaoOrgaoExpedidorId?.contract);
            }
        }
    }
}

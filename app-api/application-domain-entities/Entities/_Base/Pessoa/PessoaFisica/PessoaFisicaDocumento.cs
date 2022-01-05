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

            Key accountId = FuncoesEspeciais.ToGuid(objetoDynamic?.AccountId);
            Key pessoaFisicaId = FuncoesEspeciais.ToGuid(objetoDynamic?.PessoaFisicaId);
            Key documentoTipoId = FuncoesEspeciais.ToGuid(objetoDynamic?.DocumentoTipoId);
            string documentoNumero = FuncoesEspeciais.ToString(objetoDynamic?.DocumentoNumero);
            Data? documentoEmissaoData = FuncoesEspeciais.ToDateTimeNull(objetoDynamic?.DocumentoEmissaoData, true, false, true);
            Data? documentoValidadeData = FuncoesEspeciais.ToDateTimeNull(objetoDynamic?.DocumentoValidadeData, true, false, true);
            Key? documentoEmissaoEstadoId = FuncoesEspeciais.ToGuidOrNull(objetoDynamic?.DocumentoEmissaoEstadoId);
            Key? documentoEmissaoOrgaoExpedidorId = FuncoesEspeciais.ToGuidOrNull(objetoDynamic?.DocumentoEmissaoOrgaoExpedidorId);

            AddNotifications(accountId.contract, cadastradoDataHora.contract);

            if (alteradoDataHora.HasValue)
                AddNotifications(alteradoDataHora?.contract, pessoaFisicaId.contract, documentoTipoId.contract);

            if (documentoEmissaoData.HasValue)
                AddNotifications(documentoEmissaoData?.contract);

            if (documentoValidadeData.HasValue)
                AddNotifications(documentoValidadeData?.contract);

            if (documentoEmissaoEstadoId.HasValue)
                AddNotifications(documentoEmissaoEstadoId?.contract);

            if (documentoEmissaoOrgaoExpedidorId.HasValue)
                AddNotifications(documentoEmissaoOrgaoExpedidorId?.contract);

            if (IsValid)
            {
                AccountId = accountId;
                CadastradoDataHora = cadastradoDataHora;
                AlteradoDataHora = alteradoDataHora;
                PessoaFisicaId = pessoaFisicaId;
                DocumentoTipoId = documentoTipoId;
                DocumentoNumero = documentoNumero;
                DocumentoEmissaoData = documentoEmissaoData;
                DocumentoEmissaoEstadoId = documentoEmissaoEstadoId;
                DocumentoEmissaoOrgaoExpedidorId = documentoEmissaoOrgaoExpedidorId;
                DocumentoValidadeData = documentoValidadeData;
            }
        }
    }
}

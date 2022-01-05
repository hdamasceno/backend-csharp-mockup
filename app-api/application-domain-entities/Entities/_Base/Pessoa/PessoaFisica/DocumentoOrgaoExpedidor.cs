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
    public class DocumentoOrgaoExpedidor : BaseEntity<Key>, IEntidade
    {
        public DataHora CadastradoDataHora { get; private set; }
        public DataHora? AlteradoDataHora { get; private set; }
        public Name Nome { get; set; }
        public DecimalPositive AppCodigo { get; set; }
        public DecimalPositive SngpcCodigo { get; set; }
        public DecimalPositive NFeCodigo { get; set; }
        public DecimalPositive NFCeCodigo { get; set; }
        public DecimalPositive SpedFiscalCodigo { get; set; }
        public DecimalPositive SpedContribuicaoCodigo { get; set; }
        public DecimalPositive SintegraCodigo { get; set; }
        public DecimalPositive SatCodigo { get; set; }

        public DocumentoOrgaoExpedidor(dynamic objetoDynamic) : base(
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
            DecimalPositive appCodigo = FuncoesEspeciais.ToDecimal(objetoDynamic?.AppCodigo);
            DecimalPositive sngpcCodigo = FuncoesEspeciais.ToDecimal(objetoDynamic?.SngpcCodigo);
            DecimalPositive nfeCodigo = FuncoesEspeciais.ToDecimal(objetoDynamic?.NFeCodigo);
            DecimalPositive nfceCodigo = FuncoesEspeciais.ToDecimal(objetoDynamic?.NFCeCodigo);
            DecimalPositive satCodigo = FuncoesEspeciais.ToDecimal(objetoDynamic?.SatCodigo);
            DecimalPositive spedFiscalCodigo = FuncoesEspeciais.ToDecimal(objetoDynamic?.SpedFiscalCodigo);
            DecimalPositive spedContribuicaoCodigo = FuncoesEspeciais.ToDecimal(objetoDynamic?.SpedContribuicaoCodigo);

            AddNotifications(cadastradoDataHora.contract, Nome.contract, appCodigo.contract);

            if (alteradoDataHora.HasValue)
                AddNotifications(alteradoDataHora?.contract);

            if (IsValid)
            {
                CadastradoDataHora = cadastradoDataHora;
                AlteradoDataHora = alteradoDataHora;
                Nome = Nome;
                AppCodigo = appCodigo;
                SngpcCodigo = sngpcCodigo;
                NFeCodigo = nfeCodigo;
                NFCeCodigo = nfceCodigo;
                SatCodigo = satCodigo;
                SpedFiscalCodigo = spedFiscalCodigo;
                SpedContribuicaoCodigo = spedContribuicaoCodigo;
            }
        }
    }
}

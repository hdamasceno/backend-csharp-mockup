using Biblioteca;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using application_domain.Abstracts;
using application_domain.Interfaces;
using application_domain.Types.Values;

namespace application_data_entities
{
    public class RegimeTributario : BaseEntity<Key>, IEntidade
    {
        public DataHora CadastradoDataHora { get; set; }
        public DataHora? AlteradoDataHora { get; set; }
        public Name Nome { get; set; }
        public DecimalPositive NFeCodigo { get; set; }
        public DecimalPositive NFCeCodigo { get; set; }
        public DecimalPositive SatCodigo { get; set; }
        public DecimalPositive SpedFiscalCodigo { get; set; }
        public DecimalPositive SpedContribuicaoCodigo { get; set; }
        public DecimalPositive SintegraCodigo { get; set; }

        public RegimeTributario(dynamic objetoDynamic) : base(id: (Key)(objetoDynamic?.Id != null ? FuncoesEspeciais.ToGuid(objetoDynamic.Id) : Guid.NewGuid()))
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
                AddNotification($"{GetType().Name}.LoadFromDynamic", $"{GetType().Name} - JSON inválido.");

                return;
            }

            DataHora cadastradoDataHora = FuncoesEspeciais.ToDateTime(objetoDynamic?.CadastradoDataHora, false, false, true);
            DataHora? alteradoDataHora = FuncoesEspeciais.ToDateTimeNull(objetoDynamic?.AlteradoDataHora, false, false, true);
            Name nome = FuncoesEspeciais.ToString(objetoDynamic?.Nome);
            DecimalPositive satCodigo = FuncoesEspeciais.ToDecimal(objetoDynamic?.SatCodigo);
            DecimalPositive nfeCodigo = FuncoesEspeciais.ToDecimal(objetoDynamic?.NFeCodigo);
            DecimalPositive nfceCodigo = FuncoesEspeciais.ToDecimal(objetoDynamic?.NFCeCodigo);
            DecimalPositive spedFiscalCodigo = FuncoesEspeciais.ToDecimal(objetoDynamic?.SpedFiscalCodigo);
            DecimalPositive spedContribuicaoCodigo = FuncoesEspeciais.ToDecimal(objetoDynamic?.SpedContribuicaoCodigo);
            DecimalPositive sintegraCodigo = FuncoesEspeciais.ToDecimal(objetoDynamic?.SintegraCodigo);

            AddNotifications
                (
                    cadastradoDataHora.contract,
                    nome.contract,
                    satCodigo.contract,
                    nfeCodigo.contract,
                    nfceCodigo.contract,
                    spedFiscalCodigo.contract,
                    spedContribuicaoCodigo.contract,
                    SintegraCodigo.contract
                );

            if (alteradoDataHora.HasValue)
                AddNotifications(alteradoDataHora?.contract);

            if (IsValid)
            {
                Nome = Nome;
                SatCodigo = satCodigo;
                NFeCodigo = nfeCodigo;
                NFCeCodigo = nfceCodigo;
                SpedContribuicaoCodigo = spedContribuicaoCodigo;
                SpedFiscalCodigo = spedFiscalCodigo;
                SintegraCodigo = sintegraCodigo;
            }
        }
    }
}

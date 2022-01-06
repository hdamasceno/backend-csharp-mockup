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

        public void Load(dynamic objetoDynamic)
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
                    Nome.contract,
                    NFeCodigo.contract,
                    NFCeCodigo.contract,
                    SpedFiscalCodigo.contract,
                    SpedContribuicaoCodigo.contract,
                    SintegraCodigo.contract,
                    SatCodigo.contract);

                if (AlteradoDataHora.HasValue)
                    AddNotifications(AlteradoDataHora?.contract);
            }
        }
    }
}

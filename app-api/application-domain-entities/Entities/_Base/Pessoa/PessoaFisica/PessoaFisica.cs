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
    public class PessoaFisica : Pessoa, IEntidadeBase
    {
        public Cpf? DocumentoCPF { get; set; }
        public Data? NascimentoData { get; set; }
        public Key? EstadoCivilId { get; set; }
        public virtual IEntidade? EstadoCivil { get; set; }
        public Key? SexoId { get; set; }
        public virtual IEntidade? Sexo { get; set; }

        public override void Load(dynamic objetoDynamic)
        {
            if (objetoDynamic == null)
            {
                AddNotification($"{GetType().Name}.Load", $"{GetType().Name} - JSON invalido.");

                return;
            }

            LoadFromDynamic<PessoaFisica>(this, objetoDynamic);

            if (IsValid)
            {
                AddNotifications(CadastradoDataHora.contract);

                if (EstadoCivilId.HasValue)
                    AddNotifications(EstadoCivilId?.contract);

                if (NascimentoData.HasValue)
                    AddNotifications(NascimentoData?.contract);

                if (SexoId.HasValue)
                    AddNotifications(SexoId?.contract);

                if (DocumentoCPF.HasValue)
                    AddNotifications(DocumentoCPF?.contract);

                if (AlteradoDataHora.HasValue)
                    AddNotifications(AlteradoDataHora?.contract);
            }
        }
    }
}

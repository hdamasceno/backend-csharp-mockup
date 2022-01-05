using Flunt.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace application_domain.Types.Values
{
    public struct DataHora
    {
        private readonly DateTime _value;
        public readonly Contract<DataHora> contract;

        private DataHora(DateTime value)
        {
            _value = value;

            contract = new Contract<DataHora>();

            Validate();
        }

        public override string ToString() =>
            _value.ToString("dd/MM/yyyy HH:mm:ss");

        public static implicit operator DataHora(DateTime value) =>
            new DataHora(value);

        private bool Validate()
        {
            if (_value == new DateTime())
                return AddNotification("Data-Hora inválida.");

            if (_value < new DateTime(1950, 1, 1, 0, 0, 0))
                return AddNotification("Data-Hora inválida. Data-Hora não pode ser menor que 01-01-1950 00:00:00");

            return true;
        }

        private bool AddNotification(string message)
        {
            contract.AddNotification(nameof(DataHora), message);

            return false;
        }
    }
}

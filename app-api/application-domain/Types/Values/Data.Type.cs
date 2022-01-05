using Flunt.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace application_domain.Types.Values
{
    public struct Data
    {
        private readonly DateTime _value;
        public readonly Contract<Data> contract;

        private Data(DateTime value)
        {
            _value = new DateTime(value.Year, value.Month, value.Day, 0, 0, 0);

            contract = new Contract<Data>();

            Validate();
        }

        public override string ToString() =>
            _value.ToString("dd/MM/yyyy");

        public static implicit operator Data(DateTime value) =>
            new Data(value);

        private bool Validate()
        {
            if (_value == new DateTime())
                return AddNotification("Data inválida.");

            if (_value < new DateTime(1950, 1, 1, 0, 0, 0))
                return AddNotification("Data inválida. Data menor que 01-01-1950");

            return true;
        }

        private bool AddNotification(string message)
        {
            contract.AddNotification(nameof(Data), message);

            return false;
        }
    }
}

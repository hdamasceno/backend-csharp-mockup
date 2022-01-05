using Flunt.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace application_domain.Types.Values
{
    public struct DecimalPositive
    {
        private readonly decimal _value;
        public readonly Contract<DecimalPositive> contract;

        private DecimalPositive(decimal value)
        {
            _value = value;
            contract = new Contract<DecimalPositive>();

            Validate();
        }

        public static implicit operator DecimalPositive(decimal value) =>
            new DecimalPositive(value);

        private bool Validate()
        {
            if (_value < 0)
                return AddNotification("O valor decimal não pode ser menor do que zero.");

            return true;
        }

        private bool AddNotification(string message)
        {
            contract.AddNotification(nameof(DecimalPositive), message);
            return false;
        }
    }
}

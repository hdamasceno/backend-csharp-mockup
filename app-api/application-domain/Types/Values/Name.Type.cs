using Flunt.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace application_domain.Types.Values
{
    public struct Name
    {
        private readonly string _value;
        public readonly Contract<Name> contract;

        private Name(string value)
        {
            _value = value;
            contract = new Contract<Name>();

            Validate();
        }

        public override string ToString() =>
            _value;

        public static implicit operator Name(string value) =>
            new Name(value);

        private bool Validate()
        {
            if (string.IsNullOrWhiteSpace(_value))
                return AddNotification("Informe um nome válido.");

            if (_value.Length < 5)
                return AddNotification("O nome precisa ter pelo meno 5 caracteres.");

            //if (!Regex.IsMatch(_value, (@"[^a-zA-Z0-9]")))
                //return AddNotification("O nome não pode ter caracteres especiais.");

            return true;
        }

        private bool AddNotification(string message)
        {
            contract.AddNotification(nameof(Name), message);
            return false;
        }
    }
}

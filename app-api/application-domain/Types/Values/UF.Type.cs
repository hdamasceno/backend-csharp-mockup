using Flunt.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace application_domain.Types.Values
{
    public struct Uf
    {
        private readonly string _value;
        public readonly Contract<Uf> contract;

        private Uf(string value)
        {
            _value = value;
            contract = new Contract<Uf>();
            Validate();
        }

        public override string ToString() =>
            _value;

        public static implicit operator Uf(string value) =>
            new Uf(value);

        private bool Validate()
        {
            if (string.IsNullOrWhiteSpace(_value))
                return AddNotification("Informe um Uf válido.");

            if (_value.Length < 2)
                return AddNotification("O Uf precisa ter pelo meno 5 caracteres.");

            if (!Regex.IsMatch(_value, (@"[^a-zA-Z0-9]")))
                return AddNotification("O Uf não pode ter caracteres especiais.");

            return true;
        }

        private bool AddNotification(string message)
        {
            contract.AddNotification(nameof(Uf), message);
            return false;
        }
    }
}

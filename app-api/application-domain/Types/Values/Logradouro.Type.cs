using Flunt.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace application_domain.Types.Values
{
    public struct Logradouro
    {
        private readonly string _value;
        public readonly Contract<Logradouro> contract;

        private Logradouro(string value)
        {
            _value = value;
            contract = new Contract<Logradouro>();

            Validate();
        }

        public override string ToString() =>
            _value;

        public static implicit operator Logradouro(string value) =>
            new Logradouro(value);

        private bool Validate()
        {
            if (string.IsNullOrWhiteSpace(_value))
                return AddNotification("Informe um logradouro válido.");

            if (_value.Length < 5)
                return AddNotification("O logradouro precisa ter pelo meno 5 caracteres.");

            //if (!Regex.IsMatch(_value, (@"[^a-zA-Z0-9]")))
            //return AddNotification("O nome não pode ter caracteres especiais."); 

            return true;
        }

        private bool AddNotification(string message)
        {
            contract.AddNotification(nameof(Logradouro), message);

            return false;
        }
    }
}

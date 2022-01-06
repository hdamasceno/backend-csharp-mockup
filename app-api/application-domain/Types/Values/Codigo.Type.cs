using Biblioteca;
using Flunt.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace application_domain.Types.Values
{
    public struct Codigo
    {
        private readonly string _value;
        public readonly Contract<Codigo> contract;

        private Codigo(string value)
        {
            _value = value;

            contract = new Contract<Codigo>();

            Validate();
        }

        public override string ToString() =>
            _value;

        public static implicit operator Codigo(string value) => 
            new Codigo(String.IsNullOrWhiteSpace(value) == false ? value : GenerateNewNumber()); 

        private bool Validate()
        {
            if (string.IsNullOrWhiteSpace(_value))
                return AddNotification("Código inválido.");

            return true;
        }

        private bool AddNotification(string message)
        {
            contract.AddNotification(nameof(Codigo), message);

            return false;
        }

        private static string GenerateNewNumber()
        {
            return Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10).ToUpper();
        }
    }
}

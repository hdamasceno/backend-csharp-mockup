using Biblioteca;
using Flunt.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace application_domain.Types.Values
{
    public struct Cnae
    {
        private readonly string _value;
        public readonly Contract<Cnae> contract;

        private Cnae(string value)
        {
            _value = FuncoesEspeciais.SomenteNumero(value);

            contract = new Contract<Cnae>();

            Validate();
        }

        public override string ToString() =>
            FuncoesEspeciais.SomenteNumero(_value);

        public static implicit operator Cnae(string value) =>
            new Cnae(value);

        private bool Validate()
        {
            if (string.IsNullOrWhiteSpace(_value))
                return AddNotification("Informe um CNAE válido.");

            if (_value.Length < 7)
                return AddNotification("O CNAE precisa ter pelo meno 7 caracteres.");

            return true;
        }

        private bool AddNotification(string message)
        {
            contract.AddNotification(nameof(Cnae), message);

            return false;
        }
    }
}

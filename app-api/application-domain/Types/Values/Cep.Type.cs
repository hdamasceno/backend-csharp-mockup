using Biblioteca;
using Flunt.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace application_domain.Types.Values // Value Objects é o nome correto e fez muito bem usar struct do que classe, pq??? depois me explique :D
{
    public struct Cep
    {
        private readonly string _value;
        public readonly Contract<Cep> contract;

        private Cep(string value)
        {
            _value = FuncoesEspeciais.SomenteNumero(value);

            contract = new Contract<Cep>();

            Validate();
        }

        public override string ToString() =>
            FuncoesEspeciais.CEP(_value);

        public static implicit operator Cep(string value) =>
            new Cep(value);

        private bool Validate()
        {
            if (string.IsNullOrWhiteSpace(_value))
                return AddNotification("Informe um CEP válido.");

            if (_value.Length < 8)
                return AddNotification("O CEP precisa ter pelo meno 8 caracteres.");

            return true;
        }

        private bool AddNotification(string message)
        {
            contract.AddNotification(nameof(Cep), message);

            return false;
        }
    }
}

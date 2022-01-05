using Biblioteca;
using Flunt.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace application_domain.Types.Values
{
    public struct Cpf
    {
        private readonly string _value;
        public readonly Contract<Cpf> contract;

        private Cpf(string value)
        {
            _value = value;
            contract = new Contract<Cpf>();

            Validate();
        }

        public override string ToString() =>
            _value;

        public static implicit operator Cpf(string input) =>
            new Cpf(input);

        private void Validate()
        {
            if (string.IsNullOrWhiteSpace(_value))
            {
                contract.AddNotification(nameof(Cpf), "É necessário informar o CPF.");
                return;
            }

            if (FuncoesEspeciais.SomenteNumero(_value).Length != 11)
            {
                contract.AddNotification(nameof(Cpf), "CPF precisa ter 11 caracteres.");
                return;
            }

            if (FuncoesEspeciais.IsDocumentAllNumberSameValue(_value))
            {
                contract.AddNotification(nameof(Cpf), "É necessário informar um CPF válido.");
                return;
            }

            int[] multiplierOne = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplierTwo = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string aux;
            string digit;
            int sum, rest;

            var value = FuncoesEspeciais.SomenteNumero(_value);

            aux = value.Substring(0, 9);
            sum = 0;

            for (int i = 0; i < 9; i++)
                sum += int.Parse(aux[i].ToString()) * multiplierOne[i];

            rest = sum % 11;

            if (rest < 2)
                rest = 0;
            else
                rest = 11 - rest;

            digit = rest.ToString();
            aux = aux + digit;
            sum = 0;

            for (int i = 0; i < 10; i++)
                sum += int.Parse(aux[i].ToString()) * multiplierTwo[i];

            rest = sum % 11;

            if (rest < 2)
                rest = 0;
            else
                rest = 11 - rest;

            digit = digit + rest.ToString();

            if (!value.EndsWith(digit))
                contract.AddNotification(nameof(Cpf), "CPF inválido.");
        }
    }
}

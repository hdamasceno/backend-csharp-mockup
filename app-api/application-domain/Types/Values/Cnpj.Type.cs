using Biblioteca;
using Flunt.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace application_domain.Types.Values
{
    public struct Cnpj
    {
        private readonly string _value;
        public readonly Contract<Cnpj> contract;

		private Cnpj(string value)
		{
			_value = FuncoesEspeciais.SomenteNumero(value);

			contract = new Contract<Cnpj>();

			Validate();
		}

		public override string ToString() =>
			FuncoesEspeciais.Cnpj(_value, false);

		public static implicit operator Cnpj(string input) =>
			new Cnpj(input);

		private void Validate()
		{
			if (string.IsNullOrWhiteSpace(_value))
			{
				contract.AddNotification(nameof(Cnpj), "É necessário informar o CNPJ.");
				return;
			}

			if (FuncoesEspeciais.IsDocumentAllNumberSameValue(_value))
            {
				contract.AddNotification(nameof(Cnpj), "É necessário informar um CNPJ válido.");
				return;
			}

			if (FuncoesEspeciais.SomenteNumero(_value).Length != 14)
			{
				contract.AddNotification(nameof(Cnpj), "O CNPJ precisa ter 14 caracteres.");
				return;
			}

			int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
			int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
			int soma;
			int resto;
			string digito;
			string tempCnpj = FuncoesEspeciais.SomenteNumero(_value).Substring(0, 12);

			soma = 0;

			for (int i = 0; i < 12; i++)
				soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];

			resto = (soma % 11);

			if (resto < 2)
				resto = 0;
			else
				resto = 11 - resto;

			digito = resto.ToString();

			tempCnpj = tempCnpj + digito;
			soma = 0;

			for (int i = 0; i < 13; i++)
				soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];

			resto = (soma % 11);

			if (resto < 2)
				resto = 0;
			else
				resto = 11 - resto;

			digito = digito + resto.ToString();

			if (!_value.EndsWith(digito))
			{
				contract.AddNotification(nameof(Cnpj), "CNPJ Inválido.");
				return;
			}
		}	
	}
}

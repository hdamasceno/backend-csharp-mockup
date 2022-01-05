using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Biblioteca
{
	public static partial class FuncoesEspeciais
	{
		public static Boolean IsDocumentAllNumberSameValue(string documentNumber)
        {
			Boolean resultado = false;

			documentNumber = FuncoesEspeciais.SomenteNumero(documentNumber);

			if (string.IsNullOrWhiteSpace(documentNumber))
				return resultado;

            for (int i = 0; i <= 9; i++)
            {
				string numberFormated = FormataTexto(FuncoesEspeciais.ToString(i), "D", FuncoesEspeciais.ToString(i), documentNumber.Length);

				if (numberFormated == documentNumber)
                {
					resultado = true;
					break;
                }	    
            }

			return resultado;
        }

		public static bool ValidaCPF(string cpf)
		{
			if (SomenteNumero(cpf).Length != 11)
			{
				return false;
			}

			int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
			int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
			string tempCpf;
			string digito;
			int soma;
			int resto;

			cpf = cpf.Trim();
			cpf = cpf.Replace(".", "").Replace("-", "");

			if (cpf.Length != 11)
				return false;

			tempCpf = cpf.Substring(0, 9);
			soma = 0;
			for (int i = 0; i < 9; i++)
				soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

			resto = soma % 11;
			if (resto < 2)
				resto = 0;
			else
				resto = 11 - resto;

			digito = resto.ToString();

			tempCpf = tempCpf + digito;

			soma = 0;
			for (int i = 0; i < 10; i++)
				soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

			resto = soma % 11;
			if (resto < 2)
				resto = 0;
			else
				resto = 11 - resto;

			digito = digito + resto.ToString();

			return cpf.EndsWith(digito);
		}

		public static bool ValidaCNPJ(string cnpj)
		{
			if (SomenteNumero(cnpj).Length != 14)
			{
				return false;
			}

			int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
			int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
			int soma;
			int resto;
			string digito;
			string tempCnpj;

			cnpj = cnpj.Trim();
			cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "");

			if (cnpj.Length != 14)
				return false;

			tempCnpj = cnpj.Substring(0, 12);

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

			return cnpj.EndsWith(digito);
		}

		/// <summary>
		/// Converte um CNPJ em um CPF considerando os 11 últimos dígitos do CNPJ.
		/// </summary>
		/// <param name="cnpj">String contendo o CNPJ formatado com a máscara "99.999.999/9999-99".</param>
		/// <returns>String contendo o CPF formatado com a máscara "999.999.999-99".</returns>
		public static string CNPJToCPF(string cnpj)
		{
			string result = null;

			if (cnpj != null)
			{
				string cpf = SomenteNumero(cnpj);
				cpf = cpf.Substring(3, 11);
				result = String.Format("{0}.{1}.{2}-{3}",
					cpf.Substring(0, 3), cpf.Substring(3, 3), cpf.Substring(6, 3), cpf.Substring(9, 2));
			}

			return result;
		}

		/// <summary>
		/// Converte um CPF em CNPJ adicionado 3 zeros à esquerda do CPF.
		/// </summary>
		/// <param name="cpf">String contendo o CPF formatado com a máscara "999.999.999-99".</param>
		/// <returns>String contendo o CNPJ formatado com a máscara "99.999.999/9999-99".</returns>
		public static string CPFtoCNPJ(string cpf)
		{
			string result = null;

			if (cpf != null)
			{
				string cnpj = SomenteNumero(cpf);

				if (cnpj.Length == 11)
				{
					result = String.Format("00.0{0}.{1}/{2}-{3}",
						cnpj.Substring(0, 2), cnpj.Substring(2, 3), cnpj.Substring(5, 4), cnpj.Substring(9, 2));
				}
			}

			return result;
		}

		/// <summary>
		/// Formatar uma string de números com 11 algarismos em um CPF (exemplo: '073.399.226-00')
		/// </summary>
		/// <param name="num_cpf">STRING contendo os 11 algarimos que serão formatados com a máscara de formação de um CPF</param>
		/// <param name="validar">FLAG que configurará o método para permitir somente aplicar a máscara se o número informado for um CPF válido</param>
		/// <returns>STRING contendo o número formatado com a máscara de CPF</returns>
		public static string Cpf(string num_cpf, Boolean validar = true)
		{
			if (String.IsNullOrWhiteSpace(num_cpf))
			{
				return num_cpf;
			}
			else if (SomenteNumero(num_cpf).Length != 11)
			{
				return num_cpf;
			}
			else if (ValidaCPF(num_cpf) && validar == true)
			{
				num_cpf = SomenteNumero(num_cpf);

				num_cpf = num_cpf.Insert(3, ".").Insert(7, ".").Insert(11, "-");
				return num_cpf;
			}
			else if (validar == false)
			{
				num_cpf = SomenteNumero(num_cpf);

				num_cpf = num_cpf.Insert(3, ".").Insert(7, ".").Insert(11, "-");
				return num_cpf;
			}
			else
			{
				return num_cpf;
			}
		}

		/// <summary>
		/// Formatar uma string de números com 14 algarismos em um CNPJ (exemplo: '07.195.280/0001-80')
		/// </summary>
		/// <param name="num_cnpj">STRING contendo os 14 algarismos que serão formatados com a máscara de formatação de um CNPJ</param>
		/// <param name="validar">FLAG que configurará o método para permitir somente aplicar a máscara se o número informado for um CNPJ válido</param>
		/// <returns>STRING contendo o número formatado com a máscara de CNPJ</returns>
		public static string Cnpj(string num_cnpj, Boolean validar = false)
		{
			num_cnpj = SomenteNumero(num_cnpj);

			if (String.IsNullOrWhiteSpace(num_cnpj))
			{
				return num_cnpj;
			}
			else if (num_cnpj.Length != 14)
			{
				if (num_cnpj.Length >= 13)
				{
					num_cnpj = num_cnpj.Insert(2, ".").Insert(6, ".").Insert(10, "/").Insert(15, "-");
				}
				else if (num_cnpj.Length >= 10)
				{
					num_cnpj = num_cnpj.Insert(2, ".").Insert(6, ".").Insert(10, "/");
				}
				else if (num_cnpj.Length >= 6)
				{
					num_cnpj = num_cnpj.Insert(2, ".").Insert(6, ".");
				}
				else if (num_cnpj.Length >= 2)
				{
					num_cnpj = num_cnpj.Insert(2, ".");
				}

				return num_cnpj;
			}
			else if (ValidaCNPJ(num_cnpj) && validar == true)
			{
				num_cnpj = num_cnpj.Insert(2, ".").Insert(6, ".").Insert(10, "/").Insert(15, "-");
				return num_cnpj;
			}
			else if (validar == false)
			{
				num_cnpj = num_cnpj.Insert(2, ".").Insert(6, ".").Insert(10, "/").Insert(15, "-");
				return num_cnpj;
			}
			else
			{
				return "";
			}
		}

		/// <summary>
		/// Formatar uma string de números com 8 algarismos em um CEP (exemplo: '36240-000')
		/// </summary>
		/// <param name="cep">STRING contendo os 8 algarismos que serão formatados com a máscara de formatação de um CEP</param>        
		/// <returns>STRING contendo o número formatado com a máscara de CEP</returns>
		public static string CEP(string cep)
		{
			cep = SomenteNumero(cep);

			if (String.IsNullOrWhiteSpace(cep))
			{
				return cep;
			}
			else if (cep.Length != 8)
			{
				return cep;
			}
			else
			{
				cep = cep.Insert(5, "-");
				return cep;
			}
		}

		public static string Telefone(string num_tel)
		{
			num_tel = FuncoesEspeciais.SomenteNumero(num_tel);

			if (String.IsNullOrWhiteSpace(num_tel))
			{
				return num_tel;
			}
			else if (num_tel.Length < 10)
			{
				return num_tel;
			}
			else if (num_tel.Length == 11)
			{
				num_tel = num_tel.Insert(0, "(").Insert(3, ")").Insert(9, "-");
				return num_tel;
			}
			else if (num_tel.Length == 12)
			{
				num_tel = num_tel.Insert(0, "+").Insert(3, "(").Insert(6, ")").Insert(11, "-");
				return num_tel;
			}
			else
			{
				num_tel = num_tel.Insert(0, "(").Insert(3, ")").Insert(8, "-");
				return num_tel;
			}
		}

        public static string Twilio_Whatsapp_Format(string toNumber)
        {
            toNumber = FuncoesEspeciais.SomenteNumero(FuncoesEspeciais.ToString(toNumber));

            if (toNumber.Length >= 8 && toNumber.Length <= 10)
            {
                string parte1 = toNumber;
                string parte2 = toNumber;

                parte1 = parte1.Substring(0, 2) + "9";
                parte2 = parte2.Substring(2, 8);

                toNumber = parte1 + parte2;
            }
            else if (toNumber.Length == 12)
            {
                string parte1 = toNumber;
                string parte2 = toNumber;

                parte1 = parte1.Substring(0, 4) + "9";
                parte2 = parte2.Substring(4, 8);

                toNumber = parte1 + parte2;
            }

            if (toNumber.StartsWith("55", StringComparison.CurrentCulture) == false)
                toNumber = "55" + toNumber;

            return toNumber;
        }

        public static string Whatsapp_Chat_Telefone(string num_tel)
        {
            num_tel = FuncoesEspeciais.SomenteNumero(num_tel);

			if (String.IsNullOrWhiteSpace(num_tel))
			{
				return num_tel;
			}
			else if (num_tel.Length < 10)
			{
				return num_tel;
			}
			else if (num_tel.StartsWith("55", StringComparison.Ordinal))
			{
				if (num_tel.Length == 12)
				{
					num_tel = num_tel.Insert(2, " ").Insert(5, " ").Insert(10, " ");

					return num_tel;
				}
				else if (num_tel.Length == 13)
				{
					num_tel = num_tel.Insert(2, " ").Insert(5, " ").Insert(7, " ").Insert(12, " ");

					return num_tel;
				}
				else
					return num_tel;
			}
			else if (num_tel.Length == 10)
			{
				num_tel = num_tel.Insert(2, " ").Insert(7, " ");

				return num_tel;
			}
			else if (num_tel.Length == 11)
			{
				num_tel = num_tel.Insert(2, " ").Insert(4, " ").Insert(8, " ");

				return num_tel;
			}
			else
			{
				return num_tel;
			}
        }
    }
}

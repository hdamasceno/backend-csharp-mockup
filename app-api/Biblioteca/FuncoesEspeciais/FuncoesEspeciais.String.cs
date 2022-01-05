using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Biblioteca
{
	public static partial class FuncoesEspeciais
	{
		/// <summary>
		/// Validações de Códigos de Barra padrão GTIN, 8, 12, 13 e 14
		/// </summary>
		/// <param name="codigoGTIN">Cógigo GTIN 8,12,13,14</param>
		/// <returns>True se válido</returns>
		public static bool ValidaCodigoGTIN(string codigoGTIN)
		{
			if (codigoGTIN != (new Regex("[^0-9]")).Replace(codigoGTIN, string.Empty))
				return false;

			switch (codigoGTIN.Length)
			{
				case 8:
					codigoGTIN = "000000" + codigoGTIN;
					break;
				case 12:
					codigoGTIN = "00" + codigoGTIN;
					break;
				case 13:
					codigoGTIN = "0" + codigoGTIN;
					break;
				case 14:
					break;
				default:
					return false;
			}

			//Calculando dígito verificador
			int[] a = new int[13];
			a[0] = int.Parse(codigoGTIN[0].ToString()) * 3;
			a[1] = int.Parse(codigoGTIN[1].ToString());
			a[2] = int.Parse(codigoGTIN[2].ToString()) * 3;
			a[3] = int.Parse(codigoGTIN[3].ToString());
			a[4] = int.Parse(codigoGTIN[4].ToString()) * 3;
			a[5] = int.Parse(codigoGTIN[5].ToString());
			a[6] = int.Parse(codigoGTIN[6].ToString()) * 3;
			a[7] = int.Parse(codigoGTIN[7].ToString());
			a[8] = int.Parse(codigoGTIN[8].ToString()) * 3;
			a[9] = int.Parse(codigoGTIN[9].ToString());
			a[10] = int.Parse(codigoGTIN[10].ToString()) * 3;
			a[11] = int.Parse(codigoGTIN[11].ToString());
			a[12] = int.Parse(codigoGTIN[12].ToString()) * 3;
			int sum = a[0] + a[1] + a[2] + a[3] + a[4] + a[5] + a[6] + a[7] + a[8] + a[9] + a[10] + a[11] + a[12];
			int check = (10 - (sum % 10)) % 10;

			//Checando
			int last = int.Parse(codigoGTIN[13].ToString());
			return check == last;
		}

		public static string SomenteNumero(string textoEntrada, int casasDecimaisSaida = 2)
		{
			if (String.IsNullOrWhiteSpace(textoEntrada) == false)
			{
				var retorno = Regex.Replace(textoEntrada, "[^0-9]", "");

				return retorno;
			}
			else
			{
				return String.Empty;
			}
		}

		public static string UrlDecode(string texto)
        {
            return System.Web.HttpUtility.UrlDecode(texto);

        }

        public static Boolean IsNullOrWhiteSpace(string objeto)
        {
            return string.IsNullOrWhiteSpace(objeto);
        }

        public static Boolean IsFieldExist(dynamic objetoDynamic, string fieldName)
        {
            if (objetoDynamic is System.Dynamic.ExpandoObject)
                return ((IDictionary<string, object>)objetoDynamic).ContainsKey(fieldName);

            try
            {
                return objetoDynamic[fieldName] != null;
            }
            catch (KeyNotFoundException)
            {
                return false;
            }
            catch
            {
                return objetoDynamic.GetType().GetProperty(fieldName) != null;
            }
        }

        public static Guid ToGuid(string objeto)
        {
            try
            {
                return new Guid(objeto);
            }
            catch
            {
                return new Guid();
            }
        }

        public static Guid? ToGuidOrNull(object objeto)
        {
            try
            {
                return new Guid(FuncoesEspeciais.ToString(objeto));
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Retorna uma string modificada quando o caractere 'procurar' existir antes da lista 'antesDe'. 
        /// Se positivo o caractere 'procurar' será substituído por 'substituir'. Exemplo: 
        /// char[] antesDe = {'C', 'D', 'F', 'G', 'H', 'J', 'L', 'M', 'N', 'Q', 'R', 'S', 'T', 'V', 'X', 'Z', 'W', 'K', 'Y'};
        /// FEspeciais.SubstituirSeAntes("ALVES", 'L', 'U', antesDe) == "AUVES"
        /// </summary>
        /// <param name="texto">String onde será realizada a pesquisa de 'procurar'</param>
        /// <param name="procurar">Caractere que será pesquisado em 'texto'.</param>
        /// <param name="substituir">Caractere que será substituído se encontrado da lista 'antesDe'.</param>
        /// <param name="antesDe">Lista de caracteres que será avaliada em conjunto com 'procurar'.</param>
        /// <returns></returns>
        public static string SubstituirSeAntes(string texto, char procurar, char substituir, char[] antesDe)
		{
			string retorno = "";

			if (string.IsNullOrWhiteSpace(texto) == false)
			{
				string filtro = new string(antesDe);

				for (int i = 0; i < texto.Length; i++)
				{
					if (i + 1 <= texto.Length)
					{
						if (texto[i] == procurar && filtro.Contains(texto[i + 1]))
						{
							retorno += substituir;
						}
						else
						{
							retorno += texto[i];
						}
					}

				}
			}

			return retorno;
		}

		/// <summary>
		/// Retorna uma string excluindo letras duplicadas quando existirem. Exemplo: 
		/// RetiraLetrasDobradas("JFFERSON WILLIAM DE MATTOS FREITAS") == "JEFERSON WILIAM DE MATOS FREITAS"
		/// </summary>
		/// <param name="texto">String que será avaliada.</param>
		/// <returns></returns>
		public static string RetiraLetrasDobradas(string texto)
		{
			string retorno = texto;

			if (string.IsNullOrWhiteSpace(texto) == false && texto.Length >= 2)
			{
				retorno = "";
				retorno += texto[0];
				char letra = texto[0];
				const string filtro = @"0123456789 '!@#$%&*()_-+=|\<>:?/;.,´`[]{}ªº°~^";

				for (int i = 0; i < texto.Length; i++)
				{
					if (filtro.Contains(texto[i]))
					{
						retorno += texto[i];
					}
					else if (texto[i] != letra)
					{
						retorno += texto[i];
						letra = texto[i];
					}
				}
			}

			return retorno;
		}

		/// <summary>
		/// Retorna uma string fonética de 'texto'. Exemplo: LUIZ = LUIS 
		/// </summary>
		/// <param name="texto">String que será avaliada.</param>
		/// <param name="somenteLetrasNumerosEspacoEmBranco">Indica se o método irá (True) ou não (False) considerar somente letras, números e espaços em branco. True: Default</param>
		/// <returns>STRING com a fonética aplicada</returns>
		public static string Fonetica(string texto, bool somenteLetrasNumerosEspacoEmBranco = true)
		{
			string retorno = texto;

			if (texto.Length > 2)
			{
				retorno = retorno.ToUpper();
				retorno = retorno.Replace('Ç', 'S');
				retorno = retorno.ToUpper();

				if (somenteLetrasNumerosEspacoEmBranco)
				{
					const string filtro = "ABCDEFGHIJKLMNOPQRSTUVWXYZ 0123456789";
					string temp = "";

					foreach (char item in retorno)
					{
						if (filtro.Contains(item))
						{
							temp += item;
						}
					}

					if (string.IsNullOrWhiteSpace(temp) == false)
					{
						retorno = temp;
					}
				}

				retorno = retorno.Replace("PH", "F"); // Substituir PH por F. Ex: PHELIPE = FELIPE

				// Substituir C antes de E e I por S Ex: Celina = Selina
				retorno = retorno.Replace("CE", "SE");
				retorno = retorno.Replace("CI", "SI");

				retorno = retorno.Replace("SCH", "C"); // Substituir SCH por C. Trocar com X. Ex: Schimenes
				retorno = retorno.Replace("H", "");   // Excluindo H
				retorno = retorno.Replace("Z", "S");  // Substituir Z por S
				retorno = retorno.Replace("X", "C");  // Substituir X por C
				retorno = retorno.Replace("Y", "I");  // Substituir Y por I
				retorno = retorno.Replace("W", "V");  // Substituir W por V
				retorno = retorno.Replace("K", "C");  // Substituir K por C
				retorno = retorno.Replace("QU", "C"); // Substituir QU por C

				// Substituir 'irt' e 'ert' por 'iut' e 'eut' (Substituir por ilt e elt) Ex: Airton e Ailton
				retorno = retorno.Replace("IRT", "IUT");
				retorno = retorno.Replace("ERT", "EUT");

				retorno = retorno.Replace("GT", "T"); // Tirar G antes de T Ex: Welington = Weliton

				retorno += " "; // Adicionando um espaço no final para as regras a seguir

				// Retirar de, da, do, dos, das, d'
				retorno = retorno.Replace(" DE ", " ");
				retorno = retorno.Replace(" DA ", " ");
				retorno = retorno.Replace(" DO ", " ");
				retorno = retorno.Replace(" D ", " ");  // D'  Ex: D'alva
				retorno = retorno.Replace(" DOS ", " ");
				retorno = retorno.Replace(" DAS ", " ");

				retorno = retorno.Replace("N ", "M ");  // Substituir N no final por M. Ex: Miran = Miram
				retorno = retorno.Replace("GIU", "JU"); // Substituir GIU por JU Ex. Giuliano e Juliano
				retorno = retorno.Replace("GEO", "JO"); // Substituir GEO por JO Ex. George e Jorge

				// Substituir G antes de E e I por J Ex. Geferson e Jerferson
				retorno = retorno.Replace("GE", "JE");
				retorno = retorno.Replace("GI", "JI");

				// Substituir I e E no final por A Ex: Camili, Camile e Camila
				retorno = retorno.Replace("E ", "A ");
				retorno = retorno.Replace("I ", "A ");

				// Substituindo UI ou EU no início por VI ou VE. Ex: Wilson -> Vilson = Uilson
				if (retorno.Length >= 2)
				{
					if (retorno.Substring(0, 2) == "UI" || retorno.Substring(0, 2) == "UE")
					{
						retorno = "V" + retorno.Substring(1, retorno.Length);
					}
				}

				// Substituir N antes de P e B por M
				retorno = retorno.Replace("NP", "MP");
				retorno = retorno.Replace("NB", "MB");

				// Substituir M antes de consoantes diferente de P e B por N
				char[] substituirM = {
											'C', 'D', 'F', 'G', 'H', 'J', 'L', 'M', 'N', 'Q',
											'R', 'S', 'T', 'V', 'X', 'Z', 'W', 'K', 'Y'
										};
				retorno = SubstituirSeAntes(retorno, 'M', 'N', substituirM);

				// Substituir L antes de consoante, menos L, por U. Ex: Alves e Auves
				char[] substituirL = {
											'B', 'C', 'D', 'F', 'G', 'H', 'J', 'M', 'N', 'P',
											'Q', 'R', 'S', 'T', 'V', 'X', 'Z', 'W', 'K', 'Y'
										};
				retorno = SubstituirSeAntes(retorno, 'L', 'U', substituirL);

				retorno = RetiraLetrasDobradas(retorno);

				retorno = retorno.Replace("ESTELA ", "STELA ");
				retorno = retorno.Replace("VICTOR", "VITOR");

				// Substituir o resultado de CERQUEIRA (serseira)
				//         pelo resultado de CIQUEIRA  (siseira)
				retorno = retorno.Replace("SERSEIRA", "SISEIRA");

				retorno = retorno.Replace("ESTEFAN", "STEFAN"); // Substituir ESTEFAN por STEFAN. Ex: Estefany, Estefania
				retorno = retorno.Replace("ABI", "AB");         // Substituir ABI por AB. Ex: Abdo, Abidallah
				retorno = retorno.Replace("ROSE", "ROSI");      // Substituir ROSE por ROSI. Ex: Rosimere, Rosemairy
				retorno = retorno.Replace("JOSEAN", "JOSIAN");  // Substituir JOSEAN por JOSIAN. Ex: Josiane, Joseany
				retorno = retorno.Replace("LOURDES", "LURDES"); // Substituir LOURDES por LURDES

				retorno = retorno.TrimEnd();
			}

			return retorno;
		}

		/// <summary>
		/// Este método formata uma string colocando a primeira letra da string em maiusculo ou todas as primeiras letras das palavras da string
		/// </summary>
		/// <param name="textoEntrada">STRING que será formatada</param>
		/// <param name="somentePrimeiraPalavra">FLAG indicando se somente a primeira palavra será formatada ou se todas as palavras serão formatadas</param>
		/// <returns>STRING formatada de acordo com a FLAG desejada</returns>
		public static string NomePessoa(string textoEntrada, Boolean somentePrimeiraPalavra = true)
		{
			if (String.IsNullOrWhiteSpace(textoEntrada) == false)
			{
				if (somentePrimeiraPalavra)
				{
					string primeiraLetra = textoEntrada[0].ToString();
					string novoTexto = primeiraLetra.ToUpper() + textoEntrada.Substring(1, textoEntrada.Length - 1).ToLower();

					return novoTexto;
				}
				else
				{
					string novoTexto = String.Empty;

					var listaPalavras = textoEntrada.Split(' ').ToList();

					foreach (var palavra in listaPalavras)
					{
						if (String.IsNullOrWhiteSpace(palavra) == false)
						{
							string primeiraLetra = palavra[0].ToString();

							if (String.IsNullOrWhiteSpace(novoTexto))
							{
								novoTexto = primeiraLetra.ToUpper() + palavra.Substring(1, palavra.Length - 1).ToLower();
							}
							else
							{
								novoTexto += String.Format(" {0}{1}", primeiraLetra.ToUpper(), palavra.Substring(1, palavra.Length - 1).ToLower());
							}
						}
						else
						{
							novoTexto += palavra;
						}
					}

					return novoTexto;
				}
			}
			else
			{
				return textoEntrada;
			}
		}

		/// <summary>
		/// Método retorna o primeiro nome contido no texto de entrada, caso o texto de entrada seja um nome próprio
		/// </summary>
		/// <param name="textoEntrada">STRING contendo um nome próprio</param>
		/// <returns>STRING contendo o primeiro nome de um nome próprio</returns>
		public static string NomePessoaPrimeiraPalavra(string textoEntrada)
		{
			if (String.IsNullOrWhiteSpace(textoEntrada) == false)
			{
				textoEntrada = NomePessoa(textoEntrada, true);

				var palavrasSplit = textoEntrada.Split(' ').ToList();

				if (palavrasSplit.Count >= 1)
				{
					return palavrasSplit[0];
				}
				else
				{
					return textoEntrada;
				}
			}
			else
			{
				return textoEntrada;
			}
		}

		/// <summary>
		/// Método retorna o primeiro nome contido no texto de entrada, caso o texto de entrada seja um nome próprio
		/// </summary>
		/// <param name="textoEntrada">STRING contendo um nome próprio</param>
		/// <returns>STRING contendo o primeiro nome de um nome próprio</returns>
		public static string NomePessoaPrimeiraSegundaPalavra(string textoEntrada)
		{
			if (String.IsNullOrWhiteSpace(textoEntrada) == false)
			{
				textoEntrada = NomePessoa(textoEntrada, true);

				var palavrasSplit = textoEntrada.Split(' ').ToList();

				if (palavrasSplit.Count >= 2)
				{
					string textoRetorno = NomePessoa(palavrasSplit[0]) + " " + NomePessoa(palavrasSplit[1]);

					if (palavrasSplit[1].Length <= 2 && palavrasSplit.Count >= 3)
					{
						textoRetorno = NomePessoa(textoRetorno) + " " + NomePessoa(palavrasSplit[2]);
					}

					return textoRetorno;
				}
				else
				{
					return textoEntrada;
				}
			}
			else
			{
				return textoEntrada;
			}
		}

		/// <summary>
		/// Método retorna uma STRING com todos os seus caracteres em LOWERCASE
		/// </summary>
		/// <param name="textoEntrada">STRING contendo o texto a ser convertido</param>
		/// <returns>STRING com seus caracteres em LOWERCASE</returns>
		public static string TextoParaLowerCase(string textoEntrada)
		{
			if (String.IsNullOrWhiteSpace(textoEntrada) == false)
			{
				string novoTexto = String.Empty;
				var frases = textoEntrada.Split('.').ToList();

				foreach (string frase in frases)
				{
					if (String.IsNullOrWhiteSpace(frase) == false)
					{
						if (String.IsNullOrWhiteSpace(novoTexto) == false)
						{
							novoTexto = String.Format("{0} {0}{1}.", novoTexto, frase[0].ToString().ToUpper(), frase.Substring(1, frase.Length - 1).ToLower());
						}
						else
						{
							novoTexto = String.Format("{0}{1}.", frase[0].ToString().ToUpper(), frase.Substring(1, frase.Length - 1).ToLower());
						}
					}
					else
					{
						novoTexto += frase;
					}
				}

				return novoTexto;
			}
			else
			{
				return textoEntrada;
			}
		}

		/// <summary>
		/// Remove de uma string os seguintes símbolos: '.', '/', '-', '(', ')'
		/// </summary>
		/// <param name="texto">STRING que terá os símbolos removidos</param>
		/// <returns>STRING contendo os símblos devidamente removidos</returns>
		public static string RemoverFormatacao(string texto)
		{
			if (String.IsNullOrWhiteSpace(texto) == false)
			{
				texto = texto.Replace(".", "").Replace("/", "").Replace("-", "").Replace(")", "").Replace("(", "");

				return texto;
			}
			else
			{
				return texto;
			}
		}

		/// <summary>
		/// Formata uma variável do tipo Double no formato de dinheiro configurado no Windows
		/// </summary>
		/// <param name="valor">DOUBLE contendo o valor a ser formatado</param>
		/// <returns>STRING contendo o valor (double) formatado para dinheiro</returns>
		public static string Dinheiro(double valor)
		{
			return String.Format("{0:F}", valor);
		}

		/// <summary>
		/// Formata uma varável do tipo Decimal no formato de dinheiro configurado no Windows
		/// </summary>
		/// <param name="valor">DECIMAL contendo o valor a ser formatado</param>
		/// <returns>STRING contendo o valor (decimal) formatado para dinheiro</returns>
		public static string Dinheiro(decimal valor)
		{
			return String.Format("{0:F}", valor);
		}

		/// <summary>
		/// Formata o número para a quantidade de casas decimais desejada sem o símbolo de Dinheiro.
		/// </summary>
		/// <param name="valor">DECIMAL contendo o valor a ser formatado</param>
		/// <param name="casasDecimais">INT contendo o número de casas decimais desejados</param>
		/// <returns>STRING contendo o valor formatado sem o símbolo da moeda (exemplo: R$) configurado no Windows</returns>
		public static string Numerico(decimal valor, int casasDecimais = 2, Boolean truncar = true)
		{
			if (truncar)
			{
				var tempList = valor.ToString().Replace(".", "").Split(',').ToList();

				if (tempList.Count == 2)
				{
					if (tempList[1].Length >= casasDecimais)
					{
						string valorTruncado = String.Format("{0},{1}", tempList[0], tempList[1].Substring(0, casasDecimais));

						valor = Convert.ToDecimal(valorTruncado);
					}
				}
			}

			return valor.ToString(String.Format("C{0}", casasDecimais)).Replace("R$", "").Replace("$", "").Trim();
		}

		/// <summary>
		/// Formata o número para a quantidade de casas decimais desejada sem o símbolo de Dinheiro.
		/// </summary>
		/// <param name="valor">DECIMAL contendo o valor a ser formatado</param>
		/// <param name="casasDecimais">INT contendo o número de casas decimais desejados</param>
		/// <returns>STRING contendo o valor formatado sem o símbolo da moeda (exemplo: R$) configurado no Windows</returns>
		public static decimal Truncar(decimal valor, int casasDecimais = 2)
		{
			var tempList = valor.ToString().Split(',').ToList();

			if (tempList.Count == 2)
			{
				if (tempList[1].Length >= casasDecimais)
				{
					string valorTruncado = String.Format("{0},{1}", tempList[0], tempList[1].Substring(0, casasDecimais));
					valor = Convert.ToDecimal(valorTruncado);
				}
			}

			return valor;
		}

		/// <summary>
		/// Formata uma string trocando o caracter ',' pelo caracter '.'
		/// </summary>
		/// <param name="texto">STRING que terá o caracter trocado</param>
		/// <returns>STRING com o caracter ',' trocado pelo caracter '.'</returns>
		public static string ColocaPonto(string texto)
		{
			if (String.IsNullOrWhiteSpace(texto) == false)
			{
				texto = texto.Replace(".", "").Replace(",", ".");
				return texto;
			}
			else
			{
				return texto;
			}
		}

		/// <summary>
		/// Formata uma string trocando o caracter '.' pelo caracter ','
		/// </summary>
		/// <param name="texto">STRING que terá o caracter trocado</param>
		/// <returns>STRING com o caracter '.' trocado pelo caracter ','</returns>
		public static string ColocaVirgula(string texto)
		{
			if (String.IsNullOrWhiteSpace(texto) == false)
			{
				texto = texto.Replace(",", "").Replace(".", ",");
				return texto;
			}
			else
			{
				return texto;
			}
		}

		/// <summary>
		/// Transforma um texto em um Decimal. O texto não pode ter o ponto flutuante, exemplo : 00100 => 1,00
		/// </summary>
		/// <param name="texto">STRING que será convertida sem o ponto flutuante</param>
		/// <param name="casasDecimais">INT contendo a quantidade de casas decimais que o texto original possui</param>
		/// <returns>DECIMAL contendo o valor devidamente formatado</returns>
		public static decimal StringToDecimal(string texto, int casasDecimais = 2)
		{
			//texto = FuncoesEspeciais.ToString(FuncoesEspeciais.ToInt(texto));

			if (String.IsNullOrWhiteSpace(texto) == false)
			{
				if ((texto.Length - casasDecimais) == 0)
				{
					return Convert.ToDecimal("0," + texto);
				}
				else if ((texto.Length - casasDecimais) >= 1)
				{
					var primeiraParte = texto.Substring(0, texto.Length - 2);
					var segundaParte = texto.Substring(texto.Length - 2, 2);

					return Convert.ToDecimal(String.Format("{0},{1}", primeiraParte, segundaParte));
				}
				else
				{
					return 0;
				}
			}
			else
			{
				return 0;
			}
		}

		/// <summary>
		/// Transforma um texto aplicando o caracter '%' para que uma consulta SQL utilizando o comando LIKE possa ter o melhor resultado possível
		/// </summary>
		/// <param name="parametro">STRING contendo o texto que será consultado no comando SQL (LIKE)</param>
		/// <returns>STRING contendo o texto devidamente formatado</returns>
		public static string FormatarCampoPesquisaSql(string parametro)
		{
			if (String.IsNullOrWhiteSpace(parametro) == false)
			{
				if (parametro.StartsWith("%") == false)
				{
					if (parametro.EndsWith("%") == false)
					{
						return String.Format("%{0}%", parametro.Replace(" ", "%"));
					}
					else
					{
						return parametro.Replace(" ", "%");
					}
				}
				else
				{
					if (parametro.EndsWith("%") == false)
					{
						return parametro.Replace(" ", "%") + "%";
					}
					else
					{
						return parametro.Replace(" ", "%");
					}
				}
			}
			else
			{
				return parametro;
			}
		}

		/// <summary>
		/// Formata um texto centrlizando o texto de entrada no meio da string total, adicionando espaços em branco na esquerda e na direita para que a string retornada tenha o tamanho máximo informado
		/// </summary>
		/// <param name="textoEntrada">STRING contendo o texto a ser centralizado</param>
		/// <param name="tamanhoString">INT contendo o tamanho máximo da string a ser retornada</param>
		/// <returns>STRING contendo o seu texto ao centro</returns>
		public static string CentralizarTexto(string textoEntrada, int tamanhoString)
		{
			if (String.IsNullOrWhiteSpace(textoEntrada))
			{
				textoEntrada = String.Empty;
			}

			if (textoEntrada.Length < tamanhoString)
			{
				int tamanhoTextoDividido = (tamanhoString - textoEntrada.Length) / 2;
				textoEntrada = FormataTexto(" ", "E", " ", tamanhoTextoDividido) + textoEntrada + FormataTexto(" ", "D", " ", tamanhoTextoDividido);

				if (textoEntrada.Length != tamanhoString)
				{
					if (textoEntrada.Length <= tamanhoString)
					{
						return FormataTexto(textoEntrada, "D", " ", tamanhoString);
					}
					else
					{
						return textoEntrada;
					}
				}
				else
				{
					return textoEntrada;
				}
			}
			else
			{
				return textoEntrada.Substring(0, tamanhoString);
			}
		}

		/// <summary>
		/// Formata um texto aplicando um caracter desejado a esquerda (E) ou a direita (D) até que o texto contenha o tamanho desejado em caracteres
		/// </summary>
		/// <param name="textoEntrada">STRING contendo o texto a ser formatado</param>
		/// <param name="orientacao">STRING de um caracter contendo a orientação de aplicação do caracter desejado: esquerda = 'E' | direita = 'D'</param>
		/// <param name="caracter">STRING de um caracter que será aplicado ao texto original</param>
		/// <param name="tamanho">INT contendo o tamanho máximo que o texto original deverá resultar</param>
		/// <returns>STRIGN contendo o texto devidamente formatado</returns>
		public static string FormataTexto(string textoEntrada, string orientacao, string caracter, int tamanho)
		{
			if (String.IsNullOrWhiteSpace(textoEntrada))
			{
				textoEntrada = "";
			}

			string retorno = textoEntrada;

			if (textoEntrada.Length < tamanho)
			{
				string preencher = "";
				for (int i = 0; i < tamanho - textoEntrada.Length; i++)
				{
					preencher += caracter;
				}

				if (String.Compare(orientacao, "E", true) == 0)
				{
					retorno = preencher + textoEntrada;
				}
				else if (String.Compare(orientacao, "D", true) == 0)
				{
					retorno = textoEntrada + preencher;
				}
			}
			else if (textoEntrada.Length > tamanho)
			{
				retorno = textoEntrada.Substring(0, tamanho);
			}

			return retorno;
		}

		/// <summary>
		/// Formata um texto retirando todo o tipo de acentuação gráfica
		/// </summary>
		/// <param name="_texto">STRING contendo o texto que será formatado</param>
		/// <returns>STRING contendo o texto devidamente formatado</returns>
		public static string RetiraAcento(Object _texto)
		{
			string textor = "";
			string texto = _texto.ToString();

			for (int i = 0; i < texto.Length; i++)
			{
				if (String.Compare(texto[i].ToString(), "ã", true) == 0) textor += "a";
				else if (String.Compare(texto[i].ToString(), "á", true) == 0) textor += "a";
				else if (String.Compare(texto[i].ToString(), "à", true) == 0) textor += "a";
				else if (String.Compare(texto[i].ToString(), "â", true) == 0) textor += "a";
				else if (String.Compare(texto[i].ToString(), "ä", true) == 0) textor += "a";
				else if (String.Compare(texto[i].ToString(), "é", true) == 0) textor += "e";
				else if (String.Compare(texto[i].ToString(), "è", true) == 0) textor += "e";
				else if (String.Compare(texto[i].ToString(), "ê", true) == 0) textor += "e";
				else if (String.Compare(texto[i].ToString(), "ë", true) == 0) textor += "e";
				else if (String.Compare(texto[i].ToString(), "í", true) == 0) textor += "i";
				else if (String.Compare(texto[i].ToString(), "ì", true) == 0) textor += "i";
				else if (String.Compare(texto[i].ToString(), "ï", true) == 0) textor += "i";
				else if (String.Compare(texto[i].ToString(), "õ", true) == 0) textor += "o";
				else if (String.Compare(texto[i].ToString(), "ó", true) == 0) textor += "o";
				else if (String.Compare(texto[i].ToString(), "ò", true) == 0) textor += "o";
				else if (String.Compare(texto[i].ToString(), "ö", true) == 0) textor += "o";
				else if (String.Compare(texto[i].ToString(), "ú", true) == 0) textor += "u";
				else if (String.Compare(texto[i].ToString(), "ù", true) == 0) textor += "u";
				else if (String.Compare(texto[i].ToString(), "ü", true) == 0) textor += "u";
				else if (String.Compare(texto[i].ToString(), "ç", true) == 0) textor += "c";
				else if (String.Compare(texto[i].ToString(), "Ã", true) == 0) textor += "A";
				else if (String.Compare(texto[i].ToString(), "Á", true) == 0) textor += "A";
				else if (String.Compare(texto[i].ToString(), "À", true) == 0) textor += "A";
				else if (String.Compare(texto[i].ToString(), "Â", true) == 0) textor += "A";
				else if (String.Compare(texto[i].ToString(), "Ä", true) == 0) textor += "A";
				else if (String.Compare(texto[i].ToString(), "É", true) == 0) textor += "E";
				else if (String.Compare(texto[i].ToString(), "È", true) == 0) textor += "E";
				else if (String.Compare(texto[i].ToString(), "Ê", true) == 0) textor += "E";
				else if (String.Compare(texto[i].ToString(), "Ë", true) == 0) textor += "E";
				else if (String.Compare(texto[i].ToString(), "Í", true) == 0) textor += "I";
				else if (String.Compare(texto[i].ToString(), "Ì", true) == 0) textor += "I";
				else if (String.Compare(texto[i].ToString(), "Ï", true) == 0) textor += "I";
				else if (String.Compare(texto[i].ToString(), "Õ", true) == 0) textor += "O";
				else if (String.Compare(texto[i].ToString(), "Ó", true) == 0) textor += "O";
				else if (String.Compare(texto[i].ToString(), "Ò", true) == 0) textor += "O";
				else if (String.Compare(texto[i].ToString(), "Ö", true) == 0) textor += "O";
				else if (String.Compare(texto[i].ToString(), "Ú", true) == 0) textor += "U";
				else if (String.Compare(texto[i].ToString(), "Ù", true) == 0) textor += "U";
				else if (String.Compare(texto[i].ToString(), "Ü", true) == 0) textor += "U";
				else if (String.Compare(texto[i].ToString(), "Ç", true) == 0) textor += "C";

				else if (String.Compare(texto[i].ToString(), "µ", true) == 0) textor += "A"; // Á
				else if (String.Compare(texto[i].ToString(), "Æ", true) == 0) textor += "A"; // Ã
				else if (String.Compare(texto[i].ToString(), "…", true) == 0) textor += "A"; // À
				else if (String.Compare(texto[i].ToString(), "¡", true) == 0) textor += "I"; // Í
				else if (String.Compare(texto[i].ToString(), "¢", true) == 0) textor += "O"; // Ó
				else if (String.Compare(texto[i].ToString(), "‡", true) == 0) textor += "C"; // Ç 

				else textor += texto[i];
			}

			return textor;
		}

		/// <summary>
		/// Formata um texto retirando todos os caracteres que não forem NÚMEROS
		/// </summary>
		/// <param name="textoEntrada">STRING que será formatada</param>
		/// <returns>STRING contendo somente números</returns>
		public static string SomenteNumero(string textoEntrada)
		{
			if (String.IsNullOrWhiteSpace(textoEntrada) == false)
			{
				var retorno = System.Text.RegularExpressions.Regex.Replace(textoEntrada, "[^0-9]", "");
				return retorno;
			}
			else
			{
				return String.Empty;
			}
		}

		/// <summary>
		/// Formata um texto retirando todos os caracteres que não forem LETRAS
		/// </summary>
		/// <param name="textoEntrada">STRING que será formatada</param>
		/// <returns>STRING contendo somente letras</returns>
		public static string SomenteLetras(string textoEntrada)
		{
			if (String.IsNullOrWhiteSpace(textoEntrada) == false)
			{
				return Regex.Replace(textoEntrada.ToUpper(), "[^A-Z]", "");
			}
			else
			{
				return "";
			}
		}

		/// <summary>
		/// Método para retornar somente os números e as letras de uma string
		/// </summary>
		/// <param name="texto"></param>
		/// <returns></returns>
		public static string SomenteNumeros_Letras(string texto)
		{
			if (String.IsNullOrWhiteSpace(texto) == false)
			{
				return Regex.Replace(texto.ToUpper(), "[^0-9A-Z]", "");
			}
			else
			{
				return "";
			}
		}

		/// <summary>
		/// Converte um texto que foi formatado com o método SomenteNumeros(string textoEntrada) em um Decimal
		/// </summary>
		/// <param name="numero">STRING contendo o texto a ser formatado (Somente Números)</param>
		/// <param name="casasDecimais">INT contendo a quantidade de casas decimais do número</param>
		/// <returns>DECIMAL contendo o valor devidamente formatado</returns>
		public static decimal SomenteNumeroParaDecimal(string numero, int casasDecimais = 2)
		{
			//numero = FuncoesEspeciais.ToString(FuncoesEspeciais.ToInt(numero));

			if (String.IsNullOrWhiteSpace(numero) == false)
			{
				if (numero.Length > 2)
				{
					numero = String.Format("{0}.{1}", numero.Substring(0, numero.Length - casasDecimais), numero.Substring(numero.Length - casasDecimais, casasDecimais));

					//return Convert.ToDecimal(numero);

					return FuncoesEspeciais.ToDecimal(numero, true);
				}
				else if (numero.Length == 2)
				{
					//return Convert.ToDecimal("0." + numero);

					return FuncoesEspeciais.ToDecimal("0." + numero, true);
				}
				else if (numero.Length == 1)
				{
					//return Convert.ToDecimal("0.0" + numero);

					return FuncoesEspeciais.ToDecimal("0.0" + numero, true);
				}
				else
				{
					return 0;
				}
			}
			else
			{
				return 0;
			}
		}

		public static decimal StringToDecimal_Marcio(string texto, int casasDecimais = 2)
		{
			//texto = FuncoesEspeciais.ToString(FuncoesEspeciais.ToInt(texto));

			if (String.IsNullOrWhiteSpace(texto) == false)
			{
				if ((texto.Length - casasDecimais) == 0)
				{
					return Convert.ToDecimal("0." + texto);
				}
				else if ((texto.Length - casasDecimais) >= 1)
				{
					var primeiraParte = texto.Substring(0, texto.Length - 2);
					var segundaParte = texto.Substring(texto.Length - 2, 2);

					return Convert.ToDecimal(primeiraParte + "." + segundaParte);
				}
				else
				{
					return 0;
				}
			}
			else
			{
				return 0;
			}
		}

		public static decimal SomenteNumeroParaDecimal(string numero, int casasDecimais = 2, Boolean forceToBrazil = true)
		{
			if (String.IsNullOrWhiteSpace(numero) == false)
			{
				if (numero.Length > 2)
				{
					if (forceToBrazil)
					{
						decimal weeklyWage;

						numero = String.Format("{0},{1}", numero.Substring(0, numero.Length - casasDecimais), numero.Substring(numero.Length - casasDecimais, casasDecimais));

						decimal.TryParse(FuncoesEspeciais.ToString(numero),
							NumberStyles.Currency | NumberStyles.AllowCurrencySymbol,
							new NumberFormatInfo()
							{
								NumberDecimalSeparator = ",",
								CurrencySymbol = "",
								CurrencyDecimalDigits = 2
							},
							out weeklyWage);

						return weeklyWage;
					} 
					else
					{
						numero = String.Format("{0}.{1}", numero.Substring(0, numero.Length - casasDecimais), numero.Substring(numero.Length - casasDecimais, casasDecimais));

						return Convert.ToDecimal(numero);
					}
				}
				else if (numero.Length == 2)
				{
					return Convert.ToDecimal("0." + numero);
				}
				else if (numero.Length == 1)
				{
					return Convert.ToDecimal("0.0" + numero);
				}
				else
				{
					return 0;
				}
			}
			else
			{
				return 0;
			}
		}

		/// <summary>
		/// Converte um texto que foi formatado com o método SomenteNumeros(string textoEntrada) em um DateTime
		/// </summary>
		/// <param name="data">STRING contendo o texto a ser formatado (Somente Números)</param>
		/// <param name="formatoString">STRIGN contendo o formato do texto para com o tipo DateTime (exemplo: DDMMYYYY)</param>
		/// <returns>DateTime contendo o valor devidamente formatado</returns>
		public static DateTime SomenteNumeroParaDateTime(string data, string formatoString = "DDMMYYYY")
		{
			if (String.Compare(formatoString, "DDMMYY", false) == 0)
			{
				return Convert.ToDateTime(String.Format("{0}/{1}/{2}", data.Substring(0, 2), data.Substring(2, 2), data.Substring(4, 2)));
			}
			else if (String.Compare(formatoString, "DDMMYYYY", false) == 0)
			{
				return Convert.ToDateTime(String.Format("{0}/{1}/{2}", data.Substring(0, 2), data.Substring(2, 2), data.Substring(4, 4)));
			}
			else if (String.Compare(formatoString, "YYMMDD", false) == 0)
			{
				return Convert.ToDateTime(String.Format("{0}/{1}/{2}", data.Substring(0, 2), data.Substring(2, 2), data.Substring(4, 2)));
			}
			else if (String.Compare(formatoString, "YYYYMMDD", false) == 0)
			{
				return Convert.ToDateTime(String.Format("{0}/{1}/{2}", data.Substring(0, 4), data.Substring(4, 2), data.Substring(6, 2)));
			}
			else
			{
				return DateTime.Today;
			}
		}

		public static string StringListToString(List<string> objetoLista, string separadorDeLinha)
		{
			string texto = string.Empty;

			foreach (var linha in objetoLista)
			{
				texto += linha + separadorDeLinha;
			}

			return texto;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using MongoDB.Bson;

namespace Biblioteca
{
    public static partial class FuncoesEspeciais
    {
        public static string NewHexStringId()
        {
            var newMongoId = Convert.ToBase64String(MongoDB.Bson.GuidConverter.ToBytes(Guid.NewGuid(), GuidRepresentation.CSharpLegacy));

            return newMongoId;
        }

        public static string ImpressaoNumericaNFe(string valorString, int casasDecimais = 2)
        {
            if (valorString == null)
                valorString = "0";
            else if (valorString.Trim() == string.Empty || valorString == "")
                valorString = "0";

            valorString = valorString.Replace(",", "");
            valorString = valorString.Replace(".", ",");

            double valor = 0;
            double.TryParse(valorString, out valor);

            var tempList = valor.ToString().Split(',').ToList();

            if (tempList.Count == 2)
            {
                if (tempList[1].Length >= casasDecimais)
                {
                    string valorTruncado = String.Format("{0},{1}", tempList[0], tempList[1].Substring(0, casasDecimais));
                    valor = Convert.ToDouble(valorTruncado);
                }
            }

            return valor.ToString(String.Format("C{0}", casasDecimais)).Replace("R$", "").Trim();
        }

        public static Guid ToGuid(object id)
        {
            try
            {
                Guid objeto = new Guid(FuncoesEspeciais.ToString(id));

                return objeto;
            }
            catch
            {
                return new Guid();
            }
        }

        public static string ToString(object objeto, Boolean toUpper = true, Boolean toLower = false, Boolean trim = true)
        {
            try
            {
                if (objeto != null)
                {
                    string texto = objeto.ToString();

                    if (String.IsNullOrWhiteSpace(texto) == false)
                    {
                        if (toUpper)
                            texto = texto.ToUpper();
                        else if (toLower)
                            texto = texto.ToLower();
                        else
                            texto = texto.Trim();

                        if (trim)
                            texto = texto.Trim();

                        return texto;
                    }
                    else
                        return texto;
                }
                else
                {
                    return String.Empty;
                }
            }
            catch
            {
                return String.Empty;
            }
        }

        public static decimal ToDecimal(object objeto, Boolean forceToBrazil = false)
        {
            try
            {
                if (forceToBrazil)
                {
                    decimal weeklyWage;

                    decimal.TryParse(FuncoesEspeciais.ToString(objeto),
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
                else if (objeto != null)
                {
                    decimal _valor = 0;

                    Decimal.TryParse(objeto.ToString(), out _valor);

                    return _valor;
                }
                else
                {
                    return 0;
                }
            }
            catch
            {
                return 0;
            }
        }

        public static decimal ToDecimal_NFe(string valor, int casasDecimais = 2, Boolean truncar = true)
        {
            if (string.IsNullOrWhiteSpace(valor))
                return 0;

            decimal weeklyWage;

            decimal.TryParse(valor, NumberStyles.Currency | NumberStyles.AllowCurrencySymbol, new NumberFormatInfo() { NumberDecimalSeparator = ".", CurrencySymbol = "", CurrencyDecimalDigits = casasDecimais }, out weeklyWage);

            return weeklyWage;
        }

        public static DateTime SomenteNumeroToDateTime(string texto)
        {
            if (texto != null)
            {
                if (texto.Length != 0)
                {
                    if (texto.Length == 6)
                    {
                        var lixo = String.Format("{0}/{1}/{2}", texto.Substring(0, 2), texto.Substring(2, 2), texto.Substring(4, 2));
                        return Convert.ToDateTime(lixo);
                    }
                    else if (texto.Length == 8)
                    {
                        var lixo = String.Format("{0}/{1}/{2}", texto.Substring(0, 2), texto.Substring(2, 2), texto.Substring(4, 4));
                        return Convert.ToDateTime(lixo);
                    }
                    else
                    {
                        var lixo = String.Format("{0}/{1}/{2}", texto.Substring(0, 2), texto.Substring(2, 2), texto.Substring(4, 2));
                        return Convert.ToDateTime(lixo);
                    }
                }
                else
                {
                    return new DateTime();
                }
            }
            else
            {
                return new DateTime();
            }
        }

        public static decimal SomenteNumeroToDecimal(string textoSomenteNumero)
        {
            if (textoSomenteNumero != null)
            {
                if (textoSomenteNumero.Length != 0)
                {
                    if (textoSomenteNumero.Length >= 2)
                    {
                        var lixo = String.Format("{0},{1}", textoSomenteNumero.Substring(0, textoSomenteNumero.Length - 2), textoSomenteNumero.Substring(textoSomenteNumero.Length - 2, 2));
                        return Convert.ToDecimal(lixo);
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
            else
            {
                return 0;
            }
        }

        public static int ToInt(object objeto)
        {
            try
            {
                if (objeto != null)
                {
                    int _valor = 0;
                    Int32.TryParse(objeto.ToString(), out _valor);
                    return _valor;
                }
                else
                {
                    return 0;
                }
            }
            catch
            {
                return 0;
            }
        }

        public static DateTime ToDateTime(object objeto, Boolean somenteData = false, Boolean somenteHora = false, Boolean padraoBrasil = false)
        {
            try
            {
                if (objeto != null)
                {
                    string strData = objeto.ToString();

                    if (padraoBrasil == true)
                    {
                        if (FuncoesEspeciais.SomenteNumero(strData).Length == 6)
                            strData = strData.Substring(0, 2) + "/" + strData.Substring(2, 2) + "/" + strData.Substring(4, 2);

                        CultureInfo MyCultureInfo = new CultureInfo("pt-BR");

                        DateTime MyDateTime = DateTime.Today;

                        try
                        {
                            MyDateTime = DateTime.Parse(strData, MyCultureInfo);

                            return MyDateTime;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message.Contains("was not recognized as a valid DateTime"))
                            {
                                MyCultureInfo = new CultureInfo("en-US");

                                MyDateTime = DateTime.Parse(strData, MyCultureInfo);

                                MyCultureInfo = new CultureInfo("pt-BR");

                                MyDateTime = DateTime.Parse(MyDateTime.ToString("dd/MM/yyyy HH:mm:ss"), MyCultureInfo);
                            }
                        }

                        if (somenteData)
                        {
                            var dataTemp = new DateTime(MyDateTime.Year, MyDateTime.Month, MyDateTime.Day);

                            MyDateTime = dataTemp;
                        }

                        if (somenteHora)
                        {
                            var dataTemp = new DateTime(MyDateTime.Year, MyDateTime.Month, MyDateTime.Day, MyDateTime.Hour, MyDateTime.Minute, MyDateTime.Second);

                            MyDateTime = dataTemp;
                        }

                        return MyDateTime;
                    }
                    else
                    {
                        if (strData.Length == 6)
                            strData = strData.Substring(0, 2) + "/" + strData.Substring(2, 2) + "/" + strData.Substring(4, 2);

                        var data = new DateTime();

                        DateTime.TryParse(strData, out data);

                        if (somenteData)
                        {
                            var dataTemp = new DateTime(data.Year, data.Month, data.Day);

                            data = dataTemp;
                        }
                        else if (somenteHora)
                        {
                            var dataTemp = new DateTime(data.Year, data.Month, data.Day, data.Hour, data.Minute, data.Second);

                            data = dataTemp;
                        }

                        return data;
                    }
                }
                else
                {
                    return new DateTime();
                }
            }
            catch
            {
                return new DateTime();
            }
        }

        public static DateTime? ToDateTimeNull(object objeto, Boolean somenteData = false, Boolean somenteHora = false, Boolean padraoBrasil = false)
        {
            try
            {
                if (objeto != null)
                {
                    if (padraoBrasil == true)
                    {
                        string strData = objeto.ToString();

                        if (FuncoesEspeciais.SomenteNumero(strData).Length == 6)
                            strData = strData.Substring(0, 2) + "/" + strData.Substring(2, 2) + "/" + strData.Substring(4, 2);

                        CultureInfo MyCultureInfo = new CultureInfo("pt-BR");

                        DateTime MyDateTime = new DateTime();

                        try
                        {
                            MyDateTime = DateTime.Parse(strData, MyCultureInfo);
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message.Contains("was not recognized as a valid DateTime"))
                            {
                                MyCultureInfo = new CultureInfo("en-US");

                                MyDateTime = DateTime.Parse(strData, MyCultureInfo);

                                MyCultureInfo = new CultureInfo("pt-BR");

                                MyDateTime = DateTime.Parse(MyDateTime.ToString("dd/MM/yyyy HH:mm:ss"), MyCultureInfo);
                            }
                        }

                        if (somenteData)
                        {
                            var dataTemp = new DateTime(MyDateTime.Year, MyDateTime.Month, MyDateTime.Day);

                            MyDateTime = dataTemp;
                        }

                        if (somenteHora)
                        {
                            var dataTemp = new DateTime(MyDateTime.Year, MyDateTime.Month, MyDateTime.Day, MyDateTime.Hour, MyDateTime.Minute, MyDateTime.Second);

                            MyDateTime = dataTemp;
                        }

                        return MyDateTime;
                    }
                    else
                    {
                        if (somenteData)
                        {
                            string strData = objeto.ToString();

                            DateTime MyDateTime = DateTime.Parse(strData);

                            var dataTemp = new DateTime(MyDateTime.Year, MyDateTime.Month, MyDateTime.Day);

                            return dataTemp;

                        }
                        else if (somenteHora)
                        {
                            string strData = objeto.ToString();

                            DateTime MyDateTime = DateTime.Parse(strData);

                            var dataTemp = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, MyDateTime.Hour, MyDateTime.Minute, MyDateTime.Second);

                            return dataTemp;
                        }
                        else
                        {
                            string strData = objeto.ToString();

                            DateTime MyDateTime = DateTime.Parse(strData);

                            return MyDateTime;
                        }
                    }
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        public static double ToDouble(object objetoCampoRegistro)
        {
            double valor = 0;

            if (objetoCampoRegistro != null)
            {
                Double.TryParse(objetoCampoRegistro.ToString(), out valor);
            }

            return valor;
        }

        public static double ToDouble(object objetoCampoRegistro, int nroCasasDecimais = 0)
        {
            if (objetoCampoRegistro == null)
            {
                return 0;
            }
            else
            {
                string texto = objetoCampoRegistro.ToString();

                texto = texto.Replace(".", ",");
                texto = getValorComUmaVirgula(texto);

                double valor = ToDouble(texto);

                if (valor != 0 && nroCasasDecimais > 0)
                {
                    double fator = 4 / Math.Pow(10, nroCasasDecimais + 1);
                    valor += fator;
                    string auxiliar = NumericoDouble(valor, nroCasasDecimais, true);
                    valor = ToDouble(auxiliar);
                }

                return valor;
            }
        }

        private static string getValorComUmaVirgula(string valorEntrada)
        {
            int contadorVirgula = 0;
            string valorSaida = String.Empty;

            for (int i = 0; i <= valorEntrada.Length - 1; i++)
            {
                if (valorEntrada[i].ToString() == ",")
                {
                    contadorVirgula += 1;
                }
            }

            if (contadorVirgula > 1)
            {
                for (int i = 0; i <= valorEntrada.Length - 1; i++)
                {
                    if (valorEntrada[i].ToString() == ",")
                    {
                        if (contadorVirgula > 1)
                        {
                            contadorVirgula -= 1;
                        }
                        else
                        {
                            valorSaida += valorEntrada[i].ToString();
                        }
                    }
                    else
                    {
                        valorSaida += valorEntrada[i].ToString();
                    }
                }
            }
            else
            {
                valorSaida = valorEntrada;
            }

            return valorSaida;
        }

        public static string NumericoDouble(double valor, int casasDecimais = 2, Boolean truncar = true)
        {
            if (truncar)
            {
                var tempList = valor.ToString().Split(',').ToList();

                if (tempList.Count == 2)
                {
                    if (tempList[1].Length >= casasDecimais)
                    {
                        string valorTruncado = String.Format("{0},{1}", tempList[0], tempList[1].Substring(0, casasDecimais));
                        valor = Convert.ToDouble(valorTruncado);
                    }
                }
            }

            return valor.ToString(String.Format("C{0}", casasDecimais)).Replace("R$", "").Trim();
        }

        public static string NumericoNFe(decimal valor, int casasDecimais = 2, Boolean truncar = true)
        {
            string strRetorno = Numerico(valor, casasDecimais, truncar);

            if (String.IsNullOrWhiteSpace(strRetorno) == false)
            {
                strRetorno = strRetorno.Replace(".", "").Replace(",", ".");
            }

            return strRetorno;
        }

        public static string NumericoNFe(double valor, int casasDecimais = 2, Boolean truncar = true)
        {
            string strRetorno = NumericoDouble(valor, casasDecimais, truncar);

            if (String.IsNullOrWhiteSpace(strRetorno) == false)
            {
                strRetorno = strRetorno.Replace(".", "").Replace(",", ".");
            }

            return strRetorno;
        }

        public static bool ehAlfanumerico(string texto)
        {
            bool retorno = false;

            if (!String.IsNullOrWhiteSpace(texto))
            {
                foreach (Char caracter in texto)
                {
                    if (Char.IsLetter(caracter))
                    {
                        return true;
                    }
                }
            }

            return retorno;
        }

        public static string AlfanumericoToNumerico(string texto, bool converterDigitos = true)
        {
            string retorno = String.Empty;

            if (String.IsNullOrWhiteSpace(texto))
                return retorno;

            const string digitos = ",0,1,2,3,4,5,6,7,8,9,";

            foreach (Char caractere in texto)
            {
                if (converterDigitos)
                {
                    int numero = (int)caractere;
                    retorno += numero.ToString();
                }
                else
                {
                    if (digitos.Contains(String.Format(",{0},", caractere)))
                        retorno += caractere.ToString();
                    else
                    {
                        int numero = (int)caractere;
                        retorno += numero.ToString();
                    }
                }
            }

            return retorno;
        }

        public static string RemoverAcentos(string texto)
        {
            if (String.IsNullOrWhiteSpace(texto) == false)
            {
                StringBuilder sbReturn = new StringBuilder();
                var arrayText = texto.Normalize(NormalizationForm.FormD).ToCharArray();

                foreach (char letter in arrayText)
                {
                    if (CharUnicodeInfo.GetUnicodeCategory(letter) != UnicodeCategory.NonSpacingMark)
                        sbReturn.Append(letter);
                }

                return sbReturn.ToString().Replace("'", "").Replace("''", "");
            }
            else
            {
                return texto;
            }
        }

        public static decimal ToPercentual(decimal valorBruto, decimal valorLiquido)
        {
            if (valorBruto == 0)
                return 0;

            if (valorLiquido == 0)
                return 0;

            var valor = (valorLiquido * 100) / valorBruto;

            return valor;
        }

        public static decimal ToPercentual_CalcularDesconto(decimal valorBruto, decimal valorLiquido)
        {
            if (valorBruto == 0)
                return 0;

            if (valorLiquido == 0)
                return 0;

            var valor = (valorLiquido * 100) / valorBruto;

            valor = 100 - valor;

            return valor;
        }
    }
}

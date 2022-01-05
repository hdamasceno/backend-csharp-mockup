using System;
using System.Linq;
using System.Collections.ObjectModel;

namespace Biblioteca
{
    public static partial class FuncoesEspeciais
    {
        private static bool isValidTimeZoneId(string timeZoneId)
        {
            if (string.IsNullOrEmpty(timeZoneId) || string.IsNullOrWhiteSpace(timeZoneId))
                return false;

            ReadOnlyCollection<TimeZoneInfo> tz;
            tz = TimeZoneInfo.GetSystemTimeZones();

            return tz.Any(x => x.Id == timeZoneId);
        }

		//private static string TIME_ZONE_US = "America/New_York";
        private static string TIME_ZONE_BR = "E. South America Standard Time";
		public static System.DateTime ConverteDataAzureBrasil(this System.DateTime dt)
		{
			try
			{
                string timeZone = TIME_ZONE_BR;                

				System.TimeZoneInfo tz = System.TimeZoneInfo.FindSystemTimeZoneById(timeZone);

				if (tz != null)
				{
					System.TimeZoneInfo tzSource = TimeZoneInfo.Utc;
					if (dt.Kind == DateTimeKind.Local)
					{
						tzSource = TimeZoneInfo.Local;
					}
					System.DateTime dataAjustada = TimeZoneInfo.ConvertTime(dt, tzSource, tz);
					return dataAjustada;
				}
				else
				{
					throw new Exception();
				}

			}
			catch
			{
                return dt;
			}
		}

        public static Boolean Sintegra_ValidateData(object data)
        {
            try
            {
                Convert.ToDateTime(data);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static string Sintegra_FormatToDateTimeBR(object data)
        {
            if (data != null)
            {
                if (Sintegra_ValidateData(data))
                {
                    var dtRetorno = Convert.ToDateTime(data);

                    return dtRetorno.ToString("dd/MM/yyyy");
                }
                else
                {
                    return DateTime.Today.Date.ToString("dd/MM/yyyy");
                }
            }
            else
            {
                return null;
            }
        }

        public static object ChangeType(object value, Type conversionType, Boolean toUpper = true, Boolean toLower = false, Boolean trim = true)
        {
            if (conversionType == null)
                throw new Exception("ConversionType Null ou Inválido.");
            else
            {
                if (conversionType == typeof(Guid))
                    return FuncoesEspeciais.ToGuid(value);
                else if (conversionType == typeof(Guid?))
                    return FuncoesEspeciais.ToGuidOrNull(value);
                else if (conversionType == typeof(DateTime))
                    return FuncoesEspeciais.ToDateTime(value, false, false, true);
                else if (conversionType == typeof(DateTime?))
                    return FuncoesEspeciais.ToDateTimeNull(value, false, false, true);
                else if (conversionType == typeof(string))
                    return FuncoesEspeciais.ToString(value, toUpper, toLower, trim);
                else if (conversionType == typeof(decimal))
                    return FuncoesEspeciais.ToDecimal(FuncoesEspeciais.ToString(value, toUpper, toLower, trim), true);
                else if (conversionType == typeof(Boolean))
                {
                    if (value == null)
                        return null;

                    string _temp = FuncoesEspeciais.ToString(value, toUpper, toLower, trim);

                    if (string.Compare(_temp, "sim", true) == 0)
                        return true;
                    else if (string.Compare(_temp, "nao", true) == 0)
                        return false;
                    else if (string.Compare(_temp, "s", true) == 0)
                        return true;
                    else if (string.Compare(_temp, "n", true) == 0)
                        return false;
                    else if (string.Compare(_temp, "y", true) == 0)
                        return true;
                    else if (string.Compare(_temp, "n", true) == 0)
                        return false;
                    else if (string.Compare(_temp, "true", true) == 0)
                        return true;
                    else if (string.Compare(_temp, "false", true) == 0)
                        return false;
                    else if (string.Compare(_temp, "1", true) == 0)
                        return true;
                    else if (string.Compare(_temp, "0", true) == 0)
                        return false;
                    else
                    {
                        System.ComponentModel.NullableConverter nullableConverter = new System.ComponentModel.NullableConverter(conversionType);

                        conversionType = nullableConverter.UnderlyingType;
                    }
                }

                if (conversionType.IsGenericType && conversionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                {
                    if (value == null)
                        return null;

                    System.ComponentModel.NullableConverter nullableConverter = new System.ComponentModel.NullableConverter(conversionType);

                    conversionType = nullableConverter.UnderlyingType;
                }

                return Convert.ChangeType(value, conversionType);
            }
        }
    }
}
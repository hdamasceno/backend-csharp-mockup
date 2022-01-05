using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca
{
	public static partial class FuncoesEspeciais
	{
		public static DateTime DateTime_ToFirstHour(object objetoData, Boolean isDateBR)
        {
			var _objetoData = FuncoesEspeciais.ToDateTime(objetoData, false, false, isDateBR);

			return new DateTime(_objetoData.Year, _objetoData.Month, _objetoData.Day, 0, 0, 0);
        }

		public static DateTime DateTime_ToLastHour(object objetoData, Boolean isDateBR)
		{
			var _objetoData = FuncoesEspeciais.ToDateTime(objetoData, false, false, isDateBR);

			return new DateTime(_objetoData.Year, _objetoData.Month, _objetoData.Day, 23, 59, 59);
		}

		public static DateTime DateTime_ToDateBR(object dataDesejada)
        {
            var _temp = FuncoesEspeciais.ToDateTime(dataDesejada, true, false, true);

			return _temp;
        }

        public static DateTime DateTime_ToDateTimeBR(object dataDesejada)
        {
            var _temp = FuncoesEspeciais.ToDateTime(dataDesejada, false, false, true);

			_temp = new DateTime(_temp.Year, _temp.Month, _temp.Day, 23, 59, 59);

			return _temp;
        }

        public static Boolean Data_EhFeriadoNacional(DateTime dataAtual)
		{
			string[] feriados = { "01/01", "21/04", "01/05", "07/09", "12/10", "02/11", "15/11", "25/12" };

			string diaMes = String.Format("{0}/{1}", FormataTexto(dataAtual.Day.ToString(), "E", "0", 2), FormataTexto(dataAtual.Month.ToString(), "E", "0", 2));

			if (feriados.Contains(diaMes))
				return true;
			else
				return false;
		}

		public static int Data_DiferencaEntreDatasEmDias(DateTime dataInicial, 
			DateTime dataFinal)
		{
			TimeSpan diferenca = dataFinal - dataInicial;
			return diferenca.Days;
		}

		public static double Hora_DiferencaEntreHorasEmHoras(DateTime dataHoraInicial, 
			DateTime dataHoraFinal)
		{

			TimeSpan ts = dataHoraFinal - dataHoraInicial;
			return ts.TotalHours;
		}

		public static double Hora_DiferencaEntreHorasEmMinutos(DateTime dataHoraInicial, 
			DateTime dataHoraFinal)
		{

			TimeSpan ts = dataHoraFinal - dataHoraInicial;
			return ts.TotalMinutes;
		}

		public static DateTime Data_DiaSemana_ProximaData(DateTime dataAtual, 
			DayOfWeek diaSemana)
		{
			DateTime proximaData = dataAtual.AddDays(1);

			while (proximaData.DayOfWeek != diaSemana)
			{
				proximaData = proximaData.AddDays(1);
			}

			return proximaData;
		}
        
        public static int Data_DiaSemana_NumeroDiaSemana(DateTime dataDesejada)
        {
            int diaNumero = 1;

            string diaSemanaNome = dataDesejada.DayOfWeek.ToString();

            if (string.Compare(diaSemanaNome, "synday", true) == 0)
                diaNumero = 1;
            else if (string.Compare(diaSemanaNome, "monday", true) == 0)
                diaNumero = 2;
            else if (string.Compare(diaSemanaNome, "tuesday", true) == 0)
                diaNumero = 3;
            else if (string.Compare(diaSemanaNome, "wednesday", true) == 0)
                diaNumero = 4;
            else if (string.Compare(diaSemanaNome, "thursday", true) == 0)
                diaNumero = 5;
            else if (string.Compare(diaSemanaNome, "friday", true) == 0)
                diaNumero = 6;
            else if (string.Compare(diaSemanaNome, "saturday", true) == 0)
                diaNumero = 7;

            return diaNumero;
        }
		
		public static DateTime DateTime_PrimeiroDia(DateTime objetoData)
		{
			return new DateTime(objetoData.Year, objetoData.Month, 1);
		}
		
		public static DateTime DateTime_UltimoDia(DateTime objetoData)
		{
			return new DateTime(objetoData.Year, objetoData.Month, DateTime.DaysInMonth(objetoData.Year, objetoData.Month));
		}
		
		public static DateTime DateTime_UltimoDiaMes(DateTime objetoData)
		{
			int ultimoDia = Convert.ToInt32(DateTime_UltimoDia(objetoData).Day);

			DateTime retorno = new DateTime(objetoData.Year, objetoData.Month, ultimoDia);

			return retorno;
		}
	}
}
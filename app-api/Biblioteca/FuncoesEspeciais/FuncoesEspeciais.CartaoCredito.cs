using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Biblioteca 
{
    public static partial class FuncoesEspeciais
    {
        private static bool CartaoCredito_IsValidLuhnn(string val)
        {

            int currentDigit;
            int valSum = 0;
            int currentProcNum = 0;

            for (int i = val.Length - 1; i >= 0; i--)
            {
                //parse to int the current rightmost digit, if fail return false (not-valid id)
                if (!int.TryParse(val.Substring(i, 1), out currentDigit))
                    return false;

                currentProcNum = currentDigit << (1 + i & 1);
                //summarize the processed digits
                valSum += (currentProcNum > 9 ? currentProcNum - 9 : currentProcNum);

            }

            // if digits sum is exactly divisible by 10, return true (valid), else false (not-valid)
            // valSum must be greater than zero to avoid validate 0000000...00 value
            return (valSum > 0 && valSum % 10 == 0);
        }

        public static bool CartaoCredito_EhValido(string cartaoNumero)
        {
            if (string.IsNullOrWhiteSpace(cartaoNumero))
                return false;

            // rule #1, must be only numbers
            if (cartaoNumero.All(Char.IsDigit) == false)
            {
                return false;
            }
            // rule #2, must have at least 12 and max of 19 digits
            if (12 > cartaoNumero.Length || cartaoNumero.Length > 19)
            {
                return false;
            }
            // rule #3, must pass Luhnn Algorithm
            return CartaoCredito_IsValidLuhnn(cartaoNumero);
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlClient;
using System.Linq;
using Biblioteca;

namespace Biblioteca
{
    public static partial class FuncoesEspeciais
    {
        public static decimal CalculatePercentOfSequence(decimal numeroTotal, decimal numeroAtual)
        {
            if (numeroAtual > numeroTotal)
                return -1;

            decimal percentualIs = (numeroAtual * 100) / numeroTotal;

            return percentualIs;
        }
    }
}

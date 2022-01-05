using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlClient;
using System.Linq;

namespace Biblioteca
{
    public partial class FuncoesEspeciais
    {
        public static List<Exception> Exception_InnerException_Update(Exception ex, List<Exception> objetoLista)
        {
            if (objetoLista == null)
                objetoLista = new List<Exception>();

            objetoLista.Add(ex);

            if (ex.InnerException != null)
                return Exception_InnerException_Update(ex.InnerException, objetoLista);
            else
                return objetoLista;
        }
    }
}

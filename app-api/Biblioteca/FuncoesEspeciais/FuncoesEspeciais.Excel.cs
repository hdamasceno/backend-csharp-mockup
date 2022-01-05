using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Biblioteca
{
    public static partial class FuncoesEspeciais
    {
        public static void Excel_LerArquivo(string filePath)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                
            }
        }
    }
}

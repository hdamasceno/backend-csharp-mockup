using MailKit;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Compression;
using System.IO;

namespace Biblioteca
{
    public partial class FuncoesEspeciais
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="arquivosList">STRING LIST contendo o caminho completo com caminho e nome dos arquivos a serem compactados</param>
        /// <param name="caminhoNomeArquivoZip">STRING contendo o caminho e nome do arquivo ZIP a ser criado</param>
        public static void ZIP_Compactar_IO(List<string> arquivosList, string caminhoNomeArquivoZip)
        {
            try
            {
                File.Delete(caminhoNomeArquivoZip);
            }
            catch { }

            ZipArchive zip = ZipFile.Open(caminhoNomeArquivoZip, ZipArchiveMode.Create);

            foreach (string arquivo in arquivosList)
            {
                zip.CreateEntryFromFile(arquivo, FuncoesEspeciais.Arquivo_RetornaNomeArquivo(arquivo));
            }

            zip.Dispose();
        }
    }
}

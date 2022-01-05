using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace Biblioteca
{
    public static partial class FuncoesEspeciais
    {

        public static void Arquivo_Compactar(string caminhoNomeArquivo, string caminhoNomeArquivoZip)
        {
            using (FileStream zipToOpen = new FileStream(caminhoNomeArquivoZip, FileMode.OpenOrCreate))
            {
                using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
                {
                    ZipArchiveEntry readmeEntry = archive.CreateEntry(caminhoNomeArquivo);

                    using (StreamWriter writer = new StreamWriter(readmeEntry.Open()))
                    {
                        // Interagir com o arquivo se necessário
                    }
                }
            }
        }

        public static void Arquivo_Compactar(List<string> caminhoNomeArquivoList, string caminhoNomeArquivoZip)
        {
            using (FileStream zipToOpen = new FileStream(caminhoNomeArquivoZip, FileMode.OpenOrCreate))
            {
                using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
                {
                    foreach (var caminhoNomeArquivo in caminhoNomeArquivoList)
                    {
                        ZipArchiveEntry readmeEntry = archive.CreateEntry(caminhoNomeArquivo);

                        using (StreamWriter writer = new StreamWriter(readmeEntry.Open()))
                        {
                            // Interagir com o arquivo se necessário
                        }
                    }
                }
            }
        }

        public static Boolean Arquivo_Descompactar(string caminhoArquivoZIP, string nomeArquivoZIP, string caminhoArquivo, string nomeArquivo)
        {
            ZipFile.CreateFromDirectory(caminhoArquivoZIP, caminhoArquivo);

            ZipFile.ExtractToDirectory(caminhoArquivoZIP + nomeArquivoZIP, caminhoArquivo + nomeArquivo);

            if (File.Exists(caminhoArquivo + nomeArquivo))
                return true;
            else
                return false;
        }

        public static void Arquivo_Descompactar(string filePath, string destinationPath)
        {
            if (Directory.Exists(destinationPath) == false)
                Directory.CreateDirectory(destinationPath);

            ZipFile.ExtractToDirectory(filePath, destinationPath);
        }
    }
}

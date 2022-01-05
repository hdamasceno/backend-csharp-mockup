using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca
{
    public static partial class FuncoesEspeciais
    {
        public static string Arquivo_TrocarExtensaoArquivo(string nomeArquivo, string extensao)
        {
            var nomeSplit = nomeArquivo.Split('.').ToList();

            if (nomeSplit.Count >= 2)
            {
                string resultado = "";

                for (int i = 0; i < nomeSplit.Count - 1; i++)
                {
                    resultado += nomeSplit[i];
                }

                resultado += "." + extensao.Replace(".", "");

                return resultado;
            }
            else
            {
                return nomeArquivo;
            }
        }

        private static string Arquivo_RetornaNomeSemExtensao(string caminhoNomeArquivo)
        {
            var nomeArquivoCompleto = FuncoesEspeciais.Arquivo_RetornaNomeArquivo(caminhoNomeArquivo);
            List<string> nomeSplit = new List<string>();

            if (String.IsNullOrWhiteSpace(nomeArquivoCompleto) == false)
            {
                nomeSplit = nomeArquivoCompleto.Split('.').ToList();
            }
            else
            {
                nomeSplit = caminhoNomeArquivo.Split('.').ToList();
            }

            if (nomeSplit.Count != 0)
            {
                return nomeSplit[0];
            }
            else
            {
                return String.Empty;
            }
        }

        private static string Arquivo_ColocarBarraFinalCaminho(string caminho)
        {
            if (String.IsNullOrWhiteSpace(caminho) == false)
            {
                if (caminho.EndsWith(@"\") == false)
                {
                    return caminho + @"\";
                }
                else
                {
                    return caminho;
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Método responsável por contar todos os arquivos com o mesmo nome de um diretório (Usado para contar as partes de arquivos zipados pela Função FZip.Compactar())
        /// </summary>
        /// <param name="caminho">STRING contendo o caminho do diretório desejado</param>
        /// <param name="nomeArquivo">STRING contendo o nome do arquivo</param>
        /// <returns>Quantidade de arquivos no diretório</returns>
        public static int Arquivo_ContarArquivosDiretorio(string caminho, string nomeArquivo)
        {
            int quantidade = 0;
            var _nomeArquivo = nomeArquivo.Split('.').ToList()[0];

            var listaArquivos = Arquivo_ListarArquivos(caminho, "*");

            foreach (var arquivo in listaArquivos)
            {
                var _nomeTemp = arquivo.Split('.').ToList()[0];

                if (String.Compare(_nomeArquivo, _nomeTemp, true) == 0)
                {
                    quantidade += 1;
                }
            }

            return quantidade;
        }

        /// <summary>
        /// Método responsável por listar todos os arquivos de um diretório
        /// </summary>
        /// <param name="caminho">Caminho a ser analisado</param>
        /// <param name="extensao">Extensão a ser verificada. Exemplo: txt</param>
        /// <returns>Lista{string} contendo os nomes dos arquivos encontrados no diretório</returns>
        public static List<string> Arquivo_ListarArquivos(string caminho, string extensao)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(caminho) == false)
                {
                    if (caminho.StartsWith("/") && caminho.EndsWith(@"/") == false)
                        caminho += @"/";
                    else if (caminho.StartsWith("/") == false && caminho.EndsWith(@"\") == false)
                        caminho += @"\";

                    var lista = new List<string>();

                    var dir = new DirectoryInfo(@caminho);
                    FileInfo[] texto = dir.GetFiles("*." + extensao);

                    foreach (var item in texto)
                    {
                        lista.Add(item.Name);
                    }

                    return lista;
                }
                else
                {
                    return new List<string>();
                }
            }
            catch (Exception)
            {
                throw new Exception("Não foi possível verificar se existem arquivos neste diretório!");
            }
        }

        public static List<string> Arquivo_ListarArquivos(string caminho)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(caminho) == false)
                {
                    if (caminho.EndsWith(@"\") == false)
                        caminho += @"\";

                    var lista = new List<string>();

                    var dir = new DirectoryInfo(@caminho);
                    FileInfo[] texto = dir.GetFiles();

                    foreach (var item in texto)
                    {
                        lista.Add(item.Name);
                    }

                    return lista;
                }
                else
                {
                    return new List<string>();
                }
            }
            catch (Exception)
            {
                throw new Exception("Não foi possível verificar se existem arquivos neste diretório!");
            }
        }

        /// <summary>
        /// Método responsável por retornar a extensão de um arquivo
        /// </summary>
        /// <param name="nomeArquivo">Nome do arquivo</param>
        /// <returns>STRING contendo a extensão do arquivo</returns>
        public static string Arquivo_RetornaExtensaoArquivo(string nomeArquivo)
        {
            var nomeSplit = nomeArquivo.Split('.').ToList();

            if (nomeSplit.Count >= 2)
            {
                string resultado = nomeSplit[nomeSplit.Count - 1];
                return resultado;
            }
            else
            {
                return nomeArquivo;
            }
        }

        /// <summary>
        /// Método responsável por retornar o diretório de um arquivo, retirando o nome do arquivo da STRING
        /// </summary>
        /// <param name="caminhoNomeArquivo">Caminho completo do arquivo</param>
        /// <returns>STRING contendo somente o diretório do arquivo</returns>
        public static string Arquivo_RetornaCaminhoArquivo(string caminhoNomeArquivo)
        {
            if (caminhoNomeArquivo.Split('\\').ToList().Count >= 2)
            {
                string retorno = "";

                for (int i = 0; i < caminhoNomeArquivo.Split('\\').ToList().Count - 1; i++)
                {
                    retorno = String.Format(@"{0}{1}\", retorno, caminhoNomeArquivo.Split('\\').ToList()[i]);
                }

                return retorno;
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Método responsável por retornar somente o nome do arquivo a partir do caminho completo do arquivo
        /// </summary>
        /// <param name="caminhoNomeArquivo">Caminho completo do arquivo</param>
        /// <returns>STRING contendo somente o nome do arquivo</returns>
        public static string Arquivo_RetornaNomeArquivo(string caminhoNomeArquivo)
        {
            var argumentos = caminhoNomeArquivo.Split('\\').ToList();

            if (argumentos.Count >= 2)
            {
                string nomeArquivo = argumentos[argumentos.Count - 1];

                argumentos = nomeArquivo.Split('/').ToList();

                if (argumentos.Count >= 2)
                    nomeArquivo = argumentos[argumentos.Count - 1];

                return nomeArquivo;
            }
            else
            {
                return "";
            }
        }

        public static void Arquivo_DiretorioCriar(string diretorio)
        {
            if (Directory.Exists(diretorio) == false)
            {
                Directory.CreateDirectory(diretorio);
            }
        }

        public static DateTime Arquivo_DataHoraUltimaEscritaArquivo(string caminhoNomeArquivo)
        {
            try
            {
                return Directory.GetLastWriteTime(caminhoNomeArquivo);
            }
            catch (Exception)
            {
                return DateTime.Now.ConverteDataAzureBrasil();
            }
        }

        public static decimal Arquivo_RetornaTamanhoArquivo(string caminhoNomeArquivo)
        {
            try
            {
                string caminhoArquivo = Arquivo_RetornaCaminhoArquivo(caminhoNomeArquivo);
                string nomeArquivo = Arquivo_RetornaNomeArquivo(caminhoNomeArquivo);

                if (Directory.Exists(caminhoArquivo))
                {
                    var diretorio = new DirectoryInfo(caminhoArquivo);
                    decimal tamanho = 0;

                    foreach (var arquivo in diretorio.GetFiles())
                    {
                        if (arquivo.Name.ToUpper() == nomeArquivo.ToUpper())
                        {
                            tamanho = arquivo.Length;
                        }
                    }

                    return tamanho;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static string Arquivo_MD5Arquivo(string pathSrc)
        {
            String md5Result;
            var sb = new StringBuilder();
            var md5Hasher = MD5.Create();

            using (var fs = File.OpenRead(pathSrc))
            {
                foreach (Byte b in md5Hasher.ComputeHash(fs))
                    sb.Append(b.ToString("x2").ToLower());
            }

            if (sb != null)
            {
                md5Result = sb.ToString().ToUpper().Trim();
            }
            else
            {
                md5Result = String.Empty;
            }

            return md5Result;
        }

        public static Boolean Arquivo_Existe(string caminhoArquivo, string nomeArquivo)
        {
            return Arquivo_Existe(caminhoArquivo + nomeArquivo);
        }

        public static Boolean Arquivo_Existe(string caminhoNomeArquivo)
        {
            return File.Exists(caminhoNomeArquivo);
        }

        public static byte[] Arquivo_LerArquivoBinario(string caminhoNomeArquivo)
        {
            byte[] buffer;
            FileStream fileStream = new FileStream(caminhoNomeArquivo, FileMode.Open, FileAccess.Read);

            try
            {
                int length = (int)fileStream.Length;  // get file length
                buffer = new byte[length];            // create buffer
                int count;                            // actual number of bytes read
                int sum = 0;                          // total number of bytes read

                // read until Read method returns 0 (end of the stream has been reached)
                while ((count = fileStream.Read(buffer, sum, length - sum)) > 0)
                    sum += count;  // sum is a buffer offset for next reading
            }
            finally
            {
                fileStream.Dispose();
            }

            return buffer;
        }

        public static List<string> Arquivo_LerArquivo(string caminhoArquivoNomeArquivo)
        {
            string pathArquivo = caminhoArquivoNomeArquivo;

            var lista = new List<string>();

            if (Arquivo_Existe(caminhoArquivoNomeArquivo))
            {
                using (var stream = new FileStream(pathArquivo, FileMode.Open))
                {
                    using (var leitor = new StreamReader(stream))
                    {
                        try
                        {
                            if (!File.Exists(pathArquivo))
                                throw new Exception("Arquivo não existe no caminho informado !");
                            else
                            {
                                while (leitor.Peek() >= 0)
                                {
                                    lista.Add(leitor.ReadLine());
                                }
                            }
                        }
                        finally
                        {
                            leitor.Dispose();
                        }
                    }
                }
            }

            return lista;
        }

        public static string Arquivo_LerArquivo_EmString(string caminhoArquivoNomeArquivo)
        {
            var objetoArquivo = Arquivo_LerArquivo(caminhoArquivoNomeArquivo);

            string texto = string.Empty;

            foreach (var linha in objetoArquivo)
            {
                texto += linha + Environment.NewLine;
            }

            return texto;
        }

        /// <summary>
        /// Método responsável por ler o conteúdo de um arquivo e disponibilizar em uma Lista[string]
        /// </summary>
        /// <param name="caminhoArquivo">Caminho do arquivo</param>
        /// <param name="nomeArquivo">Nome do arquivo com extensão</param>
        /// <returns>Lista{string} com o conteúdo do arquivo</returns>
        public static List<string> Arquivo_LerArquivo(string caminhoArquivo, string nomeArquivo)
        {
            string pathArquivo = @caminhoArquivo + nomeArquivo;
            var lista = new List<string>();

            using (var stream = new FileStream(pathArquivo, FileMode.Open))
            {
                using (var leitor = new StreamReader(stream))
                {
                    try
                    {
                        if (File.Exists(pathArquivo) == false)
                            throw new Exception("Arquivo não existe no caminho informado !");
                        else
                        {
                            while (leitor.Peek() >= 0)
                            {
                                lista.Add(leitor.ReadLine());
                            }
                        }
                    }
                    finally
                    {
                        leitor.Dispose();
                    }
                }
            }

            return lista;
        }

        public static List<string> Arquivo_ListarDiretorios(string caminho)
        {
            if (String.IsNullOrWhiteSpace(caminho) == false)
            {
                if (caminho.EndsWith(@"\") == false)
                    caminho += @"\";

                var lista = new List<string>();

                var dir = new DirectoryInfo(@caminho);
                DirectoryInfo[] subDiretorios = dir.GetDirectories();

                foreach (var item in subDiretorios)
                {
                    lista.Add(item.Name);
                }

                return lista;
            }
            else
            {
                return new List<string>();
            }
        }

        public static List<string> Arquivo_ListarArquivos_DiretorioMaisNomeArquivo(string caminho, string extensao)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(caminho) == false)
                {
                    if (caminho.EndsWith(@"\") == false)
                        caminho += @"\";

                    var lista = new List<string>();

                    var dir = new DirectoryInfo(@caminho);
                    FileInfo[] texto = dir.GetFiles("*." + extensao);

                    foreach (var item in texto)
                    {
                        lista.Add(caminho + item.Name);
                    }

                    var listaSubDiretorios = Arquivo_ListarDiretorios(caminho);
                    foreach (var subDiretorio in listaSubDiretorios)
                    {
                        var listaArquivosSubDiretorio = Arquivo_ListarArquivos_DiretorioMaisNomeArquivo(caminho + subDiretorio, extensao);

                        foreach (var item in listaArquivosSubDiretorio)
                        {
                            lista.Add(item);
                        }
                    }

                    return lista;
                }
                else
                {
                    return new List<string>();
                }
            }
            catch (Exception)
            {
                throw new Exception("Não foi possível verificar se existem arquivos neste diretório!");
            }
        }

        public static void Arquivo_Salvar(string caminhoArquivo, string nomeArquivo, List<string> conteudo)
        {
            Arquivo_DiretorioCriar(caminhoArquivo);

            if (caminhoArquivo.EndsWith(@"\") == false)
                caminhoArquivo += @"\";

            string pathArquivo = caminhoArquivo + nomeArquivo;

            if (File.Exists(pathArquivo))
            {
                File.Delete(pathArquivo);
            }

            using (var stream = new FileStream(pathArquivo, FileMode.OpenOrCreate))
            {
                using (var gravarTexto = new StreamWriter(stream, Encoding.GetEncoding("ISO-8859-1")))
                {
                    foreach (string item in conteudo)
                    {
                        try
                        {
                            gravarTexto.WriteLine(item);
                        }
                        catch (Exception)
                        {
                            throw new Exception("Erro ao salvar arquivo: " + nomeArquivo);
                        }
                    }
                }
            }
        }

        public static void Arquivo_Salvar(string caminhoNomeArquivo, List<string> conteudo)
        {
            string pathArquivo = caminhoNomeArquivo;

            if (File.Exists(pathArquivo))
            {
                File.Delete(pathArquivo);
            }

            using (var stream = new FileStream(pathArquivo, FileMode.OpenOrCreate))
            {
                using (var gravarTexto = new StreamWriter(stream, Encoding.GetEncoding("ISO-8859-1")))
                {
                    foreach (string item in conteudo)
                    {
                        try
                        {
                            gravarTexto.WriteLine(item);
                        }
                        catch (Exception)
                        {
                            throw new Exception("Erro ao salvar arquivo: " + caminhoNomeArquivo);
                        }
                    }
                }
            }
        }

        public static async void Arquivo_Salvar_Async(string caminhoNomeArquivo, List<string> conteudo)
        {
            string pathArquivo = caminhoNomeArquivo;

            if (File.Exists(pathArquivo))
            {
                File.Delete(pathArquivo);
            }

            using (var stream = new FileStream(pathArquivo, FileMode.OpenOrCreate))
            {
                using (var gravarTexto = new StreamWriter(stream, Encoding.GetEncoding("ISO-8859-1")))
                {
                    foreach (string item in conteudo)
                    {
                        try
                        {
                            await gravarTexto.WriteLineAsync(item);
                        }
                        catch (Exception)
                        {
                            throw new Exception("Erro ao salvar arquivo: " + caminhoNomeArquivo);
                        }
                    }
                }
            }
        }

        public static string Arquivo_Salvar_FromIFormFile_ToTempDir(IFormFile file, string folderName, string nomeArquivoComExtensao)
        {
            string tempDir = Path.Combine(Path.GetTempPath(), folderName);

            if (Directory.Exists(tempDir) == false)
            {
                Directory.CreateDirectory(tempDir);
            }

            string tempFile = Path.Combine(tempDir, nomeArquivoComExtensao);
            string filePath = tempFile;

            if (File.Exists(filePath))
            {
                try
                {
                    File.Delete(filePath);
                }
                catch { }
            }

            using (var stream = System.IO.File.Create(filePath))
            {
                file.CopyTo(stream);
            }

            return filePath;
        }

        public static string Arquivo_Salvar_DownloadFromUrl_ToApiFunctions(string url, string arquivoExtensao)
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "functions");

            if (Directory.Exists(tempDir) == false)
            {
                Directory.CreateDirectory(tempDir);
            }

            string tempFile = Path.Combine(tempDir, Guid.NewGuid() + "." + arquivoExtensao);
            string filePath = tempFile;

            if (File.Exists(filePath))
            {
                try
                {
                    File.Delete(filePath);
                }
                catch { }
            }

            using (WebClient objetoWebClient = new WebClient())
            {
                objetoWebClient.DownloadFile(new Uri(url, UriKind.Absolute), filePath);
            }

            if (File.Exists(filePath))
                return filePath;
            else
                return null;
        }

        public static string Arquivo_Salvar_ToTempDir_FromStringList(List<string> objetoArquivo, string nomeArquivoComExtensao)
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "temp");

            if (Directory.Exists(tempDir) == false)
            {
                Directory.CreateDirectory(tempDir);
            }

            string tempFile = Path.Combine(tempDir, nomeArquivoComExtensao);
            string filePath = tempFile;

            if (File.Exists(filePath))
            {
                try
                {
                    File.Delete(filePath);
                }
                catch { }
            }

            FuncoesEspeciais.Arquivo_Salvar(filePath, objetoArquivo);

            return filePath;
        }
    }
}

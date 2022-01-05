using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Authentication;
using System.Text;
using FluentFTP;

namespace Biblioteca
{
    public static partial class FuncoesEspeciais
    {
        public static void FTP_Native_UploadFile(string ftp,
            string userName,
            string password,
            string caminhoArquivo,
            string nomeArquivo,
            string ftpCaminhoUpload,
            string ftpNomeArquivo)
        {
            if (ftpCaminhoUpload.EndsWith(@"/", StringComparison.CurrentCulture) == false)
                ftpCaminhoUpload += @"/";

            if (ftp.StartsWith("ftp://", StringComparison.CurrentCulture) == false)
                ftp = @"ftp://" + ftp;

            if (ftp.EndsWith(@"/", StringComparison.CurrentCulture) == false)
                ftp += @"/";

            // Get the object used to communicate with the server.
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftp + ftpCaminhoUpload + ftpNomeArquivo);
            request.Method = WebRequestMethods.Ftp.UploadFile;

            // This example assumes the FTP site uses anonymous logon.
            request.Credentials = new NetworkCredential(userName, password);

            // Copy the contents of the file to the request stream.
            byte[] fileContents;
            using (StreamReader sourceStream = new StreamReader(caminhoArquivo + nomeArquivo))
            {
                fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
            }

            request.ContentLength = fileContents.Length;

            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(fileContents, 0, fileContents.Length);
            }

            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            {
                var objetoT = new
                {
                    responseStatus = response.StatusDescription
                };                
            }
        }

        /// <summary>
        /// Upload de arquivo para servidor FTP
        /// </summary>
        /// <returns>The upload file.</returns>
        /// <param name="url">189.12.1.1</param>
        /// <param name="filePathLocal">Exemplo: c:\arquivos\teste.txt</param>
        /// <param name="filePathFTP">Exemplo: /ped/arquivo.txt</param>
        /// <param name="username">Username.</param>
        /// <param name="password">Password.</param>
        public static void FTP_UploadFile(string url, string nomeArquivo, string caminhoArquivoLocal, string caminhoArquivoFTP, string username, string password)
        {
            FtpClient client = new FtpClient(url);
            client.Credentials = new NetworkCredential(username, password);
            client.Connect();

            if (client.IsConnected == false)
                throw new Exception("Não foi possível conectar o servidor FTP.");
            else
            {
                client.RetryAttempts = 3;

                string nomeTemp = FuncoesEspeciais.Arquivo_RetornaNomeSemExtensao(nomeArquivo) + ".tmp";

                client.UploadFile(caminhoArquivoLocal + nomeTemp, caminhoArquivoFTP + nomeTemp, FtpRemoteExists.Overwrite, false, FtpVerify.Retry);

                var objetoRespostaArquivoExiste = FTP_Core_ArquivoExiste(url, username, password, caminhoArquivoFTP, nomeTemp);

                client.Rename(caminhoArquivoFTP + nomeTemp, caminhoArquivoFTP + nomeArquivo);

                client.Disconnect();                
            }
        }

        public static void FTP_RenameFile(string url, string username, string password, string nomeArquivoAnterior, string nomeArquivoPosterior, string caminhoArquivoFTP)
        {
            FtpClient client = new FtpClient(url);

            client.Credentials = new NetworkCredential(username, password);
            client.Connect();

            if (client.IsConnected == false)
                throw new Exception("Não foi possível conectar o servidor FTP.");
            else
            {
                client.Rename(caminhoArquivoFTP + nomeArquivoAnterior, caminhoArquivoFTP + nomeArquivoPosterior);

                client.Disconnect();
            }
        }

        public static void FTP_Download(string ip, string usuario, string senha, string caminhoNomeArquivoLocal, string caminhoNomeArquivoFTP)
        {
            FtpClient client = new FtpClient(ip);
            client.Credentials = new NetworkCredential(usuario, senha);
            client.Connect();

            if (client.IsConnected == false)
                throw new Exception("Não foi possível conectar o servidor FTP.");
            else
            {
                client.RetryAttempts = 3;
                //client.DownloadFile(caminhoNomeArquivoLocal, caminhoNomeArquivoFTP, true, FtpVerify.Retry);

                Stream istream = client.OpenRead(caminhoNomeArquivoFTP);
                SaveStreamToFile(caminhoNomeArquivoLocal, istream);

                client.Disconnect();
            }
        }

        public static List<object> FTP_ListaArquivos(string ip, string usuario, string senha, string caminhoFTP, string extensao = null)
        {
            FtpClient client = new FtpClient(ip);
            client.Credentials = new NetworkCredential(usuario, senha);
            client.Connect();

            if (client.IsConnected == false)
                throw new Exception("Não foi possível conectar o servidor FTP.");
            else
            {
                var objetoLista = new List<object>();

                foreach (FtpListItem item in client.GetListing(caminhoFTP))
                {
                    if (item.Type == FtpFileSystemObjectType.File)
                    {
                        long size = client.GetFileSize(item.FullName);
                        DateTime time = client.GetModifiedTime(item.FullName);

                        dynamic objeto = new
                        {
                            arquivoNome = item.FullName,
                            arquivoTamanho = size,
                            arquivoDataHoraUltimaEscrita = time.ToString("dd/MM/yyyy HH:mm:ss")
                        };

                        objetoLista.Add(objeto);
                    }
                }

                client.Disconnect();

                return objetoLista;
            }
        }

        public static Boolean FPT_ArquivoExiste(string ip, string usuario, string senha, string caminhoFTP, string arquivoNome)
        {
            FtpClient client = new FtpClient(ip);

            client.Credentials = new NetworkCredential(usuario, senha);
            client.Connect();

            if (client.IsConnected == false)
                throw new Exception("Não foi possível conectar no servidor FTP.");
            else
            {
                var objetoArquivos = client.GetListing(caminhoFTP, FtpListOption.ForceNameList);

                Boolean resposta = false;

                foreach (FtpListItem item in objetoArquivos)
                {
                    if (item.Type == FtpFileSystemObjectType.File)
                    {
                        string _nomeArquivo = FuncoesEspeciais.Arquivo_RetornaNomeArquivo(item.Name);

                        if (string.Compare(_nomeArquivo, arquivoNome, true) == 0)
                        {
                            resposta = true;
                            break;
                        }
                    }
                }

                client.Disconnect();

                return resposta;
            }
        }

        private static void SaveStreamToFile(string fileFullPath, Stream stream)
        {
            if (stream.Length == 0) return;

            // Create a FileStream object to write a stream to a file
            using (FileStream fileStream = System.IO.File.Create(fileFullPath, (int)stream.Length))
            {
                // Fill the bytes[] array with the stream data
                byte[] bytesInStream = new byte[stream.Length];
                stream.Read(bytesInStream, 0, (int)bytesInStream.Length);

                // Use FileStream object to write to the specified file
                fileStream.Write(bytesInStream, 0, bytesInStream.Length);
            }
        }
    }
}

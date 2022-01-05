using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using CoreFtp;
using CoreFtp.Infrastructure;

namespace Biblioteca 
{
    public static partial class FuncoesEspeciais
    {
        public static async Task FTP_Core_Download(string host, string usuario, string senha, string nomeArquivo, string caminhoArquivoLocal, string caminhoArquivoFTP)
        {
			var credentials = new FtpClientConfiguration
			{
				Host = host,
				Username = usuario,
				Password = senha
            };

            using (var ftpClient = new FtpClient(credentials))
			{
                var tempFile = new FileInfo(caminhoArquivoLocal + nomeArquivo);
                await ftpClient.LoginAsync();

                using (var ftpReadStream = await ftpClient.OpenFileReadStreamAsync(caminhoArquivoFTP + nomeArquivo))
				{
					using (var fileWriteStream = tempFile.OpenWrite())
					{
						await ftpReadStream.CopyToAsync(fileWriteStream);
					}
				}
			}
        }

        public static async Task FTP_Core_Upload(string host, string usuario, string senha, string nomeArquivo, string caminhoArquivoLocal, string caminhoArquivoFTP)
        {
            var credentials = new FtpClientConfiguration
            {
                Host = host,
                Username = usuario,
                Password = senha
            };

            using (var ftpClient = new FtpClient(credentials))
            {
                var fileinfo = new FileInfo(caminhoArquivoLocal + nomeArquivo);
                await ftpClient.LoginAsync();

                ftpClient.ChangeWorkingDirectoryAsync(caminhoArquivoFTP).Wait();

                using (var writeStream = await ftpClient.OpenFileWriteStreamAsync(nomeArquivo))
                {
                    var fileReadStream = fileinfo.OpenRead();
                    await fileReadStream.CopyToAsync(writeStream);
                }
            }
        }

		public static async Task FTP_Core_Upload(string host, string usuario, string senha, string nomeArquivo, string nomeArquivoTemp, string caminhoArquivoLocal, string caminhoArquivoFTP)
		{
            var credentials = new FtpClientConfiguration
            {
                Host = host,
                Username = usuario,
                Password = senha
            };

            using (var ftpClient = new FtpClient(credentials))
            {
                var fileinfo = new FileInfo(caminhoArquivoLocal + nomeArquivo);
                await ftpClient.LoginAsync();

                ftpClient.ChangeWorkingDirectoryAsync(caminhoArquivoFTP).Wait();

                using (var writeStream = await ftpClient.OpenFileWriteStreamAsync(nomeArquivoTemp))
                {
                    var fileReadStream = fileinfo.OpenRead();
                    await fileReadStream.CopyToAsync(writeStream);
                }

                ftpClient.RenameAsync(nomeArquivoTemp, nomeArquivo).Wait();                
            }
        }

        public static async Task FTP_Core_RenameFile(string host, string usuario, string senha, string nomeArquivoTemp, string nomeArquivo, string caminhoArquivoFTP)
		{
            var credentials = new FtpClientConfiguration
            {
                Host = host,
                Username = usuario,
                Password = senha
            };

            using (var ftpClient = new FtpClient(credentials))
            {
                await ftpClient.LoginAsync();

                ftpClient.ChangeWorkingDirectoryAsync(caminhoArquivoFTP).Wait();
                ftpClient.RenameAsync(nomeArquivoTemp, nomeArquivo).Wait();
            }
        }

        public static async Task FTP_Core_DeleteFile(string host, string usuario, string senha, string caminhoArquivoFTP, string nomeArquivo)
        {
            var credentials = new FtpClientConfiguration
            {
                Host = host,
                Username = usuario,
                Password = senha
            };

            using (var ftpClient = new FtpClient(credentials))
            {
                await ftpClient.LoginAsync();

                ftpClient.ChangeWorkingDirectoryAsync(caminhoArquivoFTP).Wait();
                ftpClient.DeleteFileAsync(nomeArquivo).Wait();
            }
        }

        public static async Task<List<string>> FTP_Core_ListarArquivos_SomenteNomeArquivo(string ip, string usuario, string senha, string caminhoFTP, string extensao = null)
		{
			var credentials = new FtpClientConfiguration
			{
				Host = ip,
				Username = usuario,
				Password = senha
			};

			using (var ftpClient = new FtpClient(credentials))
			{
                await ftpClient.LoginAsync();
                ftpClient.ChangeWorkingDirectoryAsync(caminhoFTP).Wait();

                var objetoArquivos = await ftpClient.ListAllAsync();
                var objetoLista = new List<string>();

                foreach(FtpNodeInformation item in objetoArquivos)
                {
                    if (item != null)
                        objetoLista.Add(item.Name);
                }

                return objetoLista;
			}
		}

        public static async Task<Boolean> FTP_Core_ArquivoExiste(string ip, string usuario, string senha, string caminhoFTP, string nomeArquivo)
		{
            var credentials = new FtpClientConfiguration
            {
                Host = ip,
                Username = usuario,
                Password = senha
            };

            using (var ftpClient = new FtpClient(credentials))
            {
                await ftpClient.LoginAsync();
                ftpClient.ChangeWorkingDirectoryAsync(caminhoFTP).Wait();

                var objetoArquivos = await ftpClient.ListAllAsync();

                Boolean encontrou = false;

                foreach (FtpNodeInformation item in objetoArquivos)
                {
                    if (string.Compare(item.Name, nomeArquivo, true) == 0)
                    {                        
                        encontrou = true;
                        break;
                    }
                }

                return encontrou;
            }
        }
    }
}

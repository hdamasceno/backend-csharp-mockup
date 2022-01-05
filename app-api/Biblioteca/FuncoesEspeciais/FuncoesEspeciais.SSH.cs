using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlClient;
using System.Linq;
using Biblioteca;
using Renci.SshNet;

namespace Biblioteca
{
    public static partial class FuncoesEspeciais
    {
        public static int SSH_UploadFile(string host, string hostFolder, string userName, string password, string fileName, string fileNameDestino)
        {
            var connectionInfo = new ConnectionInfo(host, 4018, userName, new PasswordAuthenticationMethod(userName, password));

            using (var sftp = new SftpClient(connectionInfo))
            {
                sftp.Connect();
                sftp.ChangeDirectory(hostFolder);

                using (var uplfileStream = System.IO.File.OpenRead(fileName))
                {
                    sftp.UploadFile(uplfileStream, fileNameDestino, true);
                }

                sftp.Disconnect();
            }

            return 0;
        }
    }
}
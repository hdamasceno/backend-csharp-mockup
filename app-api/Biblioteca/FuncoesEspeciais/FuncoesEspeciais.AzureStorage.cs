using System;
using System.Collections.Generic;
using Microsoft.WindowsAzure; // Namespace for CloudConfigurationManager
using Microsoft.WindowsAzure.Storage; // Namespace for CloudStorageAccount
using Microsoft.WindowsAzure.Storage.Blob; // Namespace for Blob storage types

namespace Biblioteca 
{
    public static partial class FuncoesEspeciais
    {
        public static string AzureStorage_File_Upload(string connectionString, 
            string containerName, 
            string blobReferenceName, 
            string caminhoNomeArquivo)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new Exception("StorageKey não informado.");

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            CloudBlobContainer container = blobClient.GetContainerReference(containerName);

            container.CreateIfNotExistsAsync();

            container.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

            CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobReferenceName);

            blockBlob.UploadFromFileAsync(caminhoNomeArquivo);

            return blockBlob.Uri.ToString();
        }

        public static void AzureStorage_File_Download(string connectionString, string containerName, string blobReferenceName, string caminhoNomeArquivoDestino)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new Exception("StorageKey não informado.");

            // Retrieve storage account from connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);

            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve reference to a previously created container.
            CloudBlobContainer container = blobClient.GetContainerReference(containerName);

            // Retrieve reference to a blob named "photo1.jpg".
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobReferenceName);

            // Save blob contents to a file.
            using (var fileStream = System.IO.File.OpenWrite(caminhoNomeArquivoDestino))
            {
                blockBlob.DownloadToStreamAsync(fileStream);
            }
        }

        public static void AzureStorage_File_Delete(string connectionString, string containerName, string blobReferenceName)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new Exception("StorageKey não informado.");

            // Retrieve storage account from connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);

            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve reference to a previously created container.
            CloudBlobContainer container = blobClient.GetContainerReference(containerName);

            // Retrieve reference to a blob named "myblob.txt".
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobReferenceName);

            // Delete the blob.
            blockBlob.DeleteAsync();
        }

        public static List<object> AzureStorage_File_Listar(string connectionString, string containerName, string blobReferenceName)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new Exception("StorageKey não informado.");

            // Retrieve storage account from connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);

            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve reference to a previously created container.
            CloudBlobContainer container = blobClient.GetContainerReference(containerName);

            var continuationToken = new BlobContinuationToken();
            var objetoListaItems = container.ListBlobsSegmentedAsync(continuationToken).Result;
            var objetoLista = new List<object>();

            // Loop over items within the container and output the length and URI.
            foreach (IListBlobItem item in objetoListaItems.Results)
            {
                if (item.GetType() == typeof(CloudBlockBlob))
                {
                    CloudBlockBlob blob = (CloudBlockBlob)item;

                    //Console.WriteLine("Block blob of length {0}: {1}", blob.Properties.Length, blob.Uri);

                    var objeto = new
                    {
                        tamanho = blob.Properties.Length,
                        uri = blob.Uri
                    };

                    objetoLista.Add(objeto);
                }
                else if (item.GetType() == typeof(CloudPageBlob))
                {
                    CloudPageBlob pageBlob = (CloudPageBlob)item;                    

                    var objeto = new
                    {
                        tamanho = pageBlob.Properties.Length,
                        uri = pageBlob.Uri
                    };

                    objetoLista.Add(objeto);

                }
                else if (item.GetType() == typeof(CloudBlobDirectory))
                {
                    CloudBlobDirectory directory = (CloudBlobDirectory)item;

                    Console.WriteLine("Directory: {0}", directory.Uri);

                    var objeto = new
                    {
                        tamanho = 0,
                        uri = directory.Uri
                    };

                    objetoLista.Add(objeto);
                }
            }

            return objetoLista;
        }
    }
}

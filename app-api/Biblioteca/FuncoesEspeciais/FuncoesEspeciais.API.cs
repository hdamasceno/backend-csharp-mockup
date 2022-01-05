using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Biblioteca
{
    public static partial class FuncoesEspeciais
    {
        public static string IP_GetMyIP()
        {
            try
            {
                string nome = Dns.GetHostName();

                IPAddress[] ip = Dns.GetHostAddresses(nome);

                if (ip.Length == 0)
                    return null;

                return ip[0]?.ToString();
            }
            catch {  return null; }
        }

        public static List<dynamic> API_Functions_ClearFolder(string arquivoExtensao)
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "functions");

            if (Directory.Exists(tempDir) == false)
            {
                Directory.CreateDirectory(tempDir);
            }

            var objetoArquivosExcluidos = new List<dynamic>();

            var objetoArquivoList = FuncoesEspeciais.Arquivo_ListarArquivos(tempDir, arquivoExtensao);

            foreach (var item in objetoArquivoList)
            {
                var objetoDynamic = FuncoesEspeciais.NewDynamic(new { arquivoNome = item });

                objetoDynamic.caminhoNomeArquivo = Path.Combine(tempDir, item);

                try
                {
                    File.Delete(Path.Combine(tempDir, item));

                    objetoDynamic.status = "DELETADO";

                }
                catch (Exception ex)
                {
                    objetoDynamic.status = "ERROR";
                    objetoDynamic.exception = ex;
                }

                objetoArquivosExcluidos.Add(objetoDynamic);
            }

            return objetoArquivosExcluidos;
        }

        public static string API_Functions_DownloadAndSaveFile(string url, string arquivoExtensao)
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

        public static string API_Functions_DownloadAndSaveFileWithFileName(string url, string fileName)
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "functions");

            if (Directory.Exists(tempDir) == false)
            {
                Directory.CreateDirectory(tempDir);
            }

            string tempFile = Path.Combine(tempDir, fileName);
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

        public static Boolean API_DownloadFile(string url, string filePath)
        {
            Boolean downloaded = false;

            using (WebClient objetoWebClient = new WebClient())
            {
                objetoWebClient.DownloadFile(new Uri(url, UriKind.Absolute), filePath);
            }

            downloaded = File.Exists(filePath);

            return downloaded;
        }

        /// <summary>
        /// Retorna a URL para download do arquivo que foi salvo e enviado para o storage
        /// </summary>
        /// <param name="objetoArquivo"></param>
        /// <param name="nomeArquivoComExtensao"></param>
        /// <returns></returns>
        public static string API_SaveToFileAndUploadToStorage(string storageAccountConnection,
            string storageAccountName,
            List<string> objetoArquivo,
            string nomeArquivoComExtensao)
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

            Arquivo_Salvar(filePath, objetoArquivo);

            string fileName = nomeArquivoComExtensao;

            return AzureStorage_File_Upload(storageAccountConnection,
                storageAccountName,
                fileName,
                ToString(filePath, false, true, true));
        }

        public static string API_SaveToFileAndUploadToStorage(string storageAccountConnection,
            string storageAccountName,
            IFormFile file,
            string nomeArquivoComExtensao)
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

            using (var stream = System.IO.File.Create(filePath))
            {
                file.CopyTo(stream);
            }

            string urlStorage = string.Empty;

            return AzureStorage_File_Upload(storageAccountConnection,
                storageAccountName,
                FuncoesEspeciais.ToString(FuncoesEspeciais.Arquivo_RetornaNomeArquivo(filePath), false, true, false).Replace(" ", "_"),
                FuncoesEspeciais.ToString(filePath, false, true, true));
        }

        public static string API_SaveToFile(IFormFile file, string folderName, string nomeArquivoComExtensao)
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

        public static string API_SaveToFile(List<string> objetoArquivo, string nomeArquivoComExtensao)
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

        public static List<string> ConvertToCSV(List<dynamic> objetoDynamicList)
        {
            var objetoTemp = new List<string>();

            foreach (dynamic item in objetoDynamicList)
            {
                var fieldDictionary = (IDictionary<string, object>)item;

                var list = fieldDictionary;

                foreach (var itemName in list)
                {

                }
            }

            return objetoTemp;
        }

        public static string API_OpenFileWhenNFeOrNFCe(IFormFile file)
        {
            string filePath = Directory.GetCurrentDirectory();

            if (filePath.EndsWith(@"\", StringComparison.CurrentCulture) == false)
                filePath += @"\";

            filePath += @"\upload\temp";

            filePath += @"\" + DateTime.Today.ToString("yyyyMMdd");

            FuncoesEspeciais.Arquivo_DiretorioCriar(filePath);

            filePath += @"\" + Guid.NewGuid() + ".xml";

            using (var stream = System.IO.File.Create(filePath))
            {
                file.CopyTo(stream);
            }

            string fileSTRING = Arquivo_LerArquivo_EmString(filePath);

            try
            {
                File.Delete(filePath);
            }
            catch { }

            return fileSTRING;
        }

        public static string GetItem(List<string> registros, int index, Boolean removeAcento = true)
        {
            if (registros.Count < index)
                return null;

            string _temp = registros[index];

            if (removeAcento)
                _temp = FuncoesEspeciais.RemoverAcentos(_temp);

            return _temp;
        }

        public static dynamic NewDynamic(object objeto)
        {
            string json = FuncoesEspeciais.WebApi_Json_Serializar(objeto);
            return FuncoesEspeciais.WebApi_Json_Deserializar<dynamic>(json);
        }

        public static string Object_GetFieldValue(object objeto, string fieldName)
        {
            if (objeto == null)
                return null;

            var objetoDynamic = FuncoesEspeciais.WebApi_Json_Deserializar<dynamic>(objeto.ToString());

            if (objetoDynamic == null)
                return null;

            if (FuncoesEspeciais.IsFieldExist(objetoDynamic, fieldName))
                return FuncoesEspeciais.ToString(objetoDynamic.fieldName);

            return null;
        }

        public static string Dynamic_GetFieldValue(dynamic objetoDynamic, string fieldName, Boolean toUpper = true, Boolean toLower = false, Boolean trim = true)
        {
            if (objetoDynamic == null)
                return null;

            if (IsFieldExist(objetoDynamic, fieldName) == false)
                return null;

            try
            {
                var z = Newtonsoft.Json.JsonConvert.DeserializeObject(FuncoesEspeciais.WebApi_Json_Serializar(objetoDynamic));

                return ChangeType(z[fieldName], typeof(string), toUpper, toLower, trim);
            }
            catch
            {
                return null;
            }
        }

        public static object Dynamic_GetFieldValue(dynamic objetoDynamic, string fieldName, Type toType, Boolean toUpper = true, Boolean toLower = false, Boolean trim = true)
        {
            try
            {
                string json = FuncoesEspeciais.WebApi_Json_Serializar(objetoDynamic);

                var z = Newtonsoft.Json.JsonConvert.DeserializeObject(FuncoesEspeciais.WebApi_Json_Serializar(objetoDynamic));

                var valor = (string)z[fieldName];

                return ChangeType(valor, toType, toUpper, toLower, trim);
            }
            catch
            {
                return null;
            }
        }

        public static string Request_ReadBody(HttpRequest Request)
        {
            try
            {
                string body = string.Empty;

                using (StreamReader stream = new StreamReader(Request.Body))
                {
                    body = stream.ReadToEndAsync().Result;
                }

                return body;
            }
            catch
            {
                return string.Empty;
            }
        }

        public static object Request_ReadBody<T>(HttpRequest Request, string parameterName)
        {
            try
            {
                var objetoJson = new StreamReader(Request.Body).ReadToEnd();

                if (string.IsNullOrWhiteSpace(objetoJson))
                    objetoJson = Request.Query[parameterName];

                if (string.IsNullOrWhiteSpace(objetoJson))
                    return null;

                return FuncoesEspeciais.ChangeType(objetoJson, typeof(T), false, false);
            }
            catch
            {
                return null;
            }
        }

        public static T WebApi_HttpGet<T>(string url)
        {
            string strResultado = string.Empty;

            using (HttpClient clienteWeb = new HttpClient())
            {
                strResultado = clienteWeb.GetStringAsync(url).Result;
            }

            return WebApi_Json_Deserializar<T>(strResultado);
        }

        public static string WebApi_HttpPost(string url)
        {
            string strResultado = string.Empty;

            using (HttpClient clienteWeb = new HttpClient())
            {
                var postData = new List<KeyValuePair<string, string>>();

                HttpContent httpContent = new FormUrlEncodedContent(postData);
                var response = clienteWeb.PostAsync(url, httpContent).Result;

                if (response.StatusCode == HttpStatusCode.OK)
                    strResultado = response.ToString();

                strResultado = response.Content.ReadAsStringAsync().Result;
            }

            return strResultado;
        }

        public static T WebApi_HttpPost<T>(string url)
        {
            var cts = new CancellationTokenSource();

            string strResultado = string.Empty;

            using (HttpClient clienteWeb = new HttpClient())
            {
                var postData = new List<KeyValuePair<string, string>>();

                HttpContent httpContent = new FormUrlEncodedContent(postData);
                var response = clienteWeb.PostAsync(url, httpContent, cts.Token).Result;

                if (response.StatusCode == HttpStatusCode.OK)
                    strResultado = response.ToString();

                strResultado = response.Content.ReadAsStringAsync().Result;
            }

            return WebApi_Json_Deserializar<T>(strResultado);
        }

        public static Boolean WebApi_HttpPostBasico(string url)
        {
            try
            {
                string strResultado = string.Empty;

                using (HttpClient clienteWeb = new HttpClient())
                {
                    var postData = new List<KeyValuePair<string, string>>();

                    HttpContent httpContent = new FormUrlEncodedContent(postData);
                    var response = clienteWeb.PostAsync(url, httpContent).Result;

                    if (response.StatusCode == HttpStatusCode.OK)
                        return true;
                    else
                        return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public static string WebApi_Json_Serializar(object objeto)
        {
            if (objeto == null)
                return "{}";

            return Newtonsoft.Json.JsonConvert.SerializeObject(objeto);
        }

        public static T WebApi_Json_Deserializar<T>(string objetoJson)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(objetoJson);
        }
    }
}

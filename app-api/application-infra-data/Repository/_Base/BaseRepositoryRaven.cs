using Biblioteca;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Raven.Client.Documents;
using Raven.Client.Documents.BulkInsert;
using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using application_domain.Abstracts;
using application_domain.Interfaces;
using application_domain.Objects;

namespace application_infra_data.Repository
{
    // pode matar isso aqui
    // raven trabalha com uma unica instancia (singleton) nao existe esse conceito repositorio base
    // a instancia é criada em Lazy para nao sobrecarregar a memoria, e nao é mais usada em momento algum, pois ja vai estar na pipeline de toda a aplicaçao
    // o Store sera configurado numa classe e voce injeta o IDocumentSession para ser usado em todo canto sem precisar disso aqui
    // IEntidade nao usa em canto nenhum, rever isso
    public class BaseRepositoryRaven<IEntidade, TKeyType> where IEntidade : BaseEntity<TKeyType>
    {
        protected string DataBaseEndPoint { get; private set; } = string.Empty; 
        protected string DataBaseName { get; private set; } = string.Empty;
        protected string DataBaseCertificatePassword { get; private set; } = string.Empty;
        protected string DataBaseCertificateUrl { get; private set; } = string.Empty;
        protected string DataBaseCertificateThumbPrint { get; private set; } = string.Empty;
        protected X509Certificate2 DataBaseCertificate { get; private set; }
        protected IDocumentStore DocumentStore { get; set; }

        public BaseRepositoryRaven(IConfiguration configuration)
        {
            DataBaseName = configuration["database:Raven:dataBaseName"];
            DataBaseEndPoint = configuration["database:Raven:dataBaseEndPointUrl"];
            DataBaseCertificateThumbPrint = configuration["database:Raven:certificateThumbPrint"];
            DataBaseCertificateUrl = configuration["database:Raven:certificateFileUrl"];
            DataBaseCertificatePassword = configuration["database:Raven:certificatePassword"];

            DataBaseCertificate = Certificate_GetFromThumbPrint(DataBaseCertificateThumbPrint);

            if (DataBaseCertificate == null)
            {
                DataBaseCertificate = Certificate_DownloadFromUrl(DataBaseCertificateUrl, DataBaseCertificatePassword);
            }

            if (DataBaseCertificate == null)
                throw new Exception("RavenDB certificate do not be null.");

            DocumentStore = DocumentStore_Create();
        }

        public BaseRepositoryRaven(string dataBaseEndPoint, string dataBaseName, X509Certificate2 objCertificate)
        {
            DataBaseEndPoint = dataBaseEndPoint;
            DataBaseName = dataBaseName;

            if (objCertificate != null)
            {
                DataBaseCertificateThumbPrint = objCertificate.Thumbprint;
                DataBaseCertificate = objCertificate;
            }
            else
                throw new Exception("RavenDB certificate do not be null.");

            DocumentStore = DocumentStore_Create();
        }

        public BaseRepositoryRaven(string dataBaseEndPoint, string dataBaseName, string certificateThumbPrint)
        {
            DataBaseEndPoint = dataBaseEndPoint;
            DataBaseName = dataBaseName;

            DataBaseCertificate = Certificate_GetFromThumbPrint(certificateThumbPrint);

            if (DataBaseCertificate != null)
            {
                DataBaseCertificateThumbPrint = DataBaseCertificate.Thumbprint;
            }
            else
                throw new Exception("RavenDB certificate do not be null.");

            DocumentStore = DocumentStore_Create();
        }

        public BaseRepositoryRaven(string dataBaseEndPoint, string dataBaseName, string certificateUrl, string certificatePassword)
        {
            DataBaseEndPoint = dataBaseEndPoint;
            DataBaseName = dataBaseName;
            DataBaseCertificateUrl = certificateUrl;
            DataBaseCertificatePassword = certificatePassword;

            DataBaseCertificate = Certificate_DownloadFromUrl(DataBaseCertificateUrl, certificatePassword);

            if (DataBaseCertificate != null)
            {
                DataBaseCertificateThumbPrint = DataBaseCertificate.Thumbprint;
            }
            else
                throw new Exception("RavenDB certificate do not be null.");

            DocumentStore = DocumentStore_Create();
        }

        private IDocumentStore DocumentStore_Create()
        {
            if (DataBaseCertificate == null)
                throw new Exception("RavenDB DataDabase or Account Certificate not loaded.");

            IDocumentStore documentStore = new DocumentStore()
            {
                Urls = new[] { DataBaseEndPoint },
                Database = DataBaseName,
                Certificate = DataBaseCertificate
            };

            documentStore.Initialize();

            return documentStore;
        }

        protected X509Certificate2 Certificate_DownloadFromUrl(string url, string password)
        {
            string filePath = Path.GetTempPath();

            filePath = Path.Combine(filePath, "download");

            FuncoesEspeciais.Arquivo_DiretorioCriar(filePath);

            filePath = Path.Combine(filePath, DateTime.Now.Ticks.ToString() + ".pfx");

            var downloadOK = FuncoesEspeciais.API_DownloadFile(url, filePath);

            if (downloadOK == false)
                throw new Exception("Certificate file not found and download failed too.");

            X509Certificate2 objetoCertificado;

            try
            {
                objetoCertificado = new X509Certificate2
                    (
                        filePath,
                        password,
                        X509KeyStorageFlags.EphemeralKeySet
                    );
            }
            catch
            {
                objetoCertificado = new X509Certificate2
                    (
                        filePath,
                        password,
                        X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.UserKeySet
                    );
            }

            return objetoCertificado;
        }

        protected X509Certificate2 Certificate_GetFromThumbPrint(string thumbPrint)
        {
            X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);

            store.Open(OpenFlags.ReadOnly);

            X509Certificate2Collection collection = store.Certificates;

            var certificateList = collection.Find(X509FindType.FindByThumbprint, thumbPrint, false).ToList();

            store.Close();

            if (certificateList?.Count > 0)            
                return certificateList[0];            
            else
                return null;
        }

        protected IResposta SaveToStore(object document)
        {
            IResposta objetoResposta = new RespostaAPI();

            try
            {
                using (IDocumentSession Session = DocumentStore.OpenSession())
                {
                    Session.Store(document);

                    Session.SaveChanges();
                }

                objetoResposta.ComandoExecutadoComSucesso();
            }
            catch (Exception ex)
            {
                objetoResposta.ComandoExecutadoComErro(ex);
            }

            return objetoResposta;
        }

        protected IResposta SaveToStoreList<T>(List<T> documents)
        {
            IResposta objetoResposta = new RespostaAPI();

            try
            {
                using (IDocumentSession Session = DocumentStore.OpenSession())
                {
                    using (BulkInsertOperation bulkInsert = DocumentStore.BulkInsert())
                    {
                        foreach (var document in documents)
                            bulkInsert.Store(document);
                    }
                }

                objetoResposta.ComandoExecutadoComSucesso();
            }
            catch (Exception ex)
            {
                objetoResposta.ComandoExecutadoComErro(ex);
            }

            return objetoResposta;
        }

        protected T GetDocument<T>(string id)
        {
            using (IDocumentSession Session = DocumentStore.OpenSession())
            {
                return GetDocument<T>(Session, id);
            }
        }

        protected IResposta DeleteDocument<T>(T document)
        {
            IResposta objetoResposta = new RespostaAPI();

            try
            {
                using (IDocumentSession Session = DocumentStore.OpenSession())
                {
                    Session.Delete(document);
                    Session.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                objetoResposta.ComandoExecutadoComErro(ex);
            }

            return objetoResposta;
        }

        private T GetDocument<T>(IDocumentSession session,
            string id)
        {
            return session.Load<T>(id);
        }
    }
}

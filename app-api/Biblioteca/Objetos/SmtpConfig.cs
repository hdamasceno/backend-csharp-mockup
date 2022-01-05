namespace Biblioteca
{
    public class SGBD_Config
    {
        public SGBD baseDadosConfig { get; set; }
    }

    public class SGBD
    {
        public string Server { get; set; }
        public string Db { get; set; }
        public string User { get; set; }
        public string Pass { get; set; }
        public int Port { get; set; }
        public string DeveloperCnpj { get; set; }
    }
}

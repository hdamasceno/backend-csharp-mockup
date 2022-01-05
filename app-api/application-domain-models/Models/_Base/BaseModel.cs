using System.Reflection;
using Biblioteca;

namespace application_data_models.Models._Base
{
    public abstract class BaseModel
    {
        public string Id { get; set; }
        public DateTime CadastradoDataHora { get; set; }
        public DateTime? AlteradoDataHora { get; set; }

        public BaseModel()
        {
            Id = Guid.NewGuid().ToString();
            CadastradoDataHora = DateTime.Now.ConverteDataAzureBrasil();
        }

        public virtual dynamic ToDynamic()
        {
            return FuncoesEspeciais.NewDynamic(this);
        }

        protected virtual void LoadFromJSON<T>(object obj, string json)
        {
            var objetoDynamic = FuncoesEspeciais.WebApi_Json_Deserializar<dynamic>(json);

            Load<T>(obj, objetoDynamic);
        }

        private void Load<T>(object _return,
            dynamic objetoDynamic,
            bool toUpper = true,
            bool toLower = false,
            bool trim = true)
        {
            if (objetoDynamic != null)
            {
                foreach (PropertyInfo pro in typeof(T).GetProperties().ToList())
                {
                    try
                    {
                        if (pro.Name.Contains("body") ||
                            pro.Name.Contains("BODY") ||
                            pro.Name.Contains("Body") ||
                            pro.Name.Contains("sid") ||
                            pro.Name.Contains("senha") ||
                            pro.Name.Contains("SENHA") ||
                            pro.Name.Contains("SID") ||
                            pro.Name.Contains("Sid") ||
                            pro.Name.Contains("token") ||
                            pro.Name.Contains("Token") ||
                            pro.Name.Contains("TOKEN") ||
                            pro.Name.Contains("json") ||
                            pro.Name.Contains("gzip") ||
                            pro.Name.Contains("JSON") ||
                            pro.Name.Contains("GZIP") ||
                            pro.Name.Contains("Json") ||
                            pro.Name.Contains("Gzip") ||
                            pro.Name.Contains("JSon") ||
                            pro.Name.Contains("GZip") ||
                            pro.Name.Contains("xml") ||
                            pro.Name.Contains("XML") ||
                            pro.Name.Contains("url") ||
                            pro.Name.Contains("Url") ||
                            pro.Name.Contains("URL") ||
                            pro.Name.Contains("email") ||
                            pro.Name.Contains("EMAIL") ||
                            pro.Name.Contains("Email") ||
                            pro.Name.Contains("html") ||
                            pro.Name.Contains("HTML") ||
                            pro.Name.Contains("endpoint") ||
                            pro.Name.Contains("host") ||
                            pro.Name.Contains("email") ||
                            pro.Name.Contains("api") ||
                            pro.Name.Contains("API") ||
                            pro.Name.Contains("query") ||
                            pro.Name.Contains("server") ||
                            pro.Name.Contains("raw") ||
                            pro.Name.Contains("RAW") ||
                            pro.Name.Contains("Base64") ||
                            pro.Name.Contains("BASE64") ||
                            pro.Name.Contains("CSC") ||
                            pro.Name.Contains("csc") ||
                            pro.Name.Contains("objetoJsonGzip") ||
                            pro.Name.Contains("tableRegistroGZip"))
                        {
                            toUpper = false;
                            toLower = false;
                        }
                        else if (pro.Name.StartsWith("id", StringComparison.CurrentCulture))
                        {
                            toUpper = false;
                            toLower = true;
                            trim = true;
                        }
                        else if (pro.Name.EndsWith("Id", StringComparison.CurrentCulture) || pro.Name.EndsWith("Id", StringComparison.CurrentCulture))
                        {
                            toUpper = false;
                            toLower = true;
                            trim = true;
                        }
                        else
                        {
                            toUpper = true;
                            toLower = false;
                            trim = true;
                        }

                        var valor = FuncoesEspeciais.Dynamic_GetFieldValue(objetoDynamic, pro.Name, pro.PropertyType, toUpper, toLower, trim);

                        if (valor == null)
                            valor = FuncoesEspeciais.Dynamic_GetFieldValue(objetoDynamic, pro.Name, toUpper, toLower, trim);

                        if (valor == null)
                            _return.GetType()?.GetProperty(pro.Name)?.SetValue(null, null);
                        else
                        {
                            valor = FuncoesEspeciais.ChangeType(valor, pro.PropertyType, toUpper, toLower, trim);

                            bool naoSetarValor = false;

                            if (pro.Name == "id" || pro.Name == "Id")
                            {
                                if (FuncoesEspeciais.ToGuid(valor) == new Guid())
                                    naoSetarValor = true;
                            }

                            if (pro.Name == "senha" || pro.Name == "Senha" || pro.Name == "password" || pro.Name == "Password")
                            {
                                if (string.IsNullOrWhiteSpace(valor != null ? valor.ToString() : string.Empty))
                                    naoSetarValor = true;
                            }

                            if (naoSetarValor == false)
                                _return.GetType()?.GetProperty(pro.Name)?.SetValue(_return, valor);
                        }
                    }
                    catch
                    {
                        // Mapear erro de conversão de tipos...
                    }
                }
            }
        }
    }
}
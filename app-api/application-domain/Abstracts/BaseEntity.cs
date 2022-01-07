using Biblioteca;
using Flunt.Notifications;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using application_domain.Types.Values;

namespace application_domain.Abstracts
{
    public abstract class BaseEntity<TKeyType> : Notifiable<Notification>
    {
        public virtual TKeyType Id { get; protected set; }
        protected BaseEntity(TKeyType id) => Id = id;

        public string GetValidationErrors()
        {
            if (Notifications?.Count <= 0)
                return string.Empty;

            string errors = string.Empty;

            if (Notifications != null)
            {
                foreach (var item in Notifications)
                    errors += item.Message + Environment.NewLine;
            }

            return errors;
        }
        
        // over engineer heehe
        // passa um objeto concreto, transforma em string e depois em dinamico?? :D
        // json é string e json convertido é objeto, ponto final! retrabalho a toa
        public virtual dynamic ToDynamic<T>()
        {
            string json = ToJSON<T>();

            return FuncoesEspeciais.WebApi_Json_Deserializar<dynamic>(json);
        }

        // pra q isso aqui?? :D
        public virtual string ToJSON<T>()
        {
            string json = string.Empty;

            foreach (PropertyInfo property in typeof(T).GetProperties())
            {
                var value = property.GetValue(this);

                if (value != null && property.Name.Equals("Notifications") == false)
                {
                    if (string.IsNullOrEmpty(json))
                        json = "{" + $"{property.Name}: '{value}'";
                    else
                        json += $", {property.Name}: '{value}'";
                }
            }

            json += "}";

            return json;
        }

        // q isso tudo aqui doidão? ahahu fiquei curioso
        protected virtual void LoadFromDynamic<T>(Object _return,
            dynamic objetoDynamic,
            Boolean toUpper = true,
            Boolean toLower = false,
            Boolean trim = true)
        {
            if (objetoDynamic == null)
            {
                AddNotification($"{this.GetType().Name}.LoadFromDynamic", $"{this.GetType().Name} - JSON inválido.");

                return;
            }

            if (objetoDynamic != null)
            {
                foreach (PropertyInfo pro in typeof(T).GetProperties().ToList())
                {
                    var originProperty = _return.GetType()?.GetProperty(pro.Name);

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
                            pro.Name.Contains("base64") ||
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
                        else if (pro.Name.EndsWith("Id", StringComparison.CurrentCulture) || pro.Name.EndsWith("Id", StringComparison.CurrentCulture))
                        {
                            toUpper = false;
                            toLower = true;
                            trim = true;
                        }
                        else if (pro.Name.EndsWith("Senha", StringComparison.CurrentCulture) || pro.Name.EndsWith("Senha", StringComparison.CurrentCulture))
                        {
                            toUpper = false;
                            toLower = false;
                            trim = false;
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

                        if (valor != null)                        
                        {
                            Boolean naoSetarValor = false;

                            if (pro.Name == "Id" && FuncoesEspeciais.IsFieldExist(objetoDynamic, "Id"))
                            {
                                if (FuncoesEspeciais.ToGuid(valor) == new Guid())
                                    naoSetarValor = true;
                            }

                            if (pro.Name == "Senha" && FuncoesEspeciais.IsFieldExist(objetoDynamic, "Senha"))
                            {
                                if (string.IsNullOrWhiteSpace(valor != null ? valor.ToString() : ""))
                                    naoSetarValor = true;
                            }

                            if (naoSetarValor == false)
                            {
                                var originPropertyFullName = originProperty?.PropertyType.FullName;                                

                                if (originPropertyFullName == "application_domain.Types.Values.Cep")
                                    _return.GetType()?.GetProperty(pro.Name)?.SetValue(_return, (Cep)valor);
                                else if (originPropertyFullName == "application_domain.Types.Values.Cnae")
                                    _return.GetType()?.GetProperty(pro.Name)?.SetValue(_return, (Cnae)valor);
                                else if (originPropertyFullName == "application_domain.Types.Values.Cnpj")
                                    _return.GetType()?.GetProperty(pro.Name)?.SetValue(_return, (Cnpj)valor);
                                else if (originPropertyFullName == "application_domain.Types.Values.Cpf")
                                    _return.GetType()?.GetProperty(pro.Name)?.SetValue(_return, (Cpf)valor);
                                else if (originPropertyFullName == "application_domain.Types.Values.Data")
                                    _return.GetType()?.GetProperty(pro.Name)?.SetValue(_return, (DataHora)FuncoesEspeciais.ToDateTimeNull(valor, true, false, true));
                                else if (originPropertyFullName == "application_domain.Types.Values.DataHora")
                                    _return.GetType()?.GetProperty(pro.Name)?.SetValue(_return, (DataHora)FuncoesEspeciais.ToDateTimeNull(valor, false, false, true));
                                else if (originPropertyFullName == "application_domain.Types.Values.DecimalPositive")
                                    _return.GetType()?.GetProperty(pro.Name)?.SetValue(_return, (DecimalPositive)FuncoesEspeciais.ToDecimal(valor, true));
                                else if (originPropertyFullName == "application_domain.Types.Values.Email")
                                    _return.GetType()?.GetProperty(pro.Name)?.SetValue(_return, (Email)valor);
                                else if (originPropertyFullName == "application_domain.Types.Values.InscricaoEstadual")
                                    _return.GetType()?.GetProperty(pro.Name)?.SetValue(_return, (InscricaoEstadual)valor);
                                else if (originPropertyFullName == "application_domain.Types.Values.InscricaoMunicipal")
                                    _return.GetType()?.GetProperty(pro.Name)?.SetValue(_return, (InscricaoMunicipal)valor);
                                else if (originPropertyFullName == "application_domain.Types.Values.Key")
                                    _return.GetType()?.GetProperty(pro.Name)?.SetValue(_return, (Key)FuncoesEspeciais.ToGuid(valor));
                                else if (originPropertyFullName == "application_domain.Types.Values.Logradouro")
                                    _return.GetType()?.GetProperty(pro.Name)?.SetValue(_return, (Logradouro)valor);
                                else if (originPropertyFullName == "application_domain.Types.Values.Name")
                                    _return.GetType()?.GetProperty(pro.Name)?.SetValue(_return, (Name)valor);
                                else if (originPropertyFullName == "application_domain.Types.Values.NameNFe")
                                    _return.GetType()?.GetProperty(pro.Name)?.SetValue(_return, (NameNFe)valor);
                                else if (originPropertyFullName == "application_domain.Types.Values.Password")
                                    _return.GetType()?.GetProperty(pro.Name)?.SetValue(_return, (Password)valor);
                                else if (originPropertyFullName == "application_domain.Types.Values.SexoSigla")
                                    _return.GetType()?.GetProperty(pro.Name)?.SetValue(_return, (SexoSigla)valor);
                                else if (originPropertyFullName == "application_domain.Types.Values.Uf")
                                    _return.GetType()?.GetProperty(pro.Name)?.SetValue(_return, (Uf)valor);
                                else
                                {
                                    var valorWithOriginalType = FuncoesEspeciais.ChangeType(valor, pro.PropertyType, toUpper, toLower, trim);

                                    _return.GetType()?.GetProperty(pro.Name)?.SetValue(_return, valorWithOriginalType);
                                }                                
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        AddNotification(originProperty != null ? originProperty.GetType().Name : pro.Name, ex.Message);
                    }
                }
            }

            foreach (PropertyInfo property in typeof(T).GetProperties())
            {
                if (property.Name.Equals("Notifications") == false)
                {
                    Boolean isNullable = property.PropertyType.Name.Contains("Nullable");
                    var value = property.GetValue(this);

                    Boolean fieldInvalid = (value == null && isNullable == false);
                    string validationErrors = "is null or with contract invalid";
                    if (fieldInvalid == false && isNullable == false)
                    {
                        var hasTypeDefined = value?.GetType();

                        if (hasTypeDefined != null)
                        {
                            string _fullName = FuncoesEspeciais.ToString(hasTypeDefined?.FullName, false, false, false);

                            if (string.IsNullOrWhiteSpace(_fullName))
                                _fullName = string.Empty;

                            if (_fullName.Contains("application_domain"))
                            {
                                string json = Newtonsoft.Json.JsonConvert.SerializeObject(value);

                                var _valueInDynamic = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(json);

                                if (_valueInDynamic?.contract != null)
                                    fieldInvalid = false;
                                else                                  
                                    fieldInvalid = _valueInDynamic?.contract != null ? FuncoesEspeciais.ToString(_valueInDynamic.contract.IsValid) != "TRUE" : true;
                            }
                        }
                    }

                    if (fieldInvalid)
                    {
                        AddNotification($"{property?.Name}", $"Campo [ {property?.Name} ] : {validationErrors}.");
                    }
                }
            }
        }

        protected virtual void AdicionarNotificacao(string entityName, string? methodName, string message)
        {
            AddNotification($"{entityName}.{methodName}", message);
        }

        protected virtual void AdicionarNotificacao(string message)
        {
            AddNotification($"{this.GetType().Name}", message);
        }
    }
}

using Microsoft.AspNetCore.Http.Internal;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace Biblioteca
{
    public abstract class EntityBackWork<T> where T : EntityBackWork<T>, new()
    {
        public string Id { get; set; }
        public DateTime CadastradoDataHora { get; set; }
        public DateTime? AlteradoDataHora { get; set; }

        public EntityBackWork()
        {
            this.Id = Guid.NewGuid().ToString();
            this.CadastradoDataHora = DateTime.Now.ConverteDataAzureBrasil();
        }

        public void LoadFromJSON(T _return, string jsonIN)
        {
            var objetoDynamic = FuncoesEspeciais.WebApi_Json_Deserializar<dynamic>(jsonIN);

            Load(_return, objetoDynamic);
        }

        public void Load(T _return,
            dynamic objetoDynamic,
            Boolean toUpper = true,
            Boolean toLower = false,
            Boolean trim = true)
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

                        if (FuncoesEspeciais.IsFieldExist(objetoDynamic, pro.Name))
                        {
                            var valor = FuncoesEspeciais.Dynamic_GetFieldValue(objetoDynamic, pro.Name, pro.PropertyType, toUpper, toLower, trim);

                            if (valor == null)
                                valor = FuncoesEspeciais.Dynamic_GetFieldValue(objetoDynamic, pro.Name, toUpper, toLower, trim);

                            if (valor == null)
                                _return.GetType().GetProperty(pro.Name).SetValue(null, null);
                            else
                            {
                                valor = FuncoesEspeciais.ChangeType(valor, pro.PropertyType, toUpper, toLower, trim);

                                Boolean naoSetarValor = false;

                                if (pro.Name == "id")
                                {
                                    if (FuncoesEspeciais.ToGuid(valor) == new Guid())
                                        naoSetarValor = true;
                                }

                                if (pro.Name == "senha")
                                {
                                    if (string.IsNullOrWhiteSpace(valor))
                                        naoSetarValor = true;
                                }

                                if (naoSetarValor == false)
                                    _return.GetType().GetProperty(pro.Name).SetValue(_return, valor);
                            }
                        }
                    }
                    catch { }
                }
            }
        }

        public override string ToString()
        {
            return FuncoesEspeciais.WebApi_Json_Serializar(this);
        }

        public dynamic ToDynamic()
        {
            return FuncoesEspeciais.NewDynamic(this);
        }

        public static T LoadByJson(string objetoJson)
        {
            T _return = new T();

            var objetoDynamic = FuncoesEspeciais.WebApi_Json_Deserializar<dynamic>(objetoJson);

            return LoadByDynamic(objetoDynamic);
        }

        public static T LoadByDynamic(dynamic objetoDynamic)
        {
            T _return = new T();

            if (objetoDynamic != null)
            {
                foreach (PropertyInfo pro in typeof(T).GetProperties().ToList())
                {
                    if (FuncoesEspeciais.IsFieldExist(objetoDynamic, pro.Name))
                    {
                        var valor = FuncoesEspeciais.Dynamic_GetFieldValue(objetoDynamic, pro.Name, pro.PropertyType);

                        if (valor == null)
                            valor = FuncoesEspeciais.Dynamic_GetFieldValue(objetoDynamic, pro.Name);

                        if (valor == null)
                            _return.GetType().GetProperty(pro.Name).SetValue(null, null);
                        else
                        {
                            valor = FuncoesEspeciais.ChangeType(valor, pro.PropertyType);

                            _return.GetType().GetProperty(pro.Name).SetValue(_return, valor);
                        }
                    }
                }

                return _return;
            }
            else
            {
                return null;
            }
        }

        public static T LoadByObject(object objeto)
        {
            T _return = new T();

            if (objeto != null)
            {
                foreach (PropertyInfo pro in typeof(T).GetProperties().ToList())
                {
                    try
                    {
                        if (FuncoesEspeciais.IsFieldExist(objeto, pro.Name))
                        {
                            object valor = objeto.GetType()?.GetProperty(pro.Name)?.GetValue(objeto);

                            if (valor == null || valor == DBNull.Value)
                                _return.GetType()?.GetProperty(pro.Name)?.SetValue(null, null);
                            else
                            {
                                valor = FuncoesEspeciais.ChangeType(valor, pro.PropertyType);

                                _return.GetType()?.GetProperty(pro.Name)?.SetValue(_return, valor);
                            }
                        }
                    }
                    catch (Exception) { }
                }

                return _return;
            }
            else
            {
                return null;
            }
        }

        public static string ConvertToSQLInsert(string tableName,
            dynamic objetoDynamic)
        {
            string comandoSQL = string.Empty;

            comandoSQL += "INSERT INTO " + tableName + " (";

            Boolean firstField = true;

            foreach (PropertyInfo pro in typeof(T).GetProperties().ToList())
            {
                if (FuncoesEspeciais.IsFieldExist(objetoDynamic, pro.Name))
                {
                    if (firstField)
                    {
                        comandoSQL += pro.Name;
                        firstField = false;
                    }
                    else
                        comandoSQL += ", " + pro.Name;
                }
            }

            comandoSQL += ") ";
            comandoSQL += "VALUES (";

            firstField = true;

            foreach (PropertyInfo pro in typeof(T).GetProperties().ToList())
            {
                if (FuncoesEspeciais.IsFieldExist(objetoDynamic, pro.Name))
                {
                    if (firstField)
                    {
                        comandoSQL += "@" + pro.Name;

                        firstField = false;
                    }
                    else
                        comandoSQL += ", @" + pro.Name;
                }
            }

            comandoSQL += ");";

            return comandoSQL;
        }

        public static string ConvertToSQLUpdate_whenToFilterOperation(string tableName,
            string fieldKey,
            dynamic objetoDynamic)
        {
            string id = string.Empty;
            string comandoSQL = string.Empty;

            comandoSQL += "UPDATE " + tableName + " SET ";

            Boolean firstField = true;

            foreach (PropertyInfo pro in typeof(T).GetProperties().ToList())
            {
                Boolean podeAdicionar = true;

                if (FuncoesEspeciais.IsFieldExist(objetoDynamic, pro.Name))
                {
                    string ehFiltroSelecionado = FuncoesEspeciais.Dynamic_GetFieldValue(objetoDynamic, pro.Name + "_filtro", "".GetType());

                    podeAdicionar = FuncoesEspeciais.ToString(ehFiltroSelecionado) == "TRUE";

                    if (podeAdicionar)
                    {
                        if (string.Compare(pro.Name, fieldKey, true) != 0)
                        {
                            if (firstField)
                            {
                                comandoSQL += pro.Name + " = @" + pro.Name;
                                firstField = false;
                            }
                            else
                                comandoSQL += ", " + pro.Name + " = @" + pro.Name;
                        }
                    }
                }
                else if (FuncoesEspeciais.IsFieldExist(objetoDynamic, pro.Name + "_filtro"))
                {
                    string ehFiltroSelecionado = FuncoesEspeciais.Dynamic_GetFieldValue(objetoDynamic, pro.Name + "_filtro", "".GetType());

                    podeAdicionar = FuncoesEspeciais.ToString(ehFiltroSelecionado) == "TRUE";

                    if (podeAdicionar)
                    {
                        if (firstField)
                        {
                            comandoSQL += pro.Name + " = @" + pro.Name;
                            firstField = false;
                        }
                        else
                            comandoSQL += ", " + pro.Name + " = @" + pro.Name;
                    }
                }
            }

            comandoSQL += " , alteradoDataHora = @alteradoDataHora ";

            comandoSQL += " WHERE " + fieldKey + " = @" + fieldKey;

            return comandoSQL;
        }

        public static string ConvertToSQLUpdate_whenToFilterOperation_WithoutWhere(string tableName,
            string fieldKey,
            dynamic objetoDynamic)
        {
            string id = string.Empty;
            string comandoSQL = string.Empty;

            comandoSQL += "UPDATE " + tableName + " SET ";

            Boolean firstField = true;

            foreach (PropertyInfo pro in typeof(T).GetProperties().ToList())
            {
                Boolean podeAdicionar = true;

                if (FuncoesEspeciais.IsFieldExist(objetoDynamic, pro.Name))
                {
                    string ehFiltroSelecionado = FuncoesEspeciais.Dynamic_GetFieldValue(objetoDynamic, pro.Name + "_filtro", "".GetType());

                    podeAdicionar = FuncoesEspeciais.ToString(ehFiltroSelecionado) == "TRUE";

                    if (podeAdicionar)
                    {
                        if (string.Compare(pro.Name, fieldKey, true) != 0)
                        {
                            if (firstField)
                            {
                                comandoSQL += pro.Name + " = @" + pro.Name;
                                firstField = false;
                            }
                            else
                                comandoSQL += ", " + pro.Name + " = @" + pro.Name;
                        }
                    }
                }
                else if (FuncoesEspeciais.IsFieldExist(objetoDynamic, pro.Name + "_filtro"))
                {
                    string ehFiltroSelecionado = FuncoesEspeciais.Dynamic_GetFieldValue(objetoDynamic, pro.Name + "_filtro", "".GetType());

                    podeAdicionar = FuncoesEspeciais.ToString(ehFiltroSelecionado) == "TRUE";

                    if (podeAdicionar)
                    {
                        if (firstField)
                        {
                            comandoSQL += pro.Name + " = @" + pro.Name;
                            firstField = false;
                        }
                        else
                            comandoSQL += ", " + pro.Name + " = @" + pro.Name;
                    }
                }
            }

            comandoSQL += " , alteradoDataHora = @alteradoDataHora ";

            return comandoSQL;
        }

        public static string ConvertToSQLUpdate(string tableName,
            string fieldKey)
        {
            string comandoSQL = string.Empty;

            comandoSQL += "UPDATE " + tableName + " SET ";

            Boolean firstField = true;

            foreach (PropertyInfo pro in typeof(T).GetProperties().ToList())
            {
                if (firstField)
                {
                    comandoSQL += pro.Name + " = @" + pro.Name;

                    firstField = false;
                }
                else
                    comandoSQL += ", " + pro.Name + " = @" + pro.Name;
            }

            comandoSQL += " , alteradoDataHora = @alteradoDataHora ";

            comandoSQL += " WHERE " + fieldKey + " = @" + fieldKey;

            return comandoSQL;
        }

        public static List<SqlParameter> ConvertToSQLParameter(dynamic objetoDynamic)
        {
            var parametros = new List<SqlParameter>();

            foreach (PropertyInfo pro in typeof(T).GetProperties().ToList())
            {
                if (FuncoesEspeciais.IsFieldExist(objetoDynamic, pro.Name))
                {
                    string fieldName = pro.Name;

                    var valor = FuncoesEspeciais.Dynamic_GetFieldValue(objetoDynamic, pro.Name, pro.PropertyType);

                    if (valor == null)
                        valor = FuncoesEspeciais.Dynamic_GetFieldValue(objetoDynamic, pro.Name);

                    if (valor == null)
                        valor = null;
                    else
                        valor = FuncoesEspeciais.ChangeType(valor, pro.PropertyType);

                    var fieldValue = valor;


                    var parametro = new SqlParameter();

                    parametro.ParameterName = fieldName;
                    parametro.Value = fieldValue;

                    parametros.Add(parametro);
                }
            }

            return parametros;
        }

        public static List<SqlParameter> ConvertToSQLParameter_whenToFilterOperation(dynamic objetoDynamic)
        {
            var parametros = new List<SqlParameter>();

            foreach (PropertyInfo pro in typeof(T).GetProperties().ToList())
            {
                string ehFiltroSelecionado = FuncoesEspeciais.Dynamic_GetFieldValue(objetoDynamic, pro.Name + "_filtro", "".GetType());

                Boolean podeAdicionar = FuncoesEspeciais.ToString(ehFiltroSelecionado) == "TRUE";

                if (podeAdicionar)
                {
                    if (FuncoesEspeciais.IsFieldExist(objetoDynamic, pro.Name))
                    {
                        string fieldName = pro.Name;

                        var valor = FuncoesEspeciais.Dynamic_GetFieldValue(objetoDynamic, pro.Name, pro.PropertyType);

                        if (valor == null)
                            valor = FuncoesEspeciais.Dynamic_GetFieldValue(objetoDynamic, pro.Name);

                        if (valor == null)
                            valor = null;
                        else
                            valor = FuncoesEspeciais.ChangeType(valor, pro.PropertyType);

                        var fieldValue = valor;


                        var parametro = new SqlParameter();

                        parametro.ParameterName = fieldName;
                        parametro.Value = fieldValue;

                        parametros.Add(parametro);
                    }
                    else if (FuncoesEspeciais.IsFieldExist(objetoDynamic, pro.Name + "_filtro"))
                    {
                        if (FuncoesEspeciais.IsFieldExist(objetoDynamic, pro.Name) == false)
                        {
                            var parametro = new SqlParameter();

                            parametro.ParameterName = pro.Name;
                            parametro.Value = DBNull.Value;

                            parametros.Add(parametro);
                        }
                    }
                }
            }

            return parametros;
        }
    }
}

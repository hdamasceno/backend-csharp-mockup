using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Biblioteca
{
    public static partial class FuncoesEspeciais
    {
        public static void SQL_ExecuteNonQuery(string sgbd, string strConnection, string comandoSQL, List<SqlParameter> parametros = null)
        {
            using (var conexao = new SqlConnection(strConnection))
            {
                using (var command = conexao.CreateCommand())
                {
                    if (parametros != null)
                    {
                        foreach (var item in parametros)
                        {
                            command.Parameters.Add(item);
                        }
                    }
                    conexao.Open();

                    command.CommandText = comandoSQL;
                    command.CommandType = CommandType.Text;

                    var iResposta = command.ExecuteNonQuery();

                    conexao.Close();                    
                }
            }
        }

        public static List<dynamic> SQL_ExecuteQuery(string sgbd, string strConnection, string query, List<SqlParameter> parametros = null)
        {
            using (var conexao = new SqlConnection(strConnection))
            {
                using (var command = conexao.CreateCommand())
                {
                    if (parametros != null)
                    {
                        foreach (var item in parametros)
                        {
                            command.Parameters.Add(item);
                        }
                    }
                    conexao.Open();

                    command.CommandText = query;
                    command.CommandType = CommandType.Text;
                    command.CommandTimeout = 540;

                    var dados = command.ExecuteReader();

                    int quantidadeCampos = dados.FieldCount;

                    var objetoLista = new List<dynamic>();

                    while (dados.Read())
                    {
                        string objetoJson = "{";

                        for (int i = 0; i <= quantidadeCampos - 1; i++)
                        {
                            if (i == 0)
                                objetoJson += dados.GetName(i) + ": '" + FuncoesEspeciais.RemoverAcentos(FuncoesEspeciais.ToString(dados.GetValue(i), false, false)) + "'";
                            else
                                objetoJson += ", " + dados.GetName(i) + ": '" + FuncoesEspeciais.RemoverAcentos(FuncoesEspeciais.ToString(dados.GetValue(i), false, false)) + "'";
                        }

                        objetoJson += "}";

                        var objetoDynamic = FuncoesEspeciais.WebApi_Json_Deserializar<dynamic>(objetoJson);

                        if (objetoDynamic != null)
                            objetoLista.Add(objetoDynamic);
                    }

                    conexao.Close();

                    return objetoLista;
                }
            }
        }

        public static void SQL_ExecuteNonQuery(string strConnection, string comandoSQL, List<SqlParameter> parametros = null, int commandTimeOut = 540)
        {
            using (var conexao = new SqlConnection(strConnection))
            {
                using (var command = conexao.CreateCommand())
                {
                    if (parametros != null)
                    {
                        foreach (var item in parametros)
                        {
                            command.Parameters.Add(item);
                        }
                    }

                    conexao.Open();

                    command.CommandText = comandoSQL;
                    command.CommandType = CommandType.Text;
                    command.CommandTimeout = commandTimeOut;

                    var iResposta = command.ExecuteNonQuery();

                    conexao.Close();
                }
            }
        }

        public static List<dynamic> SQL_ExecuteQuery(string strConnection, string query, List<SqlParameter> parametros = null)
        {
            using (var conexao = new SqlConnection(strConnection))
            {
                using (var command = conexao.CreateCommand())
                {
                    if (parametros != null)
                    {
                        foreach (var item in parametros)
                        {
                            command.Parameters.Add(item);
                        }
                    }
                    conexao.Open();

                    command.CommandText = query;
                    command.CommandType = CommandType.Text;
                    command.CommandTimeout = 540;

                    var dados = command.ExecuteReader();

                    int quantidadeCampos = dados.FieldCount;

                    var objetoLista = new List<dynamic>();

                    while (dados.Read())
                    {
                        string objetoJson = "{";

                        for (int i = 0; i <= quantidadeCampos - 1; i++)
                        {
                            string campo = dados.GetName(i);
                            string valor = FuncoesEspeciais.RemoverAcentos(FuncoesEspeciais.ToString(dados.GetValue(i), false, false));

                            valor = valor.Replace(@"\", @"");

                            if (i == 0)
                                objetoJson += campo + ": '" + valor + "'";
                            else
                                objetoJson += ", " + campo + ": '" + valor + "'";
                        }

                        objetoJson += "}";

                        var objetoDynamic = FuncoesEspeciais.WebApi_Json_Deserializar<dynamic>(objetoJson);

                        if (objetoDynamic != null)
                            objetoLista.Add(objetoDynamic);
                    }

                    conexao.Close();

                    return objetoLista;
                }
            }
        }
    }
}
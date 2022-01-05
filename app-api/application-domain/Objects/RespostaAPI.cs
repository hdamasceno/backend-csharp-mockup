using Biblioteca;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using application_domain.Interfaces;

namespace application_domain.Objects
{
    public class RespostaAPI : IResposta
    {
        private string status { get; set; }
        private string? message { get; set; }
        private object? objeto { get; set; }
        public string Status { get => status; private set => this.status = value; }
        public string? Message { get => message; private set => this.Message = value; }
        public object? Objeto { get => objeto; private set => this.objeto = value; }

        public RespostaAPI()
        {
            this.status = "NOT_EXECUTED";
        }

        public RespostaAPI(string _status)
        {
            this.status = _status;
        }

        public RespostaAPI(string _status, object objeto)
        {
            this.status = _status;
            this.objeto = objeto;
        }

        public void ComandoExecutadoComSucesso()
        {
            this.status = "OK";
        }

        public void ComandoExecutadoComSucesso(string status)
        {
            this.status = status;
        }

        public void ComandoExecutadoComSucesso(string status, object objeto)
        {
            this.status = status;
            this.objeto = objeto;
        }

        public void ComandoExecutadoComSucesso(object objeto)
        {
            this.status = "OK";
            this.objeto = objeto;
        }

        public IResposta ComandoExecutadoComErro()
        {
            this.status = "ERROR";

            return this;
        }

        public IResposta ComandoExecutadoComErro(string status, string message)
        {
            this.status = status;
            this.message = message;

            return this;
        }

        public IResposta ComandoExecutadoComErro(string status, string message, object objeto)
        {
            this.status = status;
            this.message = message;
            this.objeto = objeto;

            return this;
        }

        public IResposta ComandoExecutadoComErro(object objeto)
        {
            this.status = "ERROR";
            this.objeto = objeto;

            return this;
        }

        public dynamic GetObjetoInDynamic()
        {
            if (objeto == null)
                return FuncoesEspeciais.NewDynamic(new { });

            return FuncoesEspeciais.NewDynamic(objeto);
        }

        public string GetObjetoInJSON()
        {
            if (objeto == null)
                return FuncoesEspeciais.WebApi_Json_Serializar(new { });

            return FuncoesEspeciais.WebApi_Json_Serializar(objeto);
        }
    }
}

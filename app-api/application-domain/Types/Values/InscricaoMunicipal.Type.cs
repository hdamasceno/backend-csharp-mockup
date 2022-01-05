using Biblioteca;
using Flunt.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace application_domain.Types.Values
{
    public struct InscricaoMunicipal
    {
        private readonly string _value;
        public readonly Contract<InscricaoMunicipal> contract;

        private InscricaoMunicipal(string value)
        {
            _value = FuncoesEspeciais.SomenteNumero(value);

            contract = new Contract<InscricaoMunicipal>();

            Validate();
        }

        public override string ToString() =>
            _value;

        public static implicit operator InscricaoMunicipal(string value) =>
            new InscricaoMunicipal(value);

        private bool Validate()
        {
            if (string.IsNullOrWhiteSpace(_value))
                return AddNotification("Informe uma inscrição municipal válida.");

            if (_value.Length < 5)
                return AddNotification("A inscrição municipal não pode ser menor que 5 caracteres.");                        

            if (FuncoesEspeciais.SomenteLetras(_value).Length > 0)
                return AddNotification("A inscrição municipal não pode conter letras.");

            return true;
        }

        private bool AddNotification(string message)
        {
            contract.AddNotification(nameof(InscricaoMunicipal), message);

            return false;
        }
    }
}

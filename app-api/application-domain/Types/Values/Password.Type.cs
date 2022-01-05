using Biblioteca;
using Flunt.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace application_domain.Types.Values
{
    public struct Password : IEquatable<Password>
    {
        private readonly string _value;
        public readonly Contract<string> contract;

        private Password(string value)
        {
            _value = value;

            contract = new Contract<string>();

            Validate();
        }

        public override string ToString() =>
            _value;

        public static implicit operator Password(string input) =>
            new Password(input.Trim());

        private bool Validate()
        {
            if (string.IsNullOrWhiteSpace(_value))
                return AddNotification("A senha não pode ser null ou somente conter espaços em branco.");

            if (_value.Length < 6)
                return AddNotification("A senha precisa ter pelo menos 6 caracteres.");

            return true;
        }

        private bool AddNotification(string message)
        {
            contract.AddNotification(nameof(Password), message);

            return false;
        }

        public string Encriptar() { return FuncoesEspeciais.GZip_Compactar(FuncoesEspeciais.MD5_String(_value + "_900dajksldhasdddasiudasiu183719271")); }

        public bool Equals(Password other)
        {
            var _resposta = _value == other.ToString();

            if (_resposta == false)
                _resposta = _value == other.Encriptar();

            if (_resposta == false)
                return Encriptar() == other.Encriptar();

            return _resposta;
        }
    }
}

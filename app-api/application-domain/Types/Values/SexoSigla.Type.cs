using Flunt.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace application_domain.Types.Values
{
    public struct SexoSigla
    {
        private readonly string _value;
        public readonly Contract<SexoSigla> contract;

        private SexoSigla(string value)
        {
            _value = value;
            contract = new Contract<SexoSigla>();

            Validate();
        }

        public override string ToString() =>
            _value;

        public static implicit operator SexoSigla(string value) =>
            new SexoSigla(value);

        private bool Validate()
        {
            if (string.IsNullOrWhiteSpace(_value))
                return AddNotification("Informe um nome válido.");

            if (_value.Length != 2)
                return AddNotification("A sigla do sexo precisa ter 2 caracteres.");
                        
            return true;
        }

        private bool AddNotification(string message)
        {
            contract.AddNotification(nameof(SexoSigla), message);
            return false;
        }
    }
}

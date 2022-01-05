using Flunt.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace application_domain.Types.Values
{
    public struct InscricaoEstadual
    {
        private readonly string _value;
        public readonly Contract<InscricaoEstadual> contract;

        private InscricaoEstadual(string value)
        {
            _value = value;
            contract = new Contract<InscricaoEstadual>();

            Validate();
        }

        public override string ToString() =>
            _value;

        public static implicit operator InscricaoEstadual(string value) =>
            new InscricaoEstadual(value);

        private bool Validate()
        {
            if (string.IsNullOrWhiteSpace(_value))
                return AddNotification("Informe uma inscrição estadual válida.");

            if (_value.Length < 5)
                return AddNotification("A inscrição estadual não pode ser menor que 5 caracteres.");

            if (!Regex.IsMatch(_value, (@"[^Z0-9]")) && _value != "ISENTO")
                return AddNotification("A inscrição estadual não pode ter caracteres especiais. OU ser diferente da palavra ISENTO");

            return true;
        }

        private bool AddNotification(string message)
        {
            contract.AddNotification(nameof(InscricaoEstadual), message);

            return false;
        }
    }
}

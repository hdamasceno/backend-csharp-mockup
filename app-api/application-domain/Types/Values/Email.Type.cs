using System;
using Flunt.Validations;

namespace application_domain.Types.Values
{
    public struct Email
    {
        private readonly string _value;
        public readonly Contract<Email> contract;

        private Email(string value)
        {
            _value = value;
            contract = new Contract<Email>();

            Validate();
        }

        public override string ToString() =>
            _value;

        public static implicit operator Email(string value) =>
            new Email(value);

        private bool Validate()
        {
            if (string.IsNullOrWhiteSpace(_value))
                return AddNotification("Informe um e-mail válido.");

            if (_value.Contains("@") == false)
                return AddNotification("Informe um e-mail válido.");

            return this.contract
                .Requires()
                .IsEmail(_value, nameof(Email), "Informe um e-mail válido.").IsValid;
        }

        private bool AddNotification(string message)
        {
            contract.AddNotification(nameof(Email), message);
            return false;
        }
    }
}


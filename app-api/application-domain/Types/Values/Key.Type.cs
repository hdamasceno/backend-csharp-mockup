using Flunt.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace application_domain.Types.Values
{
    public struct Key : IEquatable<Key>
    {
        private readonly Guid _value;
        public readonly Contract<Key> contract;

        private Key(Guid value)
        {
            _value = value;
            contract = new Contract<Key>();

            Validate();
        }

        public override string ToString() =>
            _value.ToString();

        public Guid ToGuid() => _value;

        public static implicit operator Key(Guid value) =>
            new Key(value);

        private bool Validate()
        {
            if (_value == new Guid())
                return AddNotification("Informe uma chave válida.");

            return true;
        }

        private bool AddNotification(string message)
        {
            contract.AddNotification(nameof(Key), message);
            return false;
        }

        public bool Equals(Key other)
        {
            return this.ToString() == other.ToString();
        }
    }
}

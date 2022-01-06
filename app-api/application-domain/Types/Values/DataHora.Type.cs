using Flunt.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace application_domain.Types.Values
{
    public struct DataHora
    {
        private readonly DateTime _value;
        public readonly Contract<DataHora> contract;

        private DataHora(DateTime value)
        {
            _value = value;

            contract = new Contract<DataHora>();

            Validate();
        }

        public override string ToString() =>
            _value.ToString("dd/MM/yyyy HH:mm:ss");

        public static implicit operator DataHora(DateTime value) =>
            new DataHora(value);

        private bool Validate()
        {
            if (_value == new DateTime())
                return AddNotification("Data-Hora inválida.");

            if (_value < new DateTime(1950, 1, 1, 0, 0, 0))
                return AddNotification("Data-Hora inválida. Data-Hora não pode ser menor que 01-01-1950 00:00:00");

            return true;
        }

        private bool AddNotification(string message)
        {
            contract.AddNotification(nameof(DataHora), message);

            return false;
        }

        public string ToNFe(string UF)
        {
            string dataHora = _value.ToString("yyyy") + "-" +
                _value.ToString("MM") + "-" +
                _value.ToString("dd") +
                "T" +
                _value.ToString("HH") + ":" +
                _value.ToString("mm") + ":" +
                _value.ToString("ss");

            if (_value.IsDaylightSavingTime())
            {
                if (UF == "MG" ||
                    UF == "DF" ||
                    UF == "RJ" ||
                    UF == "SP" ||
                    UF == "MA" ||
                    UF == "PA" ||
                    UF == "BA")
                {
                    dataHora = dataHora + "-02:00";
                }
                else if (UF == "MT")
                    dataHora = dataHora + "-03:00";
                else if (UF == "AM" || UF == "RO")
                    dataHora = dataHora + "-03:00";
            }
            else
            {
                if (UF == "MG" ||
                    UF == "DF" ||
                    UF == "RJ" ||
                    UF == "SP" ||
                    UF == "MA" ||
                    UF == "PA" ||
                    UF == "BA")
                {
                    dataHora = dataHora + "-03:00";
                }
                else if (UF == "MT")
                    dataHora = dataHora + "-03:00";
                else if (UF == "AM" || UF == "RO")
                    dataHora = dataHora + "-04:00";
            }

            return dataHora;
        }
    }
}

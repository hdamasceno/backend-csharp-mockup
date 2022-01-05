using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using application_domain.Types.Values;

namespace application_domain.Interfaces
{
    public interface IEntidade
    {
        Key Id { get; }
        DataHora CadastradoDataHora { get; }
        DataHora? AlteradoDataHora { get; }

        string ToJSON<T>();
        dynamic ToDynamic<T>();
        void Load(dynamic objetoDynamic);        
    }
}

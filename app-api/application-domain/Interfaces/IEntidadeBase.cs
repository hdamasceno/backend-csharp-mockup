using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using application_domain.Types.Values;

namespace application_domain.Interfaces
{
    public interface IEntidadeBase : IEntidade
    {
        Key AccountId { get; }
        IEntidade? Account { get; }
    }
}

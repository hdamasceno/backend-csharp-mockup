using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace application_domain.Interfaces
{
    public interface IServiceBase<T>
    {
        IResposta Save(T obj);

        IResposta Delete(Guid id);

        IResposta GetById(Guid id);

        IResposta GetAll();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using application_domain.Types.Values;

namespace application_domain.Interfaces
{
    public interface IRepositoryBase<T>
    {
        IResposta Save(T entity);
        T GetById(Key id);
        List<T> GetAll();
        IResposta Delete(Key id);
    }
}

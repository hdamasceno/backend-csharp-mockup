using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using application_domain.Types.Values;

namespace application_domain.Interfaces
{
    public interface IRepository<T>
    {
        IQueryOption<T> QueryOptions { get; set; }
        IResposta Save(T entity);
        T GetById(Guid id);
        List<T> GetAll();
        IResposta Delete(Guid id);
    }
}

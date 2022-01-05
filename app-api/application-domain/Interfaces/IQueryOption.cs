using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace application_domain.Interfaces
{
    public interface IQueryOption<T>
    {
        int TakeNumber { get; set; }
        int SkipNumber { get; set; }        
        IQueryable<T> QueryParams { get; set; }
    }
}

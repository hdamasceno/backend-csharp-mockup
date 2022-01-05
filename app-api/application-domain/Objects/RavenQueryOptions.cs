using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using application_domain.Interfaces;

namespace application_domain.Objects
{
    public class RavenQueryOptions<IEntidade> : IQueryOption<IEntidade>
    {
        public int TakeNumber { get; set; }
        public int SkipNumber { get; set; }
        public IQueryable<IEntidade> QueryParams { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GossipDashboard.Repository
{
    interface IRepository<T>
    {
        T Add(T vm);

        T Update(T vm);

        T Delete(int id);

        T Select(int id);
        IQueryable<T> SelectAll(string condition);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using webCaisse.Db;
using webCaisse.Db.Repositories;

namespace webCaisse.uows
{
    public interface IUnitOfWork
    {
        IGenericRepository<T> Repos<T>() where T : class;
        void saveChanges();
    }
}

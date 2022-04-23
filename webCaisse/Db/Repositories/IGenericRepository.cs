using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace webCaisse.Db.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        T GetById(object id);
        T Create();
        void Insert(T obj);
        void Update(T obj);
        void Delete(object id);
        void DeleteObject(object _obj);
        void Save();
    }
}
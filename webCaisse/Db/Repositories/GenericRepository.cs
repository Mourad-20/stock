using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace webCaisse.Db.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private MyCtxOne _myCtxOne = null;

        private DbSet<T> table = null;

        public GenericRepository(MyCtxOne _myCtxOne)
        {
            this._myCtxOne = _myCtxOne;
            table = _myCtxOne.Set<T>();
        }
        public IQueryable<T> GetAll()
        {
            return table.AsQueryable<T>();
        }
        public T GetById(object id)
        {
            return table.Find(id);
        }
        public T Create()
        {
            return table.Create();
        }
        public void Insert(T obj)
        {
            table.Add(obj);
        }
        public void Update(T obj)
        {
            table.Attach(obj);
            _myCtxOne.Entry(obj).State = EntityState.Modified;
        }
        public void Delete(object id)
        {
            T existing = table.Find(id);
            table.Remove(existing);
        }
        public void Save()
        {
            _myCtxOne.SaveChanges();
        }

        public void DeleteObject(object _obj)
        {
            _myCtxOne.Entry(_obj).State = EntityState.Deleted;
        }
    }
}
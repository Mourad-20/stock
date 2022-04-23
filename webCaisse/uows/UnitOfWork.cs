using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using webCaisse.Db;
using webCaisse.Db.Repositories;
using webCaisse.Tools;

namespace webCaisse.uows
{
    public class UnitOfWork : IUnitOfWork
    {
        private MyCtxOne _myCtxOne;

        private Dictionary<Type, Object> _repositories = new Dictionary<Type, Object>();
        public UnitOfWork()
        {
            this._myCtxOne = IoCContainer.Resolve<MyCtxOne>();
        }
        public IGenericRepository<T> Repos<T>() where T : class
        {
            if (_repositories.Keys.Contains(typeof(T)) == true)
            {
                return _repositories[typeof(T)] as IGenericRepository<T>;
            }
            IGenericRepository<T> repo = new GenericRepository<T>(_myCtxOne);
            _repositories.Add(typeof(T), repo);
            return repo;
        }

        public void saveChanges()
        {
            _myCtxOne.SaveChanges();
        }
    }
}
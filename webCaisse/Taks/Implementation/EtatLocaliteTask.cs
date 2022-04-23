using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using webCaisse.Db.Entities;
using webCaisse.DMs;
using webCaisse.Taks.Contracts;
using webCaisse.Tools;
using webCaisse.uows;

namespace webCaisse.Taks.Implementation
{
    public class EtatLocaliteTask : IEtatLocaliteTask
    {
        private IUnitOfWork _uow = null;
        public EtatLocaliteTask()
        {
            _uow = IoCContainer.Resolve<IUnitOfWork>();
        }
        
        public EtatLocaliteDM getEtatLocaliteDMByCode(string _code)
        {
            return _uow.Repos<EtatLocalite>()
                .GetAll().Where(a => a.Code.ToUpper() == _code.ToUpper())
                .Select(a => new EtatLocaliteDM()
                {
                    Identifiant = a.Identifiant,
                    Code = a.Code,
                    Libelle = a.Libelle,
                }).FirstOrDefault();
        }
    }
}
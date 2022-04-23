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
    public class SituationCommandeTask : ISituationCommandeTask
    {
        private IUnitOfWork _uow = null;
        public SituationCommandeTask()
        {
            _uow = IoCContainer.Resolve<IUnitOfWork>();
        }
        
        public SituationCommandeDM getSituationCommandeDMByCode(string _code)
        {
            return _uow.Repos<SituationCommande>()
                .GetAll().Where(a => a.Code.ToUpper() == _code.ToUpper() && a.Affichable == 1)
                .Select(a => new SituationCommandeDM()
                {
                    Identifiant = a.Identifiant,
                    Code = a.Code,
                    EnActivite = a.EnActivite,
                    Libelle = a.Libelle,
                }).FirstOrDefault();
        }
    }
}
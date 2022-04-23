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
    public class EtatCommandeTask : IEtatCommandeTask
    {
        private IUnitOfWork _uow = null;
        public EtatCommandeTask()
        {
            _uow = IoCContainer.Resolve<IUnitOfWork>();
        }
        
        public EtatCommandeDM getEtatCommandeDMByCode(string _code)
        {
            return _uow.Repos<EtatCommande>()
                .GetAll().Where(a => a.Code.ToUpper() == _code.ToUpper() && a.Affichable == 1)
                .Select(a => new EtatCommandeDM()
                {
                    Identifiant = a.Identifiant,
                    Code = a.Code,
                    EnActivite = a.EnActivite,
                    Libelle = a.Libelle,
                }).FirstOrDefault();
        }
        public ICollection<EtatCommandeDM> getAllEtatCommandes()
        {
            return _uow.Repos<EtatCommande>()
                .GetAll().Where(a => a.Affichable == 1)
                .Select(a => new EtatCommandeDM()
                {
                    Identifiant = a.Identifiant,
                    Code = a.Code,
                    EnActivite = a.EnActivite,
                    Libelle = a.Libelle,
                }).ToList();
        }
    }
}
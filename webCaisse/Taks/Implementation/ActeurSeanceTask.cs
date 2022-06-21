using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Hosting;
using webCaisse.Db.Entities;
using webCaisse.DMs;
using webCaisse.DMs.Codes;
using webCaisse.Reports.DMs;
using webCaisse.Taks.Contracts;
using webCaisse.Tools;
using webCaisse.uows;

namespace webCaisse.Taks.Implementation
{
    public class ActeurSeanceTask : IActeurSeanceTask
    {
        private IUnitOfWork _uow = null;
        public ActeurSeanceTask()
        {
            _uow = IoCContainer.Resolve<IUnitOfWork>();
        }

        private void addActeurSeance(ActeurSeance obj)
        {
            //ok
            _uow.Repos<ActeurSeance>().Insert(obj);
            _uow.saveChanges();
        }

        private ActeurSeance getActeurSeanceById(long _identifiant)
        {
            //ok
            return _uow.Repos<ActeurSeance>().GetAll().Where(a => a.Identifiant == _identifiant).FirstOrDefault();
        }

        //=#########################################################################
        //=#########################################################################
        
        public long? addActeurSeanceDM(ActeurSeanceDM _acteurSeanceDM)
        {
            ActeurSeance _obj = _uow.Repos<ActeurSeance>().Create();
            _obj.Affichable = 1;
            _obj.EnActivite = 1;
            _obj.IdSeance = _acteurSeanceDM.IdSeance;
            _obj.IdUtilisateur = _acteurSeanceDM.IdUtilisateur;
            _uow.Repos<ActeurSeance>().Insert(_obj);
            _uow.saveChanges();
            return _obj.Identifiant;
        }

        public ICollection<ActeurSeanceDM> getActeurSeanceDMsByIdSeance(long _idSeance)
        {
            return _uow.Repos<ActeurSeance>()
                            .GetAll().Where(a => a.IdSeance == _idSeance && a.EnActivite == 1 && a.Affichable == 1)
                            .Select(a => new ActeurSeanceDM()
                            {
                                Identifiant = a.Identifiant,
                                IdSeance = a.IdSeance,
                                IdUtilisateur = a.IdUtilisateur,
                                NomPrenom = a.Utilisateur.Nom + " " + a.Utilisateur.Prenom,                                                                
                            }).ToList();
        }

        public void removeActeurSeanceDM(long _identifiant)
        {
            ActeurSeance _obj = _uow.Repos<ActeurSeance>().GetAll().Where(a => a.Identifiant == _identifiant && a.EnActivite == 1 && a.Affichable == 1).FirstOrDefault();
            _uow.Repos<ActeurSeance>().DeleteObject(_obj);
            _uow.saveChanges();
        }
        
    }
}
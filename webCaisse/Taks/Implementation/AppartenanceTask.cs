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
    public class AppartenanceTask : IAppartenanceTask
    {
        private IUnitOfWork _uow = null;
        public AppartenanceTask()
        {
            _uow = IoCContainer.Resolve<IUnitOfWork>();
        }

       
        private Appartenance getAppartenanceById(long _identifiant)
        {
            //ok
            return _uow.Repos<Appartenance>().GetAll().Where(a => a.Identifiant == _identifiant).FirstOrDefault();
        }

        //=#########################################################################
        public Int64 updateAppartenance(AppartenanceDM _appartenanceDM)
        {

            Appartenance _appartenanace = _uow.Repos<Appartenance>().GetAll().Where(a => a.Identifiant == _appartenanceDM.Identifiant && a.Affichable == 1 && a.EnActivite == 1).FirstOrDefault();
            _appartenanace.Affichable = 1;
            _appartenanace.EnActivite = 1;
            _appartenanace.IdUtilisateur = _appartenanceDM.IdUtilisateur;
            _appartenanace.IdGroupe = _appartenanceDM.IdGroupe;


            _uow.Repos<Appartenance>().Update(_appartenanace);
            _uow.saveChanges();
            return _appartenanace.Identifiant;
        }

        //=#########################################################################

        public Int64 addAppartenance(AppartenanceDM _appartenanceDM)
        {
            Appartenance _appartenanace = _uow.Repos<Appartenance>().Create();
            _appartenanace.Affichable = 1;
            _appartenanace.EnActivite = 1;
            _appartenanace.IdUtilisateur = _appartenanceDM.IdUtilisateur;
            _appartenanace.IdGroupe = _appartenanceDM.IdGroupe;


            _uow.Repos<Appartenance>().Insert(_appartenanace);
            _uow.saveChanges();
            return _appartenanace.Identifiant;
        }

        public ICollection<AppartenanceDM> getAppartenanceDMsByIdUtilisateur(long _idUtilisateur)
        {
            return _uow.Repos<Appartenance>()
                            .GetAll().Where(a => a.IdUtilisateur == _idUtilisateur && a.EnActivite == 1 && a.Affichable == 1)
                            .Select(a => new AppartenanceDM()
                            {
                                Identifiant = a.Identifiant,
                                IdGroupe = a.IdGroupe,
                                IdUtilisateur = a.IdUtilisateur,
                                LibelleGroupe = a.Groupe.Libelle,                                                                
                            }).ToList();
        }

        public void removeAppartenanceDM(long _identifiant)
        {
            Appartenance _obj = _uow.Repos<Appartenance>().GetAll().Where(a => a.Identifiant == _identifiant && a.EnActivite == 1 && a.Affichable == 1).FirstOrDefault();
            _uow.Repos<Appartenance>().DeleteObject(_obj);
            _uow.saveChanges();
        }
        
    }
}
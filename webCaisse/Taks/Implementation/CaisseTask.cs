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
    public class CaisseTask : ICaisseTask
    {
        private IUnitOfWork _uow = null;
        public CaisseTask()
        {
            _uow = IoCContainer.Resolve<IUnitOfWork>();
        }
        

        private Caisse getCaisseById(long _identifiant)
        {
            return _uow.Repos<Caisse>().GetAll().Where(a => a.Identifiant == _identifiant).FirstOrDefault();
        }

        //=#########################################################################
        //=#########################################################################
        
        public CaisseDM getCaisseDMById(long? _identifiant)
        {
            return _uow.Repos<Caisse>()
                            .GetAll().Where(a => a.Identifiant == _identifiant)
                            .Select(o=> new CaisseDM() {
                                Identifiant = o.Identifiant,
                                EnActivite = o.EnActivite,
                                Libelle = o.Libelle,
                            }).FirstOrDefault();
        }
        
        public ICollection<CaisseDM> getCaisseDMs([Optional] long? _enActivite)
        {
            bool _shouldGetResult = (_enActivite != null);
            ICollection<CaisseDM> _result = _uow.Repos<Caisse>().GetAll()
                .Where(
                    a =>
                    (_shouldGetResult)
                    && (a.Affichable == 1)
                    && (a.EnActivite == _enActivite)
                    ).Select(
                        o => new CaisseDM()
                        {
                            Identifiant = o.Identifiant,
                            Libelle = o.Libelle,
                            EnActivite = o.EnActivite,
                        }
                    ).ToList();
            return _result;
        }

        public CaisseDM getCaisseOfUtilisateur(long _idUtilisateur)
        {
            return _uow.Repos<AffectationCaisse>().GetAll()
                .Where(
                    a =>
                    (a.Affichable == 1)
                    && (a.EnActivite == 1)
                    && (a.IdUtilisateur == _idUtilisateur)
                    ).Select(
                        o => new CaisseDM()
                        {
                            Identifiant = (long)o.IdCaisse,
                            Libelle = o.Caisse.Libelle,
                            EnActivite = o.EnActivite,
                        }
                    ).FirstOrDefault();
        }

        public Int64? addCaisse(CaisseDM _caisseDM)
        {
            Caisse _caisse = _uow.Repos<Caisse>().Create();
            _caisse.Affichable = 1;
            _caisse.EnActivite = 1;
            _caisse.Libelle = _caisseDM.Libelle;
            _uow.Repos<Caisse>().Insert(_caisse);
            _uow.saveChanges();
            return _caisse.Identifiant;
        }
        public Int64? updateCaisse(CaisseDM _caisseDM)
        {
            Caisse _caisse = _uow.Repos<Caisse>().GetAll().Where(a => a.Identifiant == _caisseDM.Identifiant).FirstOrDefault();
            _caisse.Affichable = 1;
            _caisse.EnActivite = 1;
            _caisse.Libelle = _caisseDM.Libelle;

            _uow.Repos<Caisse>().Update(_caisse);
            _uow.saveChanges();
            return _caisseDM.Identifiant;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using webCaisse.Db.Entities;
using webCaisse.DMs;
using webCaisse.DMs.Codes;
using webCaisse.Taks.Contracts;
using webCaisse.Tools;
using webCaisse.uows;
using System.Runtime.InteropServices;

namespace webCaisse.Taks.Implementation
{
    public class SeanceTask : ISeanceTask
    {
        private IUnitOfWork _uow = null;
        public SeanceTask()
        {
            _uow = IoCContainer.Resolve<IUnitOfWork>();
        }

        public void fermerSeance(SeanceDM _seanceDM)
        {
            Seance _seance = _uow.Repos<Seance>().GetAll().Where(a => a.Identifiant == _seanceDM.Identifiant).FirstOrDefault();
            _seance.DateFin = _seanceDM.DateFin;
            _seance.MontantFin = _seanceDM.MontantFin;
            _seance.MontantPreleve = _seanceDM.MontantPreleve;
            _uow.Repos<Seance>().Update(_seance);
            _uow.saveChanges();
        }

        public long? ouvrirSeance(SeanceDM _seanceDM)
        {
            Seance _seance = _uow.Repos<Seance>().Create();
            _seance.Affichable = 1;
            _seance.EnActivite = 1;
            _seance.IdCaisse = _seanceDM.IdCaisse;
            _seance.IdUtilisateur = _seanceDM.IdUtilisateur;
            _seance.DateDebut = DateTime.Now;
            _seance.DateFin = null;
            _seance.MontantDebut = _seanceDM.MontantDebut;
            _seance.MontantFin = 0;
            _seance.MontantPreleve = 0;
            _seance.Numero = genererNumeroSeance();
            _uow.Repos<Seance>().Insert(_seance);
            _uow.saveChanges();
            return _seance.Identifiant;
        }

        public String genererNumeroSeance()
        {
            // voir ceci a modifier
            String _numero = "";
            DateTime _now = DateTime.Now;
            String _lastNumero = _uow.Repos<Seance>()
                .GetAll().Where(a => a.DateDebut.Value.Year == _now.Year && a.DateDebut.Value.Month == _now.Month && a.DateDebut.Value.Day == _now.Day && a.EnActivite == 1 && a.Affichable == 1).OrderByDescending(a => a.Identifiant).Select(a => a.Numero).FirstOrDefault();
            if (_lastNumero == null)
            {
                _numero = "1";
            }
            else
            {
                _numero = "" + (Int64.Parse(_lastNumero) + 1);
            }
            return _numero;
        }
        public SeanceDM getSeanceCaisse(Int64 _idCaisse)
        {
            return _uow.Repos<Seance>().GetAll()
                  .Where(a => a.DateFin == null && a.IdCaisse == _idCaisse)
                  .Select(a => new SeanceDM()
                  {
                      IdUtilisateur = a.IdUtilisateur,
                      NomPrenom = ((a.Utilisateur != null) ? a.Utilisateur.Nom + " " + a.Utilisateur.Prenom : ""),
                      DateDebut = a.DateDebut,
                      DateFin = a.DateFin,
                      EnActivite = a.EnActivite,
                      Identifiant = a.Identifiant,
                      MontantDebut = a.MontantDebut,
                      MontantFin = a.MontantFin,
                      MontantPreleve = a.MontantPreleve,
                      Numero = a.Numero,
                      IdCaisse = a.IdCaisse,
                      LibelleCaisse = ((a.Caisse != null) ? a.Caisse.Libelle : "")
                  })
                  .FirstOrDefault();
        }
        public SeanceDM getSeancebyId(Int64 _idSeance)
        {
            return _uow.Repos<Seance>().GetAll()
                  .Where(a => a.Identifiant == _idSeance)
                  .Select(a => new SeanceDM()
                  {
                      IdUtilisateur = a.IdUtilisateur,
                      NomPrenom = ((a.Utilisateur != null) ? a.Utilisateur.Nom + " " + a.Utilisateur.Prenom : ""),
                      DateDebut = a.DateDebut,
                      DateFin = a.DateFin,
                      EnActivite = a.EnActivite,
                      Identifiant = a.Identifiant,
                      MontantDebut = a.MontantDebut,
                      MontantFin = a.MontantFin,
                      MontantPreleve = a.MontantPreleve,
                      Numero = a.Numero,
                      IdCaisse = a.IdCaisse,
                      LibelleCaisse = ((a.Caisse != null) ? a.Caisse.Libelle : "")
                  })
                  .FirstOrDefault();
        }

        public SeanceDM getSeanceActive(Int64 _idUtilisateur)
        {
            SeanceDM _result = null;
            IGroupeTask _groupeTask = IoCContainer.Resolve<IGroupeTask>();
            IActeurSeanceTask _acteurSeanceTask = IoCContainer.Resolve<IActeurSeanceTask>();
            GroupeDM _groupeDM = _groupeTask.getGroupesOfUtilisateur(_idUtilisateur).FirstOrDefault();
            if (_groupeDM != null) {
                if (_groupeDM.Code == GroupeCode.CAISSIER) {
                    return _uow.Repos<Seance>().GetAll()
                    .Where(a => a.DateFin == null && a.IdUtilisateur == _idUtilisateur)
                    .Select(a => new SeanceDM()
                    {
                        IdUtilisateur = a.IdUtilisateur,
                        NomPrenom = ((a.Utilisateur != null) ? a.Utilisateur.Nom + " " + a.Utilisateur.Prenom : ""),
                        DateDebut = a.DateDebut,
                        DateFin = a.DateFin,
                        EnActivite = a.EnActivite,
                        Identifiant = a.Identifiant,
                        MontantDebut = a.MontantDebut,
                        MontantFin = a.MontantFin,
                        MontantPreleve = a.MontantPreleve,
                        Numero = a.Numero,
                        IdCaisse = a.IdCaisse,
                        LibelleCaisse = ((a.Caisse != null) ? a.Caisse.Libelle : "")
                    })
                    .FirstOrDefault();
                }
                else if (_groupeDM.Code == GroupeCode.SERVEUR) {
                    ICollection<SeanceDM> _seancesActives = getSeancesActives();
                    if (_seancesActives != null) {
                        foreach (SeanceDM _seanceDM in _seancesActives)
                        {
                            ICollection<ActeurSeanceDM> _acteurSeanceDMs = _acteurSeanceTask.getActeurSeanceDMsByIdSeance(_seanceDM.Identifiant);
                            if (_acteurSeanceDMs != null) {
                                if (_acteurSeanceDMs.Where(a => a.IdUtilisateur == _idUtilisateur).Count() > 0) {
                                    _result = _seanceDM;
                                    break;
                                }
                            }
                        }
                    }
                }
                else if (_groupeDM.Code == GroupeCode.ADMIN || _groupeDM.Code == GroupeCode.GERANT)
                {
                    ICollection<SeanceDM> _seancesActives = getSeancesActives();
                    if (_seancesActives != null)
                    {
                      
                        _result = _seancesActives.FirstOrDefault();
                    }
                }
            }
            return _result;
        }

        public ICollection<SeanceDM> getSeancesActives()
        {
            ICollection<SeanceDM> _result = null;
            _result = _uow.Repos<Seance>().GetAll()
                    .Where(a => a.DateFin == null)
                    .Select(a => new SeanceDM()
                    {
                        IdUtilisateur = a.IdUtilisateur,
                        NomPrenom = ((a.Utilisateur != null) ? a.Utilisateur.Nom + " " + a.Utilisateur.Prenom : ""),
                        DateDebut = a.DateDebut,
                        DateFin = a.DateFin,
                        EnActivite = a.EnActivite,
                        Identifiant = a.Identifiant,
                        MontantDebut = a.MontantDebut,
                        MontantFin = a.MontantFin,
                        MontantPreleve = a.MontantPreleve,
                        Numero = a.Numero,
                        IdCaisse = a.IdCaisse,
                        LibelleCaisse = ((a.Caisse != null) ? a.Caisse.Libelle : "")
                    }).ToList();
            return _result;
        }

        public ICollection<SeanceDM> getSeanceDMs([Optional]DateTime? _ddebut, [Optional]DateTime? _dfin)
        {
            bool _shouldGetResult =  (_ddebut != null) || (_dfin != null);
            ICollection<SeanceDM> _result = _uow.Repos<Seance>().GetAll()
                .Where(
                    a =>
                    (_shouldGetResult)
                    && (a.Affichable == 1)
                    && (a.EnActivite == 1)
                   
                     && ((_ddebut != null) ? a.DateDebut >= _ddebut : true)
                    && ((_dfin != null) ? a.DateDebut <= _dfin : true)
                    ).Select(
                        a => new SeanceDM()
                        {
                            IdUtilisateur = a.IdUtilisateur,
                            NomPrenom = ((a.Utilisateur != null) ? a.Utilisateur.Nom + " " + a.Utilisateur.Prenom : ""),
                            DateDebut = a.DateDebut,
                            DateFin = a.DateFin,
                            EnActivite = a.EnActivite,
                            Identifiant = a.Identifiant,
                            MontantDebut = a.MontantDebut,
                            MontantFin = a.MontantFin,
                            MontantPreleve = a.MontantPreleve,
                            Numero = a.Numero,
                            IdCaisse = a.IdCaisse,
                            LibelleCaisse = ((a.Caisse != null) ? a.Caisse.Libelle : "")
                        }
                    ).ToList();
            return _result;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using webCaisse.Db.Entities;
using webCaisse.DMs;
using webCaisse.DMs.Codes;
using webCaisse.Taks.Contracts;
using webCaisse.Tools;
using webCaisse.uows;

namespace webCaisse.Taks.Implementation
{
    public class UtilisateurTask : IUtilisateurTask
    {
        private IUnitOfWork _uow = null;
        public UtilisateurTask()
        {
            _uow = IoCContainer.Resolve<IUnitOfWork>();
        }
        private Utilisateur getUtilisateurById(long _identifiant)
        {
            //ok
            return _uow.Repos<Utilisateur>().GetAll().Where(a => a.Identifiant == _identifiant).FirstOrDefault();
        }

        public UtilisateurDM authentification(string _login, string _password)
        {
            return _uow.Repos<Utilisateur>()
                .GetAll()
                .Where(a => a.Login == _login && a.Password == _password && a.EnActivite == 1 && a.Affichable == 1)
                .Select(a => new UtilisateurDM()
                {
                    Identifiant = a.Identifiant,
                    Nom = a.Nom,
                    Prenom = a.Prenom,
                    EnActivite = a.EnActivite,
                })
                .FirstOrDefault();
        }
        public UtilisateurDM authentificationJeton(string _jeton)
        {
            return _uow.Repos<Utilisateur>()
                .GetAll()
                .Where(a => a.Jeton == _jeton && a.EnActivite == 1 && a.Affichable == 1)
                .Select(a => new UtilisateurDM()
                {
                    Identifiant = a.Identifiant,
                    Nom = a.Nom,
                    Prenom = a.Prenom,
                    EnActivite = a.EnActivite,
                })
                .FirstOrDefault();
        }

        public void bloquerUtilisateur(long _identifiant)
        {
            throw new NotImplementedException();
        }

        public void debloquerUtilisateur(long _identifiant)
        {
            throw new NotImplementedException();
        }
        
        public ICollection<UtilisateurDM> getUtilisateurByGroupe(string _code)
        {
            IGroupeTask _groupeTask = IoCContainer.Resolve<IGroupeTask>();
            GroupeDM _groupeDM = _groupeTask.getGroupeDMByCode(_code);
            return _uow.Repos<Appartenance>()
                .GetAll()
                .Where(a => a.IdGroupe == _groupeDM.Identifiant && a.Affichable == 1)
                .Select(a => new UtilisateurDM()
                {
                    Identifiant = (Int64) a.IdUtilisateur,
                    Nom = a.Utilisateur.Nom,
                    Prenom = a.Utilisateur.Prenom,
                    EnActivite = a.EnActivite,
                })
                .ToList();
        }

        public UtilisateurDM getUtilisateurDMByIdentifiant(long _identifiant)
        {
            return _uow.Repos<Utilisateur>()
                            .GetAll().Where(a => a.Identifiant == _identifiant)
                            .Select(o => new UtilisateurDM()
                            {
                                Identifiant = o.Identifiant,
                                EnActivite = o.EnActivite,
                                Login = o.Login,
                                Nom = o.Nom,
                                Prenom = o.Prenom,
                            }).FirstOrDefault();
        }

        public ICollection<UtilisateurDM> getServeursOfSeance(long _idSeance)
        {
            ICollection<UtilisateurDM> _result = null;
            IGroupeTask _groupeTask = IoCContainer.Resolve<IGroupeTask>();
            ICollection<ActeurSeance> _acteurSeances = _uow.Repos<ActeurSeance>()
                            .GetAll().Where(a => a.IdSeance == _idSeance).ToList();
            if (_acteurSeances != null) {
                _result = new List<UtilisateurDM>();
                foreach (ActeurSeance _acteurSeance in _acteurSeances) {
                    ICollection<GroupeDM> _groupeDMs = _groupeTask.getGroupesOfUtilisateur((long)_acteurSeance.IdUtilisateur);
                    if (_groupeDMs != null && _groupeDMs.Where(a => a.Code.ToUpper() == GroupeCode.SERVEUR).Count() > 0) {
                        UtilisateurDM _utilisateurDM = getUtilisateurDMByIdentifiant((long)_acteurSeance.IdUtilisateur);
                        _result.Add(_utilisateurDM);
                    } 
                }
            }
            return _result;
        }

        public bool isUtilisateurOnGroupes(ICollection<GroupeDM> _groupeDMs, List<String> _groupeCodes)
        {
            Boolean _appartient = false;
            if (_groupeCodes != null && _groupeCodes.Count > 0 && _groupeDMs != null && _groupeDMs.Count > 0) {
                if (_groupeDMs != null)
                {
                    foreach (String _code in _groupeCodes) {
                        GroupeDM _groupeDM = _groupeDMs.Where(a => a.Code.ToUpper() == _code.ToUpper()).FirstOrDefault();
                        if (_groupeDM != null) {
                            _appartient = true;
                            break;
                        }
                    }
                }
            }
            return _appartient;
        }

        public ICollection<UtilisateurDM> getListeUtilisateurDMs([Optional] Int64? _enActivite)
        {
            bool _shouldGetResult = (_enActivite != null);
            _shouldGetResult = (_shouldGetResult != false) ? _shouldGetResult : true;
            ICollection <UtilisateurDM> _result = _uow.Repos<Utilisateur>().GetAll()
                .Where(
                    a =>
                    (_shouldGetResult)
                    && (a.Affichable == 1)
                    && ((_enActivite != null) ? a.EnActivite == _enActivite : true)
                    ).Select(
                        o => new UtilisateurDM()
                        {
                            Identifiant = o.Identifiant,
                            EnActivite = o.EnActivite,
                            Login = o.Login,
                            Nom = o.Nom,
                            Prenom = o.Prenom,
                        }
                    ).ToList();
            return _result;
        }

        public ICollection<UtilisateurDM> getServeursOfCaisse(long _idCaisse)
        {
            ICollection<UtilisateurDM> _result = null;
            IGroupeTask _groupeTask = IoCContainer.Resolve<IGroupeTask>();
            ICollection<AffectationCaisse> _affectationCaisses = _uow.Repos<AffectationCaisse>()
                            .GetAll().Where(a => a.IdCaisse == _idCaisse).ToList();
            if (_affectationCaisses != null)
            {
                _result = new List<UtilisateurDM>();
                foreach (AffectationCaisse _affectationCaisse in _affectationCaisses)
                {
                    ICollection<GroupeDM> _groupeDMs = _groupeTask.getGroupesOfUtilisateur((long)_affectationCaisse.IdUtilisateur);
                    if (_groupeDMs != null && _groupeDMs.Where(a => a.Code.ToUpper() == GroupeCode.SERVEUR).Count() > 0)
                    {
                        UtilisateurDM _utilisateurDM = getUtilisateurDMByIdentifiant((long)_affectationCaisse.IdUtilisateur);
                        _result.Add(_utilisateurDM);
                    }
                }
            }
            return _result;
        }

        public Int64? addUtilisateur(UtilisateurDM _utilisateurDM)
        {
            Utilisateur _utilisateur = _uow.Repos<Utilisateur>().Create();
            _utilisateur.Affichable = 1;
            _utilisateur.EnActivite = 1;
            _utilisateur.Nom = _utilisateurDM.Nom;
            _utilisateur.Prenom = _utilisateurDM.Prenom;
            _utilisateur.Login = _utilisateurDM.Login;
            _utilisateur.Password = _utilisateurDM.Password;
            _utilisateur.Jeton = _utilisateurDM.Jeton;

            _uow.Repos<Utilisateur>().Insert(_utilisateur);
            _uow.saveChanges();




            return _utilisateur.Identifiant;
        }

        public Int64? updateUtilisateur(UtilisateurDM _utilisateurDM)
        {
            Utilisateur _utilisateur = _uow.Repos<Utilisateur>().GetAll().Where(a => a.Identifiant == _utilisateurDM.Identifiant).FirstOrDefault();
            _utilisateur.Affichable = 1;
            _utilisateur.EnActivite = 1;
            _utilisateur.Nom = _utilisateurDM.Nom;
            _utilisateur.Prenom = _utilisateurDM.Prenom;
            _utilisateur.Login = _utilisateurDM.Login;
            _utilisateur.Password = _utilisateurDM.Password;
            _utilisateur.Jeton = _utilisateurDM.Jeton;
            _uow.Repos<Utilisateur>().Update(_utilisateur);
            _uow.saveChanges();
            return _utilisateurDM.Identifiant;
        }

    }
}
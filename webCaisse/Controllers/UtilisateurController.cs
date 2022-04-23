using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using webCaisse.Db;
using webCaisse.Db.Entities;
using webCaisse.DMs;
using webCaisse.DMs.Codes;
using webCaisse.Filters;
using webCaisse.Mappers;
using webCaisse.Taks.Contracts;
using webCaisse.Tools;
using webCaisse.VMs.Models;
using webCaisse.VMs.Params;

namespace webCaisse.Controllers
{
    public class UtilisateurController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage ajouterUtilisateur(UtilisateurVM utilisateurVM)
        {
            Dictionary<String, Object> _model = new Dictionary<String, Object>();
            HttpState _httpState = new HttpState();
            EtatReponse _etatRep = new EtatReponse();
            Int64? _idUtilisateur = null;
            Int64? _idappartenanace = null;
            try
            {
                //Int64? _idUtilisateur = TokenManager.getIdentifiantFromToken(Request);
                IUtilisateurTask _utilisateurTask = IoCContainer.Resolve<IUtilisateurTask>();
                IAppartenanceTask _appartenanceTask = IoCContainer.Resolve<IAppartenanceTask>();
                IGroupeTask _groupeTask = IoCContainer.Resolve<IGroupeTask>();
                //-----------------------------------------------------------------
                String _message = "";
                Boolean _isOk = true;

                //ICollection<GroupeDM> _groupeDMs = _groupeTask.getGroupesOfUtilisateur((long)_idUtilisateur);

              

                //-----------------------------------------------------------------
                if (_isOk)
                {


                    UtilisateurDM _utilisateurDM = UtilisateurMapper.UtilisateurVMtoUtilisateurDM(utilisateurVM);


                    _idUtilisateur = _utilisateurTask.addUtilisateur(_utilisateurDM);

                    if (_idUtilisateur != null)
                    {
                        string code = utilisateurVM.CodesGroupes.FirstOrDefault();
                        GroupeDM _groupeDM = _groupeTask.getGroupeDMByCode(code);
                        if (_groupeDM != null)
                        {
                            AppartenanceDM _appartenanceDM = new AppartenanceDM();
                            _appartenanceDM.IdUtilisateur = (int)_idUtilisateur;
                            _appartenanceDM.IdGroupe = (int)_groupeDM.Identifiant;

                            _idappartenanace = _appartenanceTask.addAppartenance(_appartenanceDM);
                        }
                    }
                    _etatRep = new EtatReponse() { Code = EtatReponseCode.SUCCESS, Message = "RETURN OK" };
                }
                else
                {
                    _etatRep = new EtatReponse() { Code = EtatReponseCode.DANGER, Message = _message };
                }
                

            }
            catch (Exception e)
            {
                _httpState = new HttpState() { Code = HttpStateCode.ERROR, Message = e.Message };
                MyLogger.log(Utilitaire.getEmplacement(this) + ":\n" + Utilitaire.getDetailsException(e), MyLoggerCode.STANDARD);
            }
            _model.Add(HttpState.Name, _httpState);
            _model.Add(EtatReponse.Name, _etatRep);
            _model.Add("idUtilisateur", _idUtilisateur);
            return Utilitaire.constructResponse(this, _model);
        }

        [HttpPost]
        public HttpResponseMessage modifierUtilisateur(UtilisateurVM utilisateurVM)
        {
            Dictionary<String, Object> _model = new Dictionary<String, Object>();
            HttpState _httpState = new HttpState();
            EtatReponse _etatRep = new EtatReponse();
            Int64? _idUtilisateur = null;
            Int64? _idappartenanace = null;
            try
            {
                //Int64? _idUtilisateur = TokenManager.getIdentifiantFromToken(Request);
                IUtilisateurTask _utilisateurTask = IoCContainer.Resolve<IUtilisateurTask>();
                IAppartenanceTask _appartenanceTask = IoCContainer.Resolve<IAppartenanceTask>();
                IGroupeTask _groupeTask = IoCContainer.Resolve<IGroupeTask>();
                //-----------------------------------------------------------------
                String _message = "";
                Boolean _isOk = true;

                //ICollection<GroupeDM> _groupeDMs = _groupeTask.getGroupesOfUtilisateur((long)_idUtilisateur);

              

                //-----------------------------------------------------------------
                if (_isOk)
                {

                    UtilisateurDM _utilisateurDM = UtilisateurMapper.UtilisateurVMtoUtilisateurDM(utilisateurVM);
                    _idUtilisateur = _utilisateurTask.updateUtilisateur(_utilisateurDM);

                    if (_idUtilisateur != null)
                    {

                        string code = utilisateurVM.CodesGroupes.FirstOrDefault();
                        GroupeDM _groupeOregineDM = _groupeTask.getGroupesOfUtilisateur((int)_idUtilisateur).FirstOrDefault();
                        GroupeDM _groupeDM = _groupeTask.getGroupeDMByCode(code);
                        if (_groupeDM != null)
                        {
                            AppartenanceDM _appartenanceOregineDM = _appartenanceTask.getAppartenanceDMsByIdUtilisateur((int)_idUtilisateur)
                                .Where(a=> a.IdGroupe == _groupeOregineDM.Identifiant).FirstOrDefault();


                            _appartenanceOregineDM.IdUtilisateur = (int)_idUtilisateur;
                            _appartenanceOregineDM.IdGroupe = (int)_groupeDM.Identifiant;

                            _idappartenanace = _appartenanceTask.updateAppartenance(_appartenanceOregineDM);
                        }
                    }
                    _etatRep = new EtatReponse() { Code = EtatReponseCode.SUCCESS, Message = "RETURN OK" };
                }
                else
                {
                    _etatRep = new EtatReponse() { Code = EtatReponseCode.DANGER, Message = _message };
                }
            }
            catch (Exception e)
            {
                _httpState = new HttpState() { Code = HttpStateCode.ERROR, Message = e.Message };
                MyLogger.log(Utilitaire.getEmplacement(this) + ":\n" + Utilitaire.getDetailsException(e), MyLoggerCode.STANDARD);
            }
            _model.Add(HttpState.Name, _httpState);
            _model.Add(EtatReponse.Name, _etatRep);
            return Utilitaire.constructResponse(this, _model);
        }

        [HttpPost]
        public HttpResponseMessage getListeUtilisateurs()
        {
            Dictionary<String, Object> _model = new Dictionary<String, Object>();
            HttpState _httpState = new HttpState();
            EtatReponse _etatRep = new EtatReponse();
            IGroupeTask _groupeTask = IoCContainer.Resolve<IGroupeTask>();

            ICollection<UtilisateurVM> _utilisateurVMs = null;
            try
            {
                IUtilisateurTask _utilisateurTask = IoCContainer.Resolve<IUtilisateurTask>();
                ICollection<UtilisateurDM> _utilisateurDMs = _utilisateurTask.getListeUtilisateurDMs();
                if (_utilisateurDMs != null)
                {
                    _utilisateurVMs = new List<UtilisateurVM>();
                    foreach (UtilisateurDM _obj in _utilisateurDMs)
                    {
                        UtilisateurVM _utilisateurVM = UtilisateurMapper.UtilisateurDMtoUtilisateurVM(_obj);
                         _utilisateurVM.CodesGroupes = new List<string>();
                        ICollection<GroupeDM> _groupeDMs = _groupeTask.getGroupesOfUtilisateur(_utilisateurVM.Identifiant);
                        if (_groupeDMs != null)
                        {
                            foreach (GroupeDM _groupeDM in _groupeDMs)
                            {
                                GroupeVM _groupeVM = GroupeMapper.GroupeDMtoGroupeVM(_groupeDM);
                                _utilisateurVM.CodesGroupes.Add(_groupeVM.Code);
                            }
                        }
                        _utilisateurVMs.Add(_utilisateurVM);
                    }
                }
                _etatRep = new EtatReponse() { Code = EtatReponseCode.SUCCESS, Message = "RETURN OK" };
            }
            catch (Exception e)
            {
                _httpState = new HttpState() { Code = HttpStateCode.ERROR, Message = e.Message };
                MyLogger.log(Utilitaire.getEmplacement(this) + ":\n" + Utilitaire.getDetailsException(e), MyLoggerCode.STANDARD);
            }

            _model.Add("utilisateurVMs", _utilisateurVMs);
            _model.Add(HttpState.Name, _httpState);
            _model.Add(EtatReponse.Name, _etatRep);
            return Utilitaire.constructResponse(this, _model);
        }

        [HttpPost]
        public HttpResponseMessage checkerConnection()
        {
            Dictionary<String, Object> _model = new Dictionary<String, Object>();
            HttpState _httpState = new HttpState();
            EtatReponse _etatRep = new EtatReponse();
            String _message = null;
            try
            {
                _message = "CHECKER OK";
                _etatRep = new EtatReponse() { Code = EtatReponseCode.SUCCESS, Message = "RETURN OK" };
            }
            catch (Exception e)
            {
                _httpState = new HttpState() { Code = HttpStateCode.ERROR, Message = e.Message };
                MyLogger.log(Utilitaire.getEmplacement(this) + ":\n" + Utilitaire.getDetailsException(e), MyLoggerCode.STANDARD);
            }
            _model.Add("message", _message);
            _model.Add(HttpState.Name, _httpState);
            _model.Add(EtatReponse.Name, _etatRep);
            return Utilitaire.constructResponse(this, _model);
        }

        [HttpPost]
        public HttpResponseMessage authentification(LoginVM loginVM)
        {
            Dictionary<String, Object> _model = new Dictionary<String, Object>();
            HttpState _httpState = new HttpState();
            EtatReponse _etatRep = new EtatReponse();
            UtilisateurVM _utilisateurVM = null;
            try
            {
                if (loginVM != null && loginVM.Login != null && loginVM.Password != null)
                {
                    IUtilisateurTask _utilisateurTask = IoCContainer.Resolve<IUtilisateurTask>();
                    IGroupeTask _groupeTask = IoCContainer.Resolve<IGroupeTask>();
                    UtilisateurDM _utilisateurDM = _utilisateurTask.authentification(loginVM.Login, loginVM.Password);
                    if (_utilisateurDM != null)
                    {
                        if (_utilisateurDM.EnActivite == 1)
                        {
                            //---------------------------------------
                            String _newToken = TokenManager.genererToken(_utilisateurDM.Identifiant);
                            HttpResponseMessage response = this.ActionContext.Response;
                            if (response.Headers.Contains("authorization"))
                            {
                                response.Headers.Remove("authorization");
                            }
                            response.Headers.Add("Authorization", "Bearer " + _newToken);
                            //---------------------------------------                            
                            _utilisateurVM = new UtilisateurVM()
                            {
                                Identifiant = _utilisateurDM.Identifiant,
                                Nom = _utilisateurDM.Nom,
                                Prenom = _utilisateurDM.Prenom,
                                EnActivite = _utilisateurDM.EnActivite,
                            };
                            ICollection<GroupeDM> _groupeDMs = _groupeTask.getGroupesOfUtilisateur(_utilisateurVM.Identifiant);
                            if (_groupeDMs != null)
                            {
                                _utilisateurVM.CodesGroupes = _groupeDMs.Select(a => a.Code).ToList();
                            }
                            _etatRep = new EtatReponse() { Code = EtatReponseCode.SUCCESS, Message = "RETURN OK" };

                        }
                        else
                        {
                            _etatRep = new EtatReponse() { Code = EtatReponseCode.DANGER, Message = "Le compte est bloqué." };
                        }
                    }
                    else
                    {
                        _etatRep = new EtatReponse() { Code = EtatReponseCode.DANGER, Message = "Les informations d'authentification sont incorrectes" };
                    }

                }
                else
                {
                    _etatRep = new EtatReponse() { Code = EtatReponseCode.DANGER, Message = "PARAM NOT VALID" };
                }

            }
            catch (Exception e)
            {
                _httpState = new HttpState() { Code = HttpStateCode.ERROR, Message = e.Message };
                MyLogger.log(Utilitaire.getEmplacement(this) + ":\n" + Utilitaire.getDetailsException(e), MyLoggerCode.STANDARD);
            }

            _model.Add("utilisateurVM", _utilisateurVM);
            _model.Add(HttpState.Name, _httpState);
            _model.Add(EtatReponse.Name, _etatRep);
            return Utilitaire.constructResponse(this, _model);
        }
        [HttpPost]
        public HttpResponseMessage AuthentificationJeton(LoginVM loginVM)
        {
            Dictionary<String, Object> _model = new Dictionary<String, Object>();
            HttpState _httpState = new HttpState();
            EtatReponse _etatRep = new EtatReponse();
            UtilisateurVM _utilisateurVM = null;
            try
            {
                if (loginVM != null && loginVM.Jeton != null)
                {
                    IUtilisateurTask _utilisateurTask = IoCContainer.Resolve<IUtilisateurTask>();
                    IGroupeTask _groupeTask = IoCContainer.Resolve<IGroupeTask>();
                    UtilisateurDM _utilisateurDM = _utilisateurTask.authentificationJeton(loginVM.Jeton);
                    if (_utilisateurDM != null)
                    {
                        if (_utilisateurDM.EnActivite == 1)
                        {
                            //---------------------------------------
                            String _newToken = TokenManager.genererToken(_utilisateurDM.Identifiant);
                            HttpResponseMessage response = this.ActionContext.Response;
                            if (response.Headers.Contains("authorization"))
                            {
                                response.Headers.Remove("authorization");
                            }
                            response.Headers.Add("Authorization", "Bearer " + _newToken);
                            //---------------------------------------                            
                            _utilisateurVM = new UtilisateurVM()
                            {
                                Identifiant = _utilisateurDM.Identifiant,
                                Nom = _utilisateurDM.Nom,
                                Prenom = _utilisateurDM.Prenom,
                                EnActivite = _utilisateurDM.EnActivite,
                            };
                            ICollection<GroupeDM> _groupeDMs = _groupeTask.getGroupesOfUtilisateur(_utilisateurVM.Identifiant);
                            if (_groupeDMs != null)
                            {
                                _utilisateurVM.CodesGroupes = _groupeDMs.Select(a => a.Code).ToList();
                            }
                            _etatRep = new EtatReponse() { Code = EtatReponseCode.SUCCESS, Message = "RETURN OK" };

                        }
                        else
                        {
                            _etatRep = new EtatReponse() { Code = EtatReponseCode.DANGER, Message = "Le compte est bloqué." };
                        }
                    }
                    else
                    {
                        _etatRep = new EtatReponse() { Code = EtatReponseCode.DANGER, Message = "Les informations d'authentification sont incorrectes" };
                    }

                }
                else
                {
                    _etatRep = new EtatReponse() { Code = EtatReponseCode.DANGER, Message = "PARAM NOT VALID" };
                }

            }
            catch (Exception e)
            {
                _httpState = new HttpState() { Code = HttpStateCode.ERROR, Message = e.Message };
                MyLogger.log(Utilitaire.getEmplacement(this) + ":\n" + Utilitaire.getDetailsException(e), MyLoggerCode.STANDARD);
            }

            _model.Add("utilisateurVM", _utilisateurVM);
            _model.Add(HttpState.Name, _httpState);
            _model.Add(EtatReponse.Name, _etatRep);
            return Utilitaire.constructResponse(this, _model);
        }

        [HttpPost]
        public HttpResponseMessage GetInfoUtilisateur()
        {
            Dictionary<String, Object> _model = new Dictionary<String, Object>();
            HttpState _httpState = new HttpState();
            EtatReponse _etatRep = new EtatReponse();
            UtilisateurVM _utilisateurVM = null;
            try
            {
                IGroupeTask _groupeTask = IoCContainer.Resolve<IGroupeTask>();
                IUtilisateurTask _utilisateurTask = IoCContainer.Resolve<IUtilisateurTask>();
                Int64? _idUtilisateur = TokenManager.getIdentifiantFromToken(Request);
                UtilisateurDM _utilisateurDM = _utilisateurTask.getUtilisateurDMByIdentifiant((long)_idUtilisateur);
                if (_utilisateurDM != null)
                {
                    _utilisateurVM = UtilisateurMapper.UtilisateurDMtoUtilisateurVM(_utilisateurDM);
                    ICollection<GroupeDM> _groupeDMs = _groupeTask.getGroupesOfUtilisateur(_utilisateurVM.Identifiant);
                    if (_groupeDMs != null)
                    {
                        _utilisateurVM.CodesGroupes = _groupeDMs.Select(a => a.Code).ToList();
                    }
                }
                _etatRep = new EtatReponse() { Code = EtatReponseCode.SUCCESS, Message = "RETURN OK" };

            }
            catch (Exception e)
            {
                _httpState = new HttpState() { Code = HttpStateCode.ERROR, Message = e.Message };
                MyLogger.log(Utilitaire.getEmplacement(this) + ":\n" + Utilitaire.getDetailsException(e), MyLoggerCode.STANDARD);
            }
            _model.Add("utilisateurVM", _utilisateurVM);
            _model.Add(HttpState.Name, _httpState);
            _model.Add(EtatReponse.Name, _etatRep);
            return Utilitaire.constructResponse(this, _model);

        }

        [HttpPost]
        public HttpResponseMessage getListeServeurs(ParamInt paramInt)
        {
            Int64? _idSeanceparam = paramInt.Valeur;
            Dictionary<String, Object> _model = new Dictionary<String, Object>();
            HttpState _httpState = new HttpState();
            EtatReponse _etatRep = new EtatReponse();
            ICollection<UtilisateurVM> _utilisateurVMs = null;
            try
            {
                IGroupeTask _groupeTask = IoCContainer.Resolve<IGroupeTask>();
                IUtilisateurTask _utilisateurTask = IoCContainer.Resolve<IUtilisateurTask>();
                ISeanceTask _seanceTask = IoCContainer.Resolve<ISeanceTask>();
                Int64? _idUtilisateur = TokenManager.getIdentifiantFromToken(Request);
                ICollection<GroupeDM> _groupeDMs = _groupeTask.getGroupesOfUtilisateur((long)_idUtilisateur);
                if (_utilisateurTask.isUtilisateurOnGroupes(_groupeDMs, new List<String>() { GroupeCode.CAISSIER, GroupeCode.ADMIN, GroupeCode.GERANT})) {
                    if (_idSeanceparam == null)
                    {
                        SeanceDM _seanceDM = _seanceTask.getSeanceActive((long)_idUtilisateur);
                        _idSeanceparam = _seanceDM.Identifiant;
                    }
                    if (_idSeanceparam != null) {
                        ICollection<UtilisateurDM> _utilisateurDMs = _utilisateurTask.getServeursOfSeance((long)_idSeanceparam);
                        if (_utilisateurDMs != null)
                        {
                            _utilisateurVMs = new List<UtilisateurVM>();
                            foreach (UtilisateurDM _utilisateurDM in _utilisateurDMs)
                            {
                                UtilisateurVM _utilisateurVM = UtilisateurMapper.UtilisateurDMtoUtilisateurVM(_utilisateurDM);
                                _utilisateurVMs.Add(_utilisateurVM);
                            }
                        }
                    }
                }
                _etatRep = new EtatReponse() { Code = EtatReponseCode.SUCCESS, Message = "RETURN OK" };
            }
            catch (Exception e)
            {
                _httpState = new HttpState() { Code = HttpStateCode.ERROR, Message = e.Message };
                MyLogger.log(Utilitaire.getEmplacement(this) + ":\n" + Utilitaire.getDetailsException(e), MyLoggerCode.STANDARD);
            }
            _model.Add("utilisateurVMs", _utilisateurVMs);
            _model.Add(HttpState.Name, _httpState);
            _model.Add(EtatReponse.Name, _etatRep);
            return Utilitaire.constructResponse(this, _model);

        }
        [HttpPost]
        public HttpResponseMessage getListeLogins()
        {
            Dictionary<String, Object> _model = new Dictionary<String, Object>();
            HttpState _httpState = new HttpState();
            EtatReponse _etatRep = new EtatReponse();
            ICollection<LoginVM>_loginVMs = null;
            try
            {
                IGroupeTask _groupeTask = IoCContainer.Resolve<IGroupeTask>();
                IUtilisateurTask _utilisateurTask = IoCContainer.Resolve<IUtilisateurTask>();
                ICollection<UtilisateurDM> _utilisateurDMs = _utilisateurTask.getListeUtilisateurDMs(_enActivite : 1);
                if (_utilisateurDMs != null) {
                    _loginVMs = new List<LoginVM>();
                    foreach (UtilisateurDM _utilisateurDM in _utilisateurDMs) {
                        ICollection<GroupeDM> _groupeDMs = _groupeTask.getGroupesOfUtilisateur(_utilisateurDM.Identifiant);
                        if (_groupeDMs != null && _groupeDMs.Any(x => x.Code.Equals(GroupeCode.CAISSIER) || x.Code.Equals(GroupeCode.SERVEUR)))
                        {
                            
                            _loginVMs.Add(new LoginVM() { Login = _utilisateurDM.Login });
                        }
                       
                    }
                }
                _etatRep = new EtatReponse() { Code = EtatReponseCode.SUCCESS, Message = "RETURN OK" };
            }
            catch (Exception e)
            {
                _httpState = new HttpState() { Code = HttpStateCode.ERROR, Message = e.Message };
                MyLogger.log(Utilitaire.getEmplacement(this) + ":\n" + Utilitaire.getDetailsException(e), MyLoggerCode.STANDARD);
            }
            _model.Add("loginVMs", _loginVMs);
            _model.Add(HttpState.Name, _httpState);
            _model.Add(EtatReponse.Name, _etatRep);
            return Utilitaire.constructResponse(this, _model);

        }

        [HttpPost]
        public HttpResponseMessage getListeCaissiers()
        {
            Dictionary<String, Object> _model = new Dictionary<String, Object>();
            HttpState _httpState = new HttpState();
            EtatReponse _etatRep = new EtatReponse();
            ICollection<UtilisateurVM> _utilisateurVMs = null;
            try
            {
                IGroupeTask _groupeTask = IoCContainer.Resolve<IGroupeTask>();
                IUtilisateurTask _utilisateurTask = IoCContainer.Resolve<IUtilisateurTask>();
                ICollection<UtilisateurDM> _utilisateurDMs = _utilisateurTask.getListeUtilisateurDMs(_enActivite: 1);
                if (_utilisateurDMs != null)
                {
                    _utilisateurVMs = new List<UtilisateurVM>();
                    foreach (UtilisateurDM _utilisateurDM in _utilisateurDMs)
                    {
                        ICollection<GroupeDM> _groupeDMs = _groupeTask.getGroupesOfUtilisateur(_utilisateurDM.Identifiant);
                        if (_groupeDMs != null && _groupeDMs.Any(x => x.Code.Equals(GroupeCode.CAISSIER) || x.Code.Equals(GroupeCode.SERVEUR)))
                        {
                            UtilisateurVM _utilisateurVM = UtilisateurMapper.UtilisateurDMtoUtilisateurVM(_utilisateurDM);
                            _utilisateurVMs.Add(_utilisateurVM);
                        }

                    }
                }
                _etatRep = new EtatReponse() { Code = EtatReponseCode.SUCCESS, Message = "RETURN OK" };
            }
            catch (Exception e)
            {
                _httpState = new HttpState() { Code = HttpStateCode.ERROR, Message = e.Message };
                MyLogger.log(Utilitaire.getEmplacement(this) + ":\n" + Utilitaire.getDetailsException(e), MyLoggerCode.STANDARD);
            }
            _model.Add("utilisateurVMs", _utilisateurVMs);
            _model.Add(HttpState.Name, _httpState);
            _model.Add(EtatReponse.Name, _etatRep);
            return Utilitaire.constructResponse(this, _model);

        }
        [HttpPost]
        public HttpResponseMessage getGroupeTemporaire()
        {
            Dictionary<String, Object> _model = new Dictionary<String, Object>();
            HttpState _httpState = new HttpState();
            EtatReponse _etatRep = new EtatReponse();
            try
            {
                Int64? _idUtilisateur = TokenManager.getIdentifiantFromToken(Request);
                IGroupeTask _groupeTask = IoCContainer.Resolve<IGroupeTask>();
                ICollection<GroupeDM> _groupeDMs = _groupeTask.getGroupesOfUtilisateur((long)_idUtilisateur);
                String _groupesAsString = string.Join(" - ", _groupeDMs.Select(a => a.Code).ToList());
                _etatRep = new EtatReponse() { Code = EtatReponseCode.SUCCESS, Message = _groupesAsString };
            }
            catch (Exception e)
            {
                _httpState = new HttpState() { Code = HttpStateCode.ERROR, Message = e.Message };
                MyLogger.log(Utilitaire.getEmplacement(this) + ":\n" + Utilitaire.getDetailsException(e), MyLoggerCode.STANDARD);
            }
            _model.Add(HttpState.Name, _httpState);
            _model.Add(EtatReponse.Name, _etatRep);
            return Utilitaire.constructResponse(this, _model);

        }

        //############################################################################################
        [HttpGet]
        public HttpResponseMessage Test(Int64 idCommande)
        {
            Int64? _idCommande = idCommande;
            Dictionary<String, Object> _model = new Dictionary<String, Object>();
            HttpState _httpState = new HttpState();
            EtatReponse _etatRep = new EtatReponse();
            CommandeVM _commandeVM = null;
            try
            {
                ICommandeTask _commandeTask = IoCContainer.Resolve<ICommandeTask>();
                IDetailCommandeTask _detailCommandeTask = IoCContainer.Resolve<IDetailCommandeTask>();
                CommandeDM _commandeDM = _commandeTask.getCommandeDMById(_idCommande);
                if (_commandeDM != null)
                {
                    _commandeDM.DetailCommandeDMs = _detailCommandeTask.getDetailCommandesDMByIdCommande(_idCommande);
                    

                    _commandeVM = CommandeMapper.CommandeDMtoCommandeVM(_commandeDM);
                    if (_commandeDM.DetailCommandeDMs != null)
                    {
                        _commandeVM.DetailCommandes = new List<DetailCommandeVM>();
                        foreach (DetailCommandeDM _detailCommandeDM in _commandeDM.DetailCommandeDMs)
                        {
                            _commandeVM.DetailCommandes.Add(DetailCommandeMapper.DetailCommandeDMtoDetailCommandeVM(_detailCommandeDM));
                        }
                    }
                }
                _etatRep = new EtatReponse() { Code = EtatReponseCode.SUCCESS, Message = "RETURN OK" };

            }
            catch (Exception e)
            {
                _httpState = new HttpState() { Code = HttpStateCode.ERROR, Message = e.Message };
                MyLogger.log(Utilitaire.getEmplacement(this) + ":\n" + Utilitaire.getDetailsException(e), MyLoggerCode.STANDARD);
            }

            _model.Add("commandeVM", _commandeVM);
            _model.Add(HttpState.Name, _httpState);
            _model.Add(EtatReponse.Name, _etatRep);
            return Utilitaire.constructResponse(this, _model);
        }


       
        
    }
}

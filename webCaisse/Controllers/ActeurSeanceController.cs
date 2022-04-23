using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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
    public class ActeurSeanceController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage validerPresence(List<ActeurSeanceVM> acteurSeanceVMs)
        {
            Dictionary<String, Object> _model = new Dictionary<String, Object>();
            HttpState _httpState = new HttpState();
            EtatReponse _etatRep = new EtatReponse();
            try
            {
                Int64? _idUtilisateur = TokenManager.getIdentifiantFromToken(Request);
                IUtilisateurTask _utilisateurTask = IoCContainer.Resolve<IUtilisateurTask>();
                IActeurSeanceTask _acteurSeanceTask = IoCContainer.Resolve<IActeurSeanceTask>();
                if (acteurSeanceVMs != null) {
                    foreach (ActeurSeanceVM _acteurSeanceVM in acteurSeanceVMs) {
                        if (_acteurSeanceVM.Presence == 1)
                        {
                            if (_acteurSeanceVM.Identifiant <= 0)
                            {
                                ActeurSeanceDM _acteurSeanceDM = new ActeurSeanceDM()
                                {
                                    IdSeance = _acteurSeanceVM.IdSeance,
                                    IdUtilisateur = _acteurSeanceVM.IdUtilisateur,
                                };
                                _acteurSeanceTask.addActeurSeanceDM(_acteurSeanceDM);
                            }
                        }
                        else {
                            if (_acteurSeanceVM.Identifiant > 0) {
                                _acteurSeanceTask.removeActeurSeanceDM(_acteurSeanceVM.Identifiant);
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
            _model.Add(HttpState.Name, _httpState);
            _model.Add(EtatReponse.Name, _etatRep);
            return Utilitaire.constructResponse(this, _model);            
        }

        [HttpPost]
        public HttpResponseMessage getActeurSeances()
        {
            Dictionary<String, Object> _model = new Dictionary<String, Object>();
            HttpState _httpState = new HttpState();
            EtatReponse _etatRep = new EtatReponse();
            ICollection<ActeurSeanceVM> _acteurSeanceVMs = null;
            try
            {
                Int64? _idUtilisateur = TokenManager.getIdentifiantFromToken(Request);
                IUtilisateurTask _utilisateurTask = IoCContainer.Resolve<IUtilisateurTask>();
                IActeurSeanceTask _acteurSeanceTask = IoCContainer.Resolve<IActeurSeanceTask>();
                ISeanceTask _seanceTask = IoCContainer.Resolve<ISeanceTask>();
                SeanceDM _seanceDM = _seanceTask.getSeanceActive((long)_idUtilisateur);

                
                ICollection<UtilisateurDM> _utilisateurDMs = _utilisateurTask.getServeursOfCaisse((long)_seanceDM.IdCaisse);
                ICollection<ActeurSeanceDM> _acteurSeanceDMs = _acteurSeanceTask.getActeurSeanceDMsByIdSeance(_seanceDM.Identifiant);


                if (_utilisateurDMs != null) {
                    _acteurSeanceVMs = new List<ActeurSeanceVM>();
                    foreach (UtilisateurDM _utilisateurDM in _utilisateurDMs) {
                        ActeurSeanceVM _acteurSeanceVM = new ActeurSeanceVM();
                        if (_acteurSeanceDMs.Where(a => a.IdUtilisateur == _utilisateurDM.Identifiant).Count() > 0)
                        {
                            ActeurSeanceDM _acteurSeanceDM = _acteurSeanceDMs.Where(a => a.IdUtilisateur == _utilisateurDM.Identifiant).FirstOrDefault();
                            _acteurSeanceVM.Identifiant = _acteurSeanceDM.Identifiant;
                            _acteurSeanceVM.IdUtilisateur = _acteurSeanceDM.IdUtilisateur;
                            _acteurSeanceVM.IdSeance = _acteurSeanceDM.IdSeance;
                            _acteurSeanceVM.NomPrenom = _acteurSeanceDM.NomPrenom;
                            _acteurSeanceVM.Presence = 1;
                        }
                        else
                        {
                            _acteurSeanceVM.IdUtilisateur = _utilisateurDM.Identifiant;
                            _acteurSeanceVM.NomPrenom = _utilisateurDM.Nom  + " " + _utilisateurDM.Prenom;
                            _acteurSeanceVM.IdSeance = _seanceDM.Identifiant;
                            _acteurSeanceVM.Presence = 0;
                        }
                        //----------------------------------
                        Boolean _canBeAdded = true;
                        ICollection<SeanceDM> _seancesActivesDMs = _seanceTask.getSeancesActives();
                        if (_seancesActivesDMs != null) {
                            ICollection<SeanceDM> _autreSeancesActivesDMs = _seancesActivesDMs.Where(a => a.IdCaisse != _seanceDM.IdCaisse).ToList();
                            foreach (SeanceDM _autreSeanceDM in _autreSeancesActivesDMs) {
                                ICollection<ActeurSeanceDM> _autreActeurSeanceDMs = _acteurSeanceTask.getActeurSeanceDMsByIdSeance(_autreSeanceDM.Identifiant);
                                if (_autreActeurSeanceDMs != null && _autreActeurSeanceDMs.Where(a => a.IdUtilisateur == _utilisateurDM.Identifiant).Count() > 0) {
                                    _canBeAdded = false;
                                    break;
                                }
                            }
                        }
                        //----------------------------------
                        if (_canBeAdded == true) {
                            _acteurSeanceVMs.Add(_acteurSeanceVM);
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
            _model.Add(HttpState.Name, _httpState);
            _model.Add(EtatReponse.Name, _etatRep);
            _model.Add("acteurSeanceVMs", _acteurSeanceVMs);

            
            return Utilitaire.constructResponse(this, _model);
        }

    }
}

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
    public class AffectationCaisseController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage validerAffectationCaisse(List<AffectationCaisseVM> affectationCaisseVMs)
        {
            Dictionary<String, Object> _model = new Dictionary<String, Object>();
            HttpState _httpState = new HttpState();
            EtatReponse _etatRep = new EtatReponse();
            try
            {
                Int64? _idUtilisateur = TokenManager.getIdentifiantFromToken(Request);
                IUtilisateurTask _utilisateurTask = IoCContainer.Resolve<IUtilisateurTask>();
                IAffectationCaisseTask _affectationCaisseTask = IoCContainer.Resolve<IAffectationCaisseTask>();
                if (affectationCaisseVMs != null) {
                    foreach (AffectationCaisseVM _affectationCaisseVM in affectationCaisseVMs) {
                        if (_affectationCaisseVM.Coche == 1)
                        {
                            if (_affectationCaisseVM.Identifiant <= 0)
                            {
                                AffectationCaisseDM _affectationCaisseDM = new AffectationCaisseDM()
                                {
                                    IdCaisse = _affectationCaisseVM.IdCaisse,
                                    IdUtilisateur = _affectationCaisseVM.IdUtilisateur,
                                };
                                _affectationCaisseTask.addAffectationCaisseDM(_affectationCaisseDM);
                            }
                        }
                        else {
                            if (_affectationCaisseVM.Identifiant > 0) {
                                _affectationCaisseTask.removeAffectationCaisseDM(_affectationCaisseVM.Identifiant);
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
        public HttpResponseMessage getAffectationCaisses(ParamInt paramInt)
        {
            Int64? _idCaisse = paramInt.Valeur;
            Dictionary<String, Object> _model = new Dictionary<String, Object>();
            HttpState _httpState = new HttpState();
            EtatReponse _etatRep = new EtatReponse();
            ICollection<AffectationCaisseVM> _affectationCaisseVMs = null;
            try
            {
                Int64? _idUtilisateur = TokenManager.getIdentifiantFromToken(Request);
                IUtilisateurTask _utilisateurTask = IoCContainer.Resolve<IUtilisateurTask>();
                IGroupeTask _groupeTask = IoCContainer.Resolve<IGroupeTask>();
                IAffectationCaisseTask _affectationCaisseTask = IoCContainer.Resolve<IAffectationCaisseTask>();
                ICategorieTask _categorieTask = IoCContainer.Resolve<ICategorieTask>();

                //ICollection<UtilisateurDM> _utilisateurDMs = _utilisateurTask.getListeUtilisateurDMs();
                ICollection<UtilisateurDM> _utilisateurDMs = _utilisateurTask.getUtilisateurByGroupe(GroupeCode.CAISSIER);
                _utilisateurDMs = _utilisateurDMs.Concat(_utilisateurTask.getUtilisateurByGroupe(GroupeCode.SERVEUR)).ToList();



                ICollection<AffectationCaisseDM> _affectationCaisseDMs = _affectationCaisseTask.getAffectationCaisseDMs(_idCaisse: _idCaisse,_enActivite : 1);

                if (_utilisateurDMs != null) {
                    _affectationCaisseVMs = new List<AffectationCaisseVM>();
                    foreach (UtilisateurDM _utilisateurDM in _utilisateurDMs) {
                        GroupeDM _groupeDM = _groupeTask.getGroupesOfUtilisateur(_utilisateurDM.Identifiant).FirstOrDefault();                        
                        AffectationCaisseVM _affectationCaisseVM = new AffectationCaisseVM();
                        if (_affectationCaisseDMs.Where(a => a.IdUtilisateur == _utilisateurDM.Identifiant).Count() > 0)
                        {
                            AffectationCaisseDM _affectationCaisseDM = _affectationCaisseDMs.Where(a => a.IdUtilisateur == _utilisateurDM.Identifiant).FirstOrDefault();
                            _affectationCaisseVM.Identifiant = _affectationCaisseDM.Identifiant;
                            _affectationCaisseVM.IdCaisse = _affectationCaisseDM.IdCaisse;
                            _affectationCaisseVM.IdUtilisateur = _affectationCaisseDM.IdUtilisateur;
                            _affectationCaisseVM.NomPrenom = _affectationCaisseDM.NomPrenom;
                            _affectationCaisseVM.Coche = 1;
                        }
                        else
                        {
                            _affectationCaisseVM.IdUtilisateur = _utilisateurDM.Identifiant;
                            _affectationCaisseVM.NomPrenom = _utilisateurDM.Nom + " " +_utilisateurDM.Prenom;
                            _affectationCaisseVM.IdCaisse = _idCaisse;
                            _affectationCaisseVM.Coche = 0;
                        }
                        _affectationCaisseVM.LibelleGroupe = _groupeDM.Libelle;
                        if (_groupeDM.Code == GroupeCode.CAISSIER)
                        {
                            ICollection<AffectationCaisseDM> _affectationCaisseDMsTemp = _affectationCaisseTask.getAffectationCaisseDMs(_enActivite : 1);
                            if (_affectationCaisseDMsTemp != null)
                            {
                                if (_affectationCaisseDMsTemp.Where(a => a.IdCaisse != _idCaisse && a.IdUtilisateur == _utilisateurDM.Identifiant).Count() == 0) {
                                    _affectationCaisseVMs.Add(_affectationCaisseVM);
                                }
                            }
                        }
                        else {
                            _affectationCaisseVMs.Add(_affectationCaisseVM);
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
            _model.Add("affectationCaisseVMs", _affectationCaisseVMs);

            
            return Utilitaire.constructResponse(this, _model);
        }

    }
}

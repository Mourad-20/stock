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
    public class AppartenanceController : ApiController
    {
        
        [HttpPost]
        public HttpResponseMessage validerAppartenance(List<AppartenanceVM> appartenanceVMs)
        {
            Dictionary<String, Object> _model = new Dictionary<String, Object>();
            HttpState _httpState = new HttpState();
            EtatReponse _etatRep = new EtatReponse();
            try
            {
                Int64? _idUtilisateur = TokenManager.getIdentifiantFromToken(Request);
                IUtilisateurTask _utilisateurTask = IoCContainer.Resolve<IUtilisateurTask>();
                IAppartenanceTask _appartenanceTask = IoCContainer.Resolve<IAppartenanceTask>();
                if (appartenanceVMs != null) {
                    foreach (AppartenanceVM _appartenanceVM in appartenanceVMs) {
                        if (_appartenanceVM.Coche == 1)
                        {
                            if (_appartenanceVM.Identifiant <= 0)
                            {
                                AppartenanceDM _appartenanceDM = new AppartenanceDM()
                                {
                                    IdGroupe = _appartenanceVM.IdGroupe,
                                    IdUtilisateur = _appartenanceVM.IdUtilisateur,
                                };
                                _appartenanceTask.addAppartenance(_appartenanceDM);
                            }
                        }
                        else {
                            if (_appartenanceVM.Identifiant > 0) {
                                _appartenanceTask.removeAppartenanceDM(_appartenanceVM.Identifiant);
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
        public HttpResponseMessage getAppartenances(ParamInt paramInt)
        {
            Int64? _idUtilisateur = paramInt.Valeur;
            Dictionary<String, Object> _model = new Dictionary<String, Object>();
            HttpState _httpState = new HttpState();
            EtatReponse _etatRep = new EtatReponse();
            ICollection<AppartenanceVM> _appartenanceVMs = null;
            try
            {
                IGroupeTask _groupeTask = IoCContainer.Resolve<IGroupeTask>();
                IAppartenanceTask _appartenanceTask = IoCContainer.Resolve<IAppartenanceTask>();
                ICollection<GroupeDM> _groupeDMs = _groupeTask.getGroupeDMs();
                ICollection<AppartenanceDM> _appartenanceDMs = _appartenanceTask.getAppartenanceDMsByIdUtilisateur((long)_idUtilisateur);
                if (_groupeDMs != null)
                {
                    _appartenanceVMs = new List<AppartenanceVM>();
                    foreach (GroupeDM _groupeDM in _groupeDMs)
                    {
                        AppartenanceVM _appartenanceVM = new AppartenanceVM();
                        if (_appartenanceDMs.Where(a => a.IdGroupe == _groupeDM.Identifiant).Count() > 0)
                        {
                            AppartenanceDM _appartenanceDM = _appartenanceDMs.Where(a => a.IdGroupe == _groupeDM.Identifiant).FirstOrDefault();
                            _appartenanceVM.Identifiant = _appartenanceDM.Identifiant;
                            _appartenanceVM.IdUtilisateur = _appartenanceDM.IdUtilisateur;
                            _appartenanceVM.IdGroupe = _appartenanceDM.IdGroupe;
                            _appartenanceVM.LibelleGroupe = _appartenanceDM.LibelleGroupe;
                            _appartenanceVM.Coche = 1;
                        }
                        else
                        {
                            _appartenanceVM.IdGroupe = _groupeDM.Identifiant;
                            _appartenanceVM.LibelleGroupe = _groupeDM.Libelle;
                            _appartenanceVM.IdUtilisateur = _idUtilisateur;
                            _appartenanceVM.Coche = 0;
                        }
                        _appartenanceVMs.Add(_appartenanceVM);
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
            _model.Add("appartenanceVMs", _appartenanceVMs);

            
            return Utilitaire.constructResponse(this, _model);
        }

    }
}

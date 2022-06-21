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
    public class AssociationMessageController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage validerAssociationMessage(List<AssociationMessageVM> associationMessageVMs)
        {
            Dictionary<String, Object> _model = new Dictionary<String, Object>();
            HttpState _httpState = new HttpState();
            EtatReponse _etatRep = new EtatReponse();
            try
            {
                Int64? _idUtilisateur = TokenManager.getIdentifiantFromToken(Request);
                IUtilisateurTask _utilisateurTask = IoCContainer.Resolve<IUtilisateurTask>();
                IAssociationMessageTask _associationMessageTask = IoCContainer.Resolve<IAssociationMessageTask>();
                if (associationMessageVMs != null) {
                    foreach (AssociationMessageVM _associationMessageVM in associationMessageVMs) {
                        if (_associationMessageVM.EnActivite == 1)
                        {
                            if (_associationMessageVM.Identifiant <= 0)
                            {
                                AssociationMessageDM _associationMessageDM = new AssociationMessageDM()
                                {
                                    IdMessage = _associationMessageVM.IdMessage,
                                    IdArticle = _associationMessageVM.IdArticle,
                                };
                                _associationMessageTask.addAssociationMessageDM(_associationMessageDM);
                            }
                        }
                        else {
                            if (_associationMessageVM.Identifiant > 0) {
                                _associationMessageTask.removeAssociationMessageDM(_associationMessageVM.Identifiant);
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
        public HttpResponseMessage getAssociationMessages(ParamInt paramInt)
        {
            Int64? _idArticle = paramInt.Valeur;
            Dictionary<String, Object> _model = new Dictionary<String, Object>();
            HttpState _httpState = new HttpState();
            EtatReponse _etatRep = new EtatReponse();
            ICollection<AssociationMessageVM> _associationMessageVMs = null;
            try
            {
                Int64? _idUtilisateur = TokenManager.getIdentifiantFromToken(Request);
                IUtilisateurTask _utilisateurTask = IoCContainer.Resolve<IUtilisateurTask>();
                IAssociationMessageTask _associationMessageTask = IoCContainer.Resolve<IAssociationMessageTask>();
                IMessageTask _messageTask = IoCContainer.Resolve<IMessageTask>();
                
                ICollection<MessageDM> _messageDMs = _messageTask.getMessages(_enActivite : 1);
                ICollection<AssociationMessageDM> _associationMessageDMs = _associationMessageTask.getAssociationMessageDMs(_idArticle: _idArticle);

                if (_messageDMs != null) {
                    _associationMessageVMs = new List<AssociationMessageVM>();
                    foreach (MessageDM _messageDM in _messageDMs) {
                        AssociationMessageVM _associationMessageVM = new AssociationMessageVM();
                        if (_associationMessageDMs.Where(a => a.IdMessage == _messageDM.Identifiant).Count() > 0)
                        {
                            AssociationMessageDM _associationMessageDM = _associationMessageDMs.Where(a => a.IdMessage == _messageDM.Identifiant).FirstOrDefault();
                            _associationMessageVM.Identifiant = _associationMessageDM.Identifiant;
                            _associationMessageVM.IdArticle = _associationMessageDM.IdArticle;
                            _associationMessageVM.IdMessage = _associationMessageDM.IdMessage;
                            _associationMessageVM.LibelleMessage = _associationMessageDM.LibelleMessage;
                            _associationMessageVM.EnActivite = 1;
                        }
                        else
                        {
                            _associationMessageVM.IdMessage = _messageDM.Identifiant;
                            _associationMessageVM.LibelleMessage = _messageDM.Libelle;
                            _associationMessageVM.IdArticle = _idArticle;
                            _associationMessageVM.EnActivite = 0;
                        }
                        _associationMessageVMs.Add(_associationMessageVM);
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
            _model.Add("associationMessageVMs", _associationMessageVMs);

            
            return Utilitaire.constructResponse(this, _model);
        }

    }
}

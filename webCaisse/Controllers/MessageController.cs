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
using webCaisse.Filters;
using webCaisse.Mappers;
using webCaisse.Taks.Contracts;
using webCaisse.Tools;
using webCaisse.VMs.Models;
using webCaisse.VMs.Params;

namespace webCaisse.Controllers
{
    public class MessageController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage getMessages()
        {
            Dictionary<String, Object> _model = new Dictionary<String, Object>();
            HttpState _httpState = new HttpState();
            EtatReponse _etatRep = new EtatReponse();
            ICollection<MessageVM> _messageVMs = null;
            try
            {
                IMessageTask _messageTask = IoCContainer.Resolve<IMessageTask>();
                ICollection<MessageDM> _messageDMs = _messageTask.getMessages();
                if (_messageDMs != null)
                {
                    _messageVMs = new List<MessageVM>();
                    foreach (MessageDM _obj in _messageDMs) {
                        MessageVM _messageVM = MessageMapper.MessageDMtoMessageVM(_obj);
                        _messageVMs.Add(_messageVM);
                    }   
                }
                _etatRep = new EtatReponse() { Code = EtatReponseCode.SUCCESS, Message = "RETURN OK" };
            }
            catch (Exception e)
            {
                _httpState = new HttpState() { Code = HttpStateCode.ERROR, Message = e.Message };
                MyLogger.log(Utilitaire.getEmplacement(this) + ":\n" + Utilitaire.getDetailsException(e), MyLoggerCode.STANDARD);
            }

            _model.Add("messageVMs", _messageVMs);
            _model.Add(HttpState.Name, _httpState);
            _model.Add(EtatReponse.Name, _etatRep);
            return Utilitaire.constructResponse(this, _model);            
        }
        [HttpPost]
        public HttpResponseMessage ajouterMessage(MessageVM messageVM)
        {
            Dictionary<String, Object> _model = new Dictionary<String, Object>();
            ITypeMessageTask _typeMessageTask = IoCContainer.Resolve<ITypeMessageTask>();

            HttpState _httpState = new HttpState();
            EtatReponse _etatRep = new EtatReponse();
            Int64? _idMessage = null;
            try
            {
                IMessageTask _messageTask = IoCContainer.Resolve<IMessageTask>();
                if (messageVM != null)
                {
                    string code = messageVM.LibelleType;
                    TypeMessageDM _typeMessageDM = _typeMessageTask.getTypeMessageDMByCode(code);

                    MessageDM _messageDM = MessageMapper.MessageVMtoMessageDM(messageVM);
                    _messageDM.IdTypeMessage = _typeMessageDM.Identifiant;
                    _idMessage = _messageTask.addMessageDM(_messageDM);
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
            _model.Add("idMessage", _idMessage);
            return Utilitaire.constructResponse(this, _model);
        }


        [HttpPost]
        public HttpResponseMessage modifierMessage(MessageVM messageVM)
        {
            Dictionary<String, Object> _model = new Dictionary<String, Object>();
            HttpState _httpState = new HttpState();
            EtatReponse _etatRep = new EtatReponse();
            ITypeMessageTask _typeMessageTask = IoCContainer.Resolve<ITypeMessageTask>();
            try
            {
                IMessageTask _messageTask = IoCContainer.Resolve<IMessageTask>();
                if (messageVM != null)
                {
                    string code = messageVM.LibelleType;
                    TypeMessageDM _typeMessageDM = _typeMessageTask.getTypeMessageDMByCode(code);

                    MessageDM _messageDM = MessageMapper.MessageVMtoMessageDM(messageVM);
                    _messageDM.IdTypeMessage = _typeMessageDM.Identifiant;
                    _messageTask.modifierMessageDM(_messageDM);
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
        public HttpResponseMessage getMessagesbyId(ParamInt paramInt)
        {
            Dictionary<String, Object> _model = new Dictionary<String, Object>();
            Int64? _idArticle = paramInt.Valeur;
            HttpState _httpState = new HttpState();
            EtatReponse _etatRep = new EtatReponse();
            ICollection<MessageVM> _messageVMs = null;
            try
            {
                IMessageTask _messageTask = IoCContainer.Resolve<IMessageTask>();
                ICollection<MessageDM> _messageDMs = _messageTask.getMessagesbyId(null, _idArticle);
                if (_messageDMs != null)
                {
                    _messageVMs = new List<MessageVM>();
                    foreach (MessageDM _obj in _messageDMs)
                    {
                        MessageVM _messageVM = MessageMapper.MessageDMtoMessageVM(_obj);
                        _messageVMs.Add(_messageVM);
                    }
                }
                _etatRep = new EtatReponse() { Code = EtatReponseCode.SUCCESS, Message = "RETURN OK" };
            }
            catch (Exception e)
            {
                _httpState = new HttpState() { Code = HttpStateCode.ERROR, Message = e.Message };
                MyLogger.log(Utilitaire.getEmplacement(this) + ":\n" + Utilitaire.getDetailsException(e), MyLoggerCode.STANDARD);
            }

            _model.Add("messageVMs", _messageVMs);
            _model.Add(HttpState.Name, _httpState);
            _model.Add(EtatReponse.Name, _etatRep);
            return Utilitaire.constructResponse(this, _model);
        }


     
    }
}

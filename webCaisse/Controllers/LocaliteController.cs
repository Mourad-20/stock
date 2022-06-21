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
    public class LocaliteController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage getLocalites()
        {
            Dictionary<String, Object> _model = new Dictionary<String, Object>();
            HttpState _httpState = new HttpState();
            EtatReponse _etatRep = new EtatReponse();
            ICollection<LocaliteVM> _localiteVMs = null;
            try
            {
                ILocaliteTask _localiteTask = IoCContainer.Resolve<ILocaliteTask>();
                ICollection<LocaliteDM> _localiteDMs = _localiteTask.getLocalites();
                if (_localiteDMs != null)
                {
                    _localiteVMs = new List<LocaliteVM>();
                    foreach (LocaliteDM _obj in _localiteDMs) {
                        LocaliteVM _localiteVM = LocaliteMapper.LocaliteDMtoLocaliteVM(_obj);
                        _localiteVMs.Add(_localiteVM);
                    }   
                }
                _etatRep = new EtatReponse() { Code = EtatReponseCode.SUCCESS, Message = "RETURN OK" };
            }
            catch (Exception e)
            {
                _httpState = new HttpState() { Code = HttpStateCode.ERROR, Message = e.Message };
                MyLogger.log(Utilitaire.getEmplacement(this) + ":\n" + Utilitaire.getDetailsException(e), MyLoggerCode.STANDARD);
            }

            _model.Add("localiteVMs", _localiteVMs);
            _model.Add(HttpState.Name, _httpState);
            _model.Add(EtatReponse.Name, _etatRep);
            return Utilitaire.constructResponse(this, _model);            
        }
        [HttpPost]
        public HttpResponseMessage getLocalitesDisponible()
        {
            Dictionary<String, Object> _model = new Dictionary<String, Object>();
            HttpState _httpState = new HttpState();
            EtatReponse _etatRep = new EtatReponse();
            ICollection<LocaliteVM> _localiteVMs = null;
            try
            {
                ILocaliteTask _localiteTask = IoCContainer.Resolve<ILocaliteTask>();
                ICollection<LocaliteDM> _localiteDMs = _localiteTask.getLocalitesDisponible();
                if (_localiteDMs != null)
                {
                    _localiteVMs = new List<LocaliteVM>();
                    foreach (LocaliteDM _obj in _localiteDMs)
                    {
                        LocaliteVM _localiteVM = LocaliteMapper.LocaliteDMtoLocaliteVM(_obj);
                        _localiteVMs.Add(_localiteVM);
                    }
                }
                _etatRep = new EtatReponse() { Code = EtatReponseCode.SUCCESS, Message = "RETURN OK" };
            }
            catch (Exception e)
            {
                _httpState = new HttpState() { Code = HttpStateCode.ERROR, Message = e.Message };
                MyLogger.log(Utilitaire.getEmplacement(this) + ":\n" + Utilitaire.getDetailsException(e), MyLoggerCode.STANDARD);
            }

            _model.Add("localiteVMs", _localiteVMs);
            _model.Add(HttpState.Name, _httpState);
            _model.Add(EtatReponse.Name, _etatRep);
            return Utilitaire.constructResponse(this, _model);
        }
        [HttpPost]
        public HttpResponseMessage ajouterLocalite(LocaliteVM localiteVM)
        {
            Dictionary<String, Object> _model = new Dictionary<String, Object>();
            HttpState _httpState = new HttpState();
            EtatReponse _etatRep = new EtatReponse();
            Int64? _idLocalite = null;
            try
            {
                IEtatLocaliteTask _etatLocaliteTask = IoCContainer.Resolve<IEtatLocaliteTask>();
                ILocaliteTask _localiteTask = IoCContainer.Resolve<ILocaliteTask>();
                if (localiteVM != null)
                {
                    EtatLocaliteDM _etatLocaliteDM = _etatLocaliteTask.getEtatLocaliteDMByCode(EtatLocaliteCode.DISPONIBLE);
                    LocaliteDM _localiteDM = LocaliteMapper.LocaliteVMtoLocaliteDM(localiteVM);
                    _localiteDM.IdEtatLocalite = _etatLocaliteDM.Identifiant;
                    _idLocalite = _localiteTask.addLocaliteDM(_localiteDM);
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
            _model.Add("idLocalite", _idLocalite);
            return Utilitaire.constructResponse(this, _model);
        }
        [HttpPost]
        public HttpResponseMessage modifierLocalite(LocaliteVM localiteVM)
        {
            Dictionary<String, Object> _model = new Dictionary<String, Object>();
            HttpState _httpState = new HttpState();
            EtatReponse _etatRep = new EtatReponse();
            try
            {
                ILocaliteTask _localiteTask = IoCContainer.Resolve<ILocaliteTask>();
                if (localiteVM != null)
                {
                    LocaliteDM _localiteDM = LocaliteMapper.LocaliteVMtoLocaliteDM(localiteVM);
                    _localiteTask.modifierLocaliteDM(_localiteDM);
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

    }
}

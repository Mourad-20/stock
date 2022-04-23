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
using webCaisse.Filters;
using webCaisse.Mappers;
using webCaisse.Taks.Contracts;
using webCaisse.Tools;
using webCaisse.VMs.Models;
using webCaisse.VMs.Params;

namespace webCaisse.Controllers
{
    public class ZoneController : ApiController
    {
        

        [HttpPost]
        public HttpResponseMessage getCategories()
        {
            Dictionary<String, Object> _model = new Dictionary<String, Object>();
            HttpState _httpState = new HttpState();
            EtatReponse _etatRep = new EtatReponse();
            ICollection<ZoneVM> _zoneVMs = null;
            try
            {
                IZoneTask _zoneTask = IoCContainer.Resolve<IZoneTask>();
                ICollection<ZoneDM> _zoneDMs = _zoneTask.getZoneDMs();
                if (_zoneDMs != null)
                {
                    _zoneVMs = new List<ZoneVM>();
                    foreach (ZoneDM _obj in _zoneDMs)
                    {
                        ZoneVM _zoneVM = ZoneMapper.ZoneDMtoZoneVM(_obj);
                        _zoneVMs.Add(_zoneVM);
                    }
                }
                _etatRep = new EtatReponse() { Code = EtatReponseCode.SUCCESS, Message = "RETURN OK" };
            }
            catch (Exception e)
            {
                _httpState = new HttpState() { Code = HttpStateCode.ERROR, Message = e.Message };
                MyLogger.log(Utilitaire.getEmplacement(this) + ":\n" + Utilitaire.getDetailsException(e), MyLoggerCode.STANDARD);
            }

            _model.Add("zoneVMs", _zoneVMs);
            _model.Add(HttpState.Name, _httpState);
            _model.Add(EtatReponse.Name, _etatRep);
            return Utilitaire.constructResponse(this, _model);
        }

        [HttpPost]
        public HttpResponseMessage getZonesForUI()
        {
            Dictionary<String, Object> _model = new Dictionary<String, Object>();
            HttpState _httpState = new HttpState();
            EtatReponse _etatRep = new EtatReponse();
            ICollection<ZoneVM> _zoneVMs = null;
            try
            {
                IZoneTask _zoneTask = IoCContainer.Resolve<IZoneTask>();
                ICollection<ZoneDM> _zoneDMs = _zoneTask.getZoneDMs(_enActivite: 1);
                if (_zoneDMs != null)
                {
                    _zoneVMs = new List<ZoneVM>();
                    foreach (ZoneDM _obj in _zoneDMs)
                    {
                        ZoneVM _zoneVM = ZoneMapper.ZoneDMtoZoneVM(_obj);
                        _zoneVMs.Add(_zoneVM);
                    }
                }
                _etatRep = new EtatReponse() { Code = EtatReponseCode.SUCCESS, Message = "RETURN OK" };
            }
            catch (Exception e)
            {
                _httpState = new HttpState() { Code = HttpStateCode.ERROR, Message = e.Message };
                MyLogger.log(Utilitaire.getEmplacement(this) + ":\n" + Utilitaire.getDetailsException(e), MyLoggerCode.STANDARD);
            }

            _model.Add("zoneVMs", _zoneVMs);
            _model.Add(HttpState.Name, _httpState);
            _model.Add(EtatReponse.Name, _etatRep);
            return Utilitaire.constructResponse(this, _model);
        }

    }
}

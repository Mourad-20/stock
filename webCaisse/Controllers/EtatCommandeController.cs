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
    public class EtatCommandeController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage getAllEtats()
        {
            Dictionary<String, Object> _model = new Dictionary<String, Object>();
            HttpState _httpState = new HttpState();
            EtatReponse _etatRep = new EtatReponse();
            ICollection<EtatCommandeVM> _etatCommandeVMs = null;
            try
            {
                IEtatCommandeTask _etatCommandeTask = IoCContainer.Resolve<IEtatCommandeTask>();
                ICollection<EtatCommandeDM> _etatCommandeDMs = _etatCommandeTask.getAllEtatCommandes();
                if (_etatCommandeDMs != null)
                {
                    _etatCommandeVMs = new List<EtatCommandeVM>();
                    foreach (EtatCommandeDM _obj in _etatCommandeDMs)
                    {
                        EtatCommandeVM _etatCommandeVM = EtatCommandeMapper.AEtatCommandeDMtoEtatCommandeVM(_obj);
                        _etatCommandeVMs.Add(_etatCommandeVM);
                    }
                }
                _etatRep = new EtatReponse() { Code = EtatReponseCode.SUCCESS, Message = "RETURN OK" };
            }
            catch (Exception e)
            {
                _httpState = new HttpState() { Code = HttpStateCode.ERROR, Message = e.Message };
                MyLogger.log(Utilitaire.getEmplacement(this) + ":\n" + Utilitaire.getDetailsException(e), MyLoggerCode.STANDARD);
            }

            _model.Add("etatCommandeVMs", _etatCommandeVMs);
            _model.Add(HttpState.Name, _httpState);
            _model.Add(EtatReponse.Name, _etatRep);
            return Utilitaire.constructResponse(this, _model);
        }

    }
}

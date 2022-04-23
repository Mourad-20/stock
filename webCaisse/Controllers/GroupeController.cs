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
    public class GroupeController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage getListeGroupes()
        {
            Dictionary<String, Object> _model = new Dictionary<String, Object>();
            HttpState _httpState = new HttpState();
            EtatReponse _etatRep = new EtatReponse();
            ICollection<GroupeVM> _groupeVMs = null;
            try
            {
                IGroupeTask _groupeTask = IoCContainer.Resolve<IGroupeTask>();
                ICollection<GroupeDM> _groupeDMs = _groupeTask.getGroupeDMs();
                if (_groupeDMs != null)
                {
                    _groupeVMs = new List<GroupeVM>();
                    foreach (GroupeDM _obj in _groupeDMs)
                    {
                        GroupeVM _groupeVM = GroupeMapper.GroupeDMtoGroupeVM(_obj);
                        _groupeVMs.Add(_groupeVM);
                    }
                }
                _etatRep = new EtatReponse() { Code = EtatReponseCode.SUCCESS, Message = "RETURN OK" };
            }
            catch (Exception e)
            {
                _httpState = new HttpState() { Code = HttpStateCode.ERROR, Message = e.Message };
                MyLogger.log(Utilitaire.getEmplacement(this) + ":\n" + Utilitaire.getDetailsException(e), MyLoggerCode.STANDARD);
            }

            _model.Add("groupeVMs", _groupeVMs);
            _model.Add(HttpState.Name, _httpState);
            _model.Add(EtatReponse.Name, _etatRep);
            return Utilitaire.constructResponse(this, _model);
        }
    }
}

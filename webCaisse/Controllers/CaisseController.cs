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
    public class CaisseController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage getCaisses()
        {
            Dictionary<String, Object> _model = new Dictionary<String, Object>();
            HttpState _httpState = new HttpState();
            EtatReponse _etatRep = new EtatReponse();
            ICollection<CaisseVM> _caisseVMs = null;
            try
            {
                ICaisseTask _caisseTask = IoCContainer.Resolve<ICaisseTask>();
                ICollection<CaisseDM> _caisseDMs = _caisseTask.getCaisseDMs(_enActivite : 1);
                if (_caisseDMs != null)
                {
                    _caisseVMs = new List<CaisseVM>();
                    foreach (CaisseDM _obj in _caisseDMs) {
                        CaisseVM _caisseVM = CaisseMapper.CaisseDMtoCaisseVM(_obj);
                        _caisseVMs.Add(_caisseVM);
                    }   
                }
                _etatRep = new EtatReponse() { Code = EtatReponseCode.SUCCESS, Message = "RETURN OK" };
            }
            catch (Exception e)
            {
                _httpState = new HttpState() { Code = HttpStateCode.ERROR, Message = e.Message };
                MyLogger.log(Utilitaire.getEmplacement(this) + ":\n" + Utilitaire.getDetailsException(e), MyLoggerCode.STANDARD);
            }
            _model.Add("caisseVMs", _caisseVMs);
            _model.Add(HttpState.Name, _httpState);
            _model.Add(EtatReponse.Name, _etatRep);
            return Utilitaire.constructResponse(this, _model);            
        }
        [HttpPost]
        public HttpResponseMessage AddCaisse(CaisseVM caisseVM)
        {
            Dictionary<String, Object> _model = new Dictionary<String, Object>();
            HttpState _httpState = new HttpState();
            EtatReponse _etatRep = new EtatReponse();
            Int64? _idCaisse = null;
            try
            {
                //Int64? _idUtilisateur = TokenManager.getIdentifiantFromToken(Request);
                ICaisseTask _caisseTask = IoCContainer.Resolve<ICaisseTask>();
                //IAppartenanceTask _appartenanceTask = IoCContainer.Resolve<IAppartenanceTask>();
                //IGroupeTask _groupeTask = IoCContainer.Resolve<IGroupeTask>();
                //-----------------------------------------------------------------
                String _message = "";
                Boolean _isOk = true;

                //ICollection<GroupeDM> _groupeDMs = _groupeTask.getGroupesOfUtilisateur((long)_idUtilisateur);



                //-----------------------------------------------------------------
                if (_isOk)
                {


                    CaisseDM _caisseDM = CaisseMapper.CaisseVMtoCaisseDM(caisseVM);



                    _idCaisse = _caisseTask.addCaisse(_caisseDM);


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
            _model.Add("idCaisse", _idCaisse);
            _model.Add(HttpState.Name, _httpState);
            _model.Add(EtatReponse.Name, _etatRep);
            return Utilitaire.constructResponse(this, _model);
        }


        [HttpPost]

        public HttpResponseMessage updateCaisse(CaisseVM caisseVM)
        {
            Dictionary<String, Object> _model = new Dictionary<String, Object>();
            HttpState _httpState = new HttpState();
            EtatReponse _etatRep = new EtatReponse();
            Int64? _idCaisse = null;
            try
            {
                //Int64? _idUtilisateur = TokenManager.getIdentifiantFromToken(Request);
                ICaisseTask _caisseTask = IoCContainer.Resolve<ICaisseTask>();
                //-----------------------------------------------------------------
                String _message = "";
                Boolean _isOk = true;

                //ICollection<GroupeDM> _groupeDMs = _groupeTask.getGroupesOfUtilisateur((long)_idUtilisateur);



                //-----------------------------------------------------------------
                if (_isOk)
                {

                    CaisseDM _caisseDM = CaisseMapper.CaisseVMtoCaisseDM(caisseVM);

                    _idCaisse = _caisseTask.updateCaisse(_caisseDM);


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
            _model.Add("idCaisse", _idCaisse);
            _model.Add(HttpState.Name, _httpState);
            _model.Add(EtatReponse.Name, _etatRep);
            return Utilitaire.constructResponse(this, _model);
        }

    }
}

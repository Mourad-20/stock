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
    public class SeanceController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage getSeanceActive()
        {
            Dictionary<String, Object> _model = new Dictionary<String, Object>();
            HttpState _httpState = new HttpState();
            EtatReponse _etatRep = new EtatReponse();
            SeanceVM _seanceVM = null;
            try
            {
                Int64? _idUtilisateur = TokenManager.getIdentifiantFromToken(Request);
                ISeanceTask _seanceTask = IoCContainer.Resolve<ISeanceTask>();
                SeanceDM _seanceDM = _seanceTask.getSeanceActive((long)_idUtilisateur);
                if (_seanceDM != null)
                {
                    _seanceVM = SeanceMapper.SeanceDMtoSeanceVM(_seanceDM);
                }
                _etatRep = new EtatReponse() { Code = EtatReponseCode.SUCCESS, Message = "RETURN OK" };
            }
            catch (Exception e)
            {
                _httpState = new HttpState() { Code = HttpStateCode.ERROR, Message = e.Message };
                MyLogger.log(Utilitaire.getEmplacement(this) + ":\n" + Utilitaire.getDetailsException(e), MyLoggerCode.STANDARD);
            }

            _model.Add("seanceVM", _seanceVM);
            _model.Add(HttpState.Name, _httpState);
            _model.Add(EtatReponse.Name, _etatRep);
            return Utilitaire.constructResponse(this, _model);            
        }
        [HttpPost]
        public HttpResponseMessage getSeanceCaisse(long _idCaisse)
        {
            Dictionary<String, Object> _model = new Dictionary<String, Object>();
            HttpState _httpState = new HttpState();
            EtatReponse _etatRep = new EtatReponse();
            SeanceVM _seanceVM = null;
            try
            {
                Int64? _idUtilisateur = TokenManager.getIdentifiantFromToken(Request);
                ISeanceTask _seanceTask = IoCContainer.Resolve<ISeanceTask>();
                SeanceDM _seanceDM = _seanceTask.getSeanceCaisse(_idCaisse);
                if (_seanceDM != null)
                {
                    _seanceVM = SeanceMapper.SeanceDMtoSeanceVM(_seanceDM);
                }
                _etatRep = new EtatReponse() { Code = EtatReponseCode.SUCCESS, Message = "RETURN OK" };
            }
            catch (Exception e)
            {
                _httpState = new HttpState() { Code = HttpStateCode.ERROR, Message = e.Message };
                MyLogger.log(Utilitaire.getEmplacement(this) + ":\n" + Utilitaire.getDetailsException(e), MyLoggerCode.STANDARD);
            }

            _model.Add("seanceVM", _seanceVM);
            _model.Add(HttpState.Name, _httpState);
            _model.Add(EtatReponse.Name, _etatRep);
            return Utilitaire.constructResponse(this, _model);
        }
        [HttpPost]
        public HttpResponseMessage getSeances(ParamDate paramsDate)
        {
            DateTime? d_debut = Cvrt.strToDateTime(paramsDate._datedebut);

            DateTime? d_fin = Cvrt.strToDateTime(paramsDate._datefin);

            Dictionary<String, Object> _model = new Dictionary<String, Object>();
            HttpState _httpState = new HttpState();
            EtatReponse _etatRep = new EtatReponse();
            SeanceVM _seanceVM = null;
            ICollection<SeanceVM> _seanceVMs=new List<SeanceVM>();
            try
            {
                Int64? _idUtilisateur = TokenManager.getIdentifiantFromToken(Request);
                ISeanceTask _seanceTask = IoCContainer.Resolve<ISeanceTask>();
                ICollection<SeanceDM> _seanceDMs = _seanceTask.getSeanceDMs(_ddebut: d_debut, _dfin: d_fin);
                if (_seanceDMs != null)
                {
                    foreach (SeanceDM item in _seanceDMs)
                    {
                        _seanceVM = SeanceMapper.SeanceDMtoSeanceVM(item);
                        _seanceVMs.Add(_seanceVM);
                    }
                 
                }
                _etatRep = new EtatReponse() { Code = EtatReponseCode.SUCCESS, Message = "RETURN OK" };
            }
            catch (Exception e)
            {
                _httpState = new HttpState() { Code = HttpStateCode.ERROR, Message = e.Message };
                MyLogger.log(Utilitaire.getEmplacement(this) + ":\n" + Utilitaire.getDetailsException(e), MyLoggerCode.STANDARD);
            }

            _model.Add("seanceVMs", _seanceVMs);
            _model.Add(HttpState.Name, _httpState);
            _model.Add(EtatReponse.Name, _etatRep);
            return Utilitaire.constructResponse(this, _model);
        }
        [HttpPost]
        public HttpResponseMessage ouvrirSeance(SeanceVM seanceVM)
        {
            Dictionary<String, Object> _model = new Dictionary<String, Object>();
            HttpState _httpState = new HttpState();
            EtatReponse _etatRep = new EtatReponse();
            Int64? _idSeance = null;
            try
            {
                Int64? _idUtilisateur = TokenManager.getIdentifiantFromToken(Request);
                ICaisseTask _caisseTask = IoCContainer.Resolve<ICaisseTask>();
                ISeanceTask _seanceTask = IoCContainer.Resolve<ISeanceTask>();
                SeanceDM _seanceDM = SeanceMapper.SeanceVMtoSeanceDM(seanceVM);
                CaisseDM _caisseDM = _caisseTask.getCaisseOfUtilisateur((long)_idUtilisateur);
                _seanceDM.IdUtilisateur = _idUtilisateur;
                _seanceDM.IdCaisse = _caisseDM.Identifiant;
                _idSeance = _seanceTask.ouvrirSeance(_seanceDM);
                _etatRep = new EtatReponse() { Code = EtatReponseCode.SUCCESS, Message = "RETURN OK" };
            }
            catch (Exception e)
            {
                _httpState = new HttpState() { Code = HttpStateCode.ERROR, Message = e.Message };
                MyLogger.log(Utilitaire.getEmplacement(this) + ":\n" + Utilitaire.getDetailsException(e), MyLoggerCode.STANDARD);
            }

            _model.Add("idSeance", _idSeance);
            _model.Add(HttpState.Name, _httpState);
            _model.Add(EtatReponse.Name, _etatRep);
            return Utilitaire.constructResponse(this, _model);
        }

        [HttpPost]
        public HttpResponseMessage fermerSeance(SeanceVM seanceVM)
        {
            Dictionary<String, Object> _model = new Dictionary<String, Object>();
            HttpState _httpState = new HttpState();
            EtatReponse _etatRep = new EtatReponse();
            try
            {
                Int64? _idUtilisateur = TokenManager.getIdentifiantFromToken(Request);
                ISeanceTask _seanceTask = IoCContainer.Resolve<ISeanceTask>();
                ICommandeTask _commandeTask = IoCContainer.Resolve<ICommandeTask>();
                SeanceDM _currentSeanceDM = _seanceTask.getSeanceActive((long)_idUtilisateur);
                ICollection<CommandeDM> _commandeDMs = _commandeTask.getCommandeDMsNonRegle(_currentSeanceDM.Identifiant);
                if (_commandeDMs == null || _commandeDMs.Count == 0)
                {
                    SeanceDM _seanceDM = SeanceMapper.SeanceVMtoSeanceDM(seanceVM);
                    _seanceDM.DateFin = DateTime.Now;
                    _seanceTask.fermerSeance(_seanceDM);
                    _etatRep = new EtatReponse() { Code = EtatReponseCode.SUCCESS, Message = "RETURN OK" };
                }
                else {
                    _etatRep = new EtatReponse() { Code = EtatReponseCode.DANGER, Message = "Commandes en attente non reglées" };
                }
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

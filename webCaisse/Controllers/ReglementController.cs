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
    public class ReglementController : ApiController
    {
        
        [HttpPost]
        public HttpResponseMessage getReglementById(ParamInt paramInt)
        {
            Int64? _idCommande = paramInt.Valeur;
            Dictionary<String, Object> _model = new Dictionary<String, Object>();
            HttpState _httpState = new HttpState();
            EtatReponse _etatRep = new EtatReponse();
            CommandeVM _commandeVM = null;
            try
            {
                ICommandeTask _commandeTask = IoCContainer.Resolve<ICommandeTask>();
                IDetailCommandeTask _detailCommandeTask = IoCContainer.Resolve<IDetailCommandeTask>();
                CommandeDM _commandeDM = _commandeTask.getCommandeDMById(_idCommande);
                if (_commandeDM != null) {
                    _commandeDM.DetailCommandeDMs = _detailCommandeTask.getDetailCommandesDMByIdCommande(_idCommande);
                    _commandeVM = CommandeMapper.CommandeDMtoCommandeVM(_commandeDM);
                    if (_commandeDM.DetailCommandeDMs != null)
                    {
                        _commandeVM.DetailCommandes = new List<DetailCommandeVM>();
                        foreach (DetailCommandeDM _detailCommandeDM in _commandeDM.DetailCommandeDMs)
                        {
                            _commandeVM.DetailCommandes.Add(DetailCommandeMapper.DetailCommandeDMtoDetailCommandeVM(_detailCommandeDM));
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
            
            _model.Add("commandeVM", _commandeVM);
            _model.Add(HttpState.Name, _httpState);
            _model.Add(EtatReponse.Name, _etatRep);
            return Utilitaire.constructResponse(this, _model);
        }

        [HttpPost]
        public HttpResponseMessage getReglementsByIdCommande(ParamInt paramInt)
        {
            Int64? _idCommande = paramInt.Valeur;
            Dictionary<String, Object> _model = new Dictionary<String, Object>();
            HttpState _httpState = new HttpState();
            EtatReponse _etatRep = new EtatReponse();
            ICollection<ReglementVM> _reglementVMs = null;
            try
            {
                IReglementTask _reglementTask = IoCContainer.Resolve<IReglementTask>();
                IDetailReglementTask _detailReglementTask = IoCContainer.Resolve<IDetailReglementTask>();
                ICollection<ReglementDM> _reglementDMs = _reglementTask.getReglementDMsByIdCommande(_idCommande);
                if (_reglementDMs != null)
                {
                    _reglementVMs = new List<ReglementVM>();
                    foreach (ReglementDM _obj in _reglementDMs)
                    {
                        ReglementVM _reglementVM = ReglementMapper.ReglementDMtoReglementVM(_obj);
                        ICollection<DetailReglementDM> _detailReglementDMs = _detailReglementTask.getDetailReglementsDMByIdReglement(_obj.Identifiant);
                        if (_detailReglementDMs != null) {
                            _reglementVM.DetailReglements = new List<DetailReglementVM>();
                            foreach (DetailReglementDM _dr in _detailReglementDMs) {
                                DetailReglementVM _detailReglementVM = DetailReglementMapper.DetailReglementDMtoDetailReglementVM(_dr);
                                _reglementVM.DetailReglements.Add(_detailReglementVM);
                            }
                        }
                        _reglementVMs.Add(_reglementVM);
                    }
                }
                _etatRep = new EtatReponse() { Code = EtatReponseCode.SUCCESS, Message = "RETURN OK" };
            }
            catch (Exception e)
            {
                _httpState = new HttpState() { Code = HttpStateCode.ERROR, Message = e.Message };
                MyLogger.log(Utilitaire.getEmplacement(this) + ":\n" + Utilitaire.getDetailsException(e), MyLoggerCode.STANDARD);
            }

            _model.Add("reglementVMs", _reglementVMs);
            _model.Add(HttpState.Name, _httpState);
            _model.Add(EtatReponse.Name, _etatRep);
            return Utilitaire.constructResponse(this, _model);
        }

        [HttpPost]
        public HttpResponseMessage etablirReglement(ReglementVM reglementVM)
        {
            Dictionary<String, Object> _model = new Dictionary<String, Object>();
            HttpState _httpState = new HttpState();
            EtatReponse _etatRep = new EtatReponse();
            Int64? _idReglement = null;
            try
            {
                Int64? _idUtilisateur = TokenManager.getIdentifiantFromToken(Request);
                IReglementTask _reglementTask = IoCContainer.Resolve<IReglementTask>();
                IModeReglementTask _modeReglementTask = IoCContainer.Resolve<IModeReglementTask>();
                ISeanceTask _seanceTask = IoCContainer.Resolve<ISeanceTask>();
                SeanceDM _seanceDM = _seanceTask.getSeanceActive((long)_idUtilisateur);
                ReglementDM _reglementDM = ReglementMapper.ReglementVMtoReglementDM(reglementVM);
                ModeReglementDM modereglement = _modeReglementTask.getModeReglementDMByCode(reglementVM.LibelleModeReglement);
                if (modereglement!=null)
                {
                    _reglementDM.IdModeReglement = modereglement.Identifiant;
                }
              
                
                
                _reglementDM.IdCreePar = _idUtilisateur;
                _reglementDM.IdSeance = _seanceDM.Identifiant;
              /*  if (reglementVM.DetailReglements != null)
                {
                    _reglementDM.DetailReglementDMs = new List<DetailReglementDM>();
                    foreach (DetailReglementVM _detailReglementVM in reglementVM.DetailReglements)
                    {
                        DetailReglementDM _detailReglementDM = DetailReglementMapper.DetailReglementVMtoDetailReglementDM(_detailReglementVM);
                        _detailReglementDM.EnActivite = 1;
                        _reglementDM.DetailReglementDMs.Add(_detailReglementDM);
                    }
                }*/

                _idReglement = _reglementTask.etablirReglement(_reglementDM);

                _etatRep = new EtatReponse() { Code = EtatReponseCode.SUCCESS, Message = "RETURN OK" };
            }
            catch (Exception e)
            {
                _httpState = new HttpState() { Code = HttpStateCode.ERROR, Message = e.Message };
                MyLogger.log(Utilitaire.getEmplacement(this) + ":\n" + Utilitaire.getDetailsException(e), MyLoggerCode.STANDARD);
            }
            _model.Add("idReglement", _idReglement);
            _model.Add(HttpState.Name, _httpState);
            _model.Add(EtatReponse.Name, _etatRep);
            return Utilitaire.constructResponse(this, _model);
        }

        [HttpPost]
        public HttpResponseMessage reglerTemporairement(ParamInt paramInt)
        {
            Int64? _idCommande = paramInt.Valeur;
            Dictionary<String, Object> _model = new Dictionary<String, Object>();
            HttpState _httpState = new HttpState();
            EtatReponse _etatRep = new EtatReponse();
            Int64? _idReglement = null;
            try
            {
                ISeanceTask _seanceTask = IoCContainer.Resolve<ISeanceTask>();
                IReglementTask _reglementTask = IoCContainer.Resolve<IReglementTask>();
                ICommandeTask _commandeTask = IoCContainer.Resolve<ICommandeTask>();
                IDetailCommandeTask _detailCommandeTask = IoCContainer.Resolve<IDetailCommandeTask>();
                ILocaliteTask _localiteTask = IoCContainer.Resolve<ILocaliteTask>();                
                Int64? _idUtilisateur = TokenManager.getIdentifiantFromToken(Request);
                SeanceDM _seanceDM = _seanceTask.getSeanceActive((long)_idUtilisateur);
                CommandeDM _commandeDM = _commandeTask.getCommandeDMById(_idCommande);
                if (_commandeDM != null)
                {
                    ReglementDM _reglementDM = new ReglementDM() {
                        DateReglement = DateTime.Now,
                        EnActivite = 1,
                        IdCommande = _commandeDM.Identifiant,
                        IdCreePar = _idUtilisateur,    
                        IdSeance = _seanceDM.Identifiant,                        
                    };
                    _commandeDM.DetailCommandeDMs = _detailCommandeTask.getDetailCommandesDMByIdCommande(_idCommande);
                    if (_commandeDM.DetailCommandeDMs != null)
                    {
                        _reglementDM.DetailReglementDMs = new List<DetailReglementDM>();
                        foreach(DetailCommandeDM dc in _commandeDM.DetailCommandeDMs) {
                            DetailReglementDM _detailReglementDM = new DetailReglementDM() {
                                EnActivite = 1,
                                IdDetailCommande = dc.Identifiant,
                                Quantite = dc.Quantite,
                                Montant = dc.Montant,                                
                            };
                            _reglementDM.DetailReglementDMs.Add(_detailReglementDM);
                        }
                    }
                    _reglementTask.etablirReglement(_reglementDM);
                }

                //_commandeTask.changerEtatCommande((long)_idCommande, EtatCommandeCode.REGLEE);
                if (_commandeDM.IdLocalite != null) {
                    _localiteTask.changerEtatLocalite((long)_commandeDM.IdLocalite, EtatLocaliteCode.DISPONIBLE);
                }
                _etatRep = new EtatReponse() { Code = EtatReponseCode.SUCCESS, Message = "RETURN OK" };
            }
            catch (Exception e)
            {
                _httpState = new HttpState() { Code = HttpStateCode.ERROR, Message = e.Message };
                MyLogger.log(Utilitaire.getEmplacement(this) + ":\n" + Utilitaire.getDetailsException(e), MyLoggerCode.STANDARD);
            }
            _model.Add("idReglement", _idReglement);
            _model.Add(HttpState.Name, _httpState);
            _model.Add(EtatReponse.Name, _etatRep);
            return Utilitaire.constructResponse(this, _model);
        }
        [HttpPost]
        public HttpResponseMessage getMontantTotalReglementForSeance(ParamInt paramInt)
        {
            Int64? _idSeance = paramInt.Valeur;
            Dictionary<String, Object> _model = new Dictionary<String, Object>();
            HttpState _httpState = new HttpState();
            EtatReponse _etatRep = new EtatReponse();
            Double? _montantDebut = 0;
            Double? _montantSeance = 0;
            Double? _montantTotal = 0;
            try
            {
                Int64? _idUtilisateur = TokenManager.getIdentifiantFromToken(Request);
                ISeanceTask _seanceTask = IoCContainer.Resolve<ISeanceTask>();
                IReglementTask _reglementTask = IoCContainer.Resolve<IReglementTask>();
                SeanceDM _seanceDM = _seanceTask.getSeanceActive((long)_idUtilisateur);
                _montantDebut = _seanceDM.MontantDebut;
                ICollection<ReglementDM> _reglementDMs = _reglementTask.getReglementDMs(_idSeance: _idSeance);
                if (_reglementDMs != null)
                {
                    _montantSeance = _reglementDMs.Sum(a => a.Montant);
                    _montantTotal = _montantDebut + _montantSeance;
                }
                _etatRep = new EtatReponse() { Code = EtatReponseCode.SUCCESS, Message = "RETURN OK" };
            }
            catch (Exception e)
            {
                _httpState = new HttpState() { Code = HttpStateCode.ERROR, Message = e.Message };
                MyLogger.log(Utilitaire.getEmplacement(this) + ":\n" + Utilitaire.getDetailsException(e), MyLoggerCode.STANDARD);
            }

            _model.Add("montantDebut", _montantDebut);
            _model.Add("montantSeance", _montantSeance);
            _model.Add("montantTotal", _montantTotal);
            _model.Add(HttpState.Name, _httpState);
            _model.Add(EtatReponse.Name, _etatRep);
            return Utilitaire.constructResponse(this, _model);
        }


        [HttpPost]
        public HttpResponseMessage getMontantTotalReglementForCaisse(ParamInt paramInt)
        {
            Int64? _idCaisse = paramInt.Valeur;
            Dictionary<String, Object> _model = new Dictionary<String, Object>();
            HttpState _httpState = new HttpState();
            EtatReponse _etatRep = new EtatReponse();
            Double? _montantDebut = 0;
            Double? _montantSeance = 0;
            Double? _montantTotal = 0;
            try
            {
                Int64? _idUtilisateur = TokenManager.getIdentifiantFromToken(Request);
                ISeanceTask _seanceTask = IoCContainer.Resolve<ISeanceTask>();
                IReglementTask _reglementTask = IoCContainer.Resolve<IReglementTask>();
                SeanceDM _seanceDM = _seanceTask.getSeanceCaisse((long)_idCaisse);
                _montantDebut = _seanceDM.MontantDebut;

                ICollection<ReglementDM> _reglementDMs = _reglementTask.getReglementDMs(_idSeance: _seanceDM.Identifiant);
                if (_reglementDMs != null)
                {
                    _montantSeance = _reglementDMs.Sum(a => a.Montant);
                    _montantTotal = _montantDebut + _montantSeance;
                }
                _etatRep = new EtatReponse() { Code = EtatReponseCode.SUCCESS, Message = "RETURN OK" };
            }
            catch (Exception e)
            {
                _httpState = new HttpState() { Code = HttpStateCode.ERROR, Message = e.Message };
                MyLogger.log(Utilitaire.getEmplacement(this) + ":\n" + Utilitaire.getDetailsException(e), MyLoggerCode.STANDARD);
            }

            _model.Add("montantDebut", _montantDebut);
            _model.Add("montantSeance", _montantSeance);
            _model.Add("montantTotal", _montantTotal);
            _model.Add(HttpState.Name, _httpState);
            _model.Add(EtatReponse.Name, _etatRep);
            return Utilitaire.constructResponse(this, _model);
        }


    }
}

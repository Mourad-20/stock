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
    public class CommandeController : ApiController
    {
        
       [HttpPost]
        public HttpResponseMessage allimentationstock(CommandeVM commandeVM)
        {
            Dictionary<String, Object> _model = new Dictionary<String, Object>();
            HttpState _httpState = new HttpState();
            EtatReponse _etatRep = new EtatReponse();
            Int64? _idCommande = null;
            try
            {
                Int64? _idUtilisateur = TokenManager.getIdentifiantFromToken(Request);
                IUtilisateurTask _utilisateurTask = IoCContainer.Resolve<IUtilisateurTask>();
                //-----------------------------------------------------------------
                String _message = "";
                Boolean _isOk = true;
                if (_isOk && (commandeVM.DetailCommandes == null || commandeVM.DetailCommandes.Count <= 0))
                {
                    _message = "Liste des articles vide";
                    _isOk = false;
                }
                ISituationCommandeTask _situationCommandeTask = IoCContainer.Resolve<ISituationCommandeTask>();
                IGroupeTask _groupeTask = IoCContainer.Resolve<IGroupeTask>();
                ICollection<GroupeDM> _groupeDMs = _groupeTask.getGroupesOfUtilisateur((long)_idUtilisateur);
                SituationCommandeDM _situationCommandeDM = _situationCommandeTask.getSituationCommandeDMByCode(SituationCommandeCode.ENCOURS_CREATION);
                if (_isOk && ((commandeVM.IdServeur == null || commandeVM.IdServeur <= 0) && _utilisateurTask.isUtilisateurOnGroupes(_groupeDMs, new List<String>() { GroupeCode.GERANT, GroupeCode.ADMIN })))
                {
                   // _message = "Serveur non selectionné";
                    //_isOk = false;
                }

                if (_isOk && ((commandeVM.IdLocalite == null || commandeVM.IdLocalite <= 0) && _utilisateurTask.isUtilisateurOnGroupes(_groupeDMs, new List<String>() { GroupeCode.GERANT, GroupeCode.ADMIN, GroupeCode.SERVEUR, GroupeCode.CAISSIER })))
                {
                   // _message = "Localité non selectionnée";
                   // _isOk = false;
                }



                //-----------------------------------------------------------------
                if (_isOk)
                {
                    ICommandeTask _commandeTask = IoCContainer.Resolve<ICommandeTask>();
                    ISeanceTask _seanceTask = IoCContainer.Resolve<ISeanceTask>();
                    ITypeArticleTask _typeUniteTask = IoCContainer.Resolve<ITypeArticleTask>();
                    SeanceDM _seanceDM = _seanceTask.getSeanceActive((long)_idUtilisateur);
                    CommandeDM _commandeDM = CommandeMapper.CommandeVMtoCommandeDM(commandeVM);
                    _commandeDM.IdCreePar = _idUtilisateur;
                    _commandeDM.IdSeance = _seanceDM.Identifiant;

                    if ((_commandeDM.IdServeur == null || _commandeDM.IdServeur <= 0) && !_utilisateurTask.isUtilisateurOnGroupes(_groupeDMs, new List<String>() { }))
                    {
                        _commandeDM.IdServeur = _idUtilisateur;
                    }

                    _commandeDM.DetailCommandeDMs = new List<DetailCommandeDM>();
                    foreach (DetailCommandeVM _detailCommandeVM in commandeVM.DetailCommandes)
                    {
                        string code = _detailCommandeVM.LibelleTypeUnite;
                        //TypeUniteDM _typeUniteDM = _typeUniteTask.getTypeUniteDMByCode(code);
                        DetailCommandeDM _detailCommandeDM = DetailCommandeMapper.DetailCommandeVMtoDetailCommandeDM(_detailCommandeVM);
                        _detailCommandeDM.EnActivite = 1;
                        _detailCommandeDM.IdCreePar = _idUtilisateur;
                        _detailCommandeDM.QuantiteServi = 0;
                        _detailCommandeDM.IdSituation = _situationCommandeDM.Identifiant;
                        _detailCommandeDM.IdCreePar = _idUtilisateur;
                        _commandeDM.DetailCommandeDMs.Add(_detailCommandeDM);
                        

                        if (_detailCommandeVM.AffectationMessages != null)
                        {
                            _detailCommandeDM.AffectationMessageDMs = new List<AffectationMessageDM>();
                            foreach (AffectationMessageVM _affectationMessageVM in _detailCommandeVM.AffectationMessages)
                            {
                                AffectationMessageDM _affectationMessageDM = AffectationMessageMapper.AffectationMessageVMtoAffectationMessageDM(_affectationMessageVM);
                                _detailCommandeDM.AffectationMessageDMs.Add(_affectationMessageDM);
                            }
                        }

                    }
                    _commandeDM.CodeCommande = TypeCommandeCode.ALLIMENTATION;
                    _idCommande = _commandeTask.allimentationstock(_commandeDM);
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
            _model.Add("idCommande", _idCommande);
            _model.Add(HttpState.Name, _httpState);
            _model.Add(EtatReponse.Name, _etatRep);
            return Utilitaire.constructResponse(this, _model);
        }

        [HttpPost]
        public HttpResponseMessage etablirCommande(CommandeVM commandeVM)
        {
            Dictionary<String, Object> _model = new Dictionary<String, Object>();
            HttpState _httpState = new HttpState();
            EtatReponse _etatRep = new EtatReponse();
            Int64? _idCommande = null;
            try
            {
                Int64? _idUtilisateur = TokenManager.getIdentifiantFromToken(Request);
                IUtilisateurTask _utilisateurTask = IoCContainer.Resolve<IUtilisateurTask>();
                ITypeArticleTask _typeUniteTask = IoCContainer.Resolve<ITypeArticleTask>();
                //-----------------------------------------------------------------
                String _message = "";
                Boolean _isOk = true;
                if (_isOk && (commandeVM.DetailCommandes == null || commandeVM.DetailCommandes.Count <= 0)) {
                    _message = "Liste des articles vide";
                    _isOk = false;
                }
                ISituationCommandeTask _situationCommandeTask = IoCContainer.Resolve<ISituationCommandeTask>();
                IGroupeTask _groupeTask = IoCContainer.Resolve<IGroupeTask>();
                ICollection<GroupeDM> _groupeDMs = _groupeTask.getGroupesOfUtilisateur((long)_idUtilisateur);
                SituationCommandeDM _situationCommandeDM = _situationCommandeTask.getSituationCommandeDMByCode(SituationCommandeCode.ENCOURS_CREATION);
                if (_isOk && ((commandeVM.IdServeur == null || commandeVM.IdServeur <= 0) && _utilisateurTask.isUtilisateurOnGroupes(_groupeDMs, new List<String>() { GroupeCode.GERANT, GroupeCode.ADMIN })))
                {
                  //  _message = "Serveur non selectionné";
                   // _isOk = false;
                }

                if (_isOk && ((commandeVM.IdLocalite == null || commandeVM.IdLocalite <= 0) && _utilisateurTask.isUtilisateurOnGroupes(_groupeDMs, new List<String>() { GroupeCode.GERANT, GroupeCode.ADMIN, GroupeCode.SERVEUR, GroupeCode.CAISSIER })))
                {
                    _message = "Client non selectionnée";
                    _isOk = false;
                }



                //-----------------------------------------------------------------
                if (_isOk)
                {
                    ICommandeTask _commandeTask = IoCContainer.Resolve<ICommandeTask>();
                    ISeanceTask _seanceTask = IoCContainer.Resolve<ISeanceTask>();
                   // SeanceDM _seanceDM = _seanceTask.getSeanceActive((long)_idUtilisateur);
                    CommandeDM _commandeDM = CommandeMapper.CommandeVMtoCommandeDM(commandeVM);
                    _commandeDM.IdCreePar = _idUtilisateur;
                   // _commandeDM.IdSeance = _seanceDM.Identifiant;

                    if ((_commandeDM.IdServeur == null || _commandeDM.IdServeur <= 0) && !_utilisateurTask.isUtilisateurOnGroupes(_groupeDMs,new List<String>() { })) {
                        _commandeDM.IdServeur = _idUtilisateur;
                    }

                    _commandeDM.DetailCommandeDMs = new List<DetailCommandeDM>();
                    foreach (DetailCommandeVM _detailCommandeVM in commandeVM.DetailCommandes)
                    {
                        DetailCommandeDM _detailCommandeDM = DetailCommandeMapper.DetailCommandeVMtoDetailCommandeDM(_detailCommandeVM);
                        _detailCommandeDM.EnActivite = 1;
                        _detailCommandeDM.IdCreePar = _idUtilisateur;
                        _detailCommandeDM.QuantiteServi = 0;
                        _detailCommandeDM.IdSituation = _situationCommandeDM.Identifiant;
                        _detailCommandeDM.IdCreePar = _idUtilisateur;

                        _commandeDM.DetailCommandeDMs.Add(_detailCommandeDM);

                        if (_detailCommandeVM.AffectationMessages != null)
                        {
                            _detailCommandeDM.AffectationMessageDMs = new List<AffectationMessageDM>();
                            foreach (AffectationMessageVM _affectationMessageVM in _detailCommandeVM.AffectationMessages)
                            {
                                AffectationMessageDM _affectationMessageDM = AffectationMessageMapper.AffectationMessageVMtoAffectationMessageDM(_affectationMessageVM);
                                _detailCommandeDM.AffectationMessageDMs.Add(_affectationMessageDM);
                            }
                        }

                    }
                    //_commandeDM.CodeCommande = TypeCommandeCode.VENT;
                    _idCommande = _commandeTask.etablirCommande(_commandeDM);
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
            _model.Add("idCommande", _idCommande);
            _model.Add(HttpState.Name, _httpState);
            _model.Add(EtatReponse.Name, _etatRep);
            return Utilitaire.constructResponse(this, _model);            
        }

        [HttpPost]
        public HttpResponseMessage modifierCommande(CommandeVM commandeVM)
        {
            Dictionary<String, Object> _model = new Dictionary<String, Object>();
            HttpState _httpState = new HttpState();
            EtatReponse _etatRep = new EtatReponse();
            Int64? _idCommande = null;
            try
            {
                Int64? _idUtilisateur = TokenManager.getIdentifiantFromToken(Request);
                IUtilisateurTask _utilisateurTask = IoCContainer.Resolve<IUtilisateurTask>();
                //-----------------------------------------------------------------
                String _message = "";
                Boolean _isOk = true;
                if (_isOk && (commandeVM.DetailCommandes == null || commandeVM.DetailCommandes.Count <= 0))
                {
                    _message = "Liste des articles vide";
                    _isOk = false;
                }


                ISituationCommandeTask _situationCommandeTask = IoCContainer.Resolve<ISituationCommandeTask>();
                IGroupeTask _groupeTask = IoCContainer.Resolve<IGroupeTask>();
                ICollection<GroupeDM> _groupeDMs = _groupeTask.getGroupesOfUtilisateur((long)_idUtilisateur);
                SituationCommandeDM _situationCommandeDM = _situationCommandeTask.getSituationCommandeDMByCode(SituationCommandeCode.ENCOURS_CREATION);

                if (_isOk && ((commandeVM.IdServeur == null || commandeVM.IdServeur <= 0) && _utilisateurTask.isUtilisateurOnGroupes(_groupeDMs, new List<String>() { GroupeCode.GERANT, GroupeCode.ADMIN })))
                {
                    //_message = "Serveur non selectionné";
                    //_isOk = false;
                }

                if (_isOk && ((commandeVM.IdLocalite == null || commandeVM.IdLocalite <= 0) && _utilisateurTask.isUtilisateurOnGroupes(_groupeDMs, new List<String>() { GroupeCode.GERANT, GroupeCode.ADMIN, GroupeCode.SERVEUR })))
                {
                    _message = "Client non selectionnée";
                    _isOk = false;
                }

                //-----------------------------------------------------------------
                if (_isOk)
                {
                    ICommandeTask _commandeTask = IoCContainer.Resolve<ICommandeTask>();
                    CommandeDM _commandeDM = CommandeMapper.CommandeVMtoCommandeDM(commandeVM);

                    if ((_commandeDM.IdServeur == null || _commandeDM.IdServeur <= 0) && !_utilisateurTask.isUtilisateurOnGroupes(_groupeDMs, new List<String>() { }))
                    {
                        _commandeDM.IdServeur = _idUtilisateur;
                    }
                    if (_commandeDM.IdCreePar!= _idUtilisateur)
                    {
                        _commandeDM.IdServeur = null;
                    }


                    if (commandeVM.DetailCommandes != null)
                    {
                        _commandeDM.DetailCommandeDMs = new List<DetailCommandeDM>();
                        foreach (DetailCommandeVM _detailCommandeVM in commandeVM.DetailCommandes)
                        {
                            DetailCommandeDM _detailCommandeDM = DetailCommandeMapper.DetailCommandeVMtoDetailCommandeDM(_detailCommandeVM);
                            _detailCommandeDM.EnActivite = 1;

                            _detailCommandeDM.IdCreePar = _idUtilisateur;
                            //_detailCommandeDM.QuantiteServi = 0;
                            _detailCommandeDM.IdSituation = _situationCommandeDM.Identifiant;
                            _detailCommandeDM.IdCreePar = _idUtilisateur;

                            //_detailCommandeDM.IdCreePar = _idUtilisateur;
                            _commandeDM.DetailCommandeDMs.Add(_detailCommandeDM);
                            _commandeDM.CodeCommande = TypeCommandeCode.VENT;
                            if (_detailCommandeVM.AffectationMessages != null) {
                                _detailCommandeDM.AffectationMessageDMs = new List<AffectationMessageDM>();
                                foreach (AffectationMessageVM _affectationMessageVM in _detailCommandeVM.AffectationMessages) {
                                    AffectationMessageDM _affectationMessageDM = AffectationMessageMapper.AffectationMessageVMtoAffectationMessageDM(_affectationMessageVM);
                                    _detailCommandeDM.AffectationMessageDMs.Add(_affectationMessageDM);
                                }
                            }
                        }
                    }

                    _idCommande = _commandeTask.modifierCommande(_commandeDM);
                    _etatRep = new EtatReponse() { Code = EtatReponseCode.SUCCESS, Message = "RETURN OK" };
                }
                else {
                    _etatRep = new EtatReponse() { Code = EtatReponseCode.DANGER, Message = _message };
                }
            }
            catch (Exception e)
            {
                _httpState = new HttpState() { Code = HttpStateCode.ERROR, Message = e.Message };
                MyLogger.log(Utilitaire.getEmplacement(this) + ":\n" + Utilitaire.getDetailsException(e), MyLoggerCode.STANDARD);
            }
            _model.Add("idCommande", _idCommande);
            _model.Add(HttpState.Name, _httpState);
            _model.Add(EtatReponse.Name, _etatRep);
            return Utilitaire.constructResponse(this, _model);
        }

        [HttpPost]
        public HttpResponseMessage validerCommande(CommandeVM commandeVM)
        {
            Dictionary<String, Object> _model = new Dictionary<String, Object>();
            HttpState _httpState = new HttpState();
            EtatReponse _etatRep = new EtatReponse();
            Int64? _idCommande = null;
            try
            {
                Int64? _idUtilisateur = TokenManager.getIdentifiantFromToken(Request);
                IUtilisateurTask _utilisateurTask = IoCContainer.Resolve<IUtilisateurTask>();
                //-----------------------------------------------------------------
                String _message = "";
                Boolean _isOk = true;
                if (_isOk && (commandeVM.DetailCommandes == null || commandeVM.DetailCommandes.Count <= 0))
                {
                    _message = "Liste des articles vide";
                    _isOk = false;
                }
                ISituationCommandeTask _situationCommandeTask = IoCContainer.Resolve<ISituationCommandeTask>();
                IGroupeTask _groupeTask = IoCContainer.Resolve<IGroupeTask>();
                ICollection<GroupeDM> _groupeDMs = _groupeTask.getGroupesOfUtilisateur((long)_idUtilisateur);
                SituationCommandeDM _situationCommandeDM = _situationCommandeTask.getSituationCommandeDMByCode(SituationCommandeCode.LIVEREE);

                if (_isOk && ((commandeVM.IdServeur == null || commandeVM.IdServeur <= 0) && _utilisateurTask.isUtilisateurOnGroupes(_groupeDMs, new List<String>() { GroupeCode.GERANT, GroupeCode.ADMIN })))
                {
                    _message = "Serveur non selectionné";
                    _isOk = false;
                }

                if (_isOk && ((commandeVM.IdLocalite == null || commandeVM.IdLocalite <= 0) && _utilisateurTask.isUtilisateurOnGroupes(_groupeDMs, new List<String>() { GroupeCode.GERANT, GroupeCode.ADMIN, GroupeCode.SERVEUR })))
                {
                    _message = "Localité non selectionnée";
                    _isOk = false;
                }

                //-----------------------------------------------------------------
                if (_isOk)
                {
                    ICommandeTask _commandeTask = IoCContainer.Resolve<ICommandeTask>();
                    CommandeDM _commandeDM = CommandeMapper.CommandeVMtoCommandeDM(commandeVM);

                    if ((_commandeDM.IdServeur == null || _commandeDM.IdServeur <= 0) && !_utilisateurTask.isUtilisateurOnGroupes(_groupeDMs, new List<String>() { }))
                    {
                        _commandeDM.IdServeur = _idUtilisateur;
                    }


                    if (commandeVM.DetailCommandes != null)
                    {
                        _commandeDM.DetailCommandeDMs = new List<DetailCommandeDM>();
                        foreach (DetailCommandeVM _detailCommandeVM in commandeVM.DetailCommandes)
                        {
                            DetailCommandeDM _detailCommandeDM = DetailCommandeMapper.DetailCommandeVMtoDetailCommandeDM(_detailCommandeVM);
                            _detailCommandeDM.IdValiderPar = _idUtilisateur;
                            _detailCommandeDM.IdSituation = _situationCommandeDM.Identifiant;
                            _commandeDM.DetailCommandeDMs.Add(_detailCommandeDM);
                           
                        }
                    }

                    _idCommande = _commandeTask.validerCommande(_commandeDM);
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
            _model.Add("idCommande", _idCommande);
            _model.Add(HttpState.Name, _httpState);
            _model.Add(EtatReponse.Name, _etatRep);
            return Utilitaire.constructResponse(this, _model);
        }

        [HttpPost]
        public HttpResponseMessage controlerCommande(CommandeVM commandeVM)
        {
            Dictionary<String, Object> _model = new Dictionary<String, Object>();
            HttpState _httpState = new HttpState();
            EtatReponse _etatRep = new EtatReponse();
            Int64? _idCommande = null;
            try
            {
                Int64? _idUtilisateur = TokenManager.getIdentifiantFromToken(Request);
                IUtilisateurTask _utilisateurTask = IoCContainer.Resolve<IUtilisateurTask>();
                //-----------------------------------------------------------------
                String _message = "";
                Boolean _isOk = true;
               
                ISituationCommandeTask _situationCommandeTask = IoCContainer.Resolve<ISituationCommandeTask>();
                IGroupeTask _groupeTask = IoCContainer.Resolve<IGroupeTask>();
                ICollection<GroupeDM> _groupeDMs = _groupeTask.getGroupesOfUtilisateur((long)_idUtilisateur);
                SituationCommandeDM _situationCommandeDM = _situationCommandeTask.getSituationCommandeDMByCode(SituationCommandeCode.LIVEREE);

                

              

                //-----------------------------------------------------------------
                if (_isOk)
                {
                    ICommandeTask _commandeTask = IoCContainer.Resolve<ICommandeTask>();
                    IDetailCommandeTask _detailCommandeTask = IoCContainer.Resolve<IDetailCommandeTask>();
                    CommandeDM _commandeDM = CommandeMapper.CommandeVMtoCommandeDM(commandeVM);
                     _commandeDM = _commandeTask.getCommandeDMById(_commandeDM.Identifiant);
                    if (_commandeDM != null)
                    {
                        ICollection<DetailCommandeDM> _detailCommandeDMs = _detailCommandeTask.getDetailCommandesDMByIdCommande(_commandeDM.Identifiant);
                       // _commandeVM = CommandeMapper.CommandeDMtoCommandeVM(_commandeDM);
                        if (_detailCommandeDMs != null)
                        {
                            _commandeDM.DetailCommandeDMs = new List<DetailCommandeDM>();
                            foreach (DetailCommandeDM _detailCommandeDM in _detailCommandeDMs)
                            {

                                if (_detailCommandeDM.IdValiderPar==null)
                                {
                                    _detailCommandeDM.IdValiderPar = _idUtilisateur;
                                    _detailCommandeDM.IdSituation = _situationCommandeDM.Identifiant;
                                    _commandeDM.DetailCommandeDMs.Add(_detailCommandeDM);
                                }
                                   
                                
                            }
                            _detailCommandeTask.validateDetailCommandeDM(_commandeDM.DetailCommandeDMs);
                        }

                    }



                    if (commandeVM.DetailCommandes != null)
                    {
                        _commandeDM.DetailCommandeDMs = new List<DetailCommandeDM>();
                        foreach (DetailCommandeVM _detailCommandeVM in commandeVM.DetailCommandes)
                        {
                            DetailCommandeDM _detailCommandeDM = DetailCommandeMapper.DetailCommandeVMtoDetailCommandeDM(_detailCommandeVM);
                            _detailCommandeDM.IdValiderPar = _idUtilisateur;
                            _detailCommandeDM.IdSituation = _situationCommandeDM.Identifiant;
                            _commandeDM.DetailCommandeDMs.Add(_detailCommandeDM);

                        }
                    }

                    _idCommande = _commandeTask.validerCommande(_commandeDM);
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
            _model.Add("idCommande", _idCommande);
            _model.Add(HttpState.Name, _httpState);
            _model.Add(EtatReponse.Name, _etatRep);
            return Utilitaire.constructResponse(this, _model);
        }


        [HttpPost]
        public HttpResponseMessage getCommandeById(ParamInt paramInt)
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
                IMessageTask _messageTask = IoCContainer.Resolve<IMessageTask>();
                IAffectationMessageTask _affectationMessageTask = IoCContainer.Resolve<IAffectationMessageTask>();
                CommandeDM _commandeDM = _commandeTask.getCommandeDMById(_idCommande);
                if (_commandeDM != null) {
                    _commandeDM.DetailCommandeDMs = _detailCommandeTask.getDetailCommandesDMByIdCommande(_idCommande);
                    _commandeVM = CommandeMapper.CommandeDMtoCommandeVM(_commandeDM);
                    if (_commandeDM.DetailCommandeDMs != null)
                    {
                        _commandeVM.DetailCommandes = new List<DetailCommandeVM>();
                        foreach (DetailCommandeDM _detailCommandeDM in _commandeDM.DetailCommandeDMs)
                        {
                            ICollection<AffectationMessageDM> _affectationMessageDMs = _affectationMessageTask.getAffectationMessageDMsByIdDetailCommande(_detailCommandeDM.Identifiant);
                            DetailCommandeVM _detailCommandeVM = DetailCommandeMapper.DetailCommandeDMtoDetailCommandeVM(_detailCommandeDM);
                            if (_affectationMessageDMs != null) {
                                _detailCommandeVM.AffectationMessages = new List<AffectationMessageVM>();
                                foreach (AffectationMessageDM am in _affectationMessageDMs) {
                                    _detailCommandeVM.AffectationMessages.Add(AffectationMessageMapper.AffectationMessageDMtoAffectationMessageVM(am));
                                }
                            }
                            _commandeVM.DetailCommandes.Add(_detailCommandeVM);
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
        public HttpResponseMessage getCommandesNonReglees()
        {
            Dictionary<String, Object> _model = new Dictionary<String, Object>();
            HttpState _httpState = new HttpState();
            EtatReponse _etatRep = new EtatReponse();
            ICollection<CommandeVM> _commandeVMs = null;
            try
            {
                Int64? _idUtilisateur = TokenManager.getIdentifiantFromToken(Request);
                ICommandeTask _commandeTask = IoCContainer.Resolve<ICommandeTask>();
                ISeanceTask _seanceTask = IoCContainer.Resolve<ISeanceTask>();
                SeanceDM _seanceDM = _seanceTask.getSeanceActive((long) _idUtilisateur);
                ICollection<CommandeDM> _commandeDMs = _commandeTask.getCommandeDMsNonRegle(null);
                if (_commandeDMs != null)
                {
                    _commandeVMs = new List<CommandeVM>();
                    foreach (CommandeDM _obj in _commandeDMs)
                    {
                        CommandeVM _commandeVM = CommandeMapper.CommandeDMtoCommandeVM(_obj);
                        _commandeVMs.Add(_commandeVM);
                    }
                }
                _etatRep = new EtatReponse() { Code = EtatReponseCode.SUCCESS, Message = "RETURN OK" };
            }
            catch (Exception e)
            {
                _httpState = new HttpState() { Code = HttpStateCode.ERROR, Message = e.Message };
                MyLogger.log(Utilitaire.getEmplacement(this) + ":\n" + Utilitaire.getDetailsException(e), MyLoggerCode.STANDARD);
            }

            _model.Add("commandeVMs", _commandeVMs);
            _model.Add(HttpState.Name, _httpState);
            _model.Add(EtatReponse.Name, _etatRep);
            return Utilitaire.constructResponse(this, _model);
        }
        [HttpPost]
        public HttpResponseMessage getCommandesNonControle()
        {
            Dictionary<String, Object> _model = new Dictionary<String, Object>();
            HttpState _httpState = new HttpState();
            EtatReponse _etatRep = new EtatReponse();
            ICollection<CommandeVM> _commandeVMs = null;
            try
            {
                Int64? _idUtilisateur = TokenManager.getIdentifiantFromToken(Request);
                ICommandeTask _commandeTask = IoCContainer.Resolve<ICommandeTask>();
                IDetailCommandeTask _detailCommande = IoCContainer.Resolve<IDetailCommandeTask>();
                ISeanceTask _seanceTask = IoCContainer.Resolve<ISeanceTask>();
                SeanceDM _seanceDM = _seanceTask.getSeanceActive((long)_idUtilisateur);
                ICollection<CommandeDM> _commandeDMs = _commandeTask.getCommandeDMsNonRegle(null);
                if (_commandeDMs != null)
                {
                    _commandeVMs = new List<CommandeVM>();
                    foreach (CommandeDM _obj in _commandeDMs)
                    {
                        ICollection<DetailCommandeDM> _detailCommandeDMs = _detailCommande.getDetailCommandesDMByIdCommande(_obj.Identifiant);
                        _detailCommandeDMs = _detailCommandeDMs.Where(a => a.IdValiderPar == null && a.DateCreation!=null).ToList();
                        if (_detailCommandeDMs.Count>0)
                        {
                            CommandeVM _commandeVM = CommandeMapper.CommandeDMtoCommandeVM(_obj);
                            _commandeVM.DetailCommandes = new List<DetailCommandeVM>();
                            foreach (DetailCommandeDM _dtc in _detailCommandeDMs)
                            {
                                DetailCommandeVM detailCommandeVM = DetailCommandeMapper.DetailCommandeDMtoDetailCommandeVM(_dtc);
                                _commandeVM.DetailCommandes.Add(detailCommandeVM);
                            }
                           
                            _commandeVMs.Add(_commandeVM);
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

            _model.Add("commandeVMs", _commandeVMs);
            _model.Add(HttpState.Name, _httpState);
            _model.Add(EtatReponse.Name, _etatRep);
            return Utilitaire.constructResponse(this, _model);
        }

        [HttpPost]
        public HttpResponseMessage getDetailCommandesNonReglees(ParamInt paramInt)
        {
            Int64? _idCommande = paramInt.Valeur;
            Dictionary<String, Object> _model = new Dictionary<String, Object>();
            HttpState _httpState = new HttpState();
            EtatReponse _etatRep = new EtatReponse();
            ICollection<DetailCommandeVM> _detailCommandeVMs = null;
            try
            {
                IDetailCommandeTask _detailCommandeTask = IoCContainer.Resolve<IDetailCommandeTask>();
                ICollection<DetailCommandeDM> _detailCommandeDMs = _detailCommandeTask.getDetailCommandesNonReglesDMByIdCommande(_idCommande);
                if (_detailCommandeDMs != null) {
                    _detailCommandeVMs = new List<DetailCommandeVM>();
                    foreach (DetailCommandeDM _detailCommandeDM in _detailCommandeDMs)
                    {
                        _detailCommandeVMs.Add(DetailCommandeMapper.DetailCommandeDMtoDetailCommandeVM(_detailCommandeDM));
                    }
                }
                _etatRep = new EtatReponse() { Code = EtatReponseCode.SUCCESS, Message = "RETURN OK" };
            }
            catch (Exception e)
            {
                _httpState = new HttpState() { Code = HttpStateCode.ERROR, Message = e.Message };
                MyLogger.log(Utilitaire.getEmplacement(this) + ":\n" + Utilitaire.getDetailsException(e), MyLoggerCode.STANDARD);
            }

            _model.Add("detailCommandeVMs", _detailCommandeVMs);
            _model.Add(HttpState.Name, _httpState);
            _model.Add(EtatReponse.Name, _etatRep);
            return Utilitaire.constructResponse(this, _model);
        }

        [HttpPost]
        public HttpResponseMessage envoyerTicketPrepation(ParamInt paramInt)
        {
            Int64? _idCommande = paramInt.Valeur;
            Dictionary<String, Object> _model = new Dictionary<String, Object>();
            HttpState _httpState = new HttpState();
            EtatReponse _etatRep = new EtatReponse();
            try
            {
                Int64? _idUtilisateur = TokenManager.getIdentifiantFromToken(Request);
                ICommandeTask _commandeTask = IoCContainer.Resolve<ICommandeTask>();
                _commandeTask.envoyerTicketPreparation((long)_idCommande);                
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
        public HttpResponseMessage envoyerTicketNote(ParamInt paramInt)
        {
            Int64? _idCommande = paramInt.Valeur;
            Dictionary<String, Object> _model = new Dictionary<String, Object>();
            HttpState _httpState = new HttpState();
            EtatReponse _etatRep = new EtatReponse();
            try
            {
                Int64? _idUtilisateur = TokenManager.getIdentifiantFromToken(Request);
                ICommandeTask _commandeTask = IoCContainer.Resolve<ICommandeTask>();
                _commandeTask.envoyerTicketNote((long)_idCommande);
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
        public HttpResponseMessage getRecap(ParamInt paramInt)
        {
            Int64? _idSeanceparam = paramInt.Valeur;
            Dictionary<String, Object> _model = new Dictionary<String, Object>();
            HttpState _httpState = new HttpState();
            EtatReponse _etatRep = new EtatReponse();
            ICollection<RecapVM> _recapVMs = null;
            ICollection<CommandeVM> _commandeVMs = new List<CommandeVM>();
            try
            {
                Int64? _idUtilisateur = TokenManager.getIdentifiantFromToken(Request);
                ICommandeTask _commandeTask = IoCContainer.Resolve<ICommandeTask>();
                ISeanceTask _seanceTask = IoCContainer.Resolve<ISeanceTask>();
                IActeurSeanceTask _acteurSeanceTask = IoCContainer.Resolve<IActeurSeanceTask>();
                SeanceDM _seanceDM = new SeanceDM();
                IUtilisateurTask _utilisateurTask = IoCContainer.Resolve<IUtilisateurTask>();
                if (_idSeanceparam==null)
                {
                     _seanceDM = _seanceTask.getSeanceActive((long)_idUtilisateur);
                    _idSeanceparam = _seanceDM.Identifiant;
                }
                else
                {
                     _seanceDM = _seanceTask.getSeancebyId((long)_idSeanceparam);
                    _idSeanceparam = _seanceDM.Identifiant;
                }
               
                Int64? _idServeur = null;
                
                IGroupeTask _groupeTask = IoCContainer.Resolve<IGroupeTask>();
                ICollection<GroupeDM> _groupeDMs = _groupeTask.getGroupesOfUtilisateur((long)_idUtilisateur);
                
                if(_utilisateurTask.isUtilisateurOnGroupes(_groupeDMs, new List<String>() { GroupeCode.SERVEUR })){
                    _idServeur = _idUtilisateur;
                }


                ICollection<CommandeDM>  _commandeDMs = _commandeTask.getCommandeDMs(_idSeance : _idSeanceparam, _idServeur : _idServeur);

               
                if (_commandeDMs != null)
                {
                    foreach (CommandeDM item in _commandeDMs)
                    {
                        CommandeVM _commandeVM = CommandeMapper.CommandeDMtoCommandeVM(item);
                        _commandeVMs.Add(_commandeVM);
                    }
                    int sumcomande = 0;
                    ICollection<ActeurSeanceDM> _acteurSeanceDMs = _acteurSeanceTask.getActeurSeanceDMsByIdSeance((long)_idSeanceparam);
                  
                    //ICollection<UtilisateurDM> _utilisateurDMs = _utilisateurTask.getServeursOfSeance((long)_idSeanceparam);
                    ICollection<Int64?> _idsUtilisateur = _commandeDMs.Select(a => a.IdServeur).Distinct().ToList();
                    if (_acteurSeanceDMs != null) {
                        _recapVMs = new List<RecapVM>();
                        _acteurSeanceDMs.Add(new ActeurSeanceDM() {
                            NomPrenom= _seanceDM.NomPrenom,
                            IdUtilisateur= _seanceDM.IdUtilisateur,
                        });
                        foreach (ActeurSeanceDM _User in _acteurSeanceDMs) {
                            String _nomUtilisateur = _User.NomPrenom;
                            Double? _montant = _commandeDMs.Where(a => a.IdServeur == _User.IdUtilisateur).Sum(a => a.Montant);
                            Int64 _nombreCommande = _commandeDMs.Where(a => a.IdServeur == _User.IdUtilisateur).Count();
                            sumcomande += (int)_nombreCommande;
                            _recapVMs.Add(new RecapVM() {
                                IdUtilisateur = (long)_User.IdUtilisateur,
                                Montant = _montant,
                                NomUtilisateur = _nomUtilisateur,
                                NombreCommande = _nombreCommande,
                            }); 
                        }
                        sumcomande = (sumcomande == 0) ? 1 : sumcomande;
                        foreach (RecapVM item in _recapVMs)
                        {
                            int poucentage = 0;

                            poucentage = (int)item.NombreCommande * 100 / sumcomande;
                            item.poucentage = poucentage;
                        }
                        _recapVMs = _recapVMs.OrderByDescending(x => x.poucentage).ToList();
                    }
                }
                _etatRep = new EtatReponse() { Code = EtatReponseCode.SUCCESS, Message = "RETURN OK" };
            }
            catch (Exception e)
            {
                _httpState = new HttpState() { Code = HttpStateCode.ERROR, Message = e.Message };
                MyLogger.log(Utilitaire.getEmplacement(this) + ":\n" + Utilitaire.getDetailsException(e), MyLoggerCode.STANDARD);
            }

            _model.Add("commandeVMs", _commandeVMs);
            _model.Add("recapVMs", _recapVMs);
            _model.Add(HttpState.Name, _httpState);
            _model.Add(EtatReponse.Name, _etatRep);
            return Utilitaire.constructResponse(this, _model);
        }

        [HttpPost]
        public HttpResponseMessage getCommandeSeance(ParamInt paramInt)
        {
           
            Dictionary<String, Object> _model = new Dictionary<String, Object>();
            HttpState _httpState = new HttpState();
            EtatReponse _etatRep = new EtatReponse();
            ICollection<CommandeVM> _commandeVMs = null;
            try
            {
                Int64? _idUtilisateur = TokenManager.getIdentifiantFromToken(Request);
                ICommandeTask _commandeTask = IoCContainer.Resolve<ICommandeTask>();
                ISeanceTask _seanceTask = IoCContainer.Resolve<ISeanceTask>();
                IUtilisateurTask _utilisateurTask = IoCContainer.Resolve<IUtilisateurTask>();
                SeanceDM _seanceDM = _seanceTask.getSeanceActive((long)_idUtilisateur);
                long? _idseance = paramInt.Valeur == null ? _seanceDM.Identifiant : paramInt.Valeur;
               
                Int64 ? _idServeur = null;


               


                ICollection<CommandeDM> _commandeDMs = _commandeTask.getCommandeDMs(_idSeance: _idseance);
                if (_commandeDMs != null)
                {
                    _commandeVMs = new List<CommandeVM>();
                   foreach (CommandeDM item in _commandeDMs)
                    {
                        CommandeVM commandeVM = CommandeMapper.CommandeDMtoCommandeVM(item);
                        _commandeVMs.Add(commandeVM);
                    }
                   
                }
                _etatRep = new EtatReponse() { Code = EtatReponseCode.SUCCESS, Message = "RETURN OK" };
            }
            catch (Exception e)
            {
                _httpState = new HttpState() { Code = HttpStateCode.ERROR, Message = e.Message };
                MyLogger.log(Utilitaire.getEmplacement(this) + ":\n" + Utilitaire.getDetailsException(e), MyLoggerCode.STANDARD);
            }

            _model.Add("commandeVMs", _commandeVMs);
            _model.Add(HttpState.Name, _httpState);
            _model.Add(EtatReponse.Name, _etatRep);
            return Utilitaire.constructResponse(this, _model);
        }

        [HttpPost]
        public HttpResponseMessage getCommandes(ParamCammande paramsDate)
       {
            DateTime? d_debut = Cvrt.strToDateTime(paramsDate._datedebut);

            DateTime? d_fin = Cvrt.strToDateTime(paramsDate._datefin);
            String c_code = paramsDate._code;
            List<Int64?> caisseids = paramsDate.listeidcaisse != null? paramsDate.listeidcaisse : new List<long?>();
           // DateTime ? d_fin = DateTime.Parse(paramsDate._datefin);
            Dictionary<String, Object> _model = new Dictionary<String, Object>();
            HttpState _httpState = new HttpState();
            EtatReponse _etatRep = new EtatReponse();
            ICollection<CommandeVM> _commandeVMs = null;
            try
            {
                Int64? _idUtilisateur = TokenManager.getIdentifiantFromToken(Request);
                ICommandeTask _commandeTask = IoCContainer.Resolve<ICommandeTask>();
                ISeanceTask _seanceTask = IoCContainer.Resolve<ISeanceTask>();
                SeanceDM _seanceDM = _seanceTask.getSeanceActive((long)_idUtilisateur);
                ICollection<CommandeDM> _commandeDMs = _commandeTask.getCommandeDMs( _code:c_code, _ddebut: d_debut, _dfin: d_fin,_caisseids: caisseids);
                if (_commandeDMs != null)
                {
                    _commandeVMs = new List<CommandeVM>();
                    foreach (CommandeDM _obj in _commandeDMs)
                    {
                        CommandeVM _commandeVM = CommandeMapper.CommandeDMtoCommandeVM(_obj);
                        _commandeVMs.Add(_commandeVM);
                    }
                    ArticleVM articleVM = new ArticleVM();
                    _commandeVMs = _commandeVMs.OrderByDescending(x => x.Identifiant).ToList();
                }
                _etatRep = new EtatReponse() { Code = EtatReponseCode.SUCCESS, Message = "RETURN OK" };
            }
            catch (Exception e)
            {
                _httpState = new HttpState() { Code = HttpStateCode.ERROR, Message = e.Message };
                MyLogger.log(Utilitaire.getEmplacement(this) + ":\n" + Utilitaire.getDetailsException(e), MyLoggerCode.STANDARD);
            }

            _model.Add("commandeVMs", _commandeVMs);
            _model.Add(HttpState.Name, _httpState);
            _model.Add(EtatReponse.Name, _etatRep);
            return Utilitaire.constructResponse(this, _model);
        }
        //===========================================================================

        [HttpPost]
        public HttpResponseMessage getMouvement(ParamCammande paramsDate)
        {
            DateTime? d_debut = Cvrt.strToDateTime(paramsDate._datedebut);

            DateTime? d_fin = Cvrt.strToDateTime(paramsDate._datefin);
            String c_code = paramsDate._code;
            List<Int64?> caisseids = paramsDate.listeidcaisse != null ? paramsDate.listeidcaisse : new List<long?>();
            // DateTime ? d_fin = DateTime.Parse(paramsDate._datefin);
            Dictionary<String, Object> _model = new Dictionary<String, Object>();
            HttpState _httpState = new HttpState();
            EtatReponse _etatRep = new EtatReponse();
            ICollection<CommandeVM> _commandeVMs = null;
            ICollection<DetailCommandeDM> detailCommandeDMs = null;
            try
            {
                Int64? _idUtilisateur = TokenManager.getIdentifiantFromToken(Request);
                ICommandeTask _commandeTask = IoCContainer.Resolve<ICommandeTask>();
                IDetailCommandeTask _detailCommandeTask = IoCContainer.Resolve<IDetailCommandeTask>();
                IArticleTask _articleTask = IoCContainer.Resolve<IArticleTask>();
                ICollection<CommandeDM> _commandeDMs = _commandeTask.getCommandeDMs (_ddebut: d_debut, _dfin: d_fin);
                if (_commandeDMs != null)
                {
                    ICollection<DetailCommandeDM> _detailcommandeDM = new List<DetailCommandeDM>();
                    _commandeVMs = new List<CommandeVM>();
                    foreach (CommandeDM _obj in _commandeDMs)
                    {
                        ICollection<DetailCommandeDM> _detailCommandeDMs = _detailCommandeTask.getDetailCommandesDMByIdCommande(_obj.Identifiant);
                        _detailCommandeDMs = _detailCommandeDMs.Where(a => a.IdValiderPar != null && a.DateCreation != null).ToList();
                        CommandeVM _commandeVM = CommandeMapper.CommandeDMtoCommandeVM(_obj);


                        _commandeVM.DetailCommandes = new List<DetailCommandeVM>();
                        foreach (DetailCommandeDM _dtc in _detailCommandeDMs)
                        {
                            DetailCommandeVM detailCommandeVM = DetailCommandeMapper.DetailCommandeDMtoDetailCommandeVM(_dtc);
                            _commandeVM.DetailCommandes.Add(detailCommandeVM);
                        }
                        
                        _commandeVMs.Add(_commandeVM);
                    }
               
                    _commandeVMs = _commandeVMs.OrderBy(x => x.Identifiant).ToList();
                }
                _etatRep = new EtatReponse() { Code = EtatReponseCode.SUCCESS, Message = "RETURN OK" };
            }
            catch (Exception e)
            {
                _httpState = new HttpState() { Code = HttpStateCode.ERROR, Message = e.Message };
                MyLogger.log(Utilitaire.getEmplacement(this) + ":\n" + Utilitaire.getDetailsException(e), MyLoggerCode.STANDARD);
            }

            _model.Add("commandeVMs", _commandeVMs);
            _model.Add(HttpState.Name, _httpState);
            _model.Add(EtatReponse.Name, _etatRep);
            return Utilitaire.constructResponse(this, _model);
        }
        //===========================================================================

        [HttpPost]
        public HttpResponseMessage getRecapArticles(ParamDate paramsDate)
        {
            DateTime? d_debut = Cvrt.strToDateTime(paramsDate._datedebut);

            DateTime? d_fin = Cvrt.strToDateTime(paramsDate._datefin);

            // DateTime ? d_fin = DateTime.Parse(paramsDate._datefin);
            Dictionary<String, Object> _model = new Dictionary<String, Object>();
            HttpState _httpState = new HttpState();
            EtatReponse _etatRep = new EtatReponse();
            ICollection<CommandeVM> _commandeVMs = null;
            ICollection<DetailCommandeDM> detailCommandeDMs = null;
            try
            {
                Int64? _idUtilisateur = TokenManager.getIdentifiantFromToken(Request);
                ICommandeTask _commandeTask = IoCContainer.Resolve<ICommandeTask>();
                IDetailCommandeTask _detailCommandeTask = IoCContainer.Resolve<IDetailCommandeTask>();
                IArticleTask _articleTask = IoCContainer.Resolve<IArticleTask>();
               // ISeanceTask _seanceTask = IoCContainer.Resolve<ISeanceTask>();
               // SeanceDM _seanceDM = _seanceTask.getSeanceActive((long)_idUtilisateur);
                ICollection<CommandeDM> _commandeDMs = _commandeTask.getCommandeDMs(_ddebut: d_debut, _dfin: d_fin);
                if (_commandeDMs != null)
                {
                    ICollection<DetailCommandeDM> _detailcommandeDM = new List<DetailCommandeDM>();
                    _commandeVMs = new List<CommandeVM>();

                    foreach (CommandeDM _obj in _commandeDMs)
                    {
                        
                        ICollection<DetailCommandeDM> dcDM = _detailCommandeTask.getDetailCommandesDMByIdCommande(_obj.Identifiant);
                        if (dcDM!=null && dcDM.Count>0)
                        {
                            foreach (DetailCommandeDM dc in dcDM)
                        {
                            _detailcommandeDM.Add(dc);
                        }
                        }
                        
                        
                        //CommandeVM _commandeVM = CommandeMapper.CommandeDMtoCommandeVM(_obj);
                        //_commandeVMs.Add(_commandeVM);
                    }

                   detailCommandeDMs = _articleTask.listeArticle(_detailcommandeDM);
                    detailCommandeDMs = detailCommandeDMs.OrderByDescending(x => x.Quantite).ToList();

                }
                _etatRep = new EtatReponse() { Code = EtatReponseCode.SUCCESS, Message = "RETURN OK" };
            }
            catch (Exception e)
            {
                _httpState = new HttpState() { Code = HttpStateCode.ERROR, Message = e.Message };
                MyLogger.log(Utilitaire.getEmplacement(this) + ":\n" + Utilitaire.getDetailsException(e), MyLoggerCode.STANDARD);
            }

            _model.Add("detailCommandeDMs", detailCommandeDMs);
            _model.Add(HttpState.Name, _httpState);
            _model.Add(EtatReponse.Name, _etatRep);
            return Utilitaire.constructResponse(this, _model);
        }
        //===========================================================================
      
        /*   [HttpPost]
           public HttpResponseMessage getQantiteStockArticles(ParamDate paramsDate)
           {
              // DateTime? d_debut = Cvrt.strToDateTime(paramsDate._datedebut);

             //  DateTime? d_fin = Cvrt.strToDateTime(paramsDate._datefin);

               // DateTime ? d_fin = DateTime.Parse(paramsDate._datefin);
               Dictionary<String, Object> _model = new Dictionary<String, Object>();
               HttpState _httpState = new HttpState();
               EtatReponse _etatRep = new EtatReponse();
               ICollection<CommandeVM> _commandeVMs = null;
               ICollection<DetailCommandeDM> detailCommandeDMs = null;
               try
               {
                   Int64? _idUtilisateur = TokenManager.getIdentifiantFromToken(Request);
                   ICommandeTask _commandeTask = IoCContainer.Resolve<ICommandeTask>();
                   IDetailCommandeTask _detailCommandeTask = IoCContainer.Resolve<IDetailCommandeTask>();
                   IArticleTask _articleTask = IoCContainer.Resolve<IArticleTask>();
                   // ISeanceTask _seanceTask = IoCContainer.Resolve<ISeanceTask>();
                   // SeanceDM _seanceDM = _seanceTask.getSeanceActive((long)_idUtilisateur);

                       ICollection<DetailCommandeDM> _detailcommandeDM = new List<DetailCommandeDM>();
                       _commandeVMs = new List<CommandeVM>();


                           ICollection<DetailCommandeDM> dcDM = _detailCommandeTask.getDetailCommandesstockDMByIdArticle(_obj.Identifiant);
                           if (dcDM != null && dcDM.Count > 0)
                           {
                               foreach (DetailCommandeDM dc in dcDM)
                               {
                                   _detailcommandeDM.Add(dc);
                               }
                           }


                           //CommandeVM _commandeVM = CommandeMapper.CommandeDMtoCommandeVM(_obj);
                           //_commandeVMs.Add(_commandeVM);


                       detailCommandeDMs = _articleTask.listeArticle(_detailcommandeDM);
                       detailCommandeDMs = detailCommandeDMs.OrderByDescending(x => x.Quantite).ToList();


                   _etatRep = new EtatReponse() { Code = EtatReponseCode.SUCCESS, Message = "RETURN OK" };
               }
               catch (Exception e)
               {
                   _httpState = new HttpState() { Code = HttpStateCode.ERROR, Message = e.Message };
                   MyLogger.log(Utilitaire.getEmplacement(this) + ":\n" + Utilitaire.getDetailsException(e), MyLoggerCode.STANDARD);
               }

               _model.Add("detailCommandeDMs", detailCommandeDMs);
               _model.Add(HttpState.Name, _httpState);
               _model.Add(EtatReponse.Name, _etatRep);
               return Utilitaire.constructResponse(this, _model);
           }
           */
    }
}

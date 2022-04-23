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
    public class CommercialisationController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage validerCommercialisation(List<CommercialisationVM> commercialisationVMs)
        {
            Dictionary<String, Object> _model = new Dictionary<String, Object>();
            HttpState _httpState = new HttpState();
            EtatReponse _etatRep = new EtatReponse();
            try
            {
                Int64? _idUtilisateur = TokenManager.getIdentifiantFromToken(Request);
                IUtilisateurTask _utilisateurTask = IoCContainer.Resolve<IUtilisateurTask>();
                ICommercialisationTask _commercialisationTask = IoCContainer.Resolve<ICommercialisationTask>();
                if (commercialisationVMs != null) {
                    foreach (CommercialisationVM _commercialisationVM in commercialisationVMs) {
                        if (_commercialisationVM.Coche == 1)
                        {
                            if (_commercialisationVM.Identifiant <= 0)
                            {
                                CommercialisationDM _commercialisationDM = new CommercialisationDM()
                                {
                                    IdCaisse = _commercialisationVM.IdCaisse,
                                    IdCategorie = _commercialisationVM.IdCategorie,
                                };
                                _commercialisationTask.addCommercialisationDM(_commercialisationDM);
                            }
                        }
                        else {
                            if (_commercialisationVM.Identifiant > 0) {
                                _commercialisationTask.removeCommercialisationDM(_commercialisationVM.Identifiant);
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
        public HttpResponseMessage getCommercialisations(ParamInt paramInt)
        {
            Int64? _idCaisse = paramInt.Valeur;
            Dictionary<String, Object> _model = new Dictionary<String, Object>();
            HttpState _httpState = new HttpState();
            EtatReponse _etatRep = new EtatReponse();
            ICollection<CommercialisationVM> _commercialisationVMs = null;
            try
            {
                Int64? _idUtilisateur = TokenManager.getIdentifiantFromToken(Request);
                IUtilisateurTask _utilisateurTask = IoCContainer.Resolve<IUtilisateurTask>();
                ICommercialisationTask _commercialisationTask = IoCContainer.Resolve<ICommercialisationTask>();
                ICategorieTask _categorieTask = IoCContainer.Resolve<ICategorieTask>();

                ICollection<CategorieDM> _categorieDMs = _categorieTask.getCategories(_enActivite : 1);
                ICollection<CommercialisationDM> _commercialisationDMs = _commercialisationTask.getCommercialisationDMs(_idCaisse: _idCaisse);

                if (_categorieDMs != null) {
                    _commercialisationVMs = new List<CommercialisationVM>();
                    foreach (CategorieDM _categorieDM in _categorieDMs) {
                        CommercialisationVM _commercialisationVM = new CommercialisationVM();
                        if (_commercialisationDMs.Where(a => a.IdCategorie == _categorieDM.Identifiant).Count() > 0)
                        {
                            CommercialisationDM _commercialisationDM = _commercialisationDMs.Where(a => a.IdCategorie == _categorieDM.Identifiant).FirstOrDefault();
                            _commercialisationVM.Identifiant = _commercialisationDM.Identifiant;
                            _commercialisationVM.IdCaisse = _commercialisationDM.IdCaisse;
                            _commercialisationVM.IdCategorie = _commercialisationDM.IdCategorie;
                            _commercialisationVM.LibelleCategorie = _commercialisationDM.LibelleCategorie;
                            _commercialisationVM.Coche = 1;
                        }
                        else
                        {
                            _commercialisationVM.IdCategorie = _categorieDM.Identifiant;
                            _commercialisationVM.LibelleCategorie = _categorieDM.Libelle;
                            _commercialisationVM.IdCaisse = _idCaisse;
                            _commercialisationVM.Coche = 0;
                        }
                        _commercialisationVMs.Add(_commercialisationVM);
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
            _model.Add("commercialisationVMs", _commercialisationVMs);

            
            return Utilitaire.constructResponse(this, _model);
        }

    }
}

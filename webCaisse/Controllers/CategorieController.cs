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
    public class CategorieController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage ajouterCategorie(CategorieVM categorieVM)
        {
            Dictionary<String, Object> _model = new Dictionary<String, Object>();
            HttpState _httpState = new HttpState();
            EtatReponse _etatRep = new EtatReponse();
            Int64? _idCategorie = null;
            try
            {
                ICategorieTask _categorieTask = IoCContainer.Resolve<ICategorieTask>();
                if (categorieVM != null)
                {
                    CategorieDM _categorieDM = CategorieMapper.CategorieVMtoCategorieDM(categorieVM);
                    //_categorieDM.Code = "";
                    _categorieDM.EnActivite = 1;
                    _idCategorie = _categorieTask.addCategorieDM(_categorieDM);
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
            _model.Add("idCategorie", _idCategorie);
            return Utilitaire.constructResponse(this, _model);
        }


        [HttpPost]
        public HttpResponseMessage modifierCategorie(CategorieVM categorieVM)
        {
            Dictionary<String, Object> _model = new Dictionary<String, Object>();
            HttpState _httpState = new HttpState();
            EtatReponse _etatRep = new EtatReponse();
            try
            {
                ICategorieTask _categorieTask = IoCContainer.Resolve<ICategorieTask>();
                if (categorieVM != null)
                {
                    CategorieDM _categorieDM = CategorieMapper.CategorieVMtoCategorieDM(categorieVM);
                    _categorieTask.modifierCategorieDM(_categorieDM);
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
        public HttpResponseMessage getCategories()
        {
            Dictionary<String, Object> _model = new Dictionary<String, Object>();
            HttpState _httpState = new HttpState();
            EtatReponse _etatRep = new EtatReponse();
            ICollection<CategorieVM> _categorieVMs = null;
            String _racineImage = ConfigInfrastructure.BO_FILE_ROOT + @"\images\categorie\";
            try
            {
                ICategorieTask _categorieTask = IoCContainer.Resolve<ICategorieTask>();
                ICollection<CategorieDM> _categorieDMs = _categorieTask.getCategories();
                if (_categorieDMs != null)
                {
                    _categorieVMs = new List<CategorieVM>();
                    foreach (CategorieDM _obj in _categorieDMs)
                    {

                        CategorieVM _categorieVM = CategorieMapper.CategorieDMtoCategorieVM(_obj);
                          _categorieVM.ImageAsString = _categorieTask.getImageAsString(_obj.Identifiant);
                        if (_categorieVM.ImageAsString != null)
                        {
                            _categorieVM.PathImage = _racineImage + _obj.Identifiant + ".jpg";
                        }
                        else
                        {
                            _categorieVM.PathImage = _racineImage + "default_categorie.jpg";
                        }
                        _categorieVMs.Add(_categorieVM);
                    }
                }
                _etatRep = new EtatReponse() { Code = EtatReponseCode.SUCCESS, Message = "RETURN OK" };
            }
            catch (Exception e)
            {
                _httpState = new HttpState() { Code = HttpStateCode.ERROR, Message = e.Message };
                MyLogger.log(Utilitaire.getEmplacement(this) + ":\n" + Utilitaire.getDetailsException(e), MyLoggerCode.STANDARD);
            }

            _model.Add("categorieVMs", _categorieVMs);
            _model.Add(HttpState.Name, _httpState);
            _model.Add(EtatReponse.Name, _etatRep);
            return Utilitaire.constructResponse(this, _model);
        }

        [HttpPost]
        public HttpResponseMessage getCategoriesForUI()
        {
            Dictionary<String, Object> _model = new Dictionary<String, Object>();
            HttpState _httpState = new HttpState();
            EtatReponse _etatRep = new EtatReponse();
            ICollection<CategorieVM> _categorieVMs = null;
            String _racineImage = ConfigInfrastructure.BO_FILE_ROOT + @"\images\categorie\";
            try
            {
                ICategorieTask _categorieTask = IoCContainer.Resolve<ICategorieTask>();
                ICollection<CategorieDM> _categorieDMs = _categorieTask.getCategories(_enActivite: 1);
                if (_categorieDMs != null)
                {
                    _categorieVMs = new List<CategorieVM>();
                    foreach (CategorieDM _obj in _categorieDMs)
                    {

                        CategorieVM _categorieVM = CategorieMapper.CategorieDMtoCategorieVM(_obj);
                        _categorieVM.ImageAsString = _categorieTask.getImageAsString(_obj.Identifiant);
                        if (_categorieVM.ImageAsString != null)
                        {
                            _categorieVM.PathImage = _racineImage + _obj.Identifiant + ".jpg";
                        }
                        else
                        {
                            _categorieVM.PathImage = _racineImage + "default_categorie.jpg";
                        }
                        //_categorieVM.ImageAsString = _categorieTask.getImageAsString(_obj.Identifiant);
                        _categorieVMs.Add(_categorieVM);
                    }
                }
                _etatRep = new EtatReponse() { Code = EtatReponseCode.SUCCESS, Message = "RETURN OK" };
            }
            catch (Exception e)
            {
                _httpState = new HttpState() { Code = HttpStateCode.ERROR, Message = e.Message };
                MyLogger.log(Utilitaire.getEmplacement(this) + ":\n" + Utilitaire.getDetailsException(e), MyLoggerCode.STANDARD);
            }

            _model.Add("categorieVMs", _categorieVMs);
            _model.Add(HttpState.Name, _httpState);
            _model.Add(EtatReponse.Name, _etatRep);
            return Utilitaire.constructResponse(this, _model);
        }

        [HttpPost]
        public HttpResponseMessage getCategoriesCommercialisees()
        {
            Dictionary<String, Object> _model = new Dictionary<String, Object>();
            HttpState _httpState = new HttpState();
            EtatReponse _etatRep = new EtatReponse();
            ICollection<CategorieVM> _categorieVMs = null;
            try
            {
                Int64? _idUtilisateur = TokenManager.getIdentifiantFromToken(Request);

                ICaisseTask _caisseTask = IoCContainer.Resolve<ICaisseTask>();
                CaisseDM _caisseDM =  _caisseTask.getCaisseOfUtilisateur((long)_idUtilisateur);
                ICategorieTask _categorieTask = IoCContainer.Resolve<ICategorieTask>();
                ICollection<CategorieDM> _categorieDMs = _categorieTask.getCategorieDMsCommercialisees(_caisseDM.Identifiant);
                if (_categorieDMs != null)
                {
                    _categorieVMs = new List<CategorieVM>();
                    String _racineImage = ConfigInfrastructure.BO_FILE_ROOT + @"\images\categorie\";
                    foreach (CategorieDM _obj in _categorieDMs)
                    {
                        //showImageCategorie(_obj.Identifiant);
                        CategorieVM _categorieVM = CategorieMapper.CategorieDMtoCategorieVM(_obj);
                        _categorieVM.ImageAsString = _categorieTask.getImageAsString(_obj.Identifiant);
                        if (_categorieVM.ImageAsString != null)
                        {
                            _categorieVM.PathImage = _racineImage + _obj.Identifiant + ".jpg";
                        }
                        else
                        {
                            _categorieVM.PathImage = _racineImage + "default_categorie.jpg";
                        }
                        _categorieVMs.Add(_categorieVM);
                    }
                }
                _etatRep = new EtatReponse() { Code = EtatReponseCode.SUCCESS, Message = "RETURN OK" };
            }
            catch (Exception e)
            {
                _httpState = new HttpState() { Code = HttpStateCode.ERROR, Message = e.Message };
                MyLogger.log(Utilitaire.getEmplacement(this) + ":\n" + Utilitaire.getDetailsException(e), MyLoggerCode.STANDARD);
            }

            _model.Add("categorieVMs", _categorieVMs);
            _model.Add(HttpState.Name, _httpState);
            _model.Add(EtatReponse.Name, _etatRep);
            return Utilitaire.constructResponse(this, _model);
        }

        [HttpGet]
        public HttpResponseMessage showImageCategorie(Int64 identifiant)
        {
            //context.Response.ContentType = "image/jpeg";
            ICategorieTask _categorieTask = IoCContainer.Resolve<ICategorieTask>();
            String _racineImage = ConfigInfrastructure.BO_FILE_ROOT + @"\images\categorie\";
            String _imageAsString = _categorieTask.getImageAsString(identifiant);
            if (_imageAsString == null)
            {
                _imageAsString = _categorieTask.getDefaultImageAsString();
            }
            Byte[] byteImage = Convert.FromBase64String(_imageAsString);

            MemoryStream ms = new MemoryStream(byteImage);
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StreamContent(ms);
            response.Content.Headers.ContentType = new
            System.Net.Http.Headers.MediaTypeHeaderValue("image/jpg");

            //send response of image/png type
            return response;
        }

        [HttpPost]
        public HttpResponseMessage getDefaultImageAsBase64()
        {
            Dictionary<String, Object> _model = new Dictionary<String, Object>();
            HttpState _httpState = new HttpState();
            EtatReponse _etatRep = new EtatReponse();
            String _defaultImageAsBase64 = null;
            try
            {
                ICategorieTask _categorieTask = IoCContainer.Resolve<ICategorieTask>();
                _defaultImageAsBase64 = _categorieTask.getDefaultImageAsString();
                _etatRep = new EtatReponse() { Code = EtatReponseCode.SUCCESS, Message = "RETURN OK" };
            }
            catch (Exception e)
            {
                _httpState = new HttpState() { Code = HttpStateCode.ERROR, Message = e.Message };
                MyLogger.log(Utilitaire.getEmplacement(this) + ":\n" + Utilitaire.getDetailsException(e), MyLoggerCode.STANDARD);
            }

            _model.Add("defaultImageAsBase64", _defaultImageAsBase64);
            _model.Add(HttpState.Name, _httpState);
            _model.Add(EtatReponse.Name, _etatRep);
            return Utilitaire.constructResponse(this, _model);
        }

    }
}

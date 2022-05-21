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
    public class ArticleController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage getArticles()
        {
            Dictionary<String, Object> _model = new Dictionary<String, Object>();
            HttpState _httpState = new HttpState();
            EtatReponse _etatRep = new EtatReponse();
            ICollection<ArticleVM> _articleVMs = null;
            String _racineImage = ConfigInfrastructure.BO_FILE_ROOT + @"\images\article\";
            try
            {
                IArticleTask _articleTask = IoCContainer.Resolve<IArticleTask>();
                ICollection<ArticleDM> _articleDMs = _articleTask.getArticles();
                if (_articleDMs != null)
                {
                    _articleVMs = new List<ArticleVM>();
                    foreach (ArticleDM _obj in _articleDMs) {
                        ArticleVM _articleVM = ArticleMapper.ArticleDMtoArticleVM(_obj);
                        _articleVM.ImageAsString = _articleTask.getImageAsString(_obj.Identifiant);
                        if (_articleVM.ImageAsString != null)
                        {
                            _articleVM.PathImage = _racineImage + _obj.Identifiant + ".jpg";
                        }
                        else
                        {
                            _articleVM.PathImage = _racineImage + "default_article.jpg";
                        }
                        _articleVMs.Add(_articleVM);
                    }   
                }
                _etatRep = new EtatReponse() { Code = EtatReponseCode.SUCCESS, Message = "RETURN OK" };
            }
            catch (Exception e)
            {
                _httpState = new HttpState() { Code = HttpStateCode.ERROR, Message = e.Message };
                MyLogger.log(Utilitaire.getEmplacement(this) + ":\n" + Utilitaire.getDetailsException(e), MyLoggerCode.STANDARD);
            }

            _model.Add("articleVMs", _articleVMs);
            _model.Add(HttpState.Name, _httpState);
            _model.Add(EtatReponse.Name, _etatRep);
            return Utilitaire.constructResponse(this, _model);            
        }
        [HttpPost]
        public HttpResponseMessage AddArticle(ArticleVM articleVM)
        {
            Dictionary<String, Object> _model = new Dictionary<String, Object>();
            HttpState _httpState = new HttpState();
            EtatReponse _etatRep = new EtatReponse();
            Int64? _idArticle = null;
            try
            {
                IArticleTask _articleTask = IoCContainer.Resolve<IArticleTask>();
                ITypeUniteTask _typeUniteTask = IoCContainer.Resolve<ITypeUniteTask>();
                ITypeArticleTask _typeArticleTask = IoCContainer.Resolve<ITypeArticleTask>();

                
                if (articleVM != null)
                {

                    string code = articleVM.LibelleTypeUnite;
                    string codetypearticle = articleVM.LibelleTypeArticle;

                    TypeUniteDM _typeUniteDM = _typeUniteTask.getTypeUniteDMByCode(code);
                    TypeArticleDM _typeArticleDM = _typeArticleTask.getTypeArticleDMByCode(codetypearticle);

                    ArticleDM _articleDM = ArticleMapper.ArticleVMtoArticleDM(articleVM);

                    _articleDM.QuantiteDisponible = 0;
                    //_articleDM.QuantiteMin = 0;
                    //_articleDM.IdTauxTva = 1;
                    _articleDM.IdTypeUnite = _typeUniteDM.Identifiant;
                    _articleDM.IdTypeArticle = _typeArticleDM.Identifiant;

                    _idArticle = _articleTask.addArticle(_articleDM);

                }
                    _etatRep = new EtatReponse() { Code = EtatReponseCode.SUCCESS, Message = "RETURN OK" };
                
            }
            catch (Exception e)
            {
                _httpState = new HttpState() { Code = HttpStateCode.ERROR, Message = e.Message };
                MyLogger.log(Utilitaire.getEmplacement(this) + ":\n" + Utilitaire.getDetailsException(e), MyLoggerCode.STANDARD);
            }
            _model.Add("idArticle", _idArticle);
            _model.Add(HttpState.Name, _httpState);
            _model.Add(EtatReponse.Name, _etatRep);
            return Utilitaire.constructResponse(this, _model);
        }
        
        [HttpPost]
        public HttpResponseMessage updateArticle(ArticleVM articleVM)
        {
            Dictionary<String, Object> _model = new Dictionary<String, Object>();
            HttpState _httpState = new HttpState();
            EtatReponse _etatRep = new EtatReponse();

            try
            {
                //Int64? _idUtilisateur = TokenManager.getIdentifiantFromToken(Request);
                IArticleTask _articleTask = IoCContainer.Resolve<IArticleTask>();
                ITypeUniteTask _typeUniteTask = IoCContainer.Resolve<ITypeUniteTask>();
                ITypeArticleTask _typeArticleTask = IoCContainer.Resolve<ITypeArticleTask>();

                if (articleVM != null)
                {

                    string code = articleVM.LibelleTypeUnite;
                    string codetypearticle = articleVM.LibelleTypeArticle;


                    TypeUniteDM _typeUniteDM = _typeUniteTask.getTypeUniteDMByCode(code);
                    TypeArticleDM _typeArticleDM = _typeArticleTask.getTypeArticleDMByCode(codetypearticle);

                    ArticleDM _articleDM = ArticleMapper.ArticleVMtoArticleDM(articleVM);
                    _articleDM.IdTypeUnite = _typeUniteDM.Identifiant;
                    _articleDM.IdTypeArticle = _typeArticleDM.Identifiant;

                    _articleTask.updateArticle(_articleDM);
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

        [HttpGet]
        public HttpResponseMessage showImageArticle(Int64 identifiant)
        {
            //context.Response.ContentType = "image/jpeg";
            IArticleTask _articleTask = IoCContainer.Resolve<IArticleTask>();
            String _racineImage = ConfigInfrastructure.BO_FILE_ROOT + @"\images\article\";
            String _imageAsString = _articleTask.getImageAsString(identifiant);
            if (_imageAsString == null)
            {
                _imageAsString = _articleTask.getDefaultImageAsString();
            }
            Byte[] byteImage = Convert.FromBase64String(_imageAsString);

            MemoryStream ms = new MemoryStream(byteImage);
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StreamContent(ms);
            response.Content.Headers.ContentType = new
            System.Net.Http.Headers.MediaTypeHeaderValue("image/jpg");
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
                IArticleTask _articleTask = IoCContainer.Resolve<IArticleTask>();
                _defaultImageAsBase64 = _articleTask.getDefaultImageAsString();
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

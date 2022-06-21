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
    public class TypeArticleController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage getListeTypeArticles()
        {
            Dictionary<String, Object> _model = new Dictionary<String, Object>();
            HttpState _httpState = new HttpState();
            EtatReponse _etatRep = new EtatReponse();
            ICollection<TypeArticleVM> _typeArticleVMs = null;
            try
            {
                ITypeArticleTask _typeArticleTask = IoCContainer.Resolve<ITypeArticleTask>();
                ICollection<TypeArticleDM> _groupeDMs = _typeArticleTask.getTypeArticleDMs();
                if (_groupeDMs != null)
                {
                    _typeArticleVMs = new List<TypeArticleVM>();
                    foreach (TypeArticleDM _obj in _groupeDMs)
                    {
                        TypeArticleVM __typeArticleVM = TypeArticleMapper.TypeArticleDMtoTypeArticleVM(_obj);
                        _typeArticleVMs.Add(__typeArticleVM);
                    }
                }
                _etatRep = new EtatReponse() { Code = EtatReponseCode.SUCCESS, Message = "RETURN OK" };
            }
            catch (Exception e)
            {
                _httpState = new HttpState() { Code = HttpStateCode.ERROR, Message = e.Message };
                MyLogger.log(Utilitaire.getEmplacement(this) + ":\n" + Utilitaire.getDetailsException(e), MyLoggerCode.STANDARD);
            }

            _model.Add("typeArticleVMs", _typeArticleVMs);
            _model.Add(HttpState.Name, _httpState);
            _model.Add(EtatReponse.Name, _etatRep);
            return Utilitaire.constructResponse(this, _model);
        }
    }
}

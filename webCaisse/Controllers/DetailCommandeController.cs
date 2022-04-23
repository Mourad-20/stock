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
    public class DetailCommandeController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage getDetailCommandesstockparam(ParamInt paramInt)
        {
            Int64? _idArticle = paramInt.Valeur;
            Dictionary<String, Object> _model = new Dictionary<String, Object>();
            HttpState _httpState = new HttpState();
            EtatReponse _etatRep = new EtatReponse();
            ICollection<DetailCommandeVM> _detailCommandeVMs = null;
            //String _racineImage = ConfigInfrastructure.BO_FILE_ROOT + @"\images\article\";
            try
            {
                   IDetailCommandeTask _detailCommandeTask = IoCContainer.Resolve<IDetailCommandeTask>();
                ICollection<DetailCommandeDM> _detailCommandeDMs = _detailCommandeTask.getDetailCommandesstockDMByIdArticle(_idArticle);
                if (_detailCommandeDMs != null)
                {
                    _detailCommandeVMs = new List<DetailCommandeVM>();
                    foreach (DetailCommandeDM _obj in _detailCommandeDMs)
                    {
                        DetailCommandeVM _detailCommandeVM = DetailCommandeMapper.DetailCommandeDMtoDetailCommandeVM(_obj);

                        _detailCommandeVMs.Add(_detailCommandeVM);
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
    }
}

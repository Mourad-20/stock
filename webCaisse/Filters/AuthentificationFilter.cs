using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http.Results;
using webCaisse.Tools;

namespace webCaisse.Filters
{
    public class AuthentificationFilter : IAuthorizationFilter
    {
        public bool AllowMultiple => false;
        private Random _random = new Random();

        private ICollection<String> _pathToSkip;

        public AuthentificationFilter()
        {
            loadPathToSkip();
        }

        private void loadPathToSkip()
        {
            _pathToSkip = new List<String>();
            _pathToSkip.Add("/api/Utilisateur/authentification".ToUpper());
            _pathToSkip.Add("/api/Utilisateur/AuthentificationJeton".ToUpper());
            _pathToSkip.Add("/api/Utilisateur/getListeLogins".ToUpper());




            _pathToSkip.Add("/api/Utilisateur/Test".ToUpper());
            _pathToSkip.Add("/api/Utilisateur/Test2".ToUpper());
            _pathToSkip.Add("/api/Impression/TestImpression".ToUpper());
            _pathToSkip.Add("/api/Commande/envoyerTicketCuisine".ToUpper());
            _pathToSkip.Add("/api/Categorie/showImageCategorie".ToUpper());
            _pathToSkip.Add("/api/Article/showImageArticle".ToUpper());



        }

        public Task<HttpResponseMessage> ExecuteAuthorizationFilterAsync(HttpActionContext actionContext, CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation)
        {
            //----------------------------------
            //Thread.Sleep(_random.Next(0,1500));
            //----------------------------------
            var request = actionContext.Request;
            if (actionContext.Response == null)
            {
                actionContext.Response = request.CreateResponse(HttpStatusCode.OK);
            }
            HttpResponseMessage response = actionContext.Response;
            //---------------------------------------------------
            String _path = request.RequestUri.AbsolutePath;
            Boolean _isPathToSkip = _pathToSkip.Contains(_path.ToUpper());
            Boolean _isAuthorizationHeaderExist = false;
            Boolean _isTokenValid = false;
            Boolean _laisserPasser = false;
            String _newToken = null;
            //---------------------------------------------------
            //String _authorization = request.Headers.SingleOrDefault(x => x.Key.ToUpper() == "authorization".ToUpper()).Value?.First();
            String _authorization = request.Headers.Authorization?.ToString();
            if (_authorization != null && _authorization.ToUpper().StartsWith("BEARER") && _authorization.Length > "BEARER".Length)
            {
                _isAuthorizationHeaderExist = true;
                String _token = _authorization.Substring("BEARER".Length + 1);
                String[] _dataToken = TokenManager.extraireDataFromToken(_token);
                if (TokenManager.isTokenValid(_dataToken) == true)
                {
                    _isTokenValid = true;
                    Int64? _identifiant = TokenManager.getIdentifiantFromToken(_dataToken);
                    _newToken = TokenManager.genererToken(_identifiant);
                }
            }
            //---------------------------------------------------
            if (_isPathToSkip == true)
            {
                if (_isAuthorizationHeaderExist == false)
                {
                    _laisserPasser = true;
                }
                else
                {
                    if (_isTokenValid == true)
                    {
                        _laisserPasser = true;
                    }
                }
            }
            else
            {
                if (_isAuthorizationHeaderExist == true && _isTokenValid == true)
                {
                    _laisserPasser = true;
                }
            }
            //---------------------------------------------------
            if (_laisserPasser == false)
            {
                HttpState _httpState = new HttpState() { Code = HttpStateCode.NOT_CONNECTED, Message = "NOT CONNECTED" };
                Dictionary<String, Object> _model = new Dictionary<String, Object>();
                _model[HttpState.Name] = _httpState;
                response.Content = new StringContent(JsonConvert.SerializeObject(_model));
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                actionContext.Response = response;
                return Task.FromResult<HttpResponseMessage>(response);
            }
            else
            {
                response.Headers.Add("authorization", "Bearer " + _newToken);
                return continuation();
            }
        }
    }
}
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;

namespace webCaisse.Tools
{
    public class Utilitaire
    {
        public static Byte[] getPictureBoxAsByte(String _imageAsString)
        {
            Byte[] _bytes = Convert.FromBase64String(_imageAsString);
            MemoryStream ms = new MemoryStream(_bytes);
            Image ret = Image.FromStream(ms);
            Image img = ScaleImage(ret);

            Byte[] _content = null;
            if (img != null)
            {
                ImageConverter converter = new ImageConverter();
                _content = (byte[])converter.ConvertTo(img, typeof(byte[]));
            }
            return _content;
        }
        public static System.Drawing.Image ScaleImage(System.Drawing.Image image)
        {
            //var ratio = (double)maxHeight / image.Height;
            var newWidth = 150;
            var newHeight = 150;
            var newImage = new Bitmap(newWidth, newHeight);
            using (var g = Graphics.FromImage(newImage))
            {
                g.DrawImage(image, 0, 0, newWidth, newHeight);
            }
            return newImage;
        }

        public static String getEmplacement(ApiController _controller)
        {
            String _emplacement = "";
            Int64? _identifiant = TokenManager.getIdentifiantFromToken(_controller.Request);
            string _source = (_identifiant != null) ? "ID:" + _identifiant : "";
            string _actionName = _controller.ControllerContext.RouteData.Values["action"].ToString();
            string _controllerName = _controller.ControllerContext.RouteData.Values["controller"].ToString();
            _emplacement = _emplacement + "[" + _controllerName + "]";
            _emplacement = _emplacement + "[" + _actionName + "]";
            _emplacement = _emplacement + "[" + _source + "]";
            return _emplacement;
        }
        public static String getDetailsException(Exception ex)
        {
            String _details = "";
            if (ex != null)
            {
                _details = _details + ((ex.Message != null) ? ex.Message + " ###\n" : "");
                _details = _details + ((ex.TargetSite != null) ? ex.TargetSite.ToString() + " ###\n" : "");
                _details = _details + ((ex.InnerException != null) ? ex.InnerException.ToString() + " ###\n" : "");
                _details = _details + ((ex.Source != null) ? ex.Source.ToString() + " ###\n" : "");
                _details = _details + ((ex.StackTrace != null) ? ex.StackTrace + " ###\n" : "");
            }
            return _details;
        }
        public static HttpResponseMessage constructResponse(ApiController _controller, Object _object)
        {
            var request = _controller.ActionContext.Request;
            if (_controller.ActionContext.Response == null)
            {
                _controller.ActionContext.Response = request.CreateResponse(HttpStatusCode.OK);
            }
            HttpResponseMessage response = _controller.ActionContext.Response;
            response.Content = new StringContent(JsonConvert.SerializeObject(_object));
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return response;
        }
    }
}
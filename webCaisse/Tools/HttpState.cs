using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webCaisse.Tools
{
    public class HttpState
    {
        public static String Name = "HttpState";
        public int Code { get; set; }
        public String Message { get; set; }
        public HttpState()
        {
            this.Code = HttpStateCode.OK;
            this.Message = "OK";
        }
    }
}
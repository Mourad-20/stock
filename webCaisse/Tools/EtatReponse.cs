using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webCaisse.Tools
{
    public class EtatReponse
    {
        public static String Name = "EtatReponse";
        public int Code { get; set; }
        public String Message { get; set; }
        public EtatReponse()
        {

        }
    }
}
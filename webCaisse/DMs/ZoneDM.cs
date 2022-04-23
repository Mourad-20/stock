using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webCaisse.DMs
{
    public class ZoneDM
    {
        public Int64 Identifiant { get; set; }
        public String Code { get; set; }
        public String Libelle { get; set; }
        public String NomImprimante { get; set; }
        public Int64? EnActivite { get; set; }
    }
}
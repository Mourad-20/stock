using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webCaisse.DMs
{
    public class CommercialisationDM
    {
        public Int64 Identifiant { get; set; }
        public Int64? IdCaisse { get; set; }
        public Int64? IdCategorie { get; set; }
        public String LibelleCategorie { get; set; }
        
        public String CodeCategorie { get; set; }
    }
}
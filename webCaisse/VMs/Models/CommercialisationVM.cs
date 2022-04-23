using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webCaisse.VMs.Models
{
    public class CommercialisationVM
    {
        public Int64 Identifiant { get; set; }
        public Int64? IdCaisse { get; set; }
        public Int64? IdCategorie { get; set; }
        public String LibelleCategorie { get; set; }        
        public Int64? Coche { get; set; }

    }
}
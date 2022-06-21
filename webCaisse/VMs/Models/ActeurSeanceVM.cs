using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webCaisse.VMs.Models
{
    public class ActeurSeanceVM
    {
        public Int64 Identifiant { get; set; }
        public Int64? IdSeance { get; set; }
        public Int64? IdUtilisateur { get; set; }
        public String NomPrenom { get; set; }        
        public Int64? Presence { get; set; }

    }
}
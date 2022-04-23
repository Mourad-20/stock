using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webCaisse.DMs
{
    public class AppartenanceDM
    {
        public Int64 Identifiant { get; set; }
        public Int64? IdUtilisateur { get; set; }
        public Int64? IdGroupe { get; set; }
        public String LibelleGroupe { get; set; }
    }
}
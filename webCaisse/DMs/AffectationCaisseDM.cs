using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webCaisse.DMs
{
    public class AffectationCaisseDM
    {
        public Int64 Identifiant { get; set; }
        public Int64? IdCaisse { get; set; }
        public Int64? IdUtilisateur { get; set; }
        public String NomPrenom { get; set; }
    }
}
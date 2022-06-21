using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webCaisse.DMs
{
    public class InterlocuteurDM
    {
       
            public Int64 Identifiant { get; set; }
            public String Nom { get; set; }
            public String Fonction { get; set; }

            public String Email1 { get; set; }
            public String Email2 { get; set; }
            public String Tel1 { get; set; }
            public String Tel2 { get; set; }
            public Int64? IdLocalite { get; set; }
            public virtual LocaliteDM LocaliteDM { get; set; }
            public Int64? EnActivite { get; set; }
            public Int64? Affichable { get; set; }
            public String Fix { get; set; }
        
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webCaisse.Db.Entities
{
    public class AffectationMessage
    {
        public Int64 Identifiant { get; set; }
        public Int64? IdDetailCommande { get; set; }
        public virtual DetailCommande DetailCommande { get; set; }
        public Int64? IdMessage { get; set; }
        public virtual Message Message { get; set; }
        public Int64? EnActivite { get; set; }
        public Int64? Affichable { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webCaisse.Db.Entities
{
    public class TauxTva
    {
        public Int64 Identifiant { get; set; }
        public Double Taux { get; set; }
        public Int64? EnActivite { get; set; }
        public Int64? Affichable { get; set; }
    }
}
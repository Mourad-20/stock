using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webCaisse.Db.Entities
{
    public class DetailReglement
    {
        public Int64 Identifiant { get; set; }
        public Double? Montant { get; set; }
        public Double? Quantite { get; set; }
        public Int64? IdReglement { get; set; }
        public virtual Reglement Reglement { get; set; }
        public Int64? IdDetailCommande { get; set; }
        public virtual DetailCommande DetailCommande { get; set; }
        public Int64? EnActivite { get; set; }
        public Int64? Affichable { get; set; }
    }
}
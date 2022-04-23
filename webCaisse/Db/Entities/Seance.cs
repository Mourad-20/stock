using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webCaisse.Db.Entities
{
    public class Seance
    {

        public Int64 Identifiant { get; set; }
        public Int64? IdUtilisateur { get; set; }
        public virtual Utilisateur Utilisateur { get; set; }
        public Int64? IdCaisse { get; set; }
        public virtual Caisse Caisse { get; set; }
        public String Numero { get; set; }
        public DateTime? DateDebut { get; set; }
        public DateTime? DateFin { get; set; }
        public Double MontantDebut { get; set; }
        public Double MontantFin { get; set; }
        public Double MontantPreleve { get; set; }
        public Int64? EnActivite { get; set; }
        public Int64? Affichable { get; set; }
    }
}
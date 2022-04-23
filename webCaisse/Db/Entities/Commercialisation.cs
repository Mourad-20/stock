using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webCaisse.Db.Entities
{
    public class Commercialisation
    {
        public Int64 Identifiant { get; set; }
        public Int64? IdCaisse { get; set; }
        public virtual Caisse Caisse { get; set; }
        public Int64? IdCategorie { get; set; }
        public virtual Categorie Categorie { get; set; }
        public Int64? EnActivite { get; set; }
        public Int64? Affichable { get; set; }
    }
}
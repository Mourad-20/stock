using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webCaisse.Db.Entities
{
    public class Appartenance
    {
        public Int64 Identifiant { get; set; }
        public Int64? IdUtilisateur { get; set; }
        public virtual Utilisateur Utilisateur { get; set; }
        public Int64? IdGroupe { get; set; }
        public virtual Groupe Groupe { get; set; }
        public Int64? EnActivite { get; set; }
        public Int64? Affichable { get; set; }
    }
}
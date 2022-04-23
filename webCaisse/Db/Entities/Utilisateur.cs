using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webCaisse.Db.Entities
{
    public class Utilisateur
    {
        public Int64 Identifiant { get; set; }
        public String Login { get; set; }
        public String Password { get; set; }
        public String Jeton { get; set; }
        public String Nom { get; set; }
        public String Prenom { get; set; }
        public Int64? EnActivite { get; set; }
        public Int64? Affichable { get; set; }
    }
}
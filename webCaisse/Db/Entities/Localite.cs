using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webCaisse.Db.Entities
{
    public class Localite
    {
        public Int64 Identifiant { get; set; }
        public String Libelle { get; set; }
        public String Code { get; set; }
        public String Numero { get; set; }
        public String Adresse1 { get; set; }
        public String Adresse2 { get; set; }
        public String Tel1 { get; set; }
        public String Tel2 { get; set; }
        public Int64? IdEtatLocalite { get; set; }
        public virtual EtatLocalite EtatLocalite { get; set; }
        public Int64? EnActivite { get; set; }
        public Int64? Affichable { get; set; }
        public String RC { get; set; }
    }
}
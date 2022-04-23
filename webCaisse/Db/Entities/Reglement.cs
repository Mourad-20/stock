using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webCaisse.Db.Entities
{
    public class Reglement
    {
        public Int64 Identifiant { get; set; }
        public String Numero { get; set; }
        public DateTime? DateReglement { get; set; }
        public Double? Montant { get; set; }
        public Int64? IdCommande { get; set; }
        public virtual Commande Commande { get; set; }
        public Int64? IdSeance { get; set; }
        public virtual Seance Seance { get; set; }
        public Int64? IdModeReglement { get; set; }
        public virtual ModeReglement ModeReglement { get; set; }
        public Int64? IdCreePar { get; set; }
        public virtual Utilisateur CreeParUtilisateur { get; set; }
        public Int64? EnActivite { get; set; }
        public Int64? Affichable { get; set; }
        public DateTime? Datecheque { get; set; }
        public String Ncheque { get; set; }
        public String NomBanque { get; set; }
        public String NCompte { get; set; }
    }
}
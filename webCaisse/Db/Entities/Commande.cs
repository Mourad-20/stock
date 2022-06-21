using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webCaisse.Db.Entities
{
    public class Commande
    {
        public Int64 Identifiant { get; set; }
        public String Numero { get; set; }
        public String CodeCommande { get; set; }
        public DateTime? DateCommande { get; set; }
        public Double? Montant { get; set; }
        public Int64? IdEtatCommande { get; set; }
        public virtual EtatCommande EtatCommande { get; set; }
        public Int64? IdSeance { get; set; }
        public virtual Seance Seance { get; set; }
        public Int64? IdLocalite { get; set; }
        public virtual Localite Localite { get; set; }
        public Int64? IdServeur { get; set; }
        public virtual Utilisateur Serveur { get; set; }
        public Int64? IdCreePar { get; set; }
        public virtual Utilisateur CreePar { get; set; }
        public Int64? EnActivite { get; set; }
        public Int64? Affichable { get; set; }
        public Int64? IdSource { get; set; }
        public virtual Commande CommandeSource { get; set; }
    }
}
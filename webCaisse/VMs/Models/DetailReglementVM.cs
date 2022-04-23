using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webCaisse.VMs.Models
{
    public class DetailReglementVM
    {
        public Int64 Identifiant { get; set; }
        public Double? Montant { get; set; }
        public Double? Quantite { get; set; }
        public Int64? IdReglement { get; set; }
        public Int64? IdDetailCommande { get; set; }
        public String LibelleArticle { get; set; }
        public Int64? EnActivite { get; set; }
    }
}
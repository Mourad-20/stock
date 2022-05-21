using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webCaisse.DMs
{
    public class ArticleDM
    {
        public Int64 Identifiant { get; set; }
        public String Libelle { get; set; }
        public String LibelleCategorie { get; set; }
        public String LibelleZone { get; set; }
        public String LibelleTypeUnite { get; set; }
        public Double Montant { get; set; }
        public Double QuantiteDisponible { get; set; }
        public Double QuantiteMin { get; set; }
        public Int64? IdCategorie { get; set; }
        public Int64? IdZone { get; set; }
        public Int64? IdTypeUnite { get; set; }
        public Int64? IdTauxTva { get; set; }
        public Int64? IdTypeArticle { get; set; }
        public String LibelleTypeArticle { get; set; }

        public Double TauxTva { get; set; }
        public String ImageAsString { get; set; }
        public String PathImage { get; set; }
        public Int64? EnActivite { get; set; }
        public String Referance { get; set; }
        public String CodeBare { get; set; }
    }
}
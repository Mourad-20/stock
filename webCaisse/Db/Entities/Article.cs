using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webCaisse.Db.Entities
{
    public class Article
    {
        public Int64 Identifiant { get; set; }
        public String Libelle { get; set; }
        public Double Montant { get; set; }
        public Double QuantiteDisponible { get; set; }
        public Double QuantiteMin { get; set; }
        public Int64? IdCategorie { get; set; }
        public virtual Categorie Categorie { get; set; }
        public Int64? IdTypeUnite { get; set; }
        public virtual TypeUnite TypeUnite { get; set; }
        public Int64? IdTauxTva { get; set; }
        public virtual TauxTva TauxTva { get; set; }
        public Int64? IdZone { get; set; }
        public Int64? IdTypeArticle { get; set; }
        public virtual TypeArticle TypeArticle { get; set; }
        public virtual Zone Zone { get; set; }
        public String PathImage { get; set; }
        public Int64? EnActivite { get; set; }
        public Int64? Affichable { get; set; }
        public String Referance { get; set; }
        public String CodeBare { get; set; }
    }
}
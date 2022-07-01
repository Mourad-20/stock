using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webCaisse.Db.Entities
{
    public class DetailCommande
    {
        public Int64 Identifiant { get; set; }
        public Double? Montant { get; set; }
        public Double? MontantDeclaration { get; set; }
        public Double? TauxTva { get; set; }
        public DateTime? DateExpiration { get; set; }
        public Int64? IdCaisse { get; set; }
        public virtual Caisse Caisse { get; set; }
        public Int64? IdTypeUnite { get; set; }

        public virtual TypeUnite TypeUnite { get; set; }

        public Double? Quantite { get; set; }
        public Double? QuantiteServi { get; set; }
        public DateTime? DateCreation { get; set; }
        public DateTime? DateValidation  { get; set; }
        public String NumerodeLot { get; set; }
        public String Description { get; set; }
        public Int64? IdArticle { get; set; }
        public virtual Article Article { get; set; }
        public Int64? IdCommande { get; set; }
        public virtual Commande Commande { get; set; }
        public Int64? IdCreePar { get; set; }
        public virtual Utilisateur CreePar { get; set; }

        public Int64? IdValiderPar { get; set; }
        public virtual Utilisateur ValiderPar { get; set; }
        public String TM { get; set; }
        public String TF { get; set; }

        public Double? SPI { get; set; }
        public Double? SPF { get; set; }
        public Int64? IdSituation { get; set; }
        public virtual SituationCommande Situation { get; set; }


        public Int64? EnActivite { get; set; }
        public Int64? Affichable { get; set; }
    }
}
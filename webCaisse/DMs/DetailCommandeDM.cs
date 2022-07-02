using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webCaisse.DMs
{
    public class DetailCommandeDM
    {
        public Int64 Identifiant { get; set; }
        public Double? Montant { get; set; }
      

        public DateTime? DateExpiration { get; set; }

        public Double? TauxTVA { get; set; }
        public Double? Quantite { get; set; }
        public Double? QuantiteServi { get; set; }
        public Int64? IdArticle { get; set; }
        public Int64? IdCaisse{ get; set; }
        public String LibelleCaisse { get; set; }

        public Int64? IdTypeUnite { get; set; }
        public String LibelleTypeUnite { get; set; }


        public String NumerodeLot { get; set; }
        public String Description { get; set; }
        public Int64? IdZone { get; set; }
        public Int64? IdCreePar { get; set; }
        public Int64? IdValiderPar { get; set; }
        public String NomControleur { get; set; }
        public DateTime? DateCreation { get; set; }
        public DateTime? DateValidation { get; set; }
        public Int64? IdSituation { get; set; }
        public String LibelleSituation { get; set; }
        public String LibelleArticle { get; set; }
        public Int64? IdCommande { get; set; }

      
        public Int64? EnActivite { get; set; }
        public ICollection<AffectationMessageDM> AffectationMessageDMs { get; set; }
    }
}
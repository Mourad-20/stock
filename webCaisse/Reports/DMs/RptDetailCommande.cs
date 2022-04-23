using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webCaisse.Reports.DMs
{
    public class RptDetailCommande
    {
        public String LibelleArticle { get; set; }
        public String Quantite { get; set; }
        public String Montant { get; set; }
        public String Total { get; set; }
    }
}
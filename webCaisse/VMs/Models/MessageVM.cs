using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webCaisse.VMs.Models
{
    public class MessageVM
    {
        public Int64 Identifiant { get; set; }
        public String Libelle { get; set; }
        public Int64? EnActivite { get; set; }
        public Int64? Quantite { get; set; }
        public Int64? IdArticle { get; set; }
        public String LibelleArticle { get; set; }
        public String LibelleType { get; set; }
        public Int64? IdArticleSrc { get; set; }
    }
}
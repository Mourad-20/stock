using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webCaisse.Db.Entities
{
    public class Message
    {
        public Int64 Identifiant { get; set; }
        public String Libelle { get; set; }
        public Int64? EnActivite { get; set; }
        public Int64? Affichable { get; set; }
        public Int64? Quantite { get; set; }
        public virtual Article Article { get; set; }
        public Int64? IdArticle { get; set; }
        public virtual Article ArticleSrc { get; set; }
        public Int64? IdArticleSrc { get; set; }
        public virtual TypeMessage TypeMessage { get; set; }
        public Int64? IdTypeMessage { get; set; }

    }
}
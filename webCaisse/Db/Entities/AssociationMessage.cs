using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webCaisse.Db.Entities
{
    public class AssociationMessage
    {
        public Int64 Identifiant { get; set; }
        public Int64? IdArticle { get; set; }
        public virtual Article Article { get; set; }
        public Int64? IdMessage { get; set; }
        public virtual Message Message { get; set; }
        public Int64? EnActivite { get; set; }
        public Int64? Affichable { get; set; }
    }
}
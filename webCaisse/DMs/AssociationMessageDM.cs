using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webCaisse.DMs
{
    public class AssociationMessageDM
    {
        public Int64 Identifiant { get; set; }
        public Int64? IdMessage { get; set; }
        public String LibelleMessage { get; set; }
        public Int64? IdArticle { get; set; }
    }
}
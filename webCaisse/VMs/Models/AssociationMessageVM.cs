using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webCaisse.VMs.Models
{
    public class AssociationMessageVM
    {
        public Int64 Identifiant { get; set; }
        public Int64? IdMessage { get; set; }
        public Int64? IdArticle { get; set; }
        public String LibelleMessage { get; set; }        
        public Int64? EnActivite { get; set; }

    }
}
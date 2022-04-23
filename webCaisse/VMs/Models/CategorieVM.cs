using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webCaisse.VMs.Models
{
    public class CategorieVM
    {
        public Int64 Identifiant { get; set; }
        public String Code { get; set; }
        public String Libelle { get; set; }
        public String PathImage { get; set; }
        public String ImageAsString { get; set; }
        public Int64? EnActivite { get; set; }
        public String Background { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webCaisse.VMs.Models
{
    public class RecapVM
    {
        public Int64 IdUtilisateur { get; set; }
        public String NomUtilisateur { get; set; }
        public Double? Montant { get; set; }
        public Int64 NombreCommande { get; set; }
        public Double? poucentage { get; set; }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webCaisse.VMs.Models
{
    public class SeanceVM
    {
        public Int64 Identifiant { get; set; }
        public String Numero { get; set; }
        public Int64? IdCaisse { get; set; }
        public String LibelleCaisse { get; set; }
        public Int64? IdUtilisateur { get; set; }
        public String NomPrenom { get; set; }
        public DateTime? DateDebut { get; set; }
        public DateTime? DateFin { get; set; }
        public Double MontantDebut { get; set; }
        public Double MontantFin { get; set; }
        public Double MontantPreleve { get; set; }
        public Int64? EnActivite { get; set; }
    }
}
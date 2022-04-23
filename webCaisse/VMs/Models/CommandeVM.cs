using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webCaisse.VMs.Models
{
    public class CommandeVM
    {
        public Int64 Identifiant { get; set; }
        public String Numero { get; set; }
        public DateTime? DateCommande { get; set; }
        public Double? Montant { get; set; }
        public String CodeCommande { get; set; }
        public Int64? IdEtatCommande { get; set; }
        public String CodeEtatCommande { get; set; }
        public String LibelleEtatCommande { get; set; }
        public Int64? IdSeance { get; set; }
        public String LibelleCaisse { get; set; }
        public String NumeroSeance { get; set; }
        public Int64? IdLocalite { get; set; }
        public String LibelleLocalite { get; set; }
        public Int64? IdServeur { get; set; }
        public String NomServeur { get; set; }
        public Int64? IdCreePar { get; set; }
        public Int64? EnActivite { get; set; }
        public ICollection<DetailCommandeVM> DetailCommandes { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webCaisse.VMs.Models
{
    public class ReglementVM
    {

        public Int64 Identifiant { get; set; }
        public String Numero { get; set; }
        public DateTime? DateReglement { get; set; }
        public Double? Montant { get; set; }
        public Int64? IdCommande { get; set; }
        public String NumeroCommande { get; set; }
        public Int64? IdModeReglement { get; set; }
        public String LibelleModeReglement { get; set; }
        public Int64? IdCreePar { get; set; }
        public Int64? EnActivite { get; set; }
        public String Datecheque { get; set; }
        public String Ncheque { get; set; }
        public String NomBanque { get; set; }
        public String NCompte { get; set; }
        public ICollection<DetailReglementVM> DetailReglements { get; set; }
    }
}
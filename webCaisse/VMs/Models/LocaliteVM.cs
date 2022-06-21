using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webCaisse.VMs.Models
{
    public class LocaliteVM
    {
        public Int64 Identifiant { get; set; }
        public String Libelle { get; set; }
        public String Code { get; set; }
        public Int64? EnActivite { get; set; }
        public String Numero { get; set; }
        public String Adresse1 { get; set; }
        public String Adresse2 { get; set; }
        public String Tel1 { get; set; }
        public String Tel2 { get; set; }
        public String RC { get; set; }
        public ICollection<InterlocuteurVM> InterlocuteurVMs { get; set; }
        public Int64? IdUtilisateur { get; set; }
        public String NomUtilisateur { get; set; }

    }
}
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webCaisse.VMs.Models
{
    public class AffectationCaisseVM
    {
        public Int64 Identifiant { get; set; }
        public Int64? IdCaisse { get; set; }
        public Int64? IdUtilisateur { get; set; }
        public String NomPrenom { get; set; }
        public String LibelleGroupe { get; set; }
        public Int64? Coche { get; set; }

    }
}
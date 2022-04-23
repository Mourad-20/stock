﻿using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using webCaisse.Db.Entities;

namespace webCaisse.Db.configs
{
    public class TauxTvaConfig : EntityTypeConfiguration<TauxTva>
    {
        public TauxTvaConfig()
        {
            ToTable("Taux_Tva");
            HasKey(e => e.Identifiant);
           // HasOptional<Utilisateur>(e => e.CreeParUtilisateur).WithMany().HasForeignKey(e => e.IdCreePar);
            //HasOptional<Pays>(e => e.Pays).WithMany().HasForeignKey(e => e.IdPays);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using webCaisse.Db.Entities;

namespace webCaisse.Db.configs
{
    public class SituationCommandeConfig : EntityTypeConfiguration<SituationCommande>
    {
        public SituationCommandeConfig()
        {
            ToTable("Situation_Commande");
            HasKey(e => e.Identifiant);
           // HasOptional<Utilisateur>(e => e.CreeParUtilisateur).WithMany().HasForeignKey(e => e.IdCreePar);
            //HasOptional<Pays>(e => e.Pays).WithMany().HasForeignKey(e => e.IdPays);
        }
    }
}
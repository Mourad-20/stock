using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using webCaisse.Db.Entities;

namespace webCaisse.Db.configs
{
    public class ReglementConfig : EntityTypeConfiguration<Reglement>
    {
        public ReglementConfig()
        {
            ToTable("Reglement");
            HasKey(e => e.Identifiant);
            HasOptional<Utilisateur>(e => e.CreeParUtilisateur).WithMany().HasForeignKey(e => e.IdCreePar);
            HasOptional<Commande>(e => e.Commande).WithMany().HasForeignKey(e => e.IdCommande);
            HasOptional<ModeReglement>(e => e.ModeReglement).WithMany().HasForeignKey(e => e.IdModeReglement);
            HasOptional<Seance>(e => e.Seance).WithMany().HasForeignKey(e => e.IdSeance);
        }
    }
}
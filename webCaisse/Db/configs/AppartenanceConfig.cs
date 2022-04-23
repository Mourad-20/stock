using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using webCaisse.Db.Entities;

namespace webCaisse.Db.configs
{
    public class AppartenanceConfig : EntityTypeConfiguration<Appartenance>
    {
        public AppartenanceConfig()
        {
            ToTable("appartenance");
            HasKey(e => e.Identifiant);
            HasOptional<Utilisateur>(e => e.Utilisateur).WithMany().HasForeignKey(e => e.IdUtilisateur);
            HasOptional<Groupe>(e => e.Groupe).WithMany().HasForeignKey(e => e.IdGroupe);
        }
    }
}
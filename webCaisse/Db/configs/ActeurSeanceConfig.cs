using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using webCaisse.Db.Entities;

namespace webCaisse.Db.configs
{
    public class ActeurSeanceConfig : EntityTypeConfiguration<ActeurSeance>
    {
        public ActeurSeanceConfig()
        {
            ToTable("Acteur_Seance");
            HasKey(e => e.Identifiant);
            HasOptional<Utilisateur>(e => e.Utilisateur).WithMany().HasForeignKey(e => e.IdUtilisateur);
            HasOptional<Seance>(e => e.Seance).WithMany().HasForeignKey(e => e.IdSeance);
        }
    }
}
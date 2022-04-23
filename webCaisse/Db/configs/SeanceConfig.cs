using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using webCaisse.Db.Entities;

namespace webCaisse.Db.configs
{
    public class SeanceConfig : EntityTypeConfiguration<Seance>
    {
        public SeanceConfig()
        {
            ToTable("seance");
            HasKey(e => e.Identifiant);
            HasOptional<Utilisateur>(e => e.Utilisateur).WithMany().HasForeignKey(e => e.IdUtilisateur);
            HasOptional<Caisse>(e => e.Caisse).WithMany().HasForeignKey(e => e.IdCaisse);
        }
    }
}
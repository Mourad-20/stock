using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using webCaisse.Db.Entities;

namespace webCaisse.Db.configs
{
    public class LocaliteConfig : EntityTypeConfiguration<Localite>
    {
        public LocaliteConfig()
        {
            ToTable("localite");
            HasKey(e => e.Identifiant);
            HasOptional<EtatLocalite>(e => e.EtatLocalite).WithMany().HasForeignKey(e => e.IdEtatLocalite);
            HasOptional<Utilisateur>(e => e.Utilisateur).WithMany().HasForeignKey(e => e.IdUtilisateur);
            //HasOptional<Pays>(e => e.Pays).WithMany().HasForeignKey(e => e.IdPays);
        }
    }
}
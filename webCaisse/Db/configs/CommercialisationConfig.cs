using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using webCaisse.Db.Entities;

namespace webCaisse.Db.configs
{
    public class CommercialisationConfig : EntityTypeConfiguration<Commercialisation>
    {
        public CommercialisationConfig()
        {
            ToTable("Commercialisation");
            HasKey(e => e.Identifiant);
            HasOptional<Categorie>(e => e.Categorie).WithMany().HasForeignKey(e => e.IdCategorie);
            HasOptional<Caisse>(e => e.Caisse).WithMany().HasForeignKey(e => e.IdCaisse);
        }
    }
}
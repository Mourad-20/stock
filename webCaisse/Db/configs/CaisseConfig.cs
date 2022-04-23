using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using webCaisse.Db.Entities;

namespace webCaisse.Db.configs
{
    public class CaisseConfig : EntityTypeConfiguration<Caisse>
    {
        public CaisseConfig()
        {
            ToTable("Caisse");
            HasKey(e => e.Identifiant);
            //HasOptional<Pays>(e => e.Pays).WithMany().HasForeignKey(e => e.IdPays);
        }
    }
}
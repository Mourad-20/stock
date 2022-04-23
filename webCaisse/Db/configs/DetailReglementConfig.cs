using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using webCaisse.Db.Entities;

namespace webCaisse.Db.configs
{
    public class DetailReglementConfig : EntityTypeConfiguration<DetailReglement>
    {
        public DetailReglementConfig()
        {
            ToTable("Detail_Reglement");
            HasKey(e => e.Identifiant);
            HasOptional<DetailCommande>(e => e.DetailCommande).WithMany().HasForeignKey(e => e.IdDetailCommande);
            HasOptional<Reglement>(e => e.Reglement).WithMany().HasForeignKey(e => e.IdReglement);
        }
    }
}
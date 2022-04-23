using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using webCaisse.Db.Entities;

namespace webCaisse.Db.configs
{
    public class AffectationMessageConfig : EntityTypeConfiguration<AffectationMessage>
    {
        public AffectationMessageConfig()
        {
            ToTable("Affectation_Message");
            HasKey(e => e.Identifiant);
            HasOptional<Message>(e => e.Message).WithMany().HasForeignKey(e => e.IdMessage);
            HasOptional<DetailCommande>(e => e.DetailCommande).WithMany().HasForeignKey(e => e.IdDetailCommande);
        }
    }
}
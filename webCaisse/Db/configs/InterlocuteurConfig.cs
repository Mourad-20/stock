using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using webCaisse.Db.Entities;

namespace webCaisse.Db.configs
{
    public class InterlocuteurConfig : EntityTypeConfiguration<Interlocuteur>
    {
        public InterlocuteurConfig()
        {
            ToTable("Interlocuteur");
            HasKey(e => e.Identifiant);
            HasOptional<Localite>(e => e.Localite).WithMany().HasForeignKey(e => e.IdLocalite);
        }
    }
}
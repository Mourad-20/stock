using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using webCaisse.Db.Entities;

namespace webCaisse.Db.configs
{
    public class AssociationMessageConfig : EntityTypeConfiguration<AssociationMessage>
    {
        public AssociationMessageConfig()
        {
            ToTable("Association_Message");
            HasKey(e => e.Identifiant);
            HasOptional<Message>(e => e.Message).WithMany().HasForeignKey(e => e.IdMessage);
            HasOptional<Article>(e => e.Article).WithMany().HasForeignKey(e => e.IdArticle);
        }
    }
}
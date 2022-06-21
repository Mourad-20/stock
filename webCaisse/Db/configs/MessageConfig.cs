using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using webCaisse.Db.Entities;

namespace webCaisse.Db.configs
{
    public class MessageConfig : EntityTypeConfiguration<Message>
    {
        public MessageConfig()
        {
            ToTable("Message");
            HasKey(e => e.Identifiant);
                HasOptional<Article>(e => e.Article).WithMany().HasForeignKey(e => e.IdArticle);
            HasOptional<Article>(e => e.ArticleSrc).WithMany().HasForeignKey(e => e.IdArticleSrc);
            HasOptional<TypeMessage>(e => e.TypeMessage).WithMany().HasForeignKey(e => e.IdTypeMessage);
        }
    }
}
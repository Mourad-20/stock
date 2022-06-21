using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using webCaisse.Db.Entities;

namespace webCaisse.Db.configs
{
    public class ArticleConfig : EntityTypeConfiguration<Article>
    {
        public ArticleConfig()
        {
            ToTable("Article");
            HasKey(e => e.Identifiant);
            HasOptional<Categorie>(e => e.Categorie).WithMany().HasForeignKey(e => e.IdCategorie);
            HasOptional<TauxTva>(e => e.TauxTva).WithMany().HasForeignKey(e => e.IdTauxTva);
            HasOptional<TypeArticle>(e => e.TypeArticle).WithMany().HasForeignKey(e => e.IdTypeArticle);
            HasOptional<TypeUnite>(e => e.TypeUnite).WithMany().HasForeignKey(e => e.IdTypeUnite);
            HasOptional<Zone>(e => e.Zone).WithMany().HasForeignKey(e => e.IdZone);
            //HasOptional<Pays>(e => e.Pays).WithMany().HasForeignKey(e => e.IdPays);
        }
    }
}
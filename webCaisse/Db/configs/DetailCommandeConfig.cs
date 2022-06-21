using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using webCaisse.Db.Entities;

namespace webCaisse.Db.configs
{
    public class DetailCommandeConfig : EntityTypeConfiguration<DetailCommande>
    {
        public DetailCommandeConfig()
        {
            ToTable("Detail_Commande");
            HasKey(e => e.Identifiant);
            HasOptional<Commande>(e => e.Commande).WithMany().HasForeignKey(e => e.IdCommande);
            HasOptional<Article>(e => e.Article).WithMany().HasForeignKey(e => e.IdArticle);
            HasOptional<TypeUnite>(e => e.TypeUnite).WithMany().HasForeignKey(e => e.IdTypeUnite);

            HasOptional<Caisse>(e => e.Caisse).WithMany().HasForeignKey(e => e.IdCaisse);
            HasOptional<Utilisateur>(e => e.CreePar).WithMany().HasForeignKey(e => e.IdCreePar);
            HasOptional<Utilisateur>(e => e.ValiderPar).WithMany().HasForeignKey(e => e.IdCreePar);
            HasOptional<SituationCommande>(e => e.Situation).WithMany().HasForeignKey(e => e.IdSituation);
        }
    }
}
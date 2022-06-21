using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using webCaisse.Db.Entities;

namespace webCaisse.Db.configs
{
    public class CommandeConfig : EntityTypeConfiguration<Commande>
    {
        public CommandeConfig()
        {
            ToTable("Commande");
            HasKey(e => e.Identifiant);
            HasOptional<Utilisateur>(e => e.CreePar).WithMany().HasForeignKey(e => e.IdCreePar);
            HasOptional<Utilisateur>(e => e.Serveur).WithMany().HasForeignKey(e => e.IdServeur);
            HasOptional<Localite>(e => e.Localite).WithMany().HasForeignKey(e => e.IdLocalite);
            HasOptional<Seance>(e => e.Seance).WithMany().HasForeignKey(e => e.IdSeance);
            HasOptional<EtatCommande>(e => e.EtatCommande).WithMany().HasForeignKey(e => e.IdEtatCommande);
            HasOptional<Commande>(e => e.CommandeSource).WithMany().HasForeignKey(e => e.IdSource);
        }
    }
}
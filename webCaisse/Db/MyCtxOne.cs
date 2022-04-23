using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using webCaisse.Db.configs;

namespace webCaisse.Db
{
    public class MyCtxOne : DbContext
    {
        public MyCtxOne() : base("name=dbOne")
        {
            //Debug.WriteLine("Initialisation MyCtxOne");
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<MyCtxOne>(null);
            modelBuilder.Configurations.Add(new VilleConfig());
            modelBuilder.Configurations.Add(new UtilisateurConfig());
            modelBuilder.Configurations.Add(new CategorieConfig());
            modelBuilder.Configurations.Add(new ArticleConfig());
            modelBuilder.Configurations.Add(new GroupeConfig());
            modelBuilder.Configurations.Add(new AppartenanceConfig());
            modelBuilder.Configurations.Add(new LocaliteConfig());
            modelBuilder.Configurations.Add(new CommandeConfig());
            modelBuilder.Configurations.Add(new DetailCommandeConfig());
            modelBuilder.Configurations.Add(new SeanceConfig());
            modelBuilder.Configurations.Add(new EtatCommandeConfig());
            modelBuilder.Configurations.Add(new ModeReglementConfig());
            modelBuilder.Configurations.Add(new ReglementConfig());
            modelBuilder.Configurations.Add(new DetailReglementConfig());
            modelBuilder.Configurations.Add(new TauxTvaConfig());
            modelBuilder.Configurations.Add(new TypeUniteConfig());
            modelBuilder.Configurations.Add(new EtatLocaliteConfig());
            modelBuilder.Configurations.Add(new SituationCommandeConfig());
            modelBuilder.Configurations.Add(new ZoneConfig());
            modelBuilder.Configurations.Add(new CommercialisationConfig());
            modelBuilder.Configurations.Add(new CaisseConfig());
            modelBuilder.Configurations.Add(new AffectationCaisseConfig());
            modelBuilder.Configurations.Add(new AffectationMessageConfig());
            modelBuilder.Configurations.Add(new MessageConfig());
            modelBuilder.Configurations.Add(new ActeurSeanceConfig());
            modelBuilder.Configurations.Add(new AssociationMessageConfig());

            base.OnModelCreating(modelBuilder);
        }

    }
}
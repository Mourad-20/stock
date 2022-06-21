using System.Web.Http;
using Unity;
using Unity.WebApi;
using webCaisse.Db;
using webCaisse.Taks.Contracts;
using webCaisse.Taks.Implementation;
using webCaisse.uows;

namespace webCaisse
{
    public static class UnityConfig
    {
        public static void RegisterComponents(IUnityContainer container)
        {
			//var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();

            container.RegisterType<MyCtxOne, MyCtxOne>();
            container.RegisterType<IUnitOfWork, UnitOfWork>();
            container.RegisterType<IUtilisateurTask, UtilisateurTask>();
            container.RegisterType<ICategorieTask, CategorieTask>();
            container.RegisterType<IArticleTask, ArticleTask>();
            container.RegisterType<IGroupeTask, GroupeTask>();
            container.RegisterType<ITypeArticleTask, TypeArticleTask>();
            container.RegisterType<ITypeUniteTask, TypeUniteTask>();

            container.RegisterType<ILocaliteTask, LocaliteTask>();
            container.RegisterType<ICommandeTask, CommandeTask>();
            container.RegisterType<IDetailCommandeTask, DetailCommandeTask>();
            container.RegisterType<IEtatCommandeTask, EtatCommandeTask>();
            container.RegisterType<ISeanceTask, SeanceTask>();
            container.RegisterType<IReglementTask, ReglementTask>();
            container.RegisterType<IDetailReglementTask, DetailReglementTask>();
            container.RegisterType<IEtatLocaliteTask, EtatLocaliteTask>();
            container.RegisterType<IModeReglementTask, ModeReglementTask>();
            container.RegisterType<IImpressionTask, ImpressionTask>();
            container.RegisterType<ISituationCommandeTask, SituationCommandeTask>();
            container.RegisterType<IRapportTask, RapportTask>();
            container.RegisterType<IZoneTask, ZoneTask>();
            container.RegisterType<ICommercialisationTask, CommercialisationTask>();
            container.RegisterType<ICaisseTask, CaisseTask>();
            container.RegisterType<ITypeMessageTask, TypeMessageTask>();
            container.RegisterType<IAffectationMessageTask, AffectationMessageTask>();
            container.RegisterType<IActeurSeanceTask, ActeurSeanceTask>();
            container.RegisterType<IAffectationCaisseTask, AffectationCaisseTask>();
            container.RegisterType<IAppartenanceTask, AppartenanceTask>();
            container.RegisterType<IMessageTask, MessageTask>();
            container.RegisterType<IAssociationMessageTask, AssociationMessageTask>();

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}
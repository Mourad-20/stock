using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webCaisse.DMs;
using webCaisse.DMs.Codes;
using webCaisse.Reports.DMs;
using webCaisse.Taks.Contracts;
using webCaisse.Tools;

namespace webCaisse.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }


        public String Test()
        {
            IRapportTask _rapportTask = IoCContainer.Resolve<IRapportTask>();
            IImpressionTask _impressionTask = IoCContainer.Resolve<IImpressionTask>();
            IZoneTask _zoneTask = IoCContainer.Resolve<IZoneTask>();
            ZoneDM _zoneDM = _zoneTask.getZoneDMs().FirstOrDefault();
            ICollection<RptDetailCommande> _rptDetailCommandes = null;

            _rptDetailCommandes = new List<RptDetailCommande>();
            for (int i = 0; i < 60; i++)
            {
                _rptDetailCommandes.Add(new RptDetailCommande()
                {
                    LibelleArticle = "Article - " + i,
                    Quantite = "x 1500"
                });
            }
            String _numeroCommande = "Com. N°: " + "25";
            String _nomServeur = "Serveur: " + "Youssef";
            String _libelleLocalite = "Localite: " + "T4";
            String _dateGeneration = "Date: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            String _libelleZone = "Zone: " + 1;
            Byte[] _content = _rapportTask.genererTicketPreparation(_rptDetailCommandes,_numeroCommande, _dateGeneration, _nomServeur, _libelleLocalite, _libelleZone);
            _impressionTask.imprimerPreparation(_content, _zoneDM.NomImprimante);
            return "OK";
        }

        public String Test2()
        {   
            return "OK";
        }
        public String TestDB()
        {
            //GO TO http://localhost:53270/Home/TestDB
            IEtatCommandeTask _etatCommandeTask = IoCContainer.Resolve<IEtatCommandeTask>();
            EtatCommandeDM _etatCommandeDM = _etatCommandeTask.getEtatCommandeDMByCode(EtatCommandeCode.NON_REGLEE);
            return JsonConvert.SerializeObject(_etatCommandeDM);
        }
    }
}

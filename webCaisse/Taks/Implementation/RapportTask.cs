using Ghostscript.NET.Processor;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using webCaisse.Db.Entities;
using webCaisse.DMs;
using webCaisse.Taks.Contracts;
using webCaisse.Tools;
using webCaisse.uows;
using webCaisse.Reports.DMs;
using Microsoft.Reporting.WinForms;
using System.Web.Hosting;
using System.IO;

namespace webCaisse.Taks.Implementation
{
    public class RapportTask : IRapportTask
    {
        private IUnitOfWork _uow = null;

        public RapportTask()
        {
            _uow = IoCContainer.Resolve<IUnitOfWork>();
        }

        public byte[] genererTicketNote(ICollection<RptDetailCommande> _rptDetailCommandes, string _numeroCommande, string _dateGeneration, string _nomServeur, string _libelleLocalite,string _totalCommande)
        {
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();

            Warning[] warnings;
            string mimeType;
            string[] streamids;
            string encoding;
            string filenameExtension;
            var viewer = new ReportViewer();
            viewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/TicketNoteRpt.rdlc");
            ReportParameter _rpTotalCommande = new ReportParameter("TotalCommande", _totalCommande);
            ReportParameter _rpNumeroCommande = new ReportParameter("NumeroCommande", _numeroCommande);
            ReportParameter _rpDateGeneration = new ReportParameter("DateGeneration", _dateGeneration);
            ReportParameter _rpLibelleLocalite = new ReportParameter("LibelleLocalite", _libelleLocalite);
            ReportParameter _rpNomServeur = new ReportParameter("NomServeur", _nomServeur);
            viewer.LocalReport.SetParameters(new ReportParameter[] { _rpDateGeneration, _rpNumeroCommande, _rpLibelleLocalite, _rpNomServeur,_rpTotalCommande });


            ReportDataSource reportDataSource = new ReportDataSource();
            reportDataSource.Name = "DSTicketNote";
            reportDataSource.Value = _rptDetailCommandes;

            viewer.LocalReport.DataSources.Add(reportDataSource);
            viewer.LocalReport.Refresh();
            Byte[] _content = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);

            watch.Stop();
            Debug.WriteLine($"genererTicketNote Execution Time: {watch.ElapsedMilliseconds} ms");
            Debug.WriteLine("===============================================================");
            return _content;
        }

        public byte[] genererTicketPreparation(ICollection<RptDetailCommande> _rptDetailCommandes, String _numeroCommande, String _dateGeneration,String _nomServeur, String _libelleLocalite, String _libelleZone)
        {
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();

            Warning[] warnings;
            string mimeType;
            string[] streamids;
            string encoding;
            string filenameExtension;
            var viewer = new ReportViewer();
            viewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/TicketCuisineRpt.rdlc");
            ReportParameter _rpNumeroCommande = new ReportParameter("NumeroCommande", _numeroCommande);
            ReportParameter _rpDateGeneration = new ReportParameter("DateGeneration", _dateGeneration);
            ReportParameter _rpLibelleLocalite = new ReportParameter("LibelleLocalite", _libelleLocalite);
            ReportParameter _rpNomServeur = new ReportParameter("NomServeur", _nomServeur);
            ReportParameter _rpLibelleZone = new ReportParameter("LibelleZone", _libelleZone);
            viewer.LocalReport.SetParameters(new ReportParameter[] { _rpDateGeneration, _rpNumeroCommande ,_rpLibelleLocalite,_rpNomServeur,_rpLibelleZone });
            

            ReportDataSource reportDataSource = new ReportDataSource();
            reportDataSource.Name = "DSTicketPrepa";
            reportDataSource.Value = _rptDetailCommandes;

            viewer.LocalReport.DataSources.Add(reportDataSource);
            viewer.LocalReport.Refresh();
            Byte[] _content = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);

            watch.Stop();
            Debug.WriteLine($"genererTicketPreparation Execution Time: {watch.ElapsedMilliseconds} ms");
            Debug.WriteLine("===============================================================");
            return _content;
        }
    }
}
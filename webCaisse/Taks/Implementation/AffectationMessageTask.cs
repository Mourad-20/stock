using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Hosting;
using webCaisse.Db.Entities;
using webCaisse.DMs;
using webCaisse.DMs.Codes;
using webCaisse.Reports.DMs;
using webCaisse.Taks.Contracts;
using webCaisse.Tools;
using webCaisse.uows;

namespace webCaisse.Taks.Implementation
{
    public class AffectationMessageTask : IAffectationMessageTask
    {
        private IUnitOfWork _uow = null;
        public AffectationMessageTask()
        {
            _uow = IoCContainer.Resolve<IUnitOfWork>();
        }

        private void addAffectationMessage(AffectationMessage obj)
        {
            //ok
            _uow.Repos<AffectationMessage>().Insert(obj);
            _uow.saveChanges();
        }

        private AffectationMessage getAffectationMessageById(long _identifiant)
        {
            //ok
            return _uow.Repos<AffectationMessage>().GetAll().Where(a => a.Identifiant == _identifiant).FirstOrDefault();
        }

        //=#########################################################################
        //=#########################################################################
        
        public long? addAffectationMessageDM(AffectationMessageDM _affectationMessageDM)
        {
            AffectationMessage _affectationMessage = _uow.Repos<AffectationMessage>().Create();
            _affectationMessage.Affichable = 1;
            _affectationMessage.EnActivite = 1;
            _affectationMessage.IdDetailCommande = _affectationMessageDM.IdDetailCommande;
            _affectationMessage.IdMessage = _affectationMessageDM.IdMessage;
            _uow.Repos<AffectationMessage>().Insert(_affectationMessage);
            _uow.saveChanges();
            return _affectationMessage.Identifiant;
        }

        public ICollection<AffectationMessageDM> getAffectationMessageDMsByIdDetailCommande(long _idDetailCommande)
        {
            return _uow.Repos<AffectationMessage>()
                            .GetAll().Where(a => a.IdDetailCommande == _idDetailCommande && a.EnActivite == 1 && a.Affichable == 1)
                            .Select(a => new AffectationMessageDM()
                            {
                                Identifiant = a.Identifiant,
                                IdDetailCommande = a.IdDetailCommande,
                                IdMessage = a.IdMessage,
                                LibelleMessage = a.Message.Libelle,
                            }).ToList();
        }

        public void updateAffectationMessageDM(AffectationMessageDM _affectationMessageDM)
        {
            AffectationMessage _affectationMessage = getAffectationMessageById(_affectationMessageDM.Identifiant);
            
            _uow.Repos<AffectationMessage>().Update(_affectationMessage);
            _uow.saveChanges();
        }

        public void removeAffectationMessageDM(long _identifiant)
        {
            AffectationMessage _obj = _uow.Repos<AffectationMessage>().GetAll().Where(a => a.Identifiant == _identifiant && a.EnActivite == 1 && a.Affichable == 1).FirstOrDefault();
            _uow.Repos<AffectationMessage>().DeleteObject(_obj);
            _uow.saveChanges();
        }
    }
}
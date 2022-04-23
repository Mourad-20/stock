using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using webCaisse.Db.Entities;
using webCaisse.DMs;
using webCaisse.Taks.Contracts;
using webCaisse.Tools;
using webCaisse.uows;

namespace webCaisse.Taks.Implementation
{
    public class AssociationMessageTask : IAssociationMessageTask
    {
        private IUnitOfWork _uow = null;
        public AssociationMessageTask()
        {
            _uow = IoCContainer.Resolve<IUnitOfWork>();
        }

        public long? addAssociationMessageDM(AssociationMessageDM _associationMessageDM)
        {
            AssociationMessage _obj = _uow.Repos<AssociationMessage>().Create();
            _obj.Affichable = 1;
            _obj.EnActivite = 1;
            _obj.IdMessage = _associationMessageDM.IdMessage;
            _obj.IdArticle = _associationMessageDM.IdArticle;
            _uow.Repos<AssociationMessage>().Insert(_obj);
            _uow.saveChanges();
            return _obj.Identifiant;
        }
        
        public ICollection<AssociationMessageDM> getAssociationMessageDMs([Optional] long? _idArticle)
        {
            bool _shouldGetResult = (_idArticle != null);
            ICollection<AssociationMessageDM> _result = _uow.Repos<AssociationMessage>().GetAll()
                .Where(
                    a =>
                    (_shouldGetResult)
                    && (a.Affichable == 1)
                    && (a.EnActivite == 1)
                    && ((_idArticle != null ) ? a.IdArticle == _idArticle : true)
                    ).Select(
                        o => new AssociationMessageDM()
                        {
                            Identifiant = o.Identifiant,
                            LibelleMessage = (o.Message != null) ? o.Message.Libelle : "",
                            IdMessage = o.IdMessage,
                            IdArticle = o.IdArticle,
                        }
                    ).ToList();
            return _result;
        }
        

        public void removeAssociationMessageDM(long _identifiant)
        {
            AssociationMessage _obj = _uow.Repos<AssociationMessage>().GetAll().Where(a => a.Identifiant == _identifiant && a.EnActivite == 1 && a.Affichable == 1).FirstOrDefault();
            _uow.Repos<AssociationMessage>().DeleteObject(_obj);
            _uow.saveChanges();
        }
        
    }
}
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
    public class CommercialisationTask : ICommercialisationTask
    {
        private IUnitOfWork _uow = null;
        public CommercialisationTask()
        {
            _uow = IoCContainer.Resolve<IUnitOfWork>();
        }

        public long? addCommercialisationDM(CommercialisationDM _commercialisationDM)
        {
            Commercialisation _obj = _uow.Repos<Commercialisation>().Create();
            _obj.Affichable = 1;
            _obj.EnActivite = 1;
            _obj.IdCategorie = _commercialisationDM.IdCategorie;
            _obj.IdCaisse = _commercialisationDM.IdCaisse;
            _uow.Repos<Commercialisation>().Insert(_obj);
            _uow.saveChanges();
            return _obj.Identifiant;
        }

        public ICollection<CommercialisationDM> getCommercialisationDMs([Optional]Int64? _idCaisse)
        {
            bool _shouldGetResult = (_idCaisse != null);
            ICollection<CommercialisationDM> _result = _uow.Repos<Commercialisation>().GetAll()
                .Where(
                    a =>
                    (_shouldGetResult)
                    && (a.Affichable == 1)
                    && (a.EnActivite == 1)
                    && ((_idCaisse != null) ? a.IdCaisse == _idCaisse : true)
                    ).Select(
                        o => new CommercialisationDM()
                        {
                            Identifiant = o.Identifiant,
                            CodeCategorie = (o.Categorie != null) ? o.Categorie.Code : "",
                            LibelleCategorie = (o.Categorie != null) ? o.Categorie.Libelle : "",
                            IdCategorie = o.IdCategorie,
                            IdCaisse = o.IdCaisse,

                        }
                    ).ToList();
            return _result;
        }

        public void removeCommercialisationDM(long _identifiant)
        {
            Commercialisation _obj = _uow.Repos<Commercialisation>().GetAll().Where(a => a.Identifiant == _identifiant && a.EnActivite == 1 && a.Affichable == 1).FirstOrDefault();
            _uow.Repos<Commercialisation>().DeleteObject(_obj);
            _uow.saveChanges();
        }
    }
}
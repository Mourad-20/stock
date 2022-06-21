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
    public class ZoneTask : IZoneTask
    {
        private IUnitOfWork _uow = null;
        public ZoneTask()
        {
            _uow = IoCContainer.Resolve<IUnitOfWork>();
        }

        public ICollection<ZoneDM> getZoneDMs([Optional] Int64? _enActivite)
        {
            bool _shouldGetResult = (_enActivite != null);
            _shouldGetResult = (_shouldGetResult != false) ? _shouldGetResult : true;
            ICollection<ZoneDM> _result = _uow.Repos<Zone>().GetAll()
                .Where(
                    a =>
                    (_shouldGetResult)
                    && (a.Affichable == 1)
                    && ((_enActivite != null) ? a.EnActivite == _enActivite : true)
                    ).Select(
                        a => new ZoneDM()
                        {
                            Identifiant = a.Identifiant,
                            Code = a.Code,
                            Libelle = a.Libelle,
                            NomImprimante = a.NomImprimante,
                            EnActivite = a.EnActivite,
                        }
                    ).ToList();
            return _result;
        }

    }
}
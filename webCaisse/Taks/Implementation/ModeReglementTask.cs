using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using webCaisse.Db.Entities;
using webCaisse.DMs;
using webCaisse.Taks.Contracts;
using webCaisse.Tools;
using webCaisse.uows;

namespace webCaisse.Taks.Implementation
{
    public class ModeReglementTask : IModeReglementTask
    {
        private IUnitOfWork _uow = null;
        public ModeReglementTask()
        {
            _uow = IoCContainer.Resolve<IUnitOfWork>();
        }

        public ModeReglementDM getModeReglementDMByCode(string _code)
        {
            return _uow.Repos<ModeReglement>()
                .GetAll().Where(a => a.Code.ToUpper() == _code.ToUpper()  && a.Affichable == 1)
                .Select(a => new ModeReglementDM()
                {
                    Identifiant = a.Identifiant,
                    Code = a.Code,
                    EnActivite = a.EnActivite,
                    Libelle = a.Libelle,
                }).FirstOrDefault();
        }

        public ICollection<ModeReglementDM> getModeReglementDMs()
        {
            return _uow.Repos<ModeReglement>()
                .GetAll().Where(a => a.EnActivite == 1 && a.Affichable == 1)
                .Select(a => new ModeReglementDM()
                {
                    Identifiant = a.Identifiant,
                    Code = a.Code,
                    Libelle = a.Libelle,
                    EnActivite = a.EnActivite,
                }).ToList();
        }

       }
}
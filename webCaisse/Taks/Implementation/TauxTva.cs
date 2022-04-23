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
    public class TauxTvaTask : ITauxTvaTask
    {
        private IUnitOfWork _uow = null;
        public TauxTvaTask()
        {
            _uow = IoCContainer.Resolve<IUnitOfWork>();
        }

        public TauxTva getTypeUniteByTaux(double? _taux)
        {
            return _uow.Repos<TauxTva>()
                 //.GetAll().Where(a => a.Code.ToUpper() == _code.ToUpper()  && a.Affichable == 1)
                 .GetAll().Where(a => a.Taux == _taux )
                .FirstOrDefault();
        }

       

      /*  public ICollection<TypeUniteDM> getGroupesOfUtilisateur(long _idUtilisateur)
        {
            return _uow.Repos<Appartenance>()
                .GetAll().Where(a => a.IdUtilisateur == _idUtilisateur && a.Affichable == 1 && a.EnActivite == 1)
                .Select(a => new GroupeDM()
                {
                    Identifiant = (Int64)a.IdGroupe,
                    Code = a.Groupe.Code,
                    Libelle = a.Groupe.Libelle,
                    EnActivite = a.EnActivite,
                }).ToList();
        }*/
    }
}
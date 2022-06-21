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
    public class TypeUniteTask : ITypeUniteTask
    {
        private IUnitOfWork _uow = null;
        public TypeUniteTask()
        {
            _uow = IoCContainer.Resolve<IUnitOfWork>();
        }

        public TypeUniteDM getTypeUniteDMByCode(string _code)
        {
            return _uow.Repos<TypeUnite>()
                 //.GetAll().Where(a => a.Code.ToUpper() == _code.ToUpper()  && a.Affichable == 1)
                 .GetAll().Where(a => a.Code.ToUpper() == _code.ToUpper() )
                .Select(a => new TypeUniteDM()
                {
                    Identifiant = a.Identifiant,
                    Code = a.Code,
                    //EnActivite = a.EnActivite,
                    Libelle = a.Libelle,
                }).FirstOrDefault();
        }

        public ICollection<TypeUniteDM> getTypeUniteDMs()
        {
            return _uow.Repos<TypeUnite>()
                //.GetAll().Where(a => a.EnActivite == 1 && a.Affichable == 1)
                .GetAll()
                .Select(a => new TypeUniteDM()
                {
                    Identifiant = a.Identifiant,
                    Code = a.Code,
                    Libelle = a.Libelle,
                    //EnActivite = a.EnActivite,
                }).ToList();
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
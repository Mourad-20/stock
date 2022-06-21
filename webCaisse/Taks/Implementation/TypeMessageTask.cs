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
    public class TypeMessageTask : ITypeMessageTask
    {
        private IUnitOfWork _uow = null;
        public TypeMessageTask()
        {
            _uow = IoCContainer.Resolve<IUnitOfWork>();
        }

        public TypeMessageDM getTypeMessageDMByCode(string _code)
        {
            return _uow.Repos<TypeMessage>()
                 //.GetAll().Where(a => a.Code.ToUpper() == _code.ToUpper()  && a.Affichable == 1)
                 .GetAll().Where(a => a.Code.ToUpper() == _code.ToUpper() )
                .Select(a => new TypeMessageDM()
                {
                    Identifiant = a.Identifiant,
                    Code = a.Code,
                    //EnActivite = a.EnActivite,
                    Libelle = a.Libelle,
                }).FirstOrDefault();
        }

        public ICollection<TypeMessageDM> getTypeMessageDMs()
        {
            return _uow.Repos<TypeMessage>()
                //.GetAll().Where(a => a.EnActivite == 1 && a.Affichable == 1)
                .GetAll()
                .Select(a => new TypeMessageDM()
                {
                    Identifiant = a.Identifiant,
                    Code = a.Code,
                    Libelle = a.Libelle,
                    //EnActivite = a.EnActivite,
                }).ToList();
        }

      /*  public ICollection<TypeMessageDM> getGroupesOfUtilisateur(long _idUtilisateur)
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
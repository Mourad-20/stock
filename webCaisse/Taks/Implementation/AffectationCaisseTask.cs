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
    public class AffectationCaisseTask : IAffectationCaisseTask
    {
        private IUnitOfWork _uow = null;
        public AffectationCaisseTask()
        {
            _uow = IoCContainer.Resolve<IUnitOfWork>();
        }

        public long? addAffectationCaisseDM(AffectationCaisseDM _affectationCaisseDM)
        {
            AffectationCaisse _obj = _uow.Repos<AffectationCaisse>().Create();
            _obj.Affichable = 1;
            _obj.EnActivite = 1;
            _obj.IdUtilisateur = _affectationCaisseDM.IdUtilisateur;
            _obj.IdCaisse = _affectationCaisseDM.IdCaisse;
            _uow.Repos<AffectationCaisse>().Insert(_obj);
            _uow.saveChanges();
            return _obj.Identifiant;
        }
       

        public ICollection<AffectationCaisseDM> getAffectationCaisseDMs([Optional]Int64? _idCaisse, [Optional]Int64? _enActivite)
        {
            bool _shouldGetResult = (_idCaisse != null) || (_enActivite != null);
            ICollection<AffectationCaisseDM> _result = _uow.Repos<AffectationCaisse>().GetAll()
                .Where(
                    a =>
                    (_shouldGetResult)
                    && (a.Affichable == 1)
                    && ((_idCaisse != null) ? a.IdCaisse == _idCaisse : true)
                    && ((_enActivite != null) ? a.EnActivite == _enActivite : true)
                    ).Select(
                        o => new AffectationCaisseDM()
                        {
                            Identifiant = o.Identifiant,
                            NomPrenom = (o.Utilisateur != null) ? o.Utilisateur.Nom + " " + o.Utilisateur.Prenom : "",
                            IdUtilisateur = o.IdUtilisateur,
                            IdCaisse = o.IdCaisse,

                        }
                    ).ToList();
            return _result;
        }
       

        public void removeAffectationCaisseDM(long _identifiant)
        {
            AffectationCaisse _obj = _uow.Repos<AffectationCaisse>().GetAll().Where(a => a.Identifiant == _identifiant && a.EnActivite == 1 && a.Affichable == 1).FirstOrDefault();
            _uow.Repos<AffectationCaisse>().DeleteObject(_obj);
            _uow.saveChanges();
        }

        public void removeAffectationCaisseeDM(long _identifiant)
        {
            throw new NotImplementedException();
        }
    }
}
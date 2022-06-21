using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using webCaisse.Db.Entities;
using webCaisse.DMs;
using webCaisse.DMs.Codes;
using webCaisse.Taks.Contracts;
using webCaisse.Tools;
using webCaisse.uows;
using webCaisse.Mappers;
using System.Runtime.InteropServices;

namespace webCaisse.Taks.Implementation
{
    public class LocaliteTask : ILocaliteTask
    {
        private IUnitOfWork _uow = null;
        public LocaliteTask()
        {
            _uow = IoCContainer.Resolve<IUnitOfWork>();
        }
        private Localite getLocaliteById(long _identifiant)
        {
            return _uow.Repos<Localite>().GetAll().Where(a => a.Identifiant == _identifiant).FirstOrDefault();
        }
        public long? addLocaliteDM(LocaliteDM _localiteDM)
        {
            Localite localite=this.getLocalitebyLibelle(_localiteDM.Libelle);
            if (localite!=null && localite.Identifiant!=0)
            {
                throw new Exception("Localite deja existe");
               
            }
            Localite _obj = _uow.Repos<Localite>().Create();
            _obj.Libelle = _localiteDM.Libelle.ToUpper();
            _obj.IdEtatLocalite = _localiteDM.IdEtatLocalite;
            _obj.EnActivite = _localiteDM.EnActivite;
            _obj.Code = _localiteDM.Code;
            _obj.Affichable = 1;

            _obj.Numero = _localiteDM.Numero;
            _obj.Adresse1 = _localiteDM.Adresse1;
            _obj.Adresse2 = _localiteDM.Adresse2;
            _obj.Tel1 = _localiteDM.Tel1;
            _obj.Tel2 = _localiteDM.Tel2;
            _obj.RC = _localiteDM.RC;
            _obj.IdUtilisateur = _localiteDM.IdUtilisateur;
            _uow.Repos<Localite>().Insert(_obj);
            _uow.saveChanges();
            return _obj.Identifiant;
        }
       public Localite getLocalitebyLibelle(String _libelle)
        {
           
            return _uow.Repos<Localite>().GetAll().Where(a => (a.Libelle.ToUpper() == _libelle.ToUpper())).FirstOrDefault();
        }
        public void changerEtatLocalite(long _idLocalite, string _codeEtat)
        {
            IEtatLocaliteTask _etatLocaliteTask = IoCContainer.Resolve<IEtatLocaliteTask>();
            EtatLocaliteDM _etatLocaliteDM = _etatLocaliteTask.getEtatLocaliteDMByCode(_codeEtat);
            Localite _localite = _uow.Repos<Localite>().GetAll()
                .Where(a => a.Identifiant == _idLocalite && a.Affichable == 1).FirstOrDefault();
            _localite.IdEtatLocalite = _etatLocaliteDM.Identifiant;
            _uow.Repos<Localite>().Update(_localite);
            _uow.saveChanges();
        }

        public ICollection<LocaliteDM> getLocalites([Optional] Int64? _enActivite)
        {
            ICollection<LocaliteDM> _articleDMs = new List<LocaliteDM>();
            bool _shouldGetResult = (_enActivite != null);
            _shouldGetResult = (_shouldGetResult != false) ? _shouldGetResult : true;
            ICollection<Localite> _result = _uow.Repos<Localite>().GetAll()
                .Where(
                    a =>
                    (_shouldGetResult)
                    && (a.Affichable == 1)
                    && ((_enActivite != null) ? a.EnActivite == _enActivite : true)
                    ).ToList();

            foreach (Localite a in _result)
            {
                LocaliteDM _localiteDM = LocaliteMapper.LocalitetoLocaliteDM(a);
                _articleDMs.Add(_localiteDM);
            }
            return _articleDMs;
         
        }

        public ICollection<LocaliteDM> getLocalitesDisponible()
        {
            IEtatLocaliteTask _etatLocaliteTask = IoCContainer.Resolve<IEtatLocaliteTask>();
            EtatLocaliteDM _etatLocaliteDM = _etatLocaliteTask.getEtatLocaliteDMByCode(EtatLocaliteCode.DISPONIBLE);
            return _uow.Repos<Localite>()
                   .GetAll().Where(a => a.EnActivite == 1 && a.Affichable == 1 && a.IdEtatLocalite == _etatLocaliteDM.Identifiant)
                   .Select(a => new LocaliteDM()
                   {
                       Identifiant = a.Identifiant,
                       Libelle = a.Libelle,
                       EnActivite = a.EnActivite,
                       Code = a.Code
                   }).ToList();
        }
        public void modifierLocaliteDM(LocaliteDM _localiteDM)
        {
            if (_localiteDM.Identifiant != 0)

            {
                Localite localite = this.getLocalitebyLibelle(_localiteDM.Libelle);
                if (localite != null && localite.Identifiant!= _localiteDM.Identifiant)
                {
                    throw new Exception("Societe deja existe");

                }
                Localite _localite = getLocaliteById(_localiteDM.Identifiant);
                _localite.Libelle = _localiteDM.Libelle.ToUpper();
                _localite.IdEtatLocalite = _localiteDM.IdEtatLocalite;
                _localite.EnActivite = _localiteDM.EnActivite;
                _localite.Code = _localiteDM.Code;
                _localite.Affichable = 1;

                _localite.Numero = _localiteDM.Numero;
                _localite.Adresse1 = _localiteDM.Adresse1;
                _localite.Adresse2 = _localiteDM.Adresse2;
                _localite.Tel1 = _localiteDM.Tel1;
                _localite.Tel2 = _localiteDM.Tel2;
                _localite.RC = _localiteDM.RC;


                _uow.Repos<Localite>().Update(_localite);
                _uow.saveChanges();
            }
        }

    }
}
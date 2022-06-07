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
    public class InterlocuteurTask : IInterlocuteurTask
    {
        private IUnitOfWork _uow = null;
        public InterlocuteurTask()
        {
            _uow = IoCContainer.Resolve<IUnitOfWork>();
        }
        private Interlocuteur getInterlocuteurById(long _identifiant)
        {
            return _uow.Repos<Interlocuteur>().GetAll().Where(a => a.Identifiant == _identifiant).FirstOrDefault();
        }
        public long? addInterlocuteurDM(InterlocuteurDM _InterlocuteurDM)
        {
        
            Interlocuteur _obj = _uow.Repos<Interlocuteur>().Create();

            _obj.Nom = _InterlocuteurDM.Nom.ToUpper();
            _obj.Tel1 = _InterlocuteurDM.Tel1;
            _obj.Tel2 = _InterlocuteurDM.Tel2;
            _obj.Email1 = _InterlocuteurDM.Email1;
            _obj.Email2 = _InterlocuteurDM.Email2;
            _obj.Affichable = _InterlocuteurDM.Affichable;

            _obj.Fonction = _InterlocuteurDM.Fonction;
            _obj.Fix = _InterlocuteurDM.Fix;
            _uow.Repos<Interlocuteur>().Insert(_obj);
            _uow.saveChanges();
            return _obj.Identifiant;
        }
        public Interlocuteur getInterlocuteurbyName(String _name)
        {

            return _uow.Repos<Interlocuteur>().GetAll().Where(a => (a.Nom.ToUpper() == _name.ToUpper())).FirstOrDefault();
        }
  
        public ICollection<InterlocuteurDM> getInterlocuteurs([Optional] Int64? _enActivite)
        {
            ICollection<InterlocuteurDM> _articleDMs = new List<InterlocuteurDM>();
            bool _shouldGetResult = (_enActivite != null);
            _shouldGetResult = (_shouldGetResult != false) ? _shouldGetResult : true;
            ICollection<Interlocuteur> _result = _uow.Repos<Interlocuteur>().GetAll()
                .Where(
                    a =>
                    (_shouldGetResult)
                    && (a.Affichable == 1)
                    && ((_enActivite != null) ? a.EnActivite == _enActivite : true)
                    ).ToList();

            foreach (Interlocuteur a in _result)
            {
                InterlocuteurDM _InterlocuteurDM = InterlocuteurMapper.InterlocuteurtoInterlocuteurDM(a);
                _articleDMs.Add(_InterlocuteurDM);
            }
            return _articleDMs;

        }

        public void modifierInterlocuteurDM(InterlocuteurDM _InterlocuteurDM)
        {
            if (_InterlocuteurDM.Identifiant != 0)

            {
            
                Interlocuteur _Interlocuteur = getInterlocuteurById(_InterlocuteurDM.Identifiant);
                _Interlocuteur.Nom = _InterlocuteurDM.Nom.ToUpper();
                _Interlocuteur.Tel1 = _InterlocuteurDM.Tel1;
                _Interlocuteur.Tel2 = _InterlocuteurDM.Tel2;
                _Interlocuteur.Email1 = _InterlocuteurDM.Email1;
                _Interlocuteur.Email2 = _InterlocuteurDM.Email2;
                _Interlocuteur.Affichable = _InterlocuteurDM.Affichable;

                _Interlocuteur.Fonction = _InterlocuteurDM.Fonction;
                _Interlocuteur.Fix = _InterlocuteurDM.Fix;
              _uow.Repos<Interlocuteur>().Update(_Interlocuteur);
                _uow.saveChanges();
            }
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using webCaisse.Db.Entities;
using webCaisse.DMs;
using webCaisse.Mappers;
using webCaisse.Taks.Contracts;
using webCaisse.Tools;
using webCaisse.uows;

namespace webCaisse.Taks.Implementation
{
    public class DetailReglementTask : IDetailReglementTask
    {
        private IUnitOfWork _uow = null;
        public DetailReglementTask()
        {
            _uow = IoCContainer.Resolve<IUnitOfWork>();
        }

        
        public long? addDetailReglementDM(DetailReglementDM _detailReglementDM)
        {
            DetailReglement _detailReglement = _uow.Repos<DetailReglement>().Create();
            _detailReglement.Affichable = 1;
            _detailReglement.EnActivite = _detailReglementDM.EnActivite;
            _detailReglement.IdDetailCommande = _detailReglementDM.IdDetailCommande;
            _detailReglement.IdReglement = _detailReglementDM.IdReglement;
            _detailReglement.Montant = _detailReglementDM.Montant;
            _detailReglement.Quantite = _detailReglementDM.Quantite;
            _uow.Repos<DetailReglement>().Insert(_detailReglement);
            _uow.saveChanges();
            return _detailReglement.Identifiant;
        }

        
        public ICollection<DetailReglementDM> getDetailReglementsDMByIdReglement(long? _idReglement)
        {
            ICollection<DetailReglementDM> _detailReglementDMs = new List<DetailReglementDM>();
            ICollection<DetailReglement> _detailReglement = _uow.Repos<DetailReglement>()

                            .GetAll().Where(a => a.IdReglement == _idReglement && a.EnActivite == 1 && a.Affichable == 1).ToList();
                           foreach (DetailReglement a in _detailReglement)
            {
                DetailReglementDM _detailreglementDM = DetailReglementMapper.DetailReglementtoDetailReglementDM(a);
                _detailReglementDMs.Add(_detailreglementDM);
            }
            return _detailReglementDMs;
        }
        

        public void removeDetailReglementDM(long _identifiant)
        {
            DetailReglement _obj = _uow.Repos<DetailReglement>().GetAll().Where(a => a.Identifiant == _identifiant && a.EnActivite == 1 && a.Affichable == 1).FirstOrDefault();
            _uow.Repos<DetailReglement>().DeleteObject(_obj);
            _uow.saveChanges();
        }
        
        private DetailReglement getDetailReglementById(Int64 _identifiant) {
            return _uow.Repos<DetailReglement>().GetAll().Where(a => a.Identifiant == _identifiant).FirstOrDefault();
        }
    }
}
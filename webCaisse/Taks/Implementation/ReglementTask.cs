using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using webCaisse.Db.Entities;
using webCaisse.DMs;
using webCaisse.DMs.Codes;
using webCaisse.Mappers;
using webCaisse.Taks.Contracts;
using webCaisse.Tools;
using webCaisse.uows;

namespace webCaisse.Taks.Implementation
{
    public class ReglementTask : IReglementTask
    {
        private IUnitOfWork _uow = null;
        public ReglementTask()
        {
            _uow = IoCContainer.Resolve<IUnitOfWork>();
        }

        
        private Double? getTotalMontantReglement(ReglementDM _reglementDM) {
            double? _mntTotal = 0;
            if (_reglementDM != null && _reglementDM.DetailReglementDMs != null) {
                foreach (DetailReglementDM _detailReglementDM in _reglementDM.DetailReglementDMs) {
                    _mntTotal = _mntTotal + (_detailReglementDM.Montant * _detailReglementDM.Quantite);
                }
            }

            return _mntTotal;
        }

        public String genererNumeroReglement() {
            String _numero = "";
            DateTime _now = DateTime.Now;
            String _lastNumero = _uow.Repos<Reglement>()
                .GetAll().Where(a => a.DateReglement.Value.Year == _now.Year && a.DateReglement.Value.Month == _now.Month && a.DateReglement.Value.Day == _now.Day && a.EnActivite == 1 && a.Affichable == 1).OrderByDescending(a=>a.Identifiant).Select(a => a.Numero).FirstOrDefault();
            if (_lastNumero == null)
            {
                _numero = "1";
            }
            else {
                _numero = "" + (Int64.Parse(_lastNumero) + 1);
            }
            return _numero;
        }

  
        public ReglementDM getReglementDMById(long? _identifiant)
        {
            ReglementDM _reglementDM = new ReglementDM();
           Reglement _reglements = _uow.Repos<Reglement>()
                           .GetAll().Where(a => a.Identifiant == _identifiant && a.EnActivite == 1 && a.Affichable == 1).FirstOrDefault();
       
                 _reglementDM = ReglementMapper.ReglementtoReglementDM(_reglements);
              
            
            return _reglementDM;
        
        //.Select(a => ReglementMapper.ReglementtoReglementDM(a)).FirstOrDefault();
    }
        
        public ICollection<ReglementDM> getReglementDMsByIdCommande(long? _idCommande)
        {
            ICollection<ReglementDM> _reglementDMs = new List<ReglementDM>();
            ICollection <Reglement>_reglements=_uow.Repos<Reglement>()
                           .GetAll().Where(a => a.IdCommande == _idCommande && a.EnActivite == 1 && a.Affichable == 1)
                           .ToList();
            foreach (Reglement a in _reglements)
            {
                ReglementDM _reglementDM = ReglementMapper.ReglementtoReglementDM(a);
                _reglementDMs.Add(_reglementDM);
            }
            return _reglementDMs;
        }

        public long? etablirReglement(ReglementDM _reglementDM)
        {
            ICommandeTask _commandeTask = IoCContainer.Resolve<CommandeTask>();
            ILocaliteTask _localiteTask = IoCContainer.Resolve<ILocaliteTask>();
            Reglement _reglement = _uow.Repos<Reglement>().Create();
            _reglement.Affichable = 1;
            _reglement.EnActivite = 1;
            _reglement.IdCreePar = _reglementDM.IdCreePar;
            _reglement.DateReglement = DateTime.Now;
            _reglement.IdSeance = _reglementDM.IdSeance;
            _reglement.IdModeReglement = _reglementDM.IdModeReglement;
            _reglement.IdCommande = _reglementDM.IdCommande;
            _reglement.IdCreePar = _reglementDM.IdCreePar;
            _reglement.Ncheque = _reglementDM.Ncheque;
            _reglement.NCompte = _reglementDM.NCompte;
            _reglement.NomBanque = _reglementDM.NomBanque;
            if (_reglementDM.Datecheque!="")
            {
            _reglement.Datecheque = DateTime.Parse(_reglementDM.Datecheque);
            }
           
            _reglement.Montant = _reglementDM.Montant;
            _reglement.Numero = genererNumeroReglement();
            _uow.Repos<Reglement>().Insert(_reglement);
            _uow.saveChanges();

            if (_commandeTask.isCommandeReglee((Int64)_reglementDM.IdCommande) == true)
                {
                    _commandeTask.changerEtatCommande((Int64)_reglementDM.IdCommande, EtatCommandeCode.REGLEE);
                    /*CommandeDM _commandeDM = _commandeTask.getCommandeDMById(_reglementDM.IdCommande);
                    if (_commandeDM.IdLocalite != null) {
                        _localiteTask.changerEtatLocalite((long)_commandeDM.IdLocalite, EtatLocaliteCode.DISPONIBLE);
                    }*/
                }
                else
                {
                    _commandeTask.changerEtatCommande((Int64)_reglementDM.IdCommande, EtatCommandeCode.REGLEE_PARTIELLEMENT);
                }
                       

            return _reglement.Identifiant;
        }

        public ICollection<ReglementDM> getReglementDMs([Optional] long? _idSeance)
        {
            bool _shouldGetResult = (_idSeance != null);
            ICollection<ReglementDM> _reglementDMs = new List<ReglementDM>();

            ICollection<Reglement> _result = _uow.Repos<Reglement>().GetAll()
                .Where(
                    a =>
                    (_shouldGetResult)
                    && (a.Affichable == 1)
                    && (a.EnActivite == 1)
                    && ((_idSeance != null) ? a.IdSeance == _idSeance : true)
                    ).ToList();
            foreach (Reglement a in _result)
            {
                ReglementDM _reglementDM = ReglementMapper.ReglementtoReglementDM(a);
                _reglementDMs.Add(_reglementDM);
            }
            return _reglementDMs;
        }
    }
}
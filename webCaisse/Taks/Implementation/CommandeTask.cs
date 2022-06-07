using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Hosting;
using webCaisse.Db.Entities;
using webCaisse.DMs;
using webCaisse.DMs.Codes;
using webCaisse.Reports.DMs;
using webCaisse.Taks.Contracts;
using webCaisse.Tools;
using webCaisse.uows;

namespace webCaisse.Taks.Implementation
{
    public class CommandeTask : ICommandeTask
    {
        private IUnitOfWork _uow = null;
        public CommandeTask()
        {
            _uow = IoCContainer.Resolve<IUnitOfWork>();
        }

        private void addCommande(Commande obj)
        {
            //ok
            _uow.Repos<Commande>().Insert(obj);
            _uow.saveChanges();
        }

        private Commande getCommandeById(long _identifiant)
        {
            //ok
            return _uow.Repos<Commande>().GetAll().Where(a => a.Identifiant == _identifiant).FirstOrDefault();
        }

        //=#########################################################################
        //=#########################################################################
        public Int64? allimentationstock(CommandeDM _commandeDM)
        {
            IEtatCommandeTask _etatCommandeTask = IoCContainer.Resolve<IEtatCommandeTask>();
            ILocaliteTask _localiteTask = IoCContainer.Resolve<ILocaliteTask>();
            EtatCommandeDM _etatCommandeDM = _etatCommandeTask.getEtatCommandeDMByCode(EtatCommandeCode.NON_REGLEE);
            Commande _commande = _uow.Repos<Commande>().Create();
           


            _commande.Affichable = 1;
            _commande.EnActivite = 1;
            
            _commande.IdCreePar = _commandeDM.IdCreePar;
          //  _commande.IdServeur = _commandeDM.IdServeur;
            _commande.IdEtatCommande = _etatCommandeDM.Identifiant;
            _commande.IdLocalite = _commandeDM.IdLocalite;
            _commande.IdSeance = _commandeDM.IdSeance;
            _commande.Montant = getTotalMontantCommande(_commandeDM);
            _commande.Numero = genererNumeroCommande();
            _commande.CodeCommande = _commandeDM.CodeCommande;
            _commande.DateCommande = DateTime.Now;
            _uow.Repos<Commande>().Insert(_commande);
            _uow.saveChanges();

            if (_commandeDM.DetailCommandeDMs != null)
            {
                IDetailCommandeTask _detailCommandeTask = IoCContainer.Resolve<IDetailCommandeTask>();
                foreach (DetailCommandeDM _detailCommandeDM in _commandeDM.DetailCommandeDMs)
                {
                    _detailCommandeDM.IdCommande = _commande.Identifiant;
                    _detailCommandeTask.addDetailCommandeDM(_detailCommandeDM);
                }
            }

            if (_commande.IdLocalite != null && _commande.IdLocalite > 0)
            {
               // _localiteTask.changerEtatLocalite((long)_commande.IdLocalite, EtatLocaliteCode.OCCUPEE);
            }

            return _commande.Identifiant;
        }


        public Int64? etablirCommande(CommandeDM _commandeDM)
        {
            IEtatCommandeTask _etatCommandeTask = IoCContainer.Resolve<IEtatCommandeTask>();
            ILocaliteTask _localiteTask = IoCContainer.Resolve<ILocaliteTask>();
            EtatCommandeDM _etatCommandeDM = _etatCommandeTask.getEtatCommandeDMByCode(EtatCommandeCode.NON_REGLEE);
            Commande _commande = _uow.Repos<Commande>().Create();
            _commande.Affichable = 1;
            _commande.EnActivite = 1;
            _commande.IdCreePar = _commandeDM.IdCreePar;
            _commande.IdServeur = _commandeDM.IdServeur;
            _commande.IdEtatCommande = _etatCommandeDM.Identifiant;
            _commande.IdLocalite = _commandeDM.IdLocalite;
            _commande.IdSeance = _commandeDM.IdSeance;
            _commande.Montant = getTotalMontantCommande(_commandeDM);
            _commande.Numero = genererNumeroCommande();
            _commande.CodeCommande = _commandeDM.CodeCommande;
            _commande.DateCommande = DateTime.Now;
            
            _uow.Repos<Commande>().Insert(_commande);
            _uow.saveChanges();

            if (_commandeDM.DetailCommandeDMs != null) {
                IDetailCommandeTask _detailCommandeTask = IoCContainer.Resolve<IDetailCommandeTask>();
                foreach (DetailCommandeDM _detailCommandeDM in _commandeDM.DetailCommandeDMs) {
                    _detailCommandeDM.IdCommande = _commande.Identifiant;
                    _detailCommandeTask.addDetailCommandeDM(_detailCommandeDM);
                    _detailCommandeTask.actualiserQuantiteServi(_detailCommandeDM);
                }
            }

            if (_commande.IdLocalite != null && _commande.IdLocalite > 0) {
                _localiteTask.changerEtatLocalite((long)_commande.IdLocalite, EtatLocaliteCode.OCCUPEE);
            }

            return _commande.Identifiant;
        }

        private Double? getTotalMontantCommande(CommandeDM _commandeDM) {
            double? _mntTotal = 0;
            if (_commandeDM != null && _commandeDM.DetailCommandeDMs != null) {
                foreach (DetailCommandeDM _detailCommandeDM in _commandeDM.DetailCommandeDMs) {
                    _mntTotal = _mntTotal + (_detailCommandeDM.Montant * _detailCommandeDM.Quantite);
                }
            }

            return _mntTotal;
        }

        public String genererNumeroCommande() {
            String _numero = "";
            DateTime _now = DateTime.Now;
            String _lastNumero = _uow.Repos<Commande>()
                .GetAll().Where(a => a.DateCommande.Value.Year == _now.Year && a.DateCommande.Value.Month == _now.Month && a.DateCommande.Value.Day == _now.Day && a.EnActivite == 1 && a.Affichable == 1).OrderByDescending(a=>a.Identifiant).Select(a => a.Numero).FirstOrDefault();
            if (_lastNumero == null)
            {
                _numero = "1";
            }
            else {
                _numero = "" + (Int64.Parse(_lastNumero) + 1);
            }
            return _numero;
        }

        public CommandeDM getCommandeDMById(long? _identifiant)
        {
            return _uow.Repos<Commande>()
                            .GetAll().Where(a => a.Identifiant == _identifiant)
                            .Select(o=> new CommandeDM() {
                                Identifiant = o.Identifiant,
                                DateCommande = o.DateCommande,
                                Montant = o.Montant,
                                IdCreePar = o.IdCreePar,
                                IdEtatCommande = o.IdEtatCommande,
                                IdLocalite = o.IdLocalite,
                                IdSeance = o.IdSeance,
                                Numero = o.Numero,
                                EnActivite = o.EnActivite,
                                CodeCommande = o.CodeCommande,
                                LibelleEtatCommande = (o.EtatCommande != null) ? o.EtatCommande.Libelle : "",
                                CodeEtatCommande = (o.EtatCommande != null) ? o.EtatCommande.Code : "",
                                LibelleLocalite = (o.Localite != null) ? o.Localite.Libelle : "",
                                NumeroSeance = (o.Seance != null) ? o.Seance.Numero : "",
                                NomServeur = (o.Serveur != null) ? o.Serveur.Nom : "",
                                IdServeur = o.IdServeur,
                            }).FirstOrDefault();
        }

        
        public Int64? validerCommande(CommandeDM _commandeDM)
        {
            ILocaliteTask _localiteTask = IoCContainer.Resolve<ILocaliteTask>();
            Commande _commande = _uow.Repos<Commande>().GetAll().Where(a => a.Identifiant == _commandeDM.Identifiant && a.EnActivite == 1 && a.Affichable == 1).FirstOrDefault();
            Int64? _oldIdLocalite = _commande.IdLocalite;
            _commande.IdCreePar = _commandeDM.IdCreePar;
            _commande.IdServeur = _commandeDM.IdServeur;
            //_commande.IdEtatCommande = _commandeDM.IdEtatCommande;
            _commande.IdLocalite = _commandeDM.IdLocalite;
            _commande.IdSeance = _commandeDM.IdSeance;
            _commande.CodeCommande = _commandeDM.CodeCommande;
            _commande.Montant = getTotalMontantCommande(_commandeDM);
            _uow.Repos<Commande>().Update(_commande);
            _uow.saveChanges();

            if (_commandeDM.DetailCommandeDMs != null)
            {
                IDetailCommandeTask _detailCommandeTask = IoCContainer.Resolve<IDetailCommandeTask>();
                ICollection<DetailCommandeDM> _detailCommadeDMsOrg = _detailCommandeTask.getDetailCommandesDMByIdCommande(_commande.Identifiant);
                
             
            }

            if (_commande.IdLocalite != null && _commande.IdLocalite > 0)
            {
                if (_oldIdLocalite != null)
                {
                    _localiteTask.changerEtatLocalite((long)_oldIdLocalite, EtatLocaliteCode.DISPONIBLE);
                }
                _localiteTask.changerEtatLocalite((long)_commande.IdLocalite, EtatLocaliteCode.OCCUPEE);
            }


            return _commande.Identifiant;
        }

        public Int64? modifierCommande(CommandeDM _commandeDM)
        {
            ILocaliteTask _localiteTask = IoCContainer.Resolve<ILocaliteTask>();
            Commande _commande = _uow.Repos<Commande>().GetAll().Where(a => a.Identifiant == _commandeDM.Identifiant && a.EnActivite == 1 && a.Affichable == 1).FirstOrDefault();
            Int64? _oldIdLocalite = _commande.IdLocalite;
           _commande.IdCreePar = _commandeDM.IdCreePar;
            if (_commandeDM.IdServeur!=null)
            {
                _commande.IdServeur = _commandeDM.IdServeur;
            }
            //_commande.IdEtatCommande = _commandeDM.IdEtatCommande;
            _commande.IdLocalite = _commandeDM.IdLocalite;
            _commande.IdSeance = _commandeDM.IdSeance;
            _commande.Montant = getTotalMontantCommande(_commandeDM);
            _uow.Repos<Commande>().Update(_commande);
            _uow.saveChanges();
            if (_commandeDM.DetailCommandeDMs != null)
            {
                IDetailCommandeTask _detailCommandeTask = IoCContainer.Resolve<IDetailCommandeTask>();
                ICollection<DetailCommandeDM> _detailCommadeDMsOrg = _detailCommandeTask.getDetailCommandesDMByIdCommande(_commande.Identifiant);

                foreach (DetailCommandeDM _detailCommandeDM in _commandeDM.DetailCommandeDMs)
                {
                    //if (_detailCommadeDMsOrg.Where(a => a.IdArticle == _detailCommandeDM.IdArticle).Count() == 0)
                    if (_detailCommadeDMsOrg.Where(a => a.Identifiant == _detailCommandeDM.Identifiant).Count() == 0)
                    {
                        _detailCommandeDM.IdCommande = _commande.Identifiant;
                        _detailCommandeDM.QuantiteServi = 0;
                        _detailCommandeTask.addDetailCommandeDM(_detailCommandeDM);
                    }
                    else
                    {
                        _detailCommandeTask.updateDetailCommandeDM(_detailCommandeDM);
                    }
                }

                foreach (DetailCommandeDM _detailCommandeDM in _detailCommadeDMsOrg)
                {
                    if (_commandeDM.DetailCommandeDMs.Where(a => a.Identifiant == _detailCommandeDM.Identifiant).Count() == 0)
                    {
                        IAffectationMessageTask _affectationMessageTask = IoCContainer.Resolve<IAffectationMessageTask>();
                        ICollection<AffectationMessageDM> _affectationMessageDMs = _affectationMessageTask.getAffectationMessageDMsByIdDetailCommande(_detailCommandeDM.Identifiant);
                        if (_affectationMessageDMs != null) {
                            foreach (AffectationMessageDM _am in _affectationMessageDMs) {
                                _affectationMessageTask.removeAffectationMessageDM(_am.Identifiant);
                            }
                        }
                        _detailCommandeTask.removeDetailCommandeDM(_detailCommandeDM.Identifiant);
                    }
                }
            }

            if (_commande.IdLocalite != null && _commande.IdLocalite > 0)
            {
                if (_oldIdLocalite != null) {
                    _localiteTask.changerEtatLocalite((long)_oldIdLocalite, EtatLocaliteCode.DISPONIBLE);
                }                
                _localiteTask.changerEtatLocalite((long)_commande.IdLocalite, EtatLocaliteCode.OCCUPEE);
            }


            return _commande.Identifiant;
        }

        public ICollection<CommandeDM> getCommandeDMsNonRegle(long? _idSeance)
        {
            return _uow.Repos<Commande>().GetAll()
                .Where(a => a.EtatCommande.Code != EtatCommandeCode.REGLEE && a.Affichable == 1)
                .Select(o => new CommandeDM() {
                    Identifiant = o.Identifiant,
                    DateCommande = o.DateCommande,
                    Montant = o.Montant,
                    IdCreePar = o.IdCreePar,
                    IdEtatCommande = o.IdEtatCommande,
                    IdLocalite = o.IdLocalite,
                    IdSeance = o.IdSeance,
                    Numero = o.Numero,
                    EnActivite = o.EnActivite,
                    CodeCommande = o.CodeCommande,
                     CodeEtatCommande = (o.EtatCommande != null) ? o.EtatCommande.Code : "",
                    LibelleEtatCommande = (o.EtatCommande != null) ? o.EtatCommande.Libelle : "",
                    LibelleLocalite = (o.Localite != null) ? o.Localite.Libelle : "",
                    NumeroSeance = (o.Seance != null) ? o.Seance.Numero : "",
                    NomServeur = (o.Serveur != null) ? o.Serveur.Nom : "",
                    IdServeur = o.IdServeur,
                }).ToList();
        }


        /*public bool isCommandeReglee(long _idCommande)
        {
            Boolean _reglee = false;

            ICollection<DetailCommande> _detailCommandes = _uow.Repos<DetailCommande>().GetAll()
                .Where(a => a.IdCommande == _idCommande && a.Affichable == 1 && a.EnActivite == 1).ToList();

            Double? _quantiteCommande = _detailCommandes.Sum(a => a.Quantite);

            ICollection<DetailReglement> _detailReglements = _uow.Repos<DetailReglement>().GetAll()
                .Where(a => a.Reglement.IdCommande == _idCommande && a.Affichable == 1 && a.EnActivite == 1).ToList();

            Double? _quantiteReglee = _detailReglements.Sum(a => a.Quantite);

            if (_quantiteCommande == _quantiteReglee)
            {
                _reglee = true;
            }
            else {
                _reglee = false;
            }
            return _reglee;
        }*/
        public bool isCommandeReglee(long _idCommande)
        {
            Boolean _reglee = false;

            Commande _Commande = _uow.Repos<Commande>().GetById(_idCommande);

            Double? _montantcommande = _Commande.Montant;

            ICollection<Reglement> _Reglements = _uow.Repos<Reglement>().GetAll()
                .Where(a => a.IdCommande == _idCommande && a.Affichable == 1 && a.EnActivite == 1).ToList();

            Double? _montantregle = _Reglements.Sum(a => a.Montant);

            if (_montantcommande == _montantregle)
            {
                _reglee = true;
            }
            else
            {
                _reglee = false;
            }
            return _reglee;
        }

        public void changerEtatCommande(Int64 _idCommande,string _codeEtat)
        {
            IEtatCommandeTask _etatCommandeTask = IoCContainer.Resolve<EtatCommandeTask>();
            EtatCommandeDM _etatCommande = _etatCommandeTask.getEtatCommandeDMByCode(_codeEtat);
            Commande _commande = _uow.Repos<Commande>().GetAll()
                .Where(a => a.Identifiant == _idCommande && a.Affichable == 1).FirstOrDefault();
            _commande.IdEtatCommande = _etatCommande.Identifiant;
            _uow.Repos<Commande>().Update(_commande);
            _uow.saveChanges();
        }

        public void envoyerTicketPreparation(long _idCommande)
        {
            //ok
            ICommandeTask _commandeTask = IoCContainer.Resolve<ICommandeTask>();
            IRapportTask _rapportTask = IoCContainer.Resolve<IRapportTask>();
            IImpressionTask _impressionTask = IoCContainer.Resolve<IImpressionTask>();
            IDetailCommandeTask _detailCommandeTask = IoCContainer.Resolve<IDetailCommandeTask>();
            IZoneTask _zoneTask = IoCContainer.Resolve<IZoneTask>();
            CommandeDM _commandeDM = _commandeTask.getCommandeDMById(_idCommande);
            //-----------------------------------------------------


            ICollection<ZoneDM> _zoneDMs = _zoneTask.getZoneDMs();
            ICollection<DetailCommandeDM> _detailCommandeDMs = _detailCommandeTask.getDetailCommandeDMsNonServi(_idCommande);
            if (_detailCommandeDMs != null && _detailCommandeDMs.Count > 0 && _zoneDMs !=null && _zoneDMs.Count > 0) {
                List<Int64?> _idsZone = _detailCommandeDMs.Select(a => a.IdZone).Distinct().ToList();
                if (_idsZone != null && _idsZone.Count > 0) {
                    foreach (Int64? _idZone in _idsZone) {
                        ZoneDM _zoneDM = _zoneDMs.Where(a => a.Identifiant == _idZone).FirstOrDefault();
                        ICollection<DetailCommandeDM> _division = _detailCommandeDMs.Where(a => a.IdZone == _idZone).ToList();
                        ICollection<RptDetailCommande> _rptDetailCommandes = null;
                        if (_division != null && _division.Count > 0) {                            
                            _rptDetailCommandes = new List<RptDetailCommande>();
                            foreach (DetailCommandeDM dc in _division)
                            {
                                _rptDetailCommandes.Add(new RptDetailCommande()
                                {
                                    //LibelleArticle = dc.LibelleArticle + "-Z" + dc.IdZone,
                                    LibelleArticle = dc.LibelleArticle,
                                    Quantite = "x " + (dc.Quantite - dc.QuantiteServi).ToString()
                                });
                            }
                            String _numeroCommande = "Com. N°: " + _commandeDM.Numero;
                            String _nomServeur = "Serveur: " + _commandeDM.NomServeur;
                            String _libelleLocalite = "Localite: " + _commandeDM.LibelleLocalite;
                            String _dateGeneration = "Date: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                            String _libelleZone = "Zone: " + _idZone;
                            Byte[] _content = _rapportTask.genererTicketPreparation(_rptDetailCommandes,_numeroCommande, _dateGeneration,_nomServeur, _libelleLocalite,_libelleZone);
                            _impressionTask.imprimerPreparation(_content, _zoneDM.NomImprimante);
                        }
                    }
                }
                //_detailCommandeTask.actualiserQuantiteServi(_detailCommandeDMs);
            }            
        }

        public ICollection<CommandeDM> getCommandeDMs([Optional]String _code, [Optional]String _numero, [Optional]Int64? _idEtatCommande,
            [Optional]Int64? _idCreePar, [Optional] Int64? _idSeance, [Optional] Int64? _idServeur, [Optional]DateTime? _ddebut,
            [Optional]DateTime? _dfin, [Optional]List<Int64?> _caisseids)
        {
            //ok

            bool _shouldGetResult = (_code!=null&&_code.Length > 0) || (_numero != null && _numero.Length > 0) || (_idEtatCommande != null) || 
                (_idCreePar != null) || (_idSeance != null) || (_idServeur != null) || (_ddebut != null) || 
                (_dfin != null)||(_caisseids!=null);
            if (_caisseids==null)
            {
                _caisseids = new List<long?>();
            }
            ICollection<CommandeDM> _result = _uow.Repos<Commande>().GetAll()
                .Where(
                    a => 
                    (_shouldGetResult)
                    && (a.Affichable == 1)
                    && (a.EnActivite == 1)

                    &&((_caisseids.Count>0)?_caisseids.Contains((int)a.Seance.IdCaisse):true)

                    && ((_idEtatCommande != null) ? a.IdEtatCommande == _idEtatCommande : true)
                    && ((_idCreePar != null) ? a.IdCreePar == _idCreePar : true)
                    && ((_idSeance != null) ? a.IdSeance == _idSeance : true)
                    && ((_idServeur != null) ? a.IdServeur == _idServeur : true)
                     && ((_ddebut != null) ? a.DateCommande >= _ddebut : true)
                     
                    && ((_dfin != null) ? a.DateCommande <= _dfin : true)
                  
                    && ((_code!=null &&_code.Length > 0) ? a.CodeCommande == _code : a.CodeCommande != null)

                    ).Select(
                        o => new CommandeDM()
                        {
                            Identifiant = o.Identifiant,
                            DateCommande = o.DateCommande,
                            Montant = o.Montant,
                            IdCreePar = o.IdCreePar,
                            IdEtatCommande = o.IdEtatCommande,
                            IdLocalite = o.IdLocalite,
                            IdSeance = o.IdSeance,
                            LibelleCaisse=o.Seance.Caisse.Libelle,
                            Numero = o.Numero,
                            EnActivite = o.EnActivite,
                            CodeCommande = o.CodeCommande,
                            CodeEtatCommande = (o.EtatCommande != null) ? o.EtatCommande.Code : "",
                            LibelleEtatCommande = (o.EtatCommande != null) ? o.EtatCommande.Libelle : "",
                            LibelleLocalite = (o.Localite != null) ? o.Localite.Libelle : "",
                            NumeroSeance = (o.Seance != null) ? o.Seance.Numero : "",
                            NomServeur = (o.Serveur != null) ? o.Serveur.Nom : "",
                            IdServeur = o.IdServeur,
                        }
                    ).ToList();
            return _result;
        }

        public void envoyerTicketNote(long _idCommande)
        {
            //ok
            ICommandeTask _commandeTask = IoCContainer.Resolve<ICommandeTask>();
            IRapportTask _rapportTask = IoCContainer.Resolve<IRapportTask>();
            IImpressionTask _impressionTask = IoCContainer.Resolve<IImpressionTask>();
            IDetailCommandeTask _detailCommandeTask = IoCContainer.Resolve<IDetailCommandeTask>();
            IZoneTask _zoneTask = IoCContainer.Resolve<IZoneTask>();
            CommandeDM _commandeDM = _commandeTask.getCommandeDMById(_idCommande);
            //-----------------------------------------------------
            ICollection<RptDetailCommande> _rptDetailCommandes = null;
            ICollection<DetailCommandeDM> _detailCommandeDMs = _detailCommandeTask.getDetailCommandeDMs(_idCommande: _idCommande);
            if (_detailCommandeDMs != null && _detailCommandeDMs.Count > 0)
            {
                _rptDetailCommandes = new List<RptDetailCommande>();
                foreach (DetailCommandeDM dc in _detailCommandeDMs)
                {
                    _rptDetailCommandes.Add(new RptDetailCommande()
                    {
                        //LibelleArticle = dc.LibelleArticle + "-Z" + dc.IdZone,
                        LibelleArticle = dc.LibelleArticle,
                        Quantite = "x " + dc.Quantite,
                        Montant = dc.Montant.ToString(),
                        Total = (dc.Montant * dc.Quantite).ToString()
                    });
                }
                String _total = "Total: " + _commandeDM.Montant + " DH";
                String _numeroCommande = "Com. N°: " + _commandeDM.Numero;
                String _nomServeur = "Serveur: " + _commandeDM.NomServeur;
                String _libelleLocalite = "Localite: " + _commandeDM.LibelleLocalite;
                String _dateGeneration = "Date: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                Byte[] _content = _rapportTask.genererTicketNote(_rptDetailCommandes, _numeroCommande, _dateGeneration, _nomServeur, _libelleLocalite,_total);
                _impressionTask.imprimerNote(_content);

            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using webCaisse.Db.Entities;
using webCaisse.DMs;
using webCaisse.DMs.Codes;
using webCaisse.Taks.Contracts;
using webCaisse.Tools;
using webCaisse.uows;

namespace webCaisse.Taks.Implementation
{
    public class DetailCommandeTask : IDetailCommandeTask
    {
        private IUnitOfWork _uow = null;
        public DetailCommandeTask()
        {
            _uow = IoCContainer.Resolve<IUnitOfWork>();
        }

        private void addDetailCommande(DetailCommande obj)
        {
            //ok
            _uow.Repos<DetailCommande>().Insert(obj);
            _uow.saveChanges();
        }

        private DetailCommande getDetailCommandeById(long _identifiant)
        {
            //ok
            return _uow.Repos<DetailCommande>().GetAll().Where(a => a.Identifiant == _identifiant).FirstOrDefault();
        }

        private void updateDetailCommande(DetailCommande obj)
        {
            //ok
            _uow.Repos<DetailCommande>().Update(obj);
            _uow.saveChanges();
        }
        //=#########################################################################
        //=#########################################################################
        public ICollection<DetailCommandeDM> getDetailCommandesstockDMByIdArticle(Int64? _idArticle)
        {
            bool _shouldGetResult = (_idArticle != null) ;
            ICollection<DetailCommandeDM> _result = _uow.Repos<DetailCommande>().GetAll()
                .Where(
                    a =>
                    //(_shouldGetResult)&&
                     (a.Affichable == 1)
                    && (a.EnActivite == 1)
                    && (a.Quantite > a.QuantiteServi)
                    && ((_idArticle != null) ? a.IdArticle == _idArticle : true)
                    && (a.Commande.CodeCommande == TypeCommandeCode.ALLIMENTATION|| a.Commande.CodeCommande == TypeCommandeCode.ACHAT)

                    ).Select(
                        o => new DetailCommandeDM()
                        {
                            Identifiant = o.Identifiant,
                            EnActivite = o.EnActivite,
                            IdArticle = o.IdArticle,
                            IdCommande = o.IdCommande,
                            IdCreePar = o.IdCommande,
                            Quantite = o.Quantite,
                            Montant = o.Montant,
                            DateCreation = o.DateCreation,
                            DateValidation = o.DateValidation,
                            LibelleSituation = (o.Situation != null) ? o.Situation.Libelle : "",
                            NomControleur = (o.ValiderPar != null) ? o.ValiderPar.Nom : "",
                            Description = o.Description,
                            NumerodeLot = o.NumerodeLot,
                            IdValiderPar = o.IdValiderPar,
                            TauxTVA = o.TauxTva,
                            QuantiteServi = o.QuantiteServi,
                            IdCaisse = o.IdCaisse,
                            DateExpiration = o.DateExpiration,
                            IdSituation=o.IdSituation,
                            LibelleArticle = (o.Article != null) ? o.Article.Libelle : "",
                            LibelleCaisse = (o.Caisse != null) ? o.Caisse.Libelle : "",
                            IdZone = (o.Article != null) ? o.Article.IdZone : null,
                            IdTypeUnite = o.IdTypeUnite,
                            
                            LibelleTypeUnite = (o.TypeUnite != null) ? o.TypeUnite.Libelle : "",
                        }
                    ).ToList();
            return _result;
        }
        //=#########################################################################
        //=#########################################################################
        public ICollection<DetailCommandeDM> getDetailCommandeDMs([Optional] long? _idCommande, [Optional] long? _idCreePar)
        {
            //ok
            bool _shouldGetResult = (_idCommande != null) || (_idCreePar != null);
            ICollection<DetailCommandeDM> _result = _uow.Repos<DetailCommande>().GetAll()
                .Where(
                    a =>
                    (_shouldGetResult)
                    && (a.Affichable == 1)
                    && (a.EnActivite == 1)
                    && ((_idCreePar != null) ? a.IdCreePar == _idCreePar : true)
                    && ((_idCommande != null) ? a.IdCommande == _idCommande : true)
                    ).Select(
                        o => new DetailCommandeDM()
                        {
                            Identifiant = o.Identifiant,
                            EnActivite = o.EnActivite,
                            IdArticle = o.IdArticle,
                            IdCommande = o.IdCommande,
                            IdCreePar = o.IdCommande,
                            Quantite = o.Quantite,
                            Montant = o.Montant,
                            DateCreation = o.DateCreation,
                            DateValidation=o.DateValidation,
                            LibelleSituation= (o.Situation != null) ? o.Situation.Libelle : "",
                            NomControleur = (o.ValiderPar != null) ? o.ValiderPar.Nom : "",
                            Description=o.Description,
                            NumerodeLot=o.NumerodeLot,
                            TauxTVA = o.TauxTva,
                            IdCaisse = o.IdCaisse,
                            DateExpiration = o.DateExpiration,
                            IdSituation = o.IdSituation,
                            QuantiteServi = o.QuantiteServi,
                            LibelleArticle = (o.Article != null) ? o.Article.Libelle : "",
                            IdZone = (o.Article != null) ? o.Article.IdZone : null,
                            IdTypeUnite = o.IdTypeUnite,
                            LibelleTypeUnite = (o.TypeUnite != null) ? o.TypeUnite.Libelle : "",
                        }
                    ).ToList();
            return _result;
        }

        public ICollection<DetailCommandeDM> getDetailCommandeDMsNonServi(long? _idCommande)
        {
            //ok
            ICollection<DetailCommandeDM> _result = null;
            ICollection<DetailCommandeDM> _detailCommandeDMs = getDetailCommandeDMs(_idCommande : _idCommande);
            if (_detailCommandeDMs != null)
            {
                _detailCommandeDMs = _detailCommandeDMs.AsQueryable().Where(a => a.Quantite > a.QuantiteServi).ToList();
                if (_detailCommandeDMs != null) {
                    _result = new List<DetailCommandeDM>();
                    foreach (DetailCommandeDM dc in _detailCommandeDMs)
                    {
                        _result.Add(dc);
                    }
                }                
            }
            return _result;
        }
        
        public void actualiserQuantiteServi(DetailCommandeDM _detailCommandeDM)
        {
            if (_detailCommandeDM != null) {
                DetailCommande _dc= _uow.Repos<DetailCommande>().GetAll().Where(a =>(a.NumerodeLot.Equals(_detailCommandeDM.NumerodeLot)) 
                && (a.Commande.CodeCommande.Equals("ALLIMENTATION"))).FirstOrDefault();
                if (_dc !=null)
                {
                    _dc.QuantiteServi += _detailCommandeDM.Quantite;
                    updateDetailCommande(_dc);
                }
                   
                                
            }
        }

        public Int64? addDetailCommandeDM(DetailCommandeDM _detailCommandeDM)
        {
            DetailCommande _detailCommande = _uow.Repos<DetailCommande>().Create();
            _detailCommande.Affichable = 1;
            _detailCommande.EnActivite = _detailCommandeDM.EnActivite;
            _detailCommande.IdArticle = _detailCommandeDM.IdArticle;
            _detailCommande.IdCommande = _detailCommandeDM.IdCommande;
            _detailCommande.Montant = _detailCommandeDM.Montant;
            _detailCommande.Quantite = _detailCommandeDM.Quantite;
            _detailCommande.QuantiteServi = _detailCommandeDM.QuantiteServi;
            _detailCommande.IdSituation = _detailCommandeDM.IdSituation;
            _detailCommande.IdCreePar = _detailCommandeDM.IdCreePar;
            _detailCommande.Description = _detailCommandeDM.Description;
            _detailCommande.NumerodeLot = _detailCommandeDM.NumerodeLot;
            _detailCommande.TauxTva = _detailCommandeDM.TauxTVA;
            _detailCommande.DateExpiration = _detailCommandeDM.DateExpiration;
            _detailCommande.IdCaisse = _detailCommandeDM.IdCaisse;
            _detailCommande.DateCreation = DateTime.Now;
            _detailCommande.IdTypeUnite = _detailCommandeDM.IdTypeUnite;
            _detailCommande.IdSituation = _detailCommandeDM.IdSituation;
            _uow.Repos<DetailCommande>().Insert(_detailCommande);
            _uow.saveChanges();

            if (_detailCommandeDM.AffectationMessageDMs != null)
            {
                IAffectationMessageTask _affectationMessageTask = IoCContainer.Resolve<IAffectationMessageTask>();
                foreach (AffectationMessageDM _affectationMessageDM in _detailCommandeDM.AffectationMessageDMs)
                {
                    _affectationMessageDM.IdDetailCommande = _detailCommande.Identifiant;
                    _affectationMessageTask.addAffectationMessageDM(_affectationMessageDM);
                }
            }

            return _detailCommande.Identifiant;
        }

        

        public ICollection<DetailCommandeDM> getDetailCommandesDMByIdCommande(long? _idCommande)
        {
            return _uow.Repos<DetailCommande>()
                            .GetAll().Where(a => a.IdCommande == _idCommande && a.EnActivite == 1 && a.Affichable == 1)
                            .Select(a => new DetailCommandeDM()
                            {
                                Identifiant = a.Identifiant,
                                EnActivite = a.EnActivite,
                                IdArticle = a.IdArticle,
                                LibelleArticle = a.Article.Libelle,
                                IdCommande = a.IdCommande,
                                Montant = a.Montant,
                                Quantite = a.Quantite,
                                DateCreation = a.DateCreation,
                                DateValidation = a.DateValidation,
                                LibelleSituation = (a.Situation != null) ? a.Situation.Libelle : "",
                                NomControleur = (a.ValiderPar != null) ? a.ValiderPar.Nom : "",
                                IdValiderPar = a.IdValiderPar,
                                IdSituation = a.IdSituation,
                                QuantiteServi = a.QuantiteServi,
                                IdCreePar = a.IdCreePar,
                                Description = a.Description,
                                NumerodeLot = a.NumerodeLot,
                                TauxTVA=a.TauxTva,
                                DateExpiration=a.DateExpiration,
                                IdCaisse = a.IdCaisse,
                                IdTypeUnite=a.IdTypeUnite,
                                LibelleTypeUnite= (a.TypeUnite != null) ? a.TypeUnite.Libelle : "",
                            }).ToList();
        }

        public ICollection<DetailCommandeDM> getDetailCommandesNonReglesDMByIdCommande(long? _idCommande)
        {
            ICollection<DetailCommandeDM> _detailCommandeDMs = null;
            ICollection<DetailCommande> _detailCommandes = _uow.Repos<DetailCommande>()
                            .GetAll().Where(a => a.IdCommande == _idCommande && a.EnActivite == 1 && a.Affichable == 1)
                            .ToList();
            if (_detailCommandes != null) {
                _detailCommandeDMs = new List<DetailCommandeDM>();
                foreach (DetailCommande dc in _detailCommandes) {
                    ICollection<DetailReglement> detailReglements = _uow.Repos<DetailReglement>()
                        .GetAll().Where(a => a.IdDetailCommande == dc.Identifiant).ToList();
                    Double? _quantiteReglee = detailReglements.Sum(a => a.Quantite);
                    if (_quantiteReglee < dc.Quantite) {
                        DetailCommandeDM _detailCommandeDM = new DetailCommandeDM() {
                            EnActivite = dc.EnActivite,
                            IdArticle = dc.IdArticle,
                            IdCommande = dc.IdCommande,
                            Identifiant = dc.Identifiant,
                            LibelleArticle = dc.Article.Libelle,
                            Montant = dc.Montant,
                            Quantite = dc.Quantite - _quantiteReglee,
                            IdCreePar = dc.IdCreePar,
                            DateCreation = dc.DateCreation,
                            DateValidation = dc.DateValidation,
                            LibelleSituation = (dc.Situation != null) ? dc.Situation.Libelle : "",
                            NomControleur = (dc.ValiderPar != null) ? dc.ValiderPar.Nom : "",
                            Description =dc.Description,
                            NumerodeLot = dc.NumerodeLot,
                            TauxTVA=dc.TauxTva,
                            DateExpiration=dc.DateExpiration,
                            IdCaisse = dc.IdCaisse,

                            IdTypeUnite = dc.IdTypeUnite,
                            LibelleTypeUnite = (dc.TypeUnite != null) ? dc.TypeUnite.Libelle : "",
                    };
                        _detailCommandeDMs.Add(_detailCommandeDM);
                    }
                }
            }
            return _detailCommandeDMs;
        }

        public void removeDetailCommandeDM(long _identifiant)
        {
            DetailCommande _obj = _uow.Repos<DetailCommande>().GetAll().Where(a => a.Identifiant == _identifiant && a.EnActivite == 1 && a.Affichable == 1).FirstOrDefault();
            _uow.Repos<DetailCommande>().DeleteObject(_obj);
            _uow.saveChanges();
        }

        public void validateDetailCommandeDM(ICollection<DetailCommandeDM> _detailCommandeDMs)
        {
            if (_detailCommandeDMs != null)
            {
                foreach (DetailCommandeDM _detailCommandeDM in _detailCommandeDMs)
                {
                    DetailCommande _detailCommande = getDetailCommandeById(_detailCommandeDM.Identifiant);
                    _detailCommande.DateValidation = DateTime.Now;
                    _detailCommande.IdValiderPar = _detailCommandeDM.IdValiderPar;
                    _detailCommande.IdSituation = _detailCommandeDM.IdSituation;
                    //_detailCommande.DateCreation = DateTime.Now;
                    _detailCommande.IdCreePar = _detailCommandeDM.IdCreePar;

                    _uow.Repos<DetailCommande>().Update(_detailCommande);
                    _uow.saveChanges();
                }
            }
        }
        public void updateDetailCommandeDM(DetailCommandeDM _detailCommandeDM)
        {
            DetailCommande _detailCommande = getDetailCommandeById(_detailCommandeDM.Identifiant);
            _detailCommande.Montant = _detailCommandeDM.Montant;
            _detailCommande.Quantite = _detailCommandeDM.Quantite;
            _detailCommande.QuantiteServi = _detailCommandeDM.QuantiteServi;
            _uow.Repos<DetailCommande>().Update(_detailCommande);
            _uow.saveChanges();



           /* if (_detailCommandeDM.AffectationMessageDMs != null)
            {
                IAffectationMessageTask _affectationMessageTask = IoCContainer.Resolve<IAffectationMessageTask>();
                ICollection<AffectationMessageDM> _affectationMessageDMsOrg = _affectationMessageTask.getAffectationMessageDMsByIdDetailCommande(_detailCommande.Identifiant);

                foreach (AffectationMessageDM _affectationMessageDM in _detailCommandeDM.AffectationMessageDMs)
                {
                    if (_affectationMessageDMsOrg.Where(a => a.Identifiant == _affectationMessageDM.Identifiant).Count() == 0)
                    {
                        _affectationMessageDM.IdDetailCommande = _detailCommande.Identifiant;
                        _affectationMessageTask.addAffectationMessageDM(_affectationMessageDM);
                    }
                    else
                    {
                        _affectationMessageTask.updateAffectationMessageDM(_affectationMessageDM);
                    }
                }

                foreach (AffectationMessageDM _affectationMessageDM in _affectationMessageDMsOrg)
                {
                    if (_detailCommandeDM.AffectationMessageDMs.Where(a => a.IdMessage == _affectationMessageDM.IdMessage).Count() == 0)
                    {
                        _affectationMessageTask.removeAffectationMessageDM(_affectationMessageDM.Identifiant);
                    }
                }
            }*/


        }
        
    }
}
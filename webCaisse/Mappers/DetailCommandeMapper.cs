using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using webCaisse.Db.Entities;
using webCaisse.DMs;
using webCaisse.VMs.Models;

namespace webCaisse.Mappers
{
    public class DetailCommandeMapper
    {
        public static DetailCommandeVM DetailCommandeDMtoDetailCommandeVM(DetailCommandeDM _src)
        {
            DetailCommandeVM _dest = null;
            if (_src != null)
            {
                _dest = new DetailCommandeVM()
                {
                    Identifiant = _src.Identifiant,
                    IdArticle = _src.IdArticle,
                    LibelleArticle = _src.LibelleArticle,
                    IdCommande = _src.IdCommande,
                    Montant = _src.Montant,
                    Quantite = _src.Quantite,
                    QuantiteServi = _src.QuantiteServi,
                    LibelleCaisse=_src.LibelleCaisse,
                    IdCreePar = _src.IdCreePar,
                    IdValiderPar=_src.IdValiderPar,
                    IdSituation = _src.IdSituation,
                    DateCreation=_src.DateCreation,
                    DateValidation=_src.DateValidation,
                     NumerodeLot = _src.NumerodeLot,
                     TauxTVA=_src.TauxTVA,
                     DateExpiration=_src.DateExpiration,
                    Description = _src.Description,
                    IdCaisse = _src.IdCaisse,
                    LibelleTypeUnite=_src.LibelleTypeUnite,
                    IdTypeUnite = _src.IdTypeUnite

                };
            }
            return _dest;
        }

        public static DetailCommandeDM DetailCommandeVMtoDetailCommandeDM(DetailCommandeVM _src)
        {
            DetailCommandeDM _dest = null;
            if (_src != null)
            {
                _dest = new DetailCommandeDM()
                {
                    Identifiant = _src.Identifiant,
                    IdArticle = _src.IdArticle,
                    Montant = _src.Montant,
                    Quantite =_src.Quantite,
                    QuantiteServi = _src.QuantiteServi,

                    IdCommande = _src.IdCommande,
                    IdValiderPar = _src.IdValiderPar,
                    IdSituation = _src.IdSituation,
                    DateCreation = _src.DateCreation,
                    DateValidation = _src.DateValidation,
                    NumerodeLot = _src.NumerodeLot,
                    Description = _src.Description,
                    TauxTVA = _src.TauxTVA,
                    DateExpiration = _src.DateExpiration,
                    IdCaisse = _src.IdCaisse,
                    IdTypeUnite= _src.IdTypeUnite,
                     LibelleTypeUnite = _src.LibelleTypeUnite,

                };
            }
            return _dest;
        }
        public static DetailCommandeDM DetailCommandetoDetailCommandeDM(DetailCommande _src)
        {
            DetailCommandeDM _dest = null;
            if (_src != null)
            {
                _dest = new DetailCommandeDM()
                {
                    Identifiant = _src.Identifiant,
                    IdArticle = _src.IdArticle,
                    Montant = _src.Montant,
                    Quantite = _src.Quantite,
                    QuantiteServi = _src.QuantiteServi,
                    IdCommande = _src.IdCommande,
                    IdValiderPar = _src.IdValiderPar,
                    IdSituation = _src.IdSituation,
                    DateCreation = _src.DateCreation,
                    DateValidation = _src.DateValidation,
                    NumerodeLot = _src.NumerodeLot,
                    Description = _src.Description,
                    TauxTVA = _src.TauxTva,
                    DateExpiration = _src.DateExpiration,
                    IdTypeUnite = _src.IdTypeUnite,
                    LibelleTypeUnite = _src.TypeUnite != null ? _src.TypeUnite.Code : "",

                    LibelleCaisse = _src.Caisse != null ? _src.Caisse.Libelle:null,

                    IdCaisse = _src.IdCaisse
                };
            }
            return _dest;
        }
    }
}
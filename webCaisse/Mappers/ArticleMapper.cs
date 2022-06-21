using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using webCaisse.DMs;
using webCaisse.VMs.Models;
using webCaisse.Db.Entities;

namespace webCaisse.Mappers
{
    public class ArticleMapper
    {
        public static ArticleVM ArticleDMtoArticleVM(ArticleDM _src)
        {
            ArticleVM _dest = null;
            if (_src != null)
            {
                _dest = new ArticleVM()
                {
                    Identifiant = _src.Identifiant,
                    Libelle = _src.Libelle,
                    IdCategorie = _src.IdCategorie,
                    LibelleCategorie = _src.LibelleCategorie,
                    LibelleZone = _src.LibelleZone,
                    IdZone = _src.IdZone,
                    IdTauxTva = _src.IdTauxTva,
                    PathImage = _src.PathImage,
                    Montant = _src.Montant,
                    QuantiteDisponible = _src.QuantiteDisponible,
                    QuantiteMin = _src.QuantiteMin,
                    EnActivite = _src.EnActivite,
                    IdTypeUnite= _src.IdTypeUnite,
                    IdTypeArticle = _src.IdTypeArticle,
                    LibelleTypeUnite = _src.LibelleTypeUnite,
                    TauxTva=_src.TauxTva,
                    Referance=_src.Referance,
                    CodeBare=_src.CodeBare,
                    LibelleTypeArticle = _src.LibelleTypeArticle,
                };
            }
            return _dest;
        }
        public static ArticleDM ArticleVMtoArticleDM(ArticleVM _src)
        {
            ArticleDM _dest = null;
            if (_src != null)
            {
                _dest = new ArticleDM()
                {
                    Identifiant = _src.Identifiant,
                    IdCategorie = _src.IdCategorie,
                    Libelle = _src.Libelle,
                    IdZone = _src.IdZone,
                    IdTauxTva = _src.IdTauxTva,
                    Montant = _src.Montant,
                    QuantiteMin = _src.QuantiteMin,
                    EnActivite = _src.EnActivite,
                    ImageAsString = _src.ImageAsString,
                    IdTypeUnite= _src.IdTypeUnite,
                    Referance = _src.Referance,
                    CodeBare = _src.CodeBare,
                    TauxTva=_src.TauxTva,
                    IdTypeArticle = _src.IdTypeArticle,

                };
            }
            return _dest;
        }
        public static ArticleDM ArticletoArticleDM(Article _src)
        {
            ArticleDM _dest = null;
            if (_src != null)
            {
                _dest = new ArticleDM()
                {
                    Identifiant = _src.Identifiant,
                    IdCategorie = _src.IdCategorie,
                    LibelleCategorie= _src.Categorie!=null? _src.Categorie.Libelle:"",
                    Libelle = _src.Libelle,
                    IdZone = 1,
                    IdTauxTva = _src.IdTauxTva,
                    TauxTva = _src.TauxTva != null ? _src.TauxTva.Taux : 0,
                    Montant = _src.Montant,
                    QuantiteMin = _src.QuantiteMin,
                    EnActivite = _src.EnActivite,
                    IdTypeUnite = _src.IdTypeUnite,
                    LibelleTypeUnite = _src.TypeUnite != null ? _src.TypeUnite.Code : "",
                    IdTypeArticle = _src.IdTypeArticle,
                    LibelleTypeArticle = _src.TypeArticle != null ? _src.TypeArticle.Code : "",
                    Referance = _src.Referance,
                    CodeBare = _src.CodeBare
                };
            }
            return _dest;
        }

    }
}
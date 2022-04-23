using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using webCaisse.Db.Entities;
using webCaisse.DMs;
using webCaisse.Mappers;
using webCaisse.Taks.Contracts;
using webCaisse.Tools;
using webCaisse.uows;

namespace webCaisse.Taks.Implementation
{
    public class ArticleTask : IArticleTask
    {
        private IUnitOfWork _uow = null;
        public ArticleTask()
        {
            _uow = IoCContainer.Resolve<IUnitOfWork>();
        }
        private Article getArticleById(long _identifiant)
        {
            //ok
            return _uow.Repos<Article>().GetAll().Where(a => a.Identifiant == _identifiant).FirstOrDefault();
        }

        public ICollection<ArticleDM> getArticles([Optional] long? _enActivite)
        {
            ICollection<ArticleDM> _articleDMs = new List<ArticleDM>();
            bool _shouldGetResult = (_enActivite != null);
            _shouldGetResult = (_shouldGetResult != false) ? _shouldGetResult : true;
           
            ICollection<Article> _articles= _uow.Repos<Article>().GetAll().Where(
                a =>
                (_shouldGetResult)
                && (a.Affichable == 1)
               && ((_enActivite != null) ? a.EnActivite == _enActivite : true)).ToList();

            foreach (Article a in _articles) {
                ArticleDM _articleDM = ArticleMapper.ArticletoArticleDM(a);
                _articleDMs.Add(_articleDM);
            }
            return _articleDMs;
        }
        public Int64? addArticle(ArticleDM _articleDM)
        {
            TauxTva _tauxtva = _uow.Repos<TauxTva>().GetAll().Where(x=>x.Taux== _articleDM.TauxTva).FirstOrDefault();
            Article _article = _uow.Repos<Article>().Create();
            _article.Affichable = 1;
            _article.EnActivite = 1;
            _article.Libelle = _articleDM.Libelle;
            _article.Montant = _articleDM.Montant;
            _article.IdCategorie = _articleDM.IdCategorie;
            _article.QuantiteDisponible = _articleDM.QuantiteDisponible;
            _article.QuantiteMin = _articleDM.QuantiteMin;
            _article.IdZone = _articleDM.IdZone;
            _article.IdTypeUnite = _articleDM.IdTypeUnite;
            _article.IdTauxTva = (_tauxtva!=null) ?_tauxtva.Identifiant:0;
            _article.Referance = _articleDM.Referance;
            _article.CodeBare = _articleDM.CodeBare;
            _uow.Repos<Article>().Insert(_article);
            _uow.saveChanges();
            //------------------------------
            if (_articleDM.ImageAsString != null && _articleDM.ImageAsString.Length > 0)
            {
                String _fileName = _article.Identifiant + ".jpg";
                addImageOnFS(_fileName, _articleDM.ImageAsString);
            }
            //------------------------------

            return _article.Identifiant;
        }

        public void updateArticle(ArticleDM _articleDM)
        {
            TauxTva _tauxtva = _uow.Repos<TauxTva>().GetAll().Where(x => x.Taux == _articleDM.TauxTva).FirstOrDefault();
            if (_articleDM.Identifiant != 0)
            {
                Article _article = getArticleById(_articleDM.Identifiant);
                _article.Affichable = 1;
            _article.EnActivite = _articleDM.EnActivite;
            _article.Libelle = _articleDM.Libelle;
            _article.Montant = _articleDM.Montant;
            _article.IdCategorie = _articleDM.IdCategorie;
            _article.QuantiteDisponible = _articleDM.QuantiteDisponible;
            _article.QuantiteMin = _articleDM.QuantiteMin;
                _article.IdTypeUnite = _articleDM.IdTypeUnite;
            _article.IdZone = _articleDM.IdZone;
            _article.IdTauxTva = _tauxtva.Identifiant;
                _article.Referance = _articleDM.Referance;
                _article.CodeBare = _articleDM.CodeBare;
                _uow.Repos<Article>().Update(_article);
            _uow.saveChanges();
                removeImageFromFS(_articleDM.Identifiant);
                //------------------------------
                if (_articleDM.ImageAsString != null && _articleDM.ImageAsString.Length > 0)
                {
                    String _fileName = _articleDM.Identifiant + ".jpg";
                    addImageOnFS(_fileName, _articleDM.ImageAsString);
                }
                //------------------------------
            }
        }
        private void addImageOnFS(String _namgeFile, String _imageAsString)
        {
           
            String _racineImage = ConfigInfrastructure.BO_FILE_ROOT + @"\images\article\";
            Byte[] _bytes =Utilitaire.getPictureBoxAsByte(_imageAsString);
            File.WriteAllBytes(_racineImage + _namgeFile, _bytes);
        }
        private void removeImageFromFS(Int64? _identifiant)
        {
            String _racineImage = ConfigInfrastructure.BO_FILE_ROOT + @"\images\article\";
            File.Delete(_racineImage + _identifiant + ".jpg");
        }

        public string getDefaultImageAsString()
        {
            String _result = null;
            String _racineImage = ConfigInfrastructure.BO_FILE_ROOT + @"\images\article\";
            Byte[] _bytes = File.ReadAllBytes(_racineImage + "default_article.jpg");
            _result = Convert.ToBase64String(_bytes);
            return _result;
        }
public ICollection<DetailCommandeDM> listeArticle(ICollection<DetailCommandeDM>detailcommandeDM)
        {
            // ICollection<DetailCommandeDM> detailcommandeDMret = detailcommandeDM.GroupBy(a => a.IdArticle);
            ICollection<DetailCommandeDM> listedetailarticleDM = new List<DetailCommandeDM>();
            ICollection<Int64?> _idarticle = detailcommandeDM.Select(a => a.IdArticle).Distinct().ToList();
            foreach(Int64 i in _idarticle)
            {
                DetailCommandeDM dcDM = new DetailCommandeDM();
                dcDM = detailcommandeDM.Where(a => a.IdArticle == i).FirstOrDefault();
                double? count = detailcommandeDM.Where(a => a.IdArticle==i).Sum(a=>a.Quantite);
                dcDM.Quantite= (double)count;
               
                listedetailarticleDM.Add(dcDM);
            }
            return listedetailarticleDM;
        }
        public string getImageAsString(long _identifiant)
        {
            String _result = null;
            if (_identifiant != null && _identifiant > 0)
            {
                try
                {
                    String _racineImage = ConfigInfrastructure.BO_FILE_ROOT + @"\images\article\";
                    Byte[] _bytes = File.ReadAllBytes(_racineImage + _identifiant + ".jpg");
                    _result = Convert.ToBase64String(_bytes);
                }
                catch (Exception ex)
                {

                }
            }
            return _result;
        }

     }
}
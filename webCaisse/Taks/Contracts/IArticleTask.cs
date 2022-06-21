using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using webCaisse.DMs;
using System.Runtime.InteropServices;

namespace webCaisse.Taks.Contracts
{
    public interface IArticleTask
    {
        ICollection<ArticleDM> getArticles([Optional] Int64? _enActivite);
        Int64? addArticle(ArticleDM _articleDM);
        void updateArticle(ArticleDM _articleDM);
        String getImageAsString(Int64 _identifiant);
        String getDefaultImageAsString();
        ICollection<DetailCommandeDM> listeArticle(ICollection<DetailCommandeDM> detailcommandeDM);
    }
}
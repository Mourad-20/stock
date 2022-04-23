using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using webCaisse.DMs;

namespace webCaisse.Taks.Contracts
{
    public interface ICategorieTask
    {
        ICollection<CategorieDM> getCategories([Optional] Int64? _enActivite);
        ICollection<CategorieDM> getCategorieDMsCommercialisees(Int64 _idCaisse);
        Int64? addCategorieDM(CategorieDM _categorieDM);
        void modifierCategorieDM(CategorieDM _categorieDM);
        String getImageAsString(Int64 _identifiant);
        String getDefaultImageAsString();
    }
}
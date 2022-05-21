using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using webCaisse.DMs;
using webCaisse.VMs.Models;

namespace webCaisse.Mappers
{
    public class TypeArticleMapper
    {
        public static TypeArticleVM TypeArticleDMtoTypeArticleVM(TypeArticleDM _src)
        {
            TypeArticleVM _dest = null;
            if (_src != null)
            {
                _dest = new TypeArticleVM()
                {
                    Identifiant = _src.Identifiant,
                    Code = _src.Code,
                    Libelle = _src.Libelle,
                };
            }
            return _dest;
        }
    }
}
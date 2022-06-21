using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using webCaisse.DMs;
using webCaisse.VMs.Models;

namespace webCaisse.Mappers
{
    public class CategorieMapper
    {
        public static CategorieVM CategorieDMtoCategorieVM(CategorieDM _src)
        {
            CategorieVM _dest = null;
            if (_src != null)
            {
                _dest = new CategorieVM()
                {
                    Identifiant = _src.Identifiant,
                    Code = _src.Code,
                    Libelle = _src.Libelle,
                    EnActivite = _src.EnActivite,
                    PathImage = _src.PathImage,
                    Background=_src.Background
                };
            }
            return _dest;
        }
        public static CategorieDM CategorieVMtoCategorieDM(CategorieVM _src)
        {
            CategorieDM _dest = null;
            if (_src != null)
            {
                _dest = new CategorieDM()
                {
                    Identifiant = _src.Identifiant,
                    Code = _src.Code,
                    Libelle = _src.Libelle,
                    EnActivite = _src.EnActivite,
                    PathImage = _src.PathImage,
                    Background = _src.Background,
                    ImageAsString = _src.ImageAsString
                };
            }
            return _dest;
        }
    }
}
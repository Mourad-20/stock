using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using webCaisse.DMs;
using webCaisse.VMs.Models;

namespace webCaisse.Mappers
{
    public class ZoneMapper
    {
        public static ZoneVM ZoneDMtoZoneVM(ZoneDM _src)
        {
            ZoneVM _dest = null;
            if (_src != null)
            {
                _dest = new ZoneVM()
                {
                    Identifiant = _src.Identifiant,
                    Code = _src.Code,
                    Libelle = _src.Libelle,
                    EnActivite = _src.EnActivite,
                    NomImprimante = _src.NomImprimante,
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
                    ImageAsString = _src.ImageAsString,
                };
            }
            return _dest;
        }
    }
}
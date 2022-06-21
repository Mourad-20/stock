using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using webCaisse.DMs;
using webCaisse.VMs.Models;

namespace webCaisse.Mappers
{
    public class CaisseMapper
    {
        public static CaisseVM CaisseDMtoCaisseVM(CaisseDM _src)
        {
            CaisseVM _dest = null;
            if (_src != null)
            {
                _dest = new CaisseVM()
                {
                    Identifiant = _src.Identifiant,
                    EnActivite = _src.EnActivite, 
                    Libelle = _src.Libelle,
                };
            }
            return _dest;
        }
        public static CaisseDM CaisseVMtoCaisseDM(CaisseVM _src)
        {
            CaisseDM _dest = null;
            if (_src != null)
            {
                _dest = new CaisseDM()
                {
                    Identifiant = _src.Identifiant,
                    EnActivite = _src.EnActivite,
                    Libelle = _src.Libelle,

                };
            }
            return _dest;
        }
    }
}
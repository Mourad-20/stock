using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using webCaisse.DMs;
using webCaisse.VMs.Models;

namespace webCaisse.Mappers
{
    public class EtatCommandeMapper
    {
        public static EtatCommandeVM AEtatCommandeDMtoEtatCommandeVM(EtatCommandeDM _src)
        {
            EtatCommandeVM _dest = null;
            if (_src != null)
            {
                _dest = new EtatCommandeVM()
                {
                    Identifiant = _src.Identifiant,
                    Libelle = _src.Libelle,
                   Code=_src.Code,
                    EnActivite = _src.EnActivite,
                };
            }
            return _dest;
        }
    }
}
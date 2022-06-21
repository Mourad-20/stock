using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using webCaisse.Db.Entities;
using webCaisse.DMs;
using webCaisse.VMs.Models;

namespace webCaisse.Mappers
{
    public class InterlocuteurMapper
    {
        public static InterlocuteurVM InterlocuteurDMtoInterlocuteurVM(InterlocuteurDM _src)
        {
            InterlocuteurVM _dest = null;
            if (_src != null)
            {
                _dest = new InterlocuteurVM()
                {
                    Identifiant = _src.Identifiant,
                    Nom = _src.Nom.ToUpper(),
                    Fonction = _src.Fonction,
                    Email1 = _src.Email1,
                    Email2 = _src.Email2,
                    Tel1 = _src.Tel1,
                    Tel2 = _src.Tel2,
                    IdLocalite = _src.IdLocalite,
                    LocaliteVM = _src.LocaliteDM != null ? LocaliteMapper.LocaliteDMtoLocaliteVM(_src.LocaliteDM):null,
                    EnActivite = _src.EnActivite,
                    Fix = _src.Fix,

                };
            }
            return _dest;
        }

        public static InterlocuteurDM InterlocuteurVMtoInterlocuteurDM(InterlocuteurVM _src)
        {
            InterlocuteurDM _dest = null;
            if (_src != null)
            {
                _dest = new InterlocuteurDM()
                {
                    Identifiant = _src.Identifiant,
                    Nom = _src.Nom.ToUpper(),
                    Fonction = _src.Fonction,
                    Email1 = _src.Email1,
                    Email2 = _src.Email2,
                    Tel1 = _src.Tel1,
                    Tel2 = _src.Tel2,
                    IdLocalite = _src.IdLocalite,
                  //  LocaliteDM = _src.LocaliteVM != null ? LocaliteMapper.LocaliteVMtoLocaliteDM(_src.LocaliteM) : null,
                    EnActivite = _src.EnActivite,
                    Fix = _src.Fix,

                };
            }
            return _dest;
        }
        public static InterlocuteurDM InterlocuteurtoInterlocuteurDM(Interlocuteur _src)
        {
            InterlocuteurDM _dest = null;
            if (_src != null)
            {
                _dest = new InterlocuteurDM()
                {
                    Identifiant = _src.Identifiant,
                    Nom = _src.Nom.ToUpper(),
                    Fonction = _src.Fonction,
                    Email1 = _src.Email1,
                    Email2 = _src.Email2,
                    Tel1 = _src.Tel1,
                    Tel2 = _src.Tel2,
                    IdLocalite = _src.IdLocalite,
                    LocaliteDM = _src.Localite != null ? LocaliteMapper.LocalitetoLocaliteDM(_src.Localite) : null,
                    EnActivite = _src.EnActivite,
                    Fix = _src.Fix,
                };
            }
            return _dest;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using webCaisse.DMs;
using webCaisse.VMs.Models;

namespace webCaisse.Mappers
{
    public class UtilisateurMapper
    {
        public static UtilisateurVM UtilisateurDMtoUtilisateurVM(UtilisateurDM _src)
        {
            UtilisateurVM _dest = null;
            if (_src != null)
            {
                _dest = new UtilisateurVM()
                {
                    Identifiant = _src.Identifiant,
                    EnActivite = _src.EnActivite,
                    Nom = _src.Nom,
                    Prenom = _src.Prenom,
                    Login = _src.Login,
                    //Password=""
                };
            }
            return _dest;
        }

        public static UtilisateurDM UtilisateurVMtoUtilisateurDM(UtilisateurVM _src)
        {
            UtilisateurDM _dest = null;
            if (_src != null)
            {
                _dest = new UtilisateurDM()
                {
                    Identifiant = _src.Identifiant,
                    EnActivite = _src.EnActivite,
                    Nom = _src.Nom,
                    Prenom = _src.Prenom,
                    Login = _src.Login,
                    Password = _src.Password
                };
            }
            return _dest;
        }
    }
}
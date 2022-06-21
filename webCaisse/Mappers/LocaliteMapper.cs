using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using webCaisse.Db.Entities;
using webCaisse.DMs;
using webCaisse.VMs.Models;

namespace webCaisse.Mappers
{
    public class LocaliteMapper
    {
        public static LocaliteVM LocaliteDMtoLocaliteVM(LocaliteDM _src)
        {
            LocaliteVM _dest = null;
            if (_src != null)
            {
                _dest = new LocaliteVM()
                {
                    Identifiant = _src.Identifiant,
                    Libelle = _src.Libelle,
                    Code = _src.Code,
                    EnActivite = _src.EnActivite,
                    Numero=_src.Numero,
                    Adresse1=_src.Adresse1,
                    Adresse2 = _src.Adresse2,
                    Tel1=_src.Tel1,
                    Tel2 = _src.Tel2,
                    RC = _src.RC,
                    NomUtilisateur=_src.NomUtilisateur

                };
            }
            return _dest;
        }
        public static LocaliteDM LocaliteVMtoLocaliteDM(LocaliteVM _src)
        {
            LocaliteDM _dest = null;
            if (_src != null)
            {
                _dest = new LocaliteDM()
                {
                    Identifiant = _src.Identifiant,
                    Libelle = _src.Libelle,
                    EnActivite = _src.EnActivite,
                    Code = _src.Code,
                    Numero = _src.Numero,
                    Adresse1 = _src.Adresse1,
                    Adresse2 = _src.Adresse2,
                    Tel1 = _src.Tel1,
                    Tel2 = _src.Tel2,
                    RC = _src.RC,
                    IdUtilisateur=_src.IdUtilisateur,
                };
            }
            return _dest;
        }

        public static LocaliteDM LocalitetoLocaliteDM(Localite _src)
        {
            LocaliteDM _dest = null;
            if (_src != null)
            {
                _dest = new LocaliteDM()
                {
                    Identifiant = _src.Identifiant,
                    Libelle = _src.Libelle,
                    EnActivite = _src.EnActivite,
                    Code = _src.Code,
                    Numero = _src.Numero,
                    Adresse1 = _src.Adresse1,
                    Adresse2 = _src.Adresse2,
                    Tel1 = _src.Tel1,
                    Tel2 = _src.Tel2,
                    RC=_src.RC,
                    NomUtilisateur= _src.Utilisateur!=null?_src.Utilisateur.Nom:""
                };
            }
            return _dest;
        }

    }
}
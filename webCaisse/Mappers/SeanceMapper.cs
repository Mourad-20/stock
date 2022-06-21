using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using webCaisse.DMs;
using webCaisse.VMs.Models;

namespace webCaisse.Mappers
{
    public class SeanceMapper
    {
        public static SeanceVM SeanceDMtoSeanceVM(SeanceDM _src)
        {
            SeanceVM _dest = null;
            if (_src != null)
            {
                _dest = new SeanceVM()
                {
                    Identifiant = _src.Identifiant,
                    EnActivite = _src.EnActivite, 
                    DateDebut = _src.DateDebut,
                    DateFin = _src.DateFin,
                    MontantDebut = _src.MontantDebut,
                    MontantFin = _src.MontantFin,
                    MontantPreleve = _src.MontantPreleve,
                    Numero = _src.Numero, 
                    LibelleCaisse = _src.LibelleCaisse,
                    IdUtilisateur = _src.IdUtilisateur,
                    NomPrenom = _src.NomPrenom,
                    IdCaisse = _src.IdCaisse,
                };
            }
            return _dest;
        }

        public static SeanceDM SeanceVMtoSeanceDM(SeanceVM _src)
        {
            SeanceDM _dest = null;
            if (_src != null)
            {
                _dest = new SeanceDM()
                {
                    Identifiant = _src.Identifiant,
                    DateDebut = _src.DateDebut,
                    DateFin = _src.DateFin,
                    MontantDebut = _src.MontantDebut,
                    MontantFin = _src.MontantFin,
                    MontantPreleve = _src.MontantPreleve,
                };
            }
            return _dest;
        }
    }
}
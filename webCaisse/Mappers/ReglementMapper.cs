using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using webCaisse.Db.Entities;
using webCaisse.DMs;
using webCaisse.VMs.Models;

namespace webCaisse.Mappers
{
    public class ReglementMapper
    {
        public static ReglementVM ReglementDMtoReglementVM(ReglementDM _src)
        {
            ReglementVM _dest = null;
            if (_src != null)
            {
                _dest = new ReglementVM()
                {
                    Identifiant = _src.Identifiant,
                    EnActivite = _src.EnActivite,
                    IdCreePar = _src.IdCreePar,
                    Montant = _src.Montant,
                    Numero = _src.Numero,
                    DateReglement = _src.DateReglement,
                    IdCommande = _src.IdCommande,
                    IdModeReglement = _src.IdModeReglement,
                    LibelleModeReglement = _src.LibelleModeReglement,
                    NumeroCommande = _src.NumeroCommande,
                    Datecheque = _src.Datecheque,
                    Ncheque = _src.Ncheque,
                    NomBanque = _src.NomBanque,
                    NCompte = _src.NCompte
    };
            }
            return _dest;
        }

        public static ReglementDM ReglementVMtoReglementDM(ReglementVM _src)
        {
            ReglementDM _dest = null;
            if (_src != null)
            {
                _dest = new ReglementDM()
                {
                    Identifiant = _src.Identifiant,
                    EnActivite = _src.EnActivite,
                    IdCreePar = _src.IdCreePar,
                    IdCommande = _src.IdCommande,
                    IdModeReglement = _src.IdModeReglement,                    
                    Montant = _src.Montant,
                    Numero = _src.Numero,
                     Datecheque = _src.Datecheque,
                    Ncheque = _src.Ncheque,
                    NomBanque = _src.NomBanque,
                    NCompte = _src.NCompte
                };
            }
            return _dest;
        }
        public static ReglementDM ReglementtoReglementDM(Reglement _src)
        {
            ReglementDM _dest = null;
            if (_src != null)
            {
                _dest = new ReglementDM()
                {
                    Identifiant = _src.Identifiant,
                    EnActivite = _src.EnActivite,
                    IdCreePar = _src.IdCreePar,
                    IdCommande = _src.IdCommande,
                    IdModeReglement = _src.IdModeReglement,
                    Montant = _src.Montant,
                    Numero = _src.Numero,
                    Datecheque = _src.Datecheque.ToString(),
                    Ncheque = _src.Ncheque,
                    NomBanque = _src.NomBanque,
                    NCompte = _src.NCompte
                };
            }
            return _dest;
        }
    }
}
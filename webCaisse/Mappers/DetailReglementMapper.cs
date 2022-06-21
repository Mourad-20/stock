using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using webCaisse.Db.Entities;
using webCaisse.DMs;
using webCaisse.VMs.Models;

namespace webCaisse.Mappers
{
    public class DetailReglementMapper
    {
        public static DetailReglementVM DetailReglementDMtoDetailReglementVM(DetailReglementDM _src)
        {
            DetailReglementVM _dest = null;
            if (_src != null)
            {
                _dest = new DetailReglementVM()
                {
                    Identifiant = _src.Identifiant,
                    EnActivite = _src.EnActivite,
                    Montant = _src.Montant,
                    IdDetailCommande = _src.IdDetailCommande,
                    IdReglement = _src.IdReglement,
                    LibelleArticle = _src.LibelleArticle,
                    Quantite = _src.Quantite,                            
                };
            }
            return _dest;
        }

        public static DetailReglementDM DetailReglementVMtoDetailReglementDM(DetailReglementVM _src)
        {
            DetailReglementDM _dest = null;
            if (_src != null)
            {
                _dest = new DetailReglementDM()
                {
                    Identifiant = _src.Identifiant,
                    EnActivite = _src.EnActivite,
                    IdDetailCommande = _src.IdDetailCommande,
                    IdReglement = _src.IdReglement,
                    Montant = _src.Montant,
                    Quantite = _src.Quantite,
                    
                };
            }
            return _dest;
        }
        public static DetailReglementDM DetailReglementtoDetailReglementDM(DetailReglement _src)
        {
            DetailReglementDM _dest = null;
            if (_src != null)
            {
                _dest = new DetailReglementDM()
                {
                    Identifiant = _src.Identifiant,
                    EnActivite = _src.EnActivite,
                    IdDetailCommande = _src.IdDetailCommande,
                    IdReglement = _src.IdReglement,
                    Montant = _src.Montant,
                    Quantite = _src.Quantite,
                };
            }
            return _dest;
        }
    }
}
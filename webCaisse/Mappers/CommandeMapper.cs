using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using webCaisse.Db.Entities;
using webCaisse.DMs;
using webCaisse.VMs.Models;

namespace webCaisse.Mappers
{
    public class CommandeMapper
    {
        public static CommandeVM CommandeDMtoCommandeVM(CommandeDM _src)
        {
            CommandeVM _dest = null;
            if (_src != null)
            {
                _dest = new CommandeVM()
                {
                    Identifiant = _src.Identifiant,
                    DateCommande = _src.DateCommande,
                    EnActivite = _src.EnActivite,
                    IdCreePar = _src.IdCreePar,
                    IdServeur = _src.IdServeur,
                    NomServeur = _src.NomServeur,
                    IdEtatCommande = _src.IdEtatCommande,
                    CodeEtatCommande = _src.CodeEtatCommande,
                    IdLocalite = _src.IdLocalite,
                    IdSeance = _src.IdSeance,
                    Montant = _src.Montant,
                    LibelleCaisse=_src.LibelleCaisse,
                    Numero = _src.Numero,  
                    LibelleEtatCommande = _src.LibelleEtatCommande,
                    LibelleLocalite = _src.LibelleLocalite,
                    NumeroSeance = _src.NumeroSeance,
                    CodeCommande=_src.CodeCommande,
                    IdSource=_src.IdSource,
                    CommandeSourceVM= CommandeMapper.CommandeDMtoCommandeVM(_src.CommandeSourceDM)
                };
            }
            return _dest;
        }

        public static CommandeDM CommandeVMtoCommandeDM(CommandeVM _src)
        {
            CommandeDM _dest = null;
            if (_src != null)
            {
                _dest = new CommandeDM()
                {
                    Identifiant = _src.Identifiant,
                    DateCommande = _src.DateCommande,
                    EnActivite = _src.EnActivite,
                    IdCreePar = _src.IdCreePar,
                    IdServeur = _src.IdServeur,
                    NomServeur = _src.NomServeur,
                    IdEtatCommande = _src.IdEtatCommande,
                    IdLocalite = _src.IdLocalite,
                    IdSeance = _src.IdSeance,
                    Montant = _src.Montant,
                    Numero = _src.Numero,
                    IdSource = _src.IdSource,
                    CodeCommande = _src.CodeCommande
                };
            }
            return _dest;
        }
        public static CommandeDM CommandetoCommandeDM(Commande _src)
        {
            CommandeDM _dest = null;
            if (_src != null)
            {
                _dest = new CommandeDM()
                {
                    Identifiant = _src.Identifiant,
                    DateCommande = _src.DateCommande,
                    EnActivite = _src.EnActivite,
                    IdCreePar = _src.IdCreePar,
                    IdServeur = _src.IdServeur,
                   
                    IdEtatCommande = _src.IdEtatCommande,
                    IdLocalite = _src.IdLocalite,
                    IdSeance = _src.IdSeance,
                    Montant = _src.Montant,
                    Numero = _src.Numero,
                    IdSource = _src.IdSource,
                    CodeCommande = _src.CodeCommande,
                    CommandeSourceDM = CommandeMapper.CommandetoCommandeDM(_src.CommandeSource)
                };
            }
            return _dest;
        }
    }
}
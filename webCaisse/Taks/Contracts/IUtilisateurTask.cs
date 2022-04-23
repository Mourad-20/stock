using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using webCaisse.DMs;

namespace webCaisse.Taks.Contracts
{
    public interface IUtilisateurTask
    {
        UtilisateurDM authentification(String _login, String _password);
        UtilisateurDM authentificationJeton(String _jeton);
        UtilisateurDM getUtilisateurDMByIdentifiant(Int64 _identifiant);
        void bloquerUtilisateur(Int64 _identifiant);
        void debloquerUtilisateur(Int64 _identifiant);
        ICollection<UtilisateurDM> getUtilisateurByGroupe(String _code);
        Boolean isUtilisateurOnGroupes(ICollection<GroupeDM> _groupeDMs, List<String> _groupeCodes);
        ICollection<UtilisateurDM> getServeursOfSeance(Int64 _idSeance);
        ICollection<UtilisateurDM> getServeursOfCaisse(Int64 _idCaisse);        
        ICollection<UtilisateurDM> getListeUtilisateurDMs([Optional] Int64? _enActivite);
        Int64? addUtilisateur(UtilisateurDM _utilisateurDM);
        Int64? updateUtilisateur(UtilisateurDM _utilisateurDM);
    }
}
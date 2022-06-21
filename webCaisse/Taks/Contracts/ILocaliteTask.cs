using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using webCaisse.DMs;
using System.Runtime.InteropServices;

namespace webCaisse.Taks.Contracts
{
    public interface ILocaliteTask
    {
        ICollection<LocaliteDM> getLocalites([Optional] Int64? _enActivite);
        ICollection<LocaliteDM> getLocalitesDisponible();
      
        void changerEtatLocalite(Int64 _idLocalite, String _codeEtat);
        Int64? addLocaliteDM(LocaliteDM _localiteDM);
        void modifierLocaliteDM(LocaliteDM _localiteDM);
    }
}
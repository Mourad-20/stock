using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using webCaisse.DMs;

namespace webCaisse.Taks.Contracts
{
    public interface ICaisseTask
    {
        ICollection<CaisseDM> getCaisseDMs([Optional] Int64? _enActivite);
        CaisseDM getCaisseOfUtilisateur(Int64 _idUtilisateur);
        Int64? addCaisse(CaisseDM _caisseDM);
        Int64? updateCaisse(CaisseDM _caisseDM);
    }
}
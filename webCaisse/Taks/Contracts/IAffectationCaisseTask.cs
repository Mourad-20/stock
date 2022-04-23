using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using webCaisse.DMs;

namespace webCaisse.Taks.Contracts
{
    public interface IAffectationCaisseTask
    {
        ICollection<AffectationCaisseDM> getAffectationCaisseDMs([Optional]Int64? _idCaisse,[Optional]Int64? _enActivite);
        Int64? addAffectationCaisseDM(AffectationCaisseDM _affectationCaisseDM);
        void removeAffectationCaisseDM(Int64 _identifiant);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using webCaisse.DMs;

namespace webCaisse.Taks.Contracts
{
    public interface IAffectationMessageTask
    {        
        Int64? addAffectationMessageDM(AffectationMessageDM _affectationMessageDM);
        ICollection<AffectationMessageDM> getAffectationMessageDMsByIdDetailCommande(Int64 _idDetailCommande);
        void updateAffectationMessageDM(AffectationMessageDM _affectationMessageDM);
        void removeAffectationMessageDM(Int64 _identifiant);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using webCaisse.DMs;
using System.Runtime.InteropServices;

namespace webCaisse.Taks.Contracts
{
    public interface IInterlocuteurTask
    {
        ICollection<InterlocuteurDM> getInterlocuteurs([Optional] Int64? _enActivite);

        Int64? addInterlocuteurDM(InterlocuteurDM _InterlocuteurDM);
        void modifierInterlocuteurDM(InterlocuteurDM _InterlocuteurDM) ;
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using webCaisse.DMs;

namespace webCaisse.Taks.Contracts
{
    public interface ICommercialisationTask
    {
        ICollection<CommercialisationDM> getCommercialisationDMs([Optional]Int64? _idCaisse);
        Int64? addCommercialisationDM(CommercialisationDM _commercialisationDM);
        void removeCommercialisationDM(Int64 _identifiant);
    }
}
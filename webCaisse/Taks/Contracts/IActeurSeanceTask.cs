using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using webCaisse.DMs;

namespace webCaisse.Taks.Contracts
{
    public interface IActeurSeanceTask
    {        
        Int64? addActeurSeanceDM(ActeurSeanceDM _acteurSeanceDM);
        ICollection<ActeurSeanceDM> getActeurSeanceDMsByIdSeance(Int64 _idSeance);
        void removeActeurSeanceDM(Int64 _identifiant);
    }
}
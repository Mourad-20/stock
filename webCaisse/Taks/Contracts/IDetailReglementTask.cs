using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using webCaisse.DMs;

namespace webCaisse.Taks.Contracts
{
    public interface IDetailReglementTask
    {
        Int64? addDetailReglementDM(DetailReglementDM _detailReglementDM);
        ICollection<DetailReglementDM> getDetailReglementsDMByIdReglement(long? _idReglement);
        void removeDetailReglementDM(Int64 _identifiant);
        
    }
}
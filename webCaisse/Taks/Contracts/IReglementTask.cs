using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using webCaisse.DMs;

namespace webCaisse.Taks.Contracts
{
    public interface IReglementTask
    {
        ReglementDM getReglementDMById(Int64? _identifiant);
        String genererNumeroReglement();
        ICollection<ReglementDM> getReglementDMsByIdCommande(Int64? _idCommande);
        Int64? etablirReglement(ReglementDM _reglementDM);
        ICollection<ReglementDM> getReglementDMs([Optional] Int64? _idSeance);

    }
}
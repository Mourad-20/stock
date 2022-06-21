using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using webCaisse.DMs;

namespace webCaisse.Taks.Contracts
{
    public interface IAppartenanceTask
    {
        Int64 addAppartenance(AppartenanceDM _appartenanace);
        Int64 updateAppartenance(AppartenanceDM _appartenanace);

        ICollection<AppartenanceDM> getAppartenanceDMsByIdUtilisateur(Int64 _idUtilisateur);
        void removeAppartenanceDM(Int64 _identifiant);

    }
}
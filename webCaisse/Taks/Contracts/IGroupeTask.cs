using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using webCaisse.DMs;

namespace webCaisse.Taks.Contracts
{
    public interface IGroupeTask
    {
        ICollection<GroupeDM> getGroupesOfUtilisateur(Int64 _idUtilisateur);
        ICollection<GroupeDM> getGroupeDMs();
        GroupeDM getGroupeDMByCode(String _code);
    }
}
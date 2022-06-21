using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using webCaisse.DMs;

namespace webCaisse.Taks.Contracts
{
    public interface ITypeMessageTask
    {
       // ICollection<TypeMessageDM> getGroupesOfUtilisateur(Int64 _idUtilisateur);
        ICollection<TypeMessageDM> getTypeMessageDMs();
        TypeMessageDM getTypeMessageDMByCode(String _code);
    }
}
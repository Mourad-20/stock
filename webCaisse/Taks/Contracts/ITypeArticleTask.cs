using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using webCaisse.DMs;

namespace webCaisse.Taks.Contracts
{
    public interface ITypeArticleTask
    {
       // ICollection<TypeUniteDM> getGroupesOfUtilisateur(Int64 _idUtilisateur);
        ICollection<TypeArticleDM> getTypeArticleDMs();
        TypeArticleDM getTypeArticleDMByCode(String _code);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using webCaisse.DMs;

namespace webCaisse.Taks.Contracts
{
    public interface IAssociationMessageTask
    {
        ICollection<AssociationMessageDM> getAssociationMessageDMs([Optional]Int64? _idArticle);
        Int64? addAssociationMessageDM(AssociationMessageDM _associationMessageDM);
        void removeAssociationMessageDM(Int64 _identifiant);
    }
}
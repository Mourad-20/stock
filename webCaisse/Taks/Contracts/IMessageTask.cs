using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using webCaisse.DMs;
using System.Runtime.InteropServices;

namespace webCaisse.Taks.Contracts
{
    public interface IMessageTask
    {
        ICollection<MessageDM> getMessages([Optional] Int64? _enActivite);
        ICollection<MessageDM> getMessagesbyId([Optional] Int64? _enActivite,[Optional] Int64? _idArticle);
        
        Int64? addMessageDM(MessageDM _messageDM);
        void modifierMessageDM(MessageDM _messageDM);
    }
}
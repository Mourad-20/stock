using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using webCaisse.DMs;
using webCaisse.VMs.Models;

namespace webCaisse.Mappers
{
    public class MessageMapper
    {
        public static MessageVM MessageDMtoMessageVM(MessageDM _src)
        {
            MessageVM _dest = null;
            if (_src != null)
            {
                _dest = new MessageVM()
                {
                    Identifiant = _src.Identifiant,
                    EnActivite = _src.EnActivite,
                    Libelle = _src.Libelle,
                    Quantite = _src.Quantite,
                    LibelleArticle= _src.LibelleArticle,
                    LibelleType= _src.LibelleType,
                    IdArticleSrc = _src.IdArticleSrc,
                };
            }
            return _dest;
        }
        public static MessageDM MessageVMtoMessageDM(MessageVM _src)
        {
            MessageDM _dest = null;
            if (_src != null)
            {
                _dest = new MessageDM()
                {
                    Identifiant = _src.Identifiant,
                    EnActivite = _src.EnActivite,
                    Libelle = _src.Libelle,
                    IdArticle= _src.IdArticle,
                    IdArticleSrc = _src.IdArticleSrc,
                };
            }
            return _dest;
        }

    }
}
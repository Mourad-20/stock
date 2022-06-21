using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using webCaisse.Db.Entities;
using webCaisse.DMs;
using webCaisse.Taks.Contracts;
using webCaisse.Tools;
using webCaisse.uows;
using System.Runtime.InteropServices;

namespace webCaisse.Taks.Implementation
{
    public class MessageTask : IMessageTask
    {
        private IUnitOfWork _uow = null;
        public MessageTask()
        {
            _uow = IoCContainer.Resolve<IUnitOfWork>();
        }
        public Message getMessagebyLibelle(String _libelle)
        {

            return _uow.Repos<Message>().GetAll().Where(a => (a.Libelle.ToUpper() == _libelle.ToUpper())).FirstOrDefault();
        }
        private Message getMessageById(long _identifiant)
        {
            //ok
            return _uow.Repos<Message>().GetAll().Where(a => a.Identifiant == _identifiant).FirstOrDefault();
        }
        public long? addMessageDM(MessageDM _messageDM)
        {
            /*Message message = this.getMessagebyLibelle(_messageDM.Libelle);
            if (message != null)
            {
                throw new Exception("Message deja existe");

            }*/
            Message _obj = _uow.Repos<Message>().Create();
            _obj.Libelle = _messageDM.Libelle;
            _obj.IdArticle = _messageDM.IdArticle;
            _obj.IdTypeMessage = _messageDM.IdTypeMessage;
            _obj.Quantite = _messageDM.Quantite;
            _obj.IdArticleSrc = _messageDM.IdArticleSrc;
            _obj.EnActivite = _messageDM.EnActivite;
            _obj.Affichable = 1;
            _uow.Repos<Message>().Insert(_obj);
            _uow.saveChanges();
            return _obj.Identifiant;
        }

        public ICollection<MessageDM> getMessages([Optional] Int64? _enActivite)
        {
            bool _shouldGetResult = (_enActivite != null);
            _shouldGetResult = (_shouldGetResult != false) ? _shouldGetResult : true;
            ICollection<MessageDM> _result = _uow.Repos<Message>().GetAll()
                .Where(
                    a =>
                    (_shouldGetResult)
                    && (a.Affichable == 1)
                    && ((_enActivite != null) ? a.EnActivite == _enActivite : true)
                    ).Select(
                        a => new MessageDM()
                        {
                            Identifiant = a.Identifiant,
                            EnActivite = a.EnActivite,
                            Libelle = a.Libelle,
                            IdArticle = a.IdArticle,
                            IdTypeMessage = a.IdTypeMessage,
                            Quantite = a.Quantite,
                            LibelleArticle = a.ArticleSrc != null ? a.ArticleSrc.Libelle : "",
                            LibelleType = a.TypeMessage != null ? a.TypeMessage.Libelle : "",
                        }
                    ).ToList();
            return _result;
        }

        public ICollection<MessageDM> getMessagesbyId([Optional] Int64? _enActivite, [Optional] Int64? _idArticle)
        {
            bool _shouldGetResult = true;
             _shouldGetResult = (_enActivite != null);
           // _shouldGetResult = (_shouldGetResult != false) ? _shouldGetResult : true;
            ICollection<MessageDM> _result = _uow.Repos<Message>().GetAll()
                .Where(
                    a =>
                  //  (_shouldGetResult)
                     (a.Affichable == 1)
                    && ((_idArticle != null) ? a.IdArticle == _idArticle : true)
                   && ((_enActivite != null) ? a.EnActivite == _enActivite : true)
                    ).Select(
                        a => new MessageDM()
                        {
                            Identifiant = a.Identifiant,
                            EnActivite = a.EnActivite,
                            Libelle = a.Libelle,
                            IdArticle = a.IdArticle,
                            IdTypeMessage = a.IdTypeMessage,
                            Quantite = a.Quantite,
                            LibelleArticle = a.ArticleSrc != null ? a.ArticleSrc.Libelle : "",
                            LibelleType = a.TypeMessage != null ? a.TypeMessage.Libelle : "",
                        }
                    ).ToList();
            return _result;
        }

        public void modifierMessageDM(MessageDM _messageDM)
        {
            Message message = this.getMessagebyLibelle(_messageDM.Libelle);
           
            if (_messageDM.Identifiant != 0)
            {
                if (message != null && message.Identifiant != _messageDM.Identifiant)
                {
                    throw new Exception("Message deja existe");

                }
                Message _message = getMessageById(_messageDM.Identifiant);
                _message.Libelle = _messageDM.Libelle;
                _message.EnActivite = _messageDM.EnActivite;
                _message.IdArticle = _messageDM.IdArticle;
                _message.IdArticleSrc = _messageDM.IdArticleSrc;
                _message.IdTypeMessage = _messageDM.IdTypeMessage;
                _message.Quantite = _messageDM.Quantite;
                _uow.Repos<Message>().Update(_message);
                _uow.saveChanges();
            }
        }

    }
}
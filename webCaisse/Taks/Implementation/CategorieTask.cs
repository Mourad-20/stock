using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using webCaisse.Db.Entities;
using webCaisse.DMs;
using webCaisse.Taks.Contracts;
using webCaisse.Tools;
using webCaisse.uows;

namespace webCaisse.Taks.Implementation
{
    public class CategorieTask : ICategorieTask
    {
        private IUnitOfWork _uow = null;
        public CategorieTask()
        {
            _uow = IoCContainer.Resolve<IUnitOfWork>();
        }

        private Categorie getCategorieById(long _identifiant)
        {
            //ok
            return _uow.Repos<Categorie>().GetAll().Where(a => a.Identifiant == _identifiant).FirstOrDefault();
        }
        public ICollection<CategorieDM> getCategorieDMsCommercialisees(long _idCaisse)
        {
            ICollection<CategorieDM> _result = null;
            ICollection<Int64?> _idsCategorie = new List<Int64?>();
            ICommercialisationTask _commercialisationTask = IoCContainer.Resolve<ICommercialisationTask>();
            ICollection<CommercialisationDM> _commercialisationDMs = _commercialisationTask.getCommercialisationDMs(_idCaisse: _idCaisse);
            if (_commercialisationDMs != null) {
                _idsCategorie = _commercialisationDMs.Select(a => a.IdCategorie).ToList();
                if (_idsCategorie != null && _idsCategorie.Count > 0)
                {
                    _result = _uow.Repos<Categorie>().GetAll()
                        .Where(a => _idsCategorie.Contains(a.Identifiant) && a.Affichable == 1 && a.EnActivite == 1)
                        .Select(a => new CategorieDM()
                        {
                            Identifiant = a.Identifiant,
                            Code = a.Code,
                            PathImage = a.PathImage,
                            EnActivite = a.EnActivite,
                            Libelle = a.Libelle,
                            Background=a.Background,
                        }).ToList();
                }
            }
            return _result;
        }

        public ICollection<CategorieDM> getCategories([Optional] Int64? _enActivite)
        {
            bool _shouldGetResult = (_enActivite != null);
            _shouldGetResult = (_shouldGetResult != false) ? _shouldGetResult : true;
            ICollection<CategorieDM> _result = _uow.Repos<Categorie>().GetAll()
                .Where(
                    a =>
                    (_shouldGetResult)
                    && (a.Affichable == 1)
                    && ((_enActivite != null) ? a.EnActivite == _enActivite : true)
                    ).Select(
                        a => new CategorieDM()
                        {
                            Identifiant = a.Identifiant,
                            Code = a.Code,
                            PathImage = a.PathImage,
                            EnActivite = a.EnActivite,
                            Libelle = a.Libelle,
                            Background = a.Background,
                        }
                    ).ToList();
            return _result;
        }

        public long? addCategorieDM(CategorieDM _categorieDM)
        {
            Categorie _obj = _uow.Repos<Categorie>().Create();
            _obj.Libelle = _categorieDM.Libelle;
            _obj.Code = _categorieDM.Code;
            _obj.EnActivite = _categorieDM.EnActivite;
            _obj.Background = _categorieDM.Background;
            _obj.Affichable = 1;
            _uow.Repos<Categorie>().Insert(_obj);
            _uow.saveChanges();
            //------------------------------
            if (_categorieDM.ImageAsString != null && _categorieDM.ImageAsString.Length > 0)
            {
                Byte[] _bytes = Utilitaire.getPictureBoxAsByte(_categorieDM.ImageAsString);

               String _fileName = _obj.Identifiant + ".jpg";
                addImageOnFS(_fileName, _bytes);
            }
            //------------------------------
            return _obj.Identifiant;
        }

        public void modifierCategorieDM(CategorieDM _categorieDM)
        {
            if (_categorieDM.Identifiant != 0)
            {
                Byte[] _bytes = Utilitaire.getPictureBoxAsByte(_categorieDM.ImageAsString);
                Categorie _categorie = getCategorieById(_categorieDM.Identifiant);
                _categorie.Code = _categorieDM.Code;
                _categorie.Libelle = _categorieDM.Libelle;
                _categorie.EnActivite = _categorieDM.EnActivite;
                _categorie.Background = _categorieDM.Background;
                _uow.Repos<Categorie>().Update(_categorie);
                _uow.saveChanges();
                removeImageFromFS(_categorieDM.Identifiant);
                //------------------------------
                if (_categorieDM.ImageAsString != null && _categorieDM.ImageAsString.Length > 0)
                {
                    String _fileName = _categorieDM.Identifiant + ".jpg";
                    addImageOnFS(_fileName, _bytes);
                }
                //------------------------------
            }
        }
        public string getDefaultImageAsString()
        {
            String _result = null;
            String _racineImage = ConfigInfrastructure.BO_FILE_ROOT + @"\images\categorie\";
            Byte[] _bytes = File.ReadAllBytes(_racineImage + "default_categorie.jpg");
            _result = Convert.ToBase64String(_bytes);
            return _result;
        }

        private void addImageOnFS(String _namgeFile, Byte[] _imageAsString)
        {
            String _racineImage = ConfigInfrastructure.BO_FILE_ROOT + @"\images\categorie\";
            //Byte[] _bytes = Utilitaire.getPictureBoxAsByte(_imageAsString);
            File.WriteAllBytes(_racineImage + _namgeFile, _imageAsString);
        }
        private void removeImageFromFS(Int64? _identifiant)
        {
            String _racineImage = ConfigInfrastructure.BO_FILE_ROOT + @"\images\categorie\";
            File.Delete(_racineImage + _identifiant + ".jpg");
        }

        public string getImageAsString(long _identifiant)
        {
            String _result = null;
            if (_identifiant != null && _identifiant > 0)
            {
                try
                {
                    String _racineImage = ConfigInfrastructure.BO_FILE_ROOT + @"\images\categorie\";
                    Byte[] _bytes = File.ReadAllBytes(_racineImage + _identifiant + ".jpg");
                    _result = Convert.ToBase64String(_bytes);
                }
                catch (Exception ex)
                {

                }
            }
            return _result;
        }

    }
}
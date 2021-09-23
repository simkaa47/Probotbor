using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Probotbor.Infrastructure
{
    public class StatusDevice: NotifyPropertyChanged
    {
        public delegate void ChangeValueHandler(StatusDevice status);
        public ChangeValueHandler ChangeValueHandlerDel;
        #region  Id - Идентификатор, нужен в качестве ключа для поиска в XML документе        
        int id;
        /// <summary>
        /// Идентификатор, нужен в качестве ключа
        /// </summary>
        public int Id { get => id; set { Set(ref id, value); } }
        #endregion

        #region Статус устройства
        string status;
        /// <summary>
        /// Статус устройства
        /// </summary>
        public string Status 
        {
            get => status; 
            set 
            { 
                Set(ref status, value);
                if (ChangeValueHandlerDel != null) ChangeValueHandlerDel(this);
            } 
        }
        #region Расположение картинки
        string picturePath = "none";
        /// <summary>
        ///Расположение картинки
        /// </summary>
        public string PicturePath 
        { 
            get => picturePath;
            set
            {
                Set(ref picturePath, value);
                if (ChangeValueHandlerDel != null) ChangeValueHandlerDel(this);
            }
        } 
        #endregion
        #endregion


    }
}

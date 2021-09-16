using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Probotbor.Infrastructure
{
    /// <summary>
    /// Описание записи в истории событий
    /// </summary>
    public class HistoryItem : NotifyPropertyChanged
    {
        public int Id { get; set; }


        #region Время возникновения события
        string date;
        /// <summary>
        /// Время возникновения события
        /// </summary>
        public string EventDate { get => date; set => Set(ref date, value); }
        #endregion

        #region Кто был пользователь
        string user;
        /// <summary>
        /// Кто был пользователь
        /// </summary>
        public string UserName { get => user; set => Set(ref user, value); }
        #endregion

        #region Сообщение
        string message;
        /// <summary>
        /// Сообщение
        /// </summary>
        public string Message { get=>message; set=>Set(ref message, value);}
        #endregion

        #region Тип события
        string eventType;
        /// <summary>
        /// Тип события
        /// </summary>
        public string EventType { get=>eventType; set=>Set(ref eventType,value); }
        #endregion

        #region Код ошибки
        string eventCode;
        /// <summary>
        /// Код ошибки
        /// </summary>
        public string EventCode
        {
            get => eventCode;
            set => Set(ref eventCode, value);
            
        }
        #endregion

        


    }
}

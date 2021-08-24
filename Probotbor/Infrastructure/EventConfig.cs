using Sharp7;
using System    ;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Probotbor.Infrastructure
{
    public class EventConfig : NotifyPropertyChanged
    {        
        public Action<EventConfig> ChangeValueHandlerDel;
        public delegate void EventFrontHandler(EventConfig config);
        public event EventFrontHandler EventIsActive;


        #region Id
        int id;
        public int Id { get=>id; set { Set(ref id, value); } }
        #endregion

        #region Описание события
        string description = "Какая то ошибка";
        public string Description
        {
            get => description;
            set
            {
                Set(ref description, value);
                if (ChangeValueHandlerDel != null) ChangeValueHandlerDel(this);
            }
        }

        #endregion

        #region Номер ДБ
        int dbNum = 1;
        public int DbNum
        {
            get => dbNum;
            set
            {
                Set(ref dbNum, (value > 1 ? value : 1));
                if (ChangeValueHandlerDel != null) ChangeValueHandlerDel(this);
            }
        }


        #endregion

        #region Номер байта
        int byteNum = 0;
        public int ByteNum
        {
            get => byteNum;
            set
            {
                Set(ref byteNum, (value >=0  ? value : 0));
                if (ChangeValueHandlerDel != null) ChangeValueHandlerDel(this);
            }

        }
        #endregion

        #region Номер бита
        int bitNum = 0;
        public int BitNum
        {
            get => bitNum;
            set
            {
                Set(ref bitNum, (value >= 0 && value <= 7 ? value : 0));
                if (ChangeValueHandlerDel != null) ChangeValueHandlerDel(this);
            }
        }
        #endregion

        #region Значение бита
        int bitValue = 1;
        public int BitValue
        {
            get => bitValue;
            set
            {
                Set(ref bitValue, (value >= 0 && value <= 1 ? value : 1));
                if (ChangeValueHandlerDel != null) ChangeValueHandlerDel(this);
            }
        }
        #endregion

        #region Тип сообщения
        string eventType = "error";
        public string EventType
        {
            get => eventType;
            set
            {
                Set(ref eventType, (value == "event" || value == "error" ? value : "error"));
                if (ChangeValueHandlerDel != null) ChangeValueHandlerDel(this);
            }
        }
        #endregion

        #region Код ошибки
        string eventCode = "0000";
        /// <summary>
        /// Код ошибки
        /// </summary>
        public string EventCode
        {
            get => eventCode;
            set
            {
                Set(ref eventCode, value);
                if (ChangeValueHandlerDel != null) ChangeValueHandlerDel(this);
            }

        }
        #endregion

        #region Активность события
        bool isActive = false;
        public bool IsActive
        {
            get => isActive;
            set
            {
                if (value == true && isActive == false)
                {
                    EventIsActive?.Invoke(this);
                }
                Set(ref isActive, value);                
            }
        }
        #endregion

        #region Метод получения события GetEvent
        public void GetEvent(byte[] buffer, int offset)
        {
            int startByte = this.ByteNum - offset;
            if (startByte < 0) return;
            if (startByte > buffer.Length - 1) return;
            bool temp = S7.GetBitAt(buffer, startByte, this.bitNum);
            if (this.BitValue == 0) IsActive = (temp == false);
            else IsActive = (temp == true);
        }
            
        #endregion


        
    }
}

using Sharp7;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Probotbor.Infrastructure
{
    public class IndicatorConfig : NotifyPropertyChanged
    {
        public delegate void ChangeValueHandler(int id, string Property, string value);
        public ChangeValueHandler ChangeValueHandlerDel;

        #region Id
        private int id;
        public int Id { get => id; set => Set(ref id, value); }
        #endregion

        #region Описание
        string description;
        public string Description
        {
            get => description;
            set
            {
                Set(ref description, value);
                if (ChangeValueHandlerDel != null) ChangeValueHandlerDel(Id, "Description", Description);
            }
        }
        #endregion

        #region Номер ДБ
        int dbNum;
        public int DbNum
        {
            get => dbNum;
            set
            {

                Set(ref dbNum, value > 0 ? value : 1);
                if (ChangeValueHandlerDel != null) ChangeValueHandlerDel(Id, "DbNum", DbNum.ToString());
            }
        }
        #endregion

        #region Смещение в байтах
        int byteNum;
        public int ByteNum
        {
            get => byteNum;
            set
            {
                Set(ref byteNum, value >= 0 ? value : 0);
                if (ChangeValueHandlerDel != null) ChangeValueHandlerDel(Id, "ByteNum", ByteNum.ToString());
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
                if (ChangeValueHandlerDel != null) ChangeValueHandlerDel(Id, "BitNum", BitNum.ToString());
            }
        }
        #endregion

        #region Инверсия бита
        private bool inverse;
        /// <summary>
        /// Инверсия бита
        /// </summary>
        public bool Inverse
        {
            get => inverse;
            set
            {
                Set(ref inverse, value);
                if (ChangeValueHandlerDel != null) ChangeValueHandlerDel(Id, "Inverse", Inverse.ToString());
            }
        }
        #endregion

        #region Значение (с учетом инверсии)
        private bool bitValue;
        /// <summary>
        /// Значение (с учетом инверсии)
        /// </summary>
        public bool BitValue
        {
            get => bitValue;
            set => Set(ref bitValue, value);
        }
        #endregion

        #region Метод получения  значения
        public void GetValue(byte[] buffer, int offset)
        {
            int startByte = this.ByteNum - offset;
            if (startByte < 0) return;
            if (startByte > buffer.Length - 1) return;
            bool value = S7.GetBitAt(buffer, startByte, this.BitNum);
            this.BitValue = Inverse ? !value : value;
        }
        #endregion



    }
}

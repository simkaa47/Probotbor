using Probotbor.Infrastructure;
using Probotbor.Infrastructure.Commands;
using Sharp7;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Probotbor.Models
{
    public class DataCell : NotifyPropertyChanged
    {
        #region Делегаты и события
        public delegate void ChangeValueHandler(int id, string Property, string value);
        public ChangeValueHandler ChangeValueHandlerDel;
        public delegate void WriteValueHandler(DataCell cell);// Делегат записи в плк
        public WriteValueHandler WriteValueHandlerDel;
        #endregion

        #region Кол-во милисекунд на запись параметра
        private int msForWrite { get; set; } = 100;

        #endregion

        #region Текущее кол-во таймаута на запись параметра
        private int CurMsForWrite { get; set; }
        #endregion

        #region Команда записи параметра
        RelayCommand writeParamCommand;
        public RelayCommand WriteParamCommand
        {
            get => writeParamCommand ?? (writeParamCommand = new RelayCommand(OnWriteParamCommandExecuted, OnWriteParamCommandExecute));
        }
        private bool OnWriteParamCommandExecute(object p) => true;
        private void OnWriteParamCommandExecuted(object p)
        {
            switch (this.DataType)
            {
                case "short":
                    short tempShort;
                    if (!short.TryParse(WriteValue.ToString(), out tempShort))
                    {
                        WriteValue = ParamValue;
                        return;
                    }
                    else Set(ref writeValue, tempShort);
                    break;
                case "ushort":
                    ushort tempUShort;
                    if (!ushort.TryParse(WriteValue.ToString(), out tempUShort))
                    {
                        WriteValue = ParamValue;
                        return;
                    }
                    else Set(ref writeValue, tempUShort);
                    break;
                case "int":
                    int tempInt;
                    if (!int.TryParse(WriteValue.ToString(), out tempInt))
                    {
                        WriteValue = ParamValue;
                        return;
                    }
                    else Set(ref writeValue, tempInt);
                    break;
                case "uint":
                    uint tempUInt;
                    if (!uint.TryParse(WriteValue.ToString(), out tempUInt))
                    {
                        WriteValue = ParamValue;
                        return;
                    }
                    else Set(ref writeValue, tempUInt);
                    break;
                case "float":
                    float tempFloat;
                    if (!float.TryParse(WriteValue.ToString().Replace(",", "."), NumberStyles.Float, CultureInfo.InvariantCulture, out tempFloat))
                    {
                        WriteValue = ParamValue;
                        return;
                    }
                    else Set(ref writeValue, tempFloat);
                    break;
                default:
                    Set(ref writeValue, WriteValue);
                    break;
            }
            WriteValueHandlerDel?.Invoke(this);
            
        }

        #endregion

        #region ID
        int id;
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

        #region Тип данных
        string dataType = "float";
        public string DataType
        {
            get => dataType;
            set
            {
                string temp = value;
                if (value != "short" && value != "ushort" && value != "int" && value != "uint" && value != "float" && value != "string") temp = "float";
                Set(ref dataType, value);
                if(ChangeValueHandlerDel!=null) ChangeValueHandlerDel(Id, "DataType", DataType);
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

                Set(ref dbNum, value>0 ? value : 1 );
                if (ChangeValueHandlerDel != null) ChangeValueHandlerDel(Id, "DbNum", DbNum.ToString());
            }
        }
        #endregion

        #region Смещение в байтах
        int byteOffset;
        public int ByteOffset
        {
            get => byteOffset;
            set
            {
                Set(ref byteOffset, value >= 0 ? value : 0);
                if (ChangeValueHandlerDel != null) ChangeValueHandlerDel(Id, "ByteOffset", ByteOffset.ToString());
            }
        }
        #endregion

        #region ScalingFactor
        float factor;
        public float Factor 
        { 
            get => factor;
            set
            {
                Set(ref factor, value);
                if (ChangeValueHandlerDel != null) ChangeValueHandlerDel(Id, "Factor", Factor.ToString());
            }
        }
        #endregion

        #region Формат
        float format;
        public float Format
        {
            get => format;
            set
            {
                float temp = value <= 1.0 ? value : 1.0f;
                Set(ref format, temp);
                if (ChangeValueHandlerDel != null) ChangeValueHandlerDel(Id, "Format", Format.ToString());
            }
        }
        #endregion

        #region Длина  (в случае строки)
        int length;
        public int Length 
        { 
            get => length;
            set
            {
                var temp = 1;
                if (this.DataType == "string") temp = value;
                Set(ref length, (temp > 0 ? temp : 1));
                if (ChangeValueHandlerDel != null) ChangeValueHandlerDel(Id, "Length", Length.ToString());
            }
        }
        #endregion

        #region Для записи
        private object writeValue;
       
        public object WriteValue
        {
            get => writeValue;
            set
            {
                Set(ref writeValue, value);                
            }
        }
        #endregion

        #region Разрешение записи
        private bool writeEnable;
        /// <summary>
        /// Флаг разрешения записи
        /// </summary>
        public bool WriteEnable
        {
            get => writeEnable;
            set 
            {
                Set(ref writeEnable, value);
                if (ChangeValueHandlerDel != null) ChangeValueHandlerDel(Id, "WriteEnable", WriteEnable.ToString());
            }
        }
        #endregion

        #region Значение параметра (object)
        private object paramValue;
        public object ParamValue
        {
            get => paramValue;
            set
            {
                if (Set(ref paramValue, value))
                {
                    WriteValue = value;
                }
                if (ParamValue.ToString() != WriteValue.ToString()) 
                {
                    
                    
                    CurMsForWrite++;
                }
                if (CurMsForWrite > msForWrite)
                {
                    WriteValue = value;
                    CurMsForWrite = 0;
                }
            }
        }
        #endregion

        #region Получить величину в зависимости от типа данных
        public void GetValue(byte[] buffer, int offset)
        {
            int startByte = this.ByteOffset - offset;
            if (startByte < 0) return;
            if (startByte > buffer.Length - 1) return;
            if (startByte + this.Length * (GetDataTypeSize(this.DataType)) > buffer.Length) return;
            switch (this.DataType)
            {
                case "string":
                    ParamValue = S7.GetStringAt(buffer, startByte);
                    break;
                case "short":                    
                    ParamValue = Convert((float)S7.GetIntAt(buffer, startByte));
                    break;
                case "ushort":                    
                    ParamValue = Convert((float)S7.GetUIntAt(buffer, startByte));
                    break;
                case "int":
                    ParamValue = Convert((float)S7.GetDIntAt(buffer, startByte));                    
                    break;
                case "uint":
                    ParamValue = Convert((float)S7.GetUDIntAt(buffer, startByte));                    
                    break;
                case "float":
                    ParamValue = Convert(S7.GetRealAt(buffer, startByte));
                    break;
                default:
                    ParamValue = Convert(S7.GetRealAt(buffer, startByte));
                    break;
            }
        } 
        #endregion

        #region Получить размер типа данных по строке
        public  static int GetDataTypeSize(string dataType)
        {
            int value = 0;
            switch (dataType)
            {
                case "bool": value = 1; break;
                case "byte": value = 1; break;
                case "string": value = 1; break;
                case "short": value = 2; break;
                case "ushort": value = 2; break;
                case "int": value = 4; break;
                case "uint": value = 4; break;
                case "float": value = 4; break;
                default:
                    value = 1;
                    break;
            }
            return value;
        }
        #endregion

        #region Преобразовать данные в зависимости от типа данных
        private float Convert(float inputValue)
        {            
            float temp = inputValue * this.Factor;
            temp = temp * (1 / this.Format);
            int tempInt = (int)(temp);
            temp = tempInt * this.Format;
            return temp;     

        }
        #endregion
        
    }
}

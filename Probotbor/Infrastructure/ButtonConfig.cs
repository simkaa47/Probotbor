using Probotbor.Infrastructure.Commands;
using Sharp7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Probotbor.Infrastructure
{
    public class ButtonConfig: IndicatorConfig
    {
        #region Делегат, вызывающийся при нажатии на кнопку
        public delegate void WriteValueHandler(ButtonConfig config, bool  value);// Делегат записи в плк
        /// <summary>
        /// Делегат, вызывающийся при нажатии на кнопку
        /// </summary>
        public WriteValueHandler WriteValueHandlerDel;
        #endregion

        #region Команда записи в ПЛК

        #region Нажатие кнопки
        RelayCommand buttonPressOnCommand;
        public RelayCommand ButtonPressOnCommand
        {
            get => buttonPressOnCommand ?? (buttonPressOnCommand = new RelayCommand(OnButtonPressOnCommandExecuted, OnButtonPressOnCommandExecute));
        }
        private bool OnButtonPressOnCommandExecute(object p) => true;
        private void OnButtonPressOnCommandExecuted(object p)
        {

            switch (Action)
            {
                case "Мгновенно":
                    WriteValueHandlerDel?.Invoke(this, true);
                    break;
                case "Переключатель":
                    WriteValueHandlerDel?.Invoke(this, !BitValueWrite);
                    break;
                case "Включить":
                    WriteValueHandlerDel?.Invoke(this, true);
                    break;
                case "Выключить":
                    WriteValueHandlerDel?.Invoke(this, false);
                    break;
                default:
                    break;
            }
        } 
        #endregion

        #region Отжатие кнопки
        RelayCommand buttonPressOffCommand;
        public RelayCommand ButtonPressOffCommand
        {
            get => buttonPressOffCommand ?? (buttonPressOffCommand = new RelayCommand(OnButtonPressOffCommandExecuted, OnButtonPressOffCommandExecute));
        }
        private bool OnButtonPressOffCommandExecute(object p) => true;
        private void OnButtonPressOffCommandExecuted(object p)
        {            
            if (Action == "Мгновенно") WriteValueHandlerDel?.Invoke(this, false);

        }
        #endregion

        #endregion

        #region Номер ДБ для записи
        int dbNumWrite;
        public int DbNumWrite
        {
            get => dbNumWrite;
            set
            {

                Set(ref dbNumWrite, value > 0 ? value : 1);
                if (ChangeValueHandlerDel != null) ChangeValueHandlerDel(Id, "DbNumWrite", DbNumWrite.ToString());
            }
        }
        #endregion

        #region Смещение в байтах для записи
        int byteNumWrite;
        public int ByteNumWrite
        {
            get => byteNumWrite;
            set
            {
                Set(ref byteNumWrite, value >= 0 ? value : 0);
                if (ChangeValueHandlerDel != null) ChangeValueHandlerDel(Id, "ByteNumWrite", ByteNumWrite.ToString());
            }
        }
        #endregion

        #region Номер бита для записи
        int bitNumWrite = 0;
        public int BitNumWrite
        {
            get => bitNumWrite;
            set
            {
                Set(ref bitNumWrite, (value >= 0 && value <= 7 ? value : 0));
                if (ChangeValueHandlerDel != null) ChangeValueHandlerDel(Id, "BitNumWrite", BitNumWrite.ToString());
            }
        }
        #endregion

        

        #region Значение 
        private bool bitValueWrite;
        /// <summary>
        /// Значение (с учетом инверсии)
        /// </summary>
        public bool BitValueWrite
        {
            get => bitValueWrite;
            set => Set(ref bitValueWrite, value);
        }
        #endregion



        #region Действие по нажатию на кнопку
        private string buttonAction = "Переключатель";
        public string Action
        {
            get => buttonAction;
            set 
            {
                switch (value)
                {
                    case "Мгновенно":
                    case "Переключатель":
                    case "Включить":
                    case "Выключить":
                        Set(ref buttonAction, value);
                        break;
                    default:
                        break;
                }
                
                if (ChangeValueHandlerDel != null) ChangeValueHandlerDel(Id, "Action", Action);
            }
        }
        #endregion

        #region Метод получения значения записи
        public void GetValueWrite(byte[] buffer, int offset)
        {
            int startByte = this.ByteNumWrite - offset;
            if (startByte < 0) return;
            if (startByte > buffer.Length - 1) return;
            BitValueWrite = S7.GetBitAt(buffer, startByte, this.BitNum);
            
        }
        #endregion
    }
}

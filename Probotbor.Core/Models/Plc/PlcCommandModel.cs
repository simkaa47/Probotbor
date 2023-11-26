using Probotbor.Core.Models.Communication;

namespace Probotbor.Core.Models.Plc
{
    public class PlcCommandModel
    {
        public PlcCommandModel()
        {
            OtborCmd1 = new Parameter<bool>(nameof(OtborCmd1), "Произвести отбор 1", false, true);
            ReturnCmd1 = new Parameter<bool>(nameof(ReturnCmd1), "Произвести возврат 1", false, true);
            OtborCmd2 = new Parameter<bool>(nameof(OtborCmd2), "Произвести отбор 2", false, true);
            ReturnCmd2 = new Parameter<bool>(nameof(ReturnCmd2), "Произвести возврат 2", false, true);
            PitatelCmd = new Parameter<bool>(nameof(PitatelCmd), "Цикл питателя", false, true);
            DrobilkaOn = new Parameter<bool>(nameof(DrobilkaOn), "Включить дробилку", false, true);
            VibratorOnOff1 = new Parameter<bool>(nameof(VibratorOnOff1), "Запуск вибратора 1", false, true);
            VibratorOnOff2 = new Parameter<bool>(nameof(VibratorOnOff2), "Запуск вибратора 2", false, true);
            VibratorOnOff3 = new Parameter<bool>(nameof(VibratorOnOff3), "Запуск вибратора 3", false, true);
            OpenHighShiberCmd = new Parameter<bool>(nameof(OpenHighShiberCmd), "Открыть верхний шибер", false, true);
            CloseHighShiberCmd = new Parameter<bool>(nameof(CloseHighShiberCmd), "Закрыть верхний шибер", false, true);
            OpenLowShiberCmd = new Parameter<bool>(nameof(OpenLowShiberCmd), "Открыть нижний шибер", false, true);
            CloseLowShiberCmd = new Parameter<bool>(nameof(CloseLowShiberCmd), "Закрыть нижний шибер", false, true);
            UpBarabanCmd = new Parameter<bool>(nameof(UpBarabanCmd), "Поднять барабан", false, true);
            DownBarabanCmd = new Parameter<bool>(nameof(DownBarabanCmd), "Опустить барабан", false, true);
            SysReturnCmd = new Parameter<bool>(nameof(SysReturnCmd), "Запустить транспортер", false, true);
            IstiratelCmd = new Parameter<bool>(nameof(IstiratelCmd), "Запустить истиратель", false, true);
            CalibrationNakopCmd = new Parameter<bool>(nameof(CalibrationNakopCmd), "Произвести калибровку накопителя", false, true);
            ChangeKanistraNakopCmd = new Parameter<bool>(nameof(ChangeKanistraNakopCmd), "Поменять канистру накопителя", false, true);
            OpenLockNakopCmd = new Parameter<bool>(nameof(OpenLockNakopCmd), "Открыть замок накопителя", false, true);
            RstCmd = new Parameter<bool>(nameof(RstCmd), "Сброс ошибок", false, true);
            DryCycleCmd = new Parameter<bool>(nameof(DryCycleCmd), "Цикл блока сушки", false, true);
        }
        #region Произвести отбор 1
        public Parameter<bool> OtborCmd1 { get; }
        #endregion
        #region Произвести возврат 1
        public Parameter<bool> ReturnCmd1 { get; } 
        #endregion

        #region Произвести отбор 2
        public Parameter<bool> OtborCmd2 { get; }
        #endregion
        #region Произвести возврат 2
        public Parameter<bool> ReturnCmd2 { get; }
        #endregion

        #region Запуск питателя
        public Parameter<bool> PitatelCmd { get; }
        #endregion
        #region Запуск дробилки
        public Parameter<bool> DrobilkaOn { get; }
        #endregion

        #region Запуск вибратора 1
        public Parameter<bool> VibratorOnOff1 { get; }
        #endregion
        #region Запуск вибратора 2
        public Parameter<bool> VibratorOnOff2 { get; }
        #endregion
        #region Запуск вибратора 3
        public Parameter<bool> VibratorOnOff3 { get; } 
        #endregion

        #region Поднять верхний шибер
        public Parameter<bool> OpenHighShiberCmd { get; } 
        #endregion
        #region Закрыть верхний шибер
        public Parameter<bool> CloseHighShiberCmd { get; } 
        #endregion
        #region Поднять нижний шибер
        public Parameter<bool> OpenLowShiberCmd { get; } 
        #endregion
        #region Закрыть нижний шибер
        public Parameter<bool> CloseLowShiberCmd { get; }
        #endregion
        #region Поднять барабан
        public Parameter<bool> UpBarabanCmd { get; } 
        #endregion
        #region Опустить барабан
        public Parameter<bool> DownBarabanCmd { get; } 
        #endregion

        #region Запустить возврат проб
        public Parameter<bool> SysReturnCmd { get; } 
        #endregion
        #region Запустить истиратель
        public Parameter<bool> IstiratelCmd { get; }    
        #endregion

        #region Произвести калибровку накопителя
        public Parameter<bool> CalibrationNakopCmd { get; }
        #endregion
        #region Поменять канистру накопителя
        public Parameter<bool> ChangeKanistraNakopCmd { get; }
        #endregion
        #region Открыть замок накопителя
        public Parameter<bool> OpenLockNakopCmd { get; } 
        #endregion

        #region Сбросить ошибки
        public Parameter<bool> RstCmd { get; } 
        #endregion

        #region Цикл блока сушки
        public Parameter<bool> DryCycleCmd { get; }
        #endregion


    }
    
}

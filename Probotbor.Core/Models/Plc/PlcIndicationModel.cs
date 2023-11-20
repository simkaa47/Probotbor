using Probotbor.Core.Models.Communication;
using Probotbor.Core.Models.Probotbor;

namespace Probotbor.Core.Models.Plc
{
    public class PlcIndicationModel
    {
        public bool IsInitialized { get; }
        public ProbotborSettings ProbotborSettings { get; }
        public PlcIndicationModel(ProbotborSettings probotborSettings)
        {
            ProbotborSettings = probotborSettings;
            NakopitelStatus = new Parameter<short>(nameof(NakopitelStatus), "Статус накопителя", 0, 100) { IsOnlyRead = true };
            Kanistras = Enumerable.Range(0, ProbotborSettings.KanistraCnt).Select(i => new Kanistra(i)).ToList();
            NakopitelCurrentCell = new Parameter<short>(nameof(NakopitelCurrentCell), "Номер текущей ячейки накопителя", 1, (short)ProbotborSettings.KanistraCnt) { IsOnlyRead = true };
            NakopitelReady = new Parameter<bool>(nameof(NakopitelReady), "Готовность накопителя", false, true) { IsOnlyRead = true };
            NakopitelBusy = new Parameter<bool>(nameof(NakopitelBusy), "Накопитель занят", false, true) { IsOnlyRead = true };
            ProbotborReady1 = new Parameter<bool>(nameof(ProbotborReady1), "Готовность к отбору", false, true) { IsOnlyRead = true };
            ProbotborBusy1 = new Parameter<bool>(nameof(ProbotborBusy1), "Занят", false, true) { IsOnlyRead = true };
            ProbotborReady2 = new Parameter<bool>(nameof(ProbotborReady2), "Готовность к отбору", false, true) { IsOnlyRead = true };
            ProbotborBusy2 = new Parameter<bool>(nameof(ProbotborBusy2), "Занят", false, true) { IsOnlyRead = true };
            DryUnitReady = new Parameter<bool>(nameof(DryUnitReady), "Готовность блока предварительной сушки", false, true) { IsOnlyRead = true };
            DryUnitBusy = new Parameter<bool>(nameof(DryUnitBusy), "Блок предварительной сушки занят", false, true) { IsOnlyRead = true };
            PitatelReady = new Parameter<bool>(nameof(PitatelReady), "Готовность питателя", false, true) { IsOnlyRead = true };
            PitatelBusy = new Parameter<bool>(nameof(PitatelBusy), "Питатель занят", false, true) { IsOnlyRead = true };
            SysReturnReady = new Parameter<bool>(nameof(SysReturnReady), "Готовность возврата проб", false, true) { IsOnlyRead = true };
            SysReturnBusy = new Parameter<bool>(nameof(SysReturnBusy), "Система возврата проб занята", false, true) { IsOnlyRead = true };
            DelitelCycle = new Parameter<bool>(nameof(DelitelCycle), "Цикл делителя", false, true) { IsOnlyRead = true };
            CommonReady = new Parameter<bool>(nameof(CommonReady), "Общая готовность", false, true) { IsOnlyRead = true };
            GlobalError = new Parameter<bool>(nameof(GlobalError), "Наличие ошибок", false, true) { IsOnlyRead = true };
            ProbotborStatus1 = new Parameter<short>(nameof(ProbotborStatus1), "Статус первичного проботборника", 0, 100) { IsOnlyRead = true };            
            ProbotborStatus2 = new Parameter<short>(nameof(ProbotborStatus2), "Статус вторичного проботборника", 0, 100) { IsOnlyRead = true };
            PitatelStatus = new Parameter<short>(nameof(PitatelStatus), "Статус питателя", 0, 100) { IsOnlyRead = true };
            DryUnitStatus = new Parameter<short>(nameof(DryUnitStatus), "Статус блока предварительной сушки", 0, 100) { IsOnlyRead = true };
            SysReturnStatus = new Parameter<short>(nameof(SysReturnStatus), "Статус системы возврата проб", 0, 100) { IsOnlyRead = true };
            CurrentHumm = new Parameter<float>(nameof(CurrentHumm), "Текущая влажность, %", 0, 100) { IsOnlyRead = true };    
            DryUnitCurrentTemperature = new Parameter<float>(nameof(DryUnitCurrentTemperature), "Текущая температура в блоке сушки, C", 0, 100) { IsOnlyRead = true };
            DrobilkaStatus = new Parameter<short>(nameof(DrobilkaStatus), "Статус первичной дробилки", 0, 100) { IsOnlyRead = true };
            IstiratelStatus = new Parameter<short>(nameof(IstiratelStatus), "Статус истирателя", 0, 100) { IsOnlyRead = true };
            DelitelTimeCurrent = new Parameter<short>(nameof(DelitelTimeCurrent), "Текущее время делителя, с", 0, short.MaxValue) { IsOnlyRead = true };       
            TimeBeforeNextOtborHours = new Parameter<short>(nameof(TimeBeforeNextOtborHours), "Время до следующего отбора, часов", 0, 23) { IsOnlyRead = true };
            TimeBeforeNextOtborMinutes = new Parameter<short>(nameof(TimeBeforeNextOtborMinutes), "Время до следующего отбора, минут", 0, 59) { IsOnlyRead = true };       
            TimeBeforeNextOtborSeconds = new Parameter<short>(nameof(TimeBeforeNextOtborSeconds), "Время до следующего отбора, секунды", 0, 59) { IsOnlyRead = true };        
            MainProcessStatus = new Parameter<ushort>(nameof(MainProcessStatus), "Статус автоматиченского отбора", 0, 10);        
            CurrentPlcTimeYear = new Parameter<short>(nameof(CurrentPlcTimeYear), "Текущее время ПЛК, год", 0, 99) { IsOnlyRead = true };        
            CurrentPlcTimeMonth = new Parameter<short>(nameof(CurrentPlcTimeMonth), "Текущее время ПЛК, месяц", 1, 12   ) { IsOnlyRead = true };        
            CurrentPlcTimeDay= new Parameter<short>(nameof(CurrentPlcTimeDay), "Текущее время ПЛК, день", 1, 31) { IsOnlyRead = true };        
            CurrentPlcTimeHour = new Parameter<short>(nameof(CurrentPlcTimeHour), "Текущее время ПЛК, час", 0, 24) { IsOnlyRead = true };        
            CurrentPlcTimeMinute = new Parameter<short>(nameof(CurrentPlcTimeMinute), "Текущее время ПЛК, минута", 0, 59 ){ IsOnlyRead = true };        
            CurrentPlcTimeSecond = new Parameter<short>(nameof(CurrentPlcTimeSecond), "Текущее время ПЛК, секунда", 0, 59) { IsOnlyRead = true };        
            PitatelTimeCurrent = new Parameter<short>(nameof(PitatelTimeCurrent), "Текущее время питателя, с, с", 0, short.MaxValue) { IsOnlyRead = true };        
            IstiratelTimeCurrent = new Parameter<short>(nameof(IstiratelTimeCurrent), "Текущее время истирателя, с, с", 0, short.MaxValue) { IsOnlyRead = true };        
            SysReturnTimeCurrent = new Parameter<short>(nameof(SysReturnTimeCurrent), "Текущее время системы возврата проб, с", 0, short.MaxValue) { IsOnlyRead = true };        
            DryCurrentTime = new Parameter<short>(nameof(DryCurrentTime), "Текущее время сушки , с", 0, short.MaxValue) { IsOnlyRead = true };        
            IsInitialized = true;
        }        

        #region Статус накопителя
        public Parameter<short> NakopitelStatus { get; }
        #endregion
        #region Канистры накопителя
        public List<Kanistra> Kanistras { get; }
        #endregion
        #region Номер текущей ячейки накопителя
        public Parameter<short> NakopitelCurrentCell { get; }
        #endregion
        #region Готовность накопителя
        public Parameter<bool> NakopitelReady { get; }
        #endregion
        #region Накопитель занят
        public Parameter<bool> NakopitelBusy { get; }
        #endregion
        #region Готовность первичного проботборника
        public Parameter<bool> ProbotborReady1 { get; }
        #endregion
        #region Первичный проботборник занят
        public Parameter<bool> ProbotborBusy1 { get; }
        #endregion
        #region Готовность вторичного проботборника
        public Parameter<bool> ProbotborReady2 { get; }
        #endregion
        #region Вторичный проботборник занят
        public Parameter<bool> ProbotborBusy2 { get; }
        #endregion
        #region Готовность блока предварительной сушки
        public Parameter<bool> DryUnitReady { get; }
        #endregion
        #region Блок предварительной сушки занят
        public Parameter<bool> DryUnitBusy { get; }
        #endregion
        #region Готовность питателя
        public Parameter<bool> PitatelReady { get; }
        #endregion
        #region Питатель занят
        public Parameter<bool> PitatelBusy { get; }
        #endregion
        #region Готовность возврата проб
        public Parameter<bool> SysReturnReady { get; }
        #endregion
        #region Система возврата проб занята
        public Parameter<bool> SysReturnBusy { get; }
        #endregion
        #region Цикл делителя
        public Parameter<bool> DelitelCycle { get; }
        #endregion
        #region Общая готовность
        public Parameter<bool> CommonReady { get; } 
        #endregion
        #region Наличие ошибок
        public Parameter<bool> GlobalError { get; }
        #endregion
        #region Статус первичного проботборника
        public Parameter<short> ProbotborStatus1 { get; }
        #endregion
        #region Статус вторичного проботборника
        public Parameter<short> ProbotborStatus2 { get; } 
        #endregion
        #region Статус питателя
        public Parameter<short> PitatelStatus { get; }
        #endregion
        #region Статус блока предварительной сушки
        public Parameter<short> DryUnitStatus { get; } 
        #endregion
        #region Статус системы возврата проб
        public Parameter<short> SysReturnStatus { get; }
        #endregion
        #region Текущая влажность, %
        public Parameter<float> CurrentHumm { get; }
        #endregion
        #region Текущая температура в блоке сушки, C
        public Parameter<float> DryUnitCurrentTemperature { get; }
        #endregion
        #region Статус первичной дробилки
        public Parameter<short> DrobilkaStatus { get; }
        #endregion
        #region Статус истирателя
        public Parameter<short> IstiratelStatus { get; }
        #endregion
        #region Текущее время делителя, с
        public Parameter<short> DelitelTimeCurrent { get; }
        #endregion
        #region Время до следующего отбора, часов
        public Parameter<short> TimeBeforeNextOtborHours { get; }
        #endregion
        #region Время до следующего отбора, минут
        public Parameter<short> TimeBeforeNextOtborMinutes { get; }
        #endregion
        #region Время до следующего отбора, секунды
        public Parameter<short> TimeBeforeNextOtborSeconds { get; } 
        #endregion
        #region Статус цикла отбора
        public Parameter<ushort> MainProcessStatus { get; }
        #endregion
        #region Текущее время ПЛК, год
        public Parameter<short> CurrentPlcTimeYear { get; }
        #endregion
        #region Текущее время ПЛК, месяц
        public Parameter<short> CurrentPlcTimeMonth { get; }
        #endregion
        #region Текущее время ПЛК, день
        public Parameter<short> CurrentPlcTimeDay { get; }
        #endregion
        #region Текущее время ПЛК, час
        public Parameter<short> CurrentPlcTimeHour { get; }
        #endregion
        #region Текущее время ПЛК, минута
        public Parameter<short> CurrentPlcTimeMinute { get; }
        #endregion
        #region Текущее время ПЛК, секунда
        public Parameter<short> CurrentPlcTimeSecond { get; }
        #endregion
        #region Текущее время питателя, с
        public Parameter<short> PitatelTimeCurrent { get; }
        #endregion
        #region Текущее время истирателя, с
        public Parameter<short> IstiratelTimeCurrent { get; } 
        #endregion
        #region Текущее время системы возврата проб, с
        public Parameter<short> SysReturnTimeCurrent { get; }
        #endregion
        #region Текущее время сушки , с
        public Parameter<short> DryCurrentTime { get; } 
        #endregion
    }
}

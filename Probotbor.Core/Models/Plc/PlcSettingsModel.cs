using Probotbor.Core.Models.Communication;

namespace Probotbor.Core.Models.Plc
{
    public  class PlcSettingsModel
    {
        public PlcSettingsModel()
        {
            KanistraSv  = new Parameter<ushort>(nameof(KanistraSv), "Уставка проб в канистру, шт", 1, 20);
            NakopitelVolume = new Parameter<short>(nameof(NakopitelVolume), "Обьем накопителя, канистр", 1, 16);
            TimeForKanistra = new Parameter<ushort>(nameof(TimeForKanistra), "Время сбора в одну канистру, мин", 4, 1440);
            ProbeId = new Parameter<string>(nameof(ProbeId), "Id пробы", string.Empty, "ZZZZZZZZZZZZZ") { Length = 12, IsOnlyRead = true };
            AutoMode = new Parameter<bool>(nameof(AutoMode), "Автоматичекий режим", false, true);
            FcFrequencesSvs = Enumerable.Range(0, 4).Select(i => new Parameter<short>(nameof(FcFrequencesSvs) + i + 1, $"Скорость ПЧ {i + 1}, Гц", 0, 50)).ToList();
            DryUnitDelayClose = new Parameter<short>(nameof(DryUnitDelayClose), "Блок осушителя - задержка перед закрытием шиберов, с", 0, 10);
            DryUnitDryTime = new Parameter<short>(nameof(DryUnitDryTime), "Блок осушителя - время сушки, с", 0, 1000);
            PitatelWorkTime = new Parameter<short>(nameof(PitatelWorkTime), "Время работы питателя в автоматическом режиме, с", 0, 1000);
            SysReturnWorkTime = new Parameter<short>(nameof(SysReturnWorkTime), "Время работы системы возврата проб в автоматическом режиме, с", 0, 1000);
            DelitelWorkTime = new Parameter<short>(nameof(DelitelWorkTime), "Время работы делителя в автоматическом режиме, с", 0, 1000);
            IstiratelWorkTime = new Parameter<short>(nameof(IstiratelWorkTime), "Время работы истирателя в автоматическом режиме, с", 0, 1000);
            DryUnitTemperatureSv = new Parameter<short>(nameof(DryUnitTemperatureSv), "Блок осушителя, уставка температуры, С", 20, 300);
            ProbotbornikTimeout1 = new Parameter<short>(nameof(ProbotbornikTimeout1), "Тайм-аут движения ковша проботборника 1, c", 1, 100);
            ProbotbornikTimeout2  = new Parameter<short>(nameof(ProbotbornikTimeout2), "Тайм-аут движения ковша проботборника 2, c", 1, 100);
        }



        #region Уставка проб в канистру, шт
        public Parameter<ushort> KanistraSv { get; }
        #endregion
        #region Канстр в накопителе, шт
        public Parameter<short> NakopitelVolume { get; } 
        #endregion
        #region Время сбора в одну канистру, мин
        public Parameter<ushort> TimeForKanistra { get; }
        #endregion
        #region Id пробы
        public Parameter<string> ProbeId { get; }
        #endregion
        #region Автоматичекий режим
        public Parameter<bool> AutoMode { get; } 
        #endregion
        #region Скорости ПЧ
        public List<Parameter<short>> FcFrequencesSvs { get; }
        #endregion
        #region Блок осушителя - задержка перед закртием шиберов
        public Parameter<short> DryUnitDelayClose { get; }
        #endregion
        #region Блок осушителя - время сушки, с
        public Parameter<short> DryUnitDryTime { get; } 
        #endregion
        #region Время работы питателя в автоматическом режиме, с
        public Parameter<short> PitatelWorkTime { get; } 
        #endregion
        #region Время работы системы возврата проб в автоматическом режиме, с
        public Parameter<short> SysReturnWorkTime { get; } 
        #endregion
        #region Время работы делителя в автоматическом режиме, с
        public Parameter<short> DelitelWorkTime { get; } 
        #endregion
        #region Время работы истирателя в автоматическом режиме, с
        public Parameter<short> IstiratelWorkTime { get; } 
        #endregion
        #region Блок осушителя, уставка температуры, С
        public Parameter<short> DryUnitTemperatureSv { get; }
        #endregion
        #region Тайм-аут движения ковша проботборника 1
        public Parameter<short> ProbotbornikTimeout1 { get; }
        #endregion
        #region Тайм-аут движения ковша проботборника 2
        public Parameter<short> ProbotbornikTimeout2 { get; }
        #endregion
    }
}

using Probotbor.Core.Models.Communication;

namespace Probotbor.Core.Models.Plc
{
    public class PlcDiModel
    {
        public PlcDiModel()
        {            
            SqProbHomeAbort1 = new Parameter<bool>(nameof(SqProbHomeAbort1), "Аварийный датчик начального положения", false, true) { IsOnlyRead = true};        
            SqProbHome1 = new Parameter<bool>(nameof(SqProbHome1), "Датчик начального положения", false, true) { IsOnlyRead = true};        
            SqProbWorkAbort1 = new Parameter<bool>(nameof(SqProbWorkAbort1), "Аварийный датчик рабочего положения", false, true) { IsOnlyRead = true};        
            SqProbWork1  = new Parameter<bool>(nameof(SqProbWork1), "Датчик рабочего положения", false, true) { IsOnlyRead = true};        
            SqProbHomeAbort2  = new Parameter<bool>(nameof(SqProbHomeAbort2), "Аварийный датчик начального положения", false, true) { IsOnlyRead = true};        
            SqProbHome2 = new Parameter<bool>(nameof(SqProbHome2), "Датчик начального положения", false, true) { IsOnlyRead = true};       
            SqProbWorkAbort2 = new Parameter<bool>(nameof(SqProbWorkAbort2), "Аварийный датчик рабочего положения", false, true) { IsOnlyRead = true };        
            SqProbWork2 = new Parameter<bool>(nameof(SqProbWork2), "Датчик рабочего положения", false, true) { IsOnlyRead = true };        
            SqDryHighShiberOpened = new Parameter<bool>(nameof(SqDryHighShiberOpened), "Верхний шибер открыт", false, true) { IsOnlyRead = true };        
            SqDryHighShiberClosed = new Parameter<bool>(nameof(SqDryHighShiberClosed), "Верхний шибер закрыт", false, true) { IsOnlyRead = true };        
            SqDryLowShiberOpened = new Parameter<bool>(nameof(SqDryLowShiberOpened), "Нижний шибер открыт", false, true) { IsOnlyRead = true };        
            SqDryLowShiberClosed = new Parameter<bool>(nameof(SqDryLowShiberClosed), "Нижний шибер закрыт", false, true) { IsOnlyRead = true };        
            SqBarabanLowPosition = new Parameter<bool>(nameof(SqBarabanLowPosition), "Барабан внизу", false, true) { IsOnlyRead = true };        
            SqBarabanHighPosition = new Parameter<bool>(nameof(SqBarabanHighPosition), "Барабан вверху", false, true) { IsOnlyRead = true };        
            SqDoorNakopitel = new Parameter<bool>(nameof(SqDoorNakopitel), "Датчик двери", false, true) { IsOnlyRead = true};        
            SqNakopFullRev = new Parameter<bool>(nameof(SqNakopFullRev), "Датчик полного оборота", false, true) { IsOnlyRead = true };        
            SqNakopCell = new Parameter<bool>(nameof(SqNakopCell), "Датчик ячейки", false, true) { IsOnlyRead = true };       
            SqNakopKanistra = new Parameter<bool>(nameof(SqNakopKanistra), "Датчик канистры", false, true) { IsOnlyRead = true };
            ProbDriveReady1 = new Parameter<bool>(nameof(ProbDriveReady1), "Неисправность привода", false, true);
            PitatelDriveReady = new Parameter<bool>(nameof(PitatelDriveReady), "Неисправность привода", false, true);
            DrobilkalDriveReady = new Parameter<bool>(nameof(DrobilkalDriveReady), "Неисправность привода", false, true);
    }


        #region Проботборник 1 - аварийный датчик начального положения
        public Parameter<bool> SqProbHomeAbort1 { get; } 
        #endregion
        #region Проботборник 1 - датчик начального положения
        public Parameter<bool> SqProbHome1 { get; } 
        #endregion
        #region Проботборник 1 - аварийный датчик рабочего положения
        public Parameter<bool> SqProbWorkAbort1 { get; } 
        #endregion
        #region Проботборник 1 - датчик рабочего положения
        public Parameter<bool> SqProbWork1 { get; } 
        #endregion

        #region Проботборник 2 - аварийный датчик начального положения
        public Parameter<bool> SqProbHomeAbort2 { get; } 
        #endregion
        #region Проботборник 2 - датчик начального положения
        public Parameter<bool> SqProbHome2 { get; } 
        #endregion
        #region Проботборник 2 - аварийный датчик рабочего положения
        public Parameter<bool> SqProbWorkAbort2 { get; } 
        #endregion
        #region Проботборник 2 - датчик рабочего положения
        public Parameter<bool> SqProbWork2 { get; } 
        #endregion


        #region Сушка  - верхний шибер открыт
        public Parameter<bool> SqDryHighShiberOpened { get; } 
        #endregion
        #region Сушка  - верхний шибер закрыт
        public Parameter<bool> SqDryHighShiberClosed { get; } 
        #endregion
        #region Сушка  - нижний шибер открыт
        public Parameter<bool> SqDryLowShiberOpened { get; } 
        #endregion
        #region Сушка  - нижний шибер закрыт
        public Parameter<bool> SqDryLowShiberClosed { get; } 
        #endregion
        #region Сушка  - барабан внизу
        public Parameter<bool> SqBarabanLowPosition { get; }
        #endregion
        #region Сушка  - барабан вверху
        public Parameter<bool> SqBarabanHighPosition { get; }
        #endregion

        #region Накопитель - датчик двери
        public Parameter<bool> SqDoorNakopitel { get; } 
        #endregion
        #region Накопитель - датчик полного оборота
        public Parameter<bool> SqNakopFullRev { get; }
        #endregion
        #region Накопитель - датчик ячейки
        public Parameter<bool> SqNakopCell { get; } 
        #endregion
        #region Накопитель - датчик канистры
        public Parameter<bool> SqNakopKanistra { get; }
        #endregion

        #region Проботбоник 1 - готовность привода
        public Parameter<bool> ProbDriveReady1 { get; }
        #endregion

        #region Питатель  1 - готовность привода
        public Parameter<bool> PitatelDriveReady { get; }
        #endregion

        #region Дробилка  - готовность привода
        public Parameter<bool> DrobilkalDriveReady { get; }
        #endregion
    }
}

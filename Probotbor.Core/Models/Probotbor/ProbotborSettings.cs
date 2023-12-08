namespace Probotbor.Core.Models.Probotbor
{
    public class ProbotborSettings
    {
        /// <summary>
        /// Тип первичного проботборника
        /// </summary>
        public ProbotborType PrimaryType { get; set; }
        public bool PitatelExist { get; set; }
        public bool DrobilkaExist { get; set; }
        public bool SecondaryExist { get; set; }
        /// <summary>
        /// Тип вторичного проботбооника
        /// </summary>
        public ProbotborType SecondaryType { get; set; }
        public bool ReturnExist { get; set; }
        public bool DryUnitExist { get; set; }
        public bool HumidMeasureExist { get; set; }
        public bool IstiratelExist { get; set; }
        public bool NakopitelExist { get; set; }
        public int KanistraCnt { get; set; } 

    }
}

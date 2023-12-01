using CommunityToolkit.Mvvm.ComponentModel;
using Probotbor.Core.Infrastructure.DataAccess;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Probotbor.Core.Models.Communication;


public partial class ParameterBase : EntityCommon
{
    public ParameterType ParType { get; set; }

    [ObservableProperty]
    private string _name = string.Empty;

    [ObservableProperty]
    [Required]
    [NotifyDataErrorInfo]
    private string _description  = string.Empty;

    [ObservableProperty]
    private ModbusRegType _modbusRegType;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Range(0, int.MaxValue)]
    private int _regNum;

    [ObservableProperty]
    [Range(0, 15)]
    [NotifyDataErrorInfo]
    private int _bitNum;

    /// <summary>
    /// Db number (Siemens only)
    /// </summary>
    [ObservableProperty]
    [Range(0, int.MaxValue)]
    [NotifyDataErrorInfo]
    private int _dbNum;

    /// <summary>
    /// Number of byte (Siemens only)
    /// </summary>
    [ObservableProperty]
    [Range(0, int.MaxValue)]
    [NotifyDataErrorInfo]
    private int _byteNum;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Range(0, int.MaxValue)]
    private int _length;

    [ObservableProperty]
    public bool _isRequired;

}

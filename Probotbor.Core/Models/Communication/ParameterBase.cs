using CommunityToolkit.Mvvm.ComponentModel;
using Probotbor.Core.Infrastructure.DataAccess;

namespace Probotbor.Core.Models.Communication;

public partial class ParameterBase : EntityCommon
{
    [ObservableProperty]
    private string _name = string.Empty;
    [ObservableProperty]
    private string _description  = string.Empty;
    [ObservableProperty]
    private ModbusRegType _modbusRegType;
    [ObservableProperty]
    private int _regNum;
    [ObservableProperty]
    private int _bitNum;
    /// <summary>
    /// Db number (Siemens only)
    /// </summary>
    [ObservableProperty]
    private int _dbNum;
    /// <summary>
    /// Number of byte (Siemens only)
    /// </summary>
    [ObservableProperty] 
    private int _byteNum;
    [ObservableProperty]
    private int _length;
    [ObservableProperty]
    public bool _isRequired;

}

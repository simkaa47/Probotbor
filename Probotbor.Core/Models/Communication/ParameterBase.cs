using Probotbor.Core.Infrastructure.DataAccess;

namespace Probotbor.Core.Models.Communication;

public class ParameterBase : EntityCommon
{    
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ModbusRegType ModbusRegType { get; set; }
    public int RegNum { get; set; }
    public int BitNum { get; set; }
    /// <summary>
    /// Db number (Siemens only)
    /// </summary>
    public int DbNum { get; set; }
    /// <summary>
    /// Number of byte (Siemens only)
    /// </summary>
    public int ByteNum { get; set; }

    public int Length { get; set; }
    public bool IsRequired { get; set; }

}

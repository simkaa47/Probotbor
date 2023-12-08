using CommunityToolkit.Mvvm.ComponentModel;
using Probotbor.Core.Attribites;
using Probotbor.Core.Models.Plc;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Timers;

namespace Probotbor.Core.Models.Communication;

public partial class Parameter<T> : ParameterBase, INotifyDataErrorInfo where T : IComparable
{
    public Parameter(string name, string description, T minValue, T maxValue)
    {
        Name = name;
        Description = description;
        MinValue = minValue;
        MaxValue = maxValue;
        timer = new System.Timers.Timer(5000);
        timer.Elapsed += OnTimerElapsed;
        PlcModel.Parameters.Add(this);
    }

    public string TypeName => typeof(T).ToString();

    public T MinValue { get; }
    public T MaxValue { get; }

    #region Таймер
    System.Timers.Timer timer;


    void RestartTimer()
    {
        if (timer.Enabled) timer.Stop();
        timer.Start();
    }

    void OnTimerElapsed(object? source, ElapsedEventArgs e)
    {
        timer.Stop();
        WriteValue = Value;
    }


    #endregion

    [ObservableProperty]
    private bool _isOnlyRead;

    [ObservableProperty]
    private bool _validationOk;

    [ObservableProperty]
    private bool _isWriting;

    private T _value;
    public T Value
    {
        get => _value;
        set
        {
            if (SetProperty(ref _value, value))
                WriteValue = value;
            IsWriting = false;
        }
    }

    T _writeValue;

    
    [InDiapasone(nameof(MinValue), nameof(MaxValue))]
    public T WriteValue
    {
        get => _writeValue;
        set
        {
            if (value != null)
            {
                ValidateProperty(value, nameof(WriteValue));
                if (value.CompareTo(Value) != 0)
                {
                    RestartTimer();
                }                
                SetProperty(ref _writeValue, value);
                ValidationOk = _writeValue.CompareTo(MinValue)>=0 && _writeValue.CompareTo(MaxValue)<=0;
            }
        }
    }    

}

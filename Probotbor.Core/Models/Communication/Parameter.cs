using CommunityToolkit.Mvvm.ComponentModel;
using Probotbor.Core.Models.Plc;
using System.Collections;
using System.ComponentModel;
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

    public T WriteValue
    {
        get => _writeValue;
        set
        {
            if (value != null)
            {
                ValidationOk = true;
                ClearError(nameof(WriteValue));
                if (value.CompareTo(MinValue) < 0 || value.CompareTo(MaxValue) > 0)
                {
                    AddError(nameof(WriteValue), $"Input value ({value}) must be between {MinValue} and {MaxValue}");
                }
                if (value.CompareTo(Value) != 0)
                {
                    RestartTimer();
                }
                SetProperty(ref _writeValue, value);

            }
        }
    }


    private readonly Dictionary<string, List<string>> _propertyErrors = new Dictionary<string, List<string>>();
    public bool HasErrors => _propertyErrors.Any();
    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

    public IEnumerable GetErrors(string? propertyName)
    {        
        return _propertyErrors.GetValueOrDefault(propertyName, null);
    }

    public void AddError(string propertyName, string message)
    {
        if (!_propertyErrors.ContainsKey(propertyName))
        {
            _propertyErrors.Add(propertyName, new List<string>());
        }
        _propertyErrors[propertyName].Add(message);
        OnPropertyChanged(propertyName);
        ValidationOk = false;
    }

    public void ClearError(string propertyName)
    {
        if (_propertyErrors.Remove(propertyName))
        {
            OnErrorsChanged(propertyName);
        }
    }

    private void OnErrorsChanged(string propertyName)
    {
        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
    }

}

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Probotbor.Core.Contracts.Communication;
using Probotbor.Core.Contracts.DataAccess;
using Probotbor.Core.Contracts.Dialog;
using Probotbor.Core.Infrastructure.DataAccess;
using Probotbor.Core.Models.Communication;
using Probotbor.Core.Models.Events;
using Probotbor.Core.Models.Plc;
using Probotbor.Core.Models.Probotbor;
using System.ComponentModel;
using System.Runtime.Intrinsics.Arm;

namespace Probotbor.Core.ViewModels
{
    public partial class ParametersVm : ObservableObject
    {
        private readonly IRepository<ParameterBase> _parametersRepository;
        private readonly DbContext _dbContext;
        private readonly ILogger<ParametersVm> _logger;
        private readonly IErrorDialog _errorDialog;
        private readonly IQuestionDialog _questionDialog;
        [ObservableProperty]
        private bool _isLoaded;


        public PlcModel PlcModel { get; set; }
        public bool IsInitialized { get; private set; }
        public ProbotborSettings ProbotborSettings { get; set; }

        public ParametersVm(IRepository<ParameterBase> parametersRepository, 
            ApplicationContext dbContext,
            ILogger<ParametersVm> logger, 
            IOptions<ProbotborSettings> options, 
            IErrorDialog errorDialog, 
            IQuestionDialog _questionDialog)
        {
            ProbotborSettings = options.Value;
            PlcModel = new PlcModel(ProbotborSettings);
            _parametersRepository = parametersRepository;
            _dbContext = dbContext;
            _logger = logger;
            _errorDialog = errorDialog;
            this._questionDialog = _questionDialog;
            SychroParametersWithDb();
            GetErrors();
            IsInitialized = true;

        }

        [ObservableProperty]
        private List<object>? _parameters;

        [ObservableProperty]
        private List<Error>? _errors = new List<Error>();

        private async void SychroParametersWithDb()
        {
            var pars = new List<object>();
            try
            {
                foreach (var parBase in PlcModel.Parameters)
                {
                    if (parBase != null && parBase.ParType == ParameterType.Parameter)
                    {
                        var parDb = await _dbContext.Set<ParameterBase>().AsNoTracking().FirstOrDefaultAsync(p => p.Name == parBase.Name);
                        if (parDb == null)
                        {
                            parBase.Id = 0;
                            await _parametersRepository.AddAsync(parBase);
                        }
                        else
                        {
                            parDb.Adapt(parBase);
                        }
                        pars.Add(parBase);
                    }
                }
                Parameters = pars;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Синхронизация параметров с БД - \"{ex.Message}\"");
            }
        }


        private async void GetErrors()
        {
            try
            {                
                var errors = await _dbContext.Set<ParameterBase>().
                    AsNoTracking().
                    Where(p => p.ParType == ParameterType.Error).
                    ToListAsync();
                foreach (var e in errors)
                {
                    var err = new Error(e.Name, e.Description, false, true) { IsOnlyRead = false };                    
                    e.Adapt(err);
                    Errors?.Add(err);
                }
                DescribeOnErrors();

            }
            catch (Exception ex)
            {
                _logger.LogError($"Загрузка конфигурации ошибок из БД - \"{ex.Message}\"");
            }
        }

        [RelayCommand]
        private async Task SaveParameters()
        {
            IsLoaded = true;
            if (Parameters is null) return;
            var pars = Parameters.Where(p => p is ParameterBase)
                .Select(p => p as ParameterBase).ToList();
            foreach (var par in pars)
            {
                if (par == null) continue;
                try
                {
                    _dbContext.Entry(par).State = EntityState.Modified;
                    await _dbContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Сохранение параметра {par.Name} - {ex.Message}");

                }
            }
            IsLoaded = false;
        }

        [RelayCommand]
        private async Task AddErrorAsync()
        {
            var par = new Error("", "", false, true) { IsOnlyRead = true, ParType = ParameterType.Error };
            if (!await _errorDialog.ShowDialog(par)) return;
            try
            {
                if (par.HasErrors)
                {
                    throw new Exception("New error adding: validation error");
                }
                _logger.LogInformation($"Выполняется добавление ошибки {par.Description}");
                await _dbContext.Set<ParameterBase>().AddAsync(par);
                await _dbContext.SaveChangesAsync();
                if (Errors is not null)
                {
                    UnsubscribeOnErrors();
                    Errors.Add(par);
                    DescribeOnErrors();
                    Errors = new List<Error>(Errors);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Добавление ошибки {par.Name} - {ex.Message}");
            }
        }

        [RelayCommand]
        private async Task DeleteErrorAsync(object obj)
        {
            if (!(obj is Error err)) return;
            try
            {
                if(await _questionDialog.Ask("Удаление ошибки", "Удалить ошибку?"))
                {
                    _logger.LogInformation($"Выполняется добавление ошибки {err.Description}");
                    await _parametersRepository.DeleteAsync(err);
                    if (Errors is not null)
                    {
                        UnsubscribeOnErrors();
                        Errors.Remove(err);
                        DescribeOnErrors();
                        Errors = new List<Error>(Errors);
                    }

                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Удаление ошибки {err.Name} - {ex.Message}");
            }
        }


        [RelayCommand]
        private async Task SaveErrorsAsync()
        {            
            if (Errors is null) return;
            var errs = Errors.Where(p => p is ParameterBase)
                .Select(p => p as ParameterBase).ToList();
            foreach (var err in errs)
            {
                if (err == null) continue;
                try
                {
                    _dbContext.Entry(err).State = EntityState.Modified;
                    await _dbContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Сохранение ошибки {err.Name} - {ex.Message}");

                }
            }
            IsLoaded = false;
        }

        private void UnsubscribeOnErrors()
        {
            if (Errors is null) return;
            foreach (var err in Errors)
            {
                err.PropertyChanged -= OnErrPropertyChanged;
            }

        }

        private void DescribeOnErrors()
        {
            if (Errors is null) return;
            foreach (var err in Errors)
            {
                err.PropertyChanged += OnErrPropertyChanged;
            }

        }

        private void OnErrPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == "Value" && sender is Error err && err.Value == true)
            {
                err.LastTimeExecute = DateTime.Now;
            }
        }

        

    }
}

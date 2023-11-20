using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Probotbor.Core.Contracts.DataAccess;
using Probotbor.Core.Infrastructure.DataAccess;
using Probotbor.Core.Models.Communication;
using Probotbor.Core.Models.Plc;
using Probotbor.Core.Services.Plc;

namespace Probotbor.Core.ViewModels
{
    public partial class ParametersVm : ObservableObject
    {
        private readonly IRepository<ParameterBase> _parametersRepository;
        private readonly DbContext _dbContext;
        private readonly PlcMainService _plcMainService;
        private readonly ILogger<ParametersVm> _logger;

        [ObservableProperty]
        private bool _isLoaded;

        public bool IsInitialized { get; private set; }

        public ParametersVm(IRepository<ParameterBase> parametersRepository, ApplicationContext dbContext,
            PlcMainService plcMainService,
            ILogger<ParametersVm> logger)
        {
            _parametersRepository = parametersRepository;
            _dbContext = dbContext;
            _plcMainService = plcMainService;
            _logger = logger;
            SychroParametersWithDb();
        }

        [ObservableProperty]
        private List<object>? _parameters;

        private async void SychroParametersWithDb()
        {
            var pars = new List<object>();
            try
            {

                if (_plcMainService.Initialized)
                {
                    foreach (var par in PlcModel.Parameters)
                    {
                        var parBase = par as ParameterBase;
                        if (parBase != null)
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

                        }
                        pars.Add(par);
                    }
                }
                Parameters = pars;
                IsInitialized = true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Синхронизация параметров с БД - \"{ex.Message}\"");
            }
        }


        public ParameterBase ekjdked;

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

    }
}

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Probotbor.Core.Contracts.Communication;
using Probotbor.Core.Models.Communication;
using Probotbor.Core.Models.Plc;
using Probotbor.Core.Models.Probotbor;
using Probotbor.Core.ViewModels;
using System.Collections.Generic;

namespace Probotbor.Core.Services.Plc
{
    public class PlcMainService
    {
        private readonly ILogger<PlcMainService> _logger;
        private readonly ICommunicationService _communicationService;
        public PlcModel? PlcModel { get; set; }

        private Queue<object> WriteCommands { get; } = new Queue<object>();
        public bool Initialized { get; }
        public bool Connected { get; private set; }
        public ProbotborSettings ProbotborSettings { get; set; }
        public PlcMainService(IOptions<ProbotborSettings> options, ILogger<PlcMainService> logger,
            ICommunicationService communicationService, ParametersVm parametersVm)
        {
            ProbotborSettings = options.Value;            
            Initialized = true;
            _logger = logger;
            _communicationService = communicationService;
            if (parametersVm.IsInitialized)
                PlcModel = parametersVm.PlcModel;
            ReadProcess();
        }

        private  async void  ReadProcess()
        {
            await Task.Run(() =>
            {

                while (_communicationService != null)
                {
                    try
                    {                        
                        Connected = _communicationService.Connected;
                        if (!Connected)
                            Thread.Sleep(2000);
                        while (WriteCommands.Count > 0)
                        {
                            var par = WriteCommands.Dequeue();
                            _communicationService.WriteParameter(par);
                        }
                        _communicationService.ReadAllData();
                        Thread.Sleep(100);

                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message);
                    }
                }
            });
        }

        public void WriteParameter(object par)
        {
            if (par is Parameter<ushort> parUshort)
                SetIsWritingFlag(parUshort);
            else if (par is Parameter<short> parShort)
                SetIsWritingFlag(parShort);
            else if (par is Parameter<bool> parBool)
                SetIsWritingFlag(parBool);
            else if (par is Parameter<int> parInt)
                SetIsWritingFlag(parInt);
            else if (par is Parameter<uint> parUint)
                SetIsWritingFlag(parUint);
            else if (par is Parameter<float> parFloat)
                SetIsWritingFlag(parFloat);
            else if (par is Parameter<string> parstring)
                SetIsWritingFlag(parstring);
        }

        private void SetIsWritingFlag<T>(Parameter<T> parameter) where T : IComparable
        {
            if(parameter.ValidationOk && Connected)
            {
                parameter.IsWriting = true;
                WriteCommands.Enqueue(parameter);   
            }
        }
    }
}

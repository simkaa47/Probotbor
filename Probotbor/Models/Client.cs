using Probotbor.Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Sharp7;
namespace Probotbor.Models
{
    class Client
    {
        #region Делегаты и события
        
        #region UpdateDataHandler - вызывается при обновлении данных
        private delegate void UpdateDataHandler();
        UpdateDataHandler UpdateDel;
        #endregion

        #region ConnEvent - событие изменения статуса подлкючения
        public delegate void ConnDiscHandler(string address, bool connect);
        public event ConnDiscHandler ConnEvent;
        #endregion

        #region Необходмо добавить запись в историю события
        public delegate void EventIsActive(EventConfig config);
        public event EventIsActive EventEvent;
        #endregion

        #endregion

        #region Ip адрес
        private string ip;
        public string Ip
        {
            get
            {
                if (ip != null) return ip;
                else
                {
                    if (CheckIP(XML_data.GetPlcAdress())) return ip = XML_data.GetPlcAdress();
                    else
                    {
                        var address = "192.168.1.178";
                        XML_data.SetPlcAdress(address);
                        return ip = address;
                    }
                }
            }
            set
            {
                if (CheckIP(value))
                {
                    ip = value;
                    XML_data.SetPlcAdress(value);
                }
            }
        }
        #endregion

        #region S7 клиент
        private S7Client s7Client { get; set; } = new S7Client() {RecvTimeout=500, SendTimeout=500, ConnTimeout=500};
        #endregion

        #region Стэк данных для записи
        private Stack<WriteData> WriteStack = new Stack<WriteData>();
        #endregion

        #region Статус соединения Connected
        public bool Connected { private set; get; }
        #endregion

        #region Требование закрыть процесс
        private bool StopProcessReq { get; set; } = false;
        #endregion

        #region Метод СheckIP
        /// <summary>
        /// Функция проверки строки адреса ПЛК Siemens
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public bool CheckIP(string ip)
        {
            var arrStr = ip.Split('.');
            int temp = 0;
            if (arrStr.Length != 4) return false;
            foreach (var item in arrStr)
            {
                if (!int.TryParse(item, out temp)) return false;
                if ((temp < 0) || (temp > 255)) return false;
            }
            return true;
        }
        XML_data xml;

        #endregion

        #region Метод ChangeXML
        ///
        public void ChangeXML(int id, string property, string value)
        {
            XML_data.SetParamCell(id, property, value);
        }
        #endregion

        #region Данные о ячейках ПЛК
        private ObservableCollection<DataCell> dataCells;
        public ObservableCollection<DataCell> DataCells
        {
            get
            {
                if (dataCells == null)
                {
                    dataCells = new ObservableCollection<DataCell>();// создаем коллекцию
                    dataCells.CollectionChanged += UpdateDataCells;// подписываемся на каждое изменение колллекции
                    XML_data.GetParamCellXML(DataCells);// Загрузка из XML данных параметров
                }
                return dataCells;
            }
            set => dataCells = value; 
        }
        #endregion

        #region Обновление коллекции ячеек
        private void UpdateDataCells(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            { 
                (e.NewItems[0] as DataCell).ChangeValueHandlerDel = EditParamCell;
                (e.NewItems[0] as DataCell).WriteValueHandlerDel = WriteParameter;
            }
        }

        #endregion

        #region Изменение в коллекции параметров
        private void EditParamCell(int id, string propertyName, string properyValue)
        {
            ChangeXML(id, propertyName, properyValue);
            UpdateDel += GetReadConfig;
        }
        #endregion

        #region Данные об индикаторах
        private ObservableCollection<IndicatorConfig> indicatorConfigs;
        public ObservableCollection<IndicatorConfig> IndicatorConfigs
        {
            get
            {
                if (indicatorConfigs == null)
                {
                    indicatorConfigs = new ObservableCollection<IndicatorConfig>();//создаем коллекцию
                    indicatorConfigs.CollectionChanged += FollowChangeIndicator;// подписываемся на каждое изменение колллекции
                    XML_data.GetIndicatorConfigXML(IndicatorConfigs);// Загрузка из XML данных индикаторов

                }
                return indicatorConfigs;
            }
            set => indicatorConfigs = value;
        }
        #endregion

        #region Подписка на каждый делегат элемента коллеции конфигурации индикаторов
        private void FollowChangeIndicator(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                (e.NewItems[0] as IndicatorConfig).ChangeValueHandlerDel = EditIndicatorXML;
            }
        }

        #endregion

        #region Данные об кнопках
        ObservableCollection<ButtonConfig> buttonConfigs;
        public ObservableCollection<ButtonConfig> ButtonConfigs
        {
            get
            {
                if (buttonConfigs == null)
                {
                    buttonConfigs = new ObservableCollection<ButtonConfig>();//создаем коллекцию
                    buttonConfigs.CollectionChanged += FollowChangeButton;// подписываемся на каждое изменение колллекции
                    XML_data.GetButtonConfigXML(ButtonConfigs);// Загрузка из XML данных индикаторов

                }
                return buttonConfigs;
            }
            set => buttonConfigs = value;
        }
        #endregion

        #region Статусы

        #region Статусы накопителя
        private ObservableCollection<StatusDevice> nakopStatuses;
        public ObservableCollection<StatusDevice> NakopStatuses
        {
            get
            {
                if (nakopStatuses == null)
                {
                    nakopStatuses = new ObservableCollection<StatusDevice>();                    
                    XML_data.GetStatus(NakopStatuses, "Nakopitel");
                    nakopStatuses.CollectionChanged += (obj,e)=>ChangeStatus(obj,e, "Nakopitel");
                    for (int i = 0; i < nakopStatuses.Count; i++)
                    {
                        nakopStatuses[i].ChangeValueHandlerDel = (status) => XML_data.EditStatus(status, "Nakopitel");// делегат изменения свойства
                    }
                }
                return nakopStatuses;
            }
            set => nakopStatuses = value;
        }
        
        #endregion

        #region Статусы системы
        private ObservableCollection<StatusDevice> sysStatuses;
        public ObservableCollection<StatusDevice> SysStatuses
        {
            get
            {
                if (sysStatuses == null)
                {
                    sysStatuses = new ObservableCollection<StatusDevice>();
                    XML_data.GetStatus(SysStatuses, "System");
                    sysStatuses.CollectionChanged += (obj, e) => ChangeStatus(obj, e, "System"); ;
                    for (int i = 0; i < sysStatuses.Count; i++)
                    {
                        sysStatuses[i].ChangeValueHandlerDel = (status) => XML_data.EditStatus(status, "System");// делегат изменения свойства
                    }
                }
                return sysStatuses;
            }
            set => sysStatuses = value;
        }

        #endregion

        #region Статусы проботбора
        private ObservableCollection<StatusDevice> probStatuses;
        public ObservableCollection<StatusDevice> ProbStatuses
        {
            get
            {
                if (probStatuses == null)
                {
                    probStatuses = new ObservableCollection<StatusDevice>();
                    XML_data.GetStatus(ProbStatuses, "Probotbornik");
                    probStatuses.CollectionChanged += (obj, e) => ChangeStatus(obj, e, "Probotbornik"); ;
                    for (int i = 0; i < probStatuses.Count; i++)
                    {
                        probStatuses[i].ChangeValueHandlerDel = (status) => XML_data.EditStatus(status, "Probotbornik");// делегат изменения свойства
                    }
                }
                return probStatuses;
            }
            set => probStatuses = value;
        }

        #endregion

        #region Статусы бункера
        private ObservableCollection<StatusDevice> bunkStatuses;
        public ObservableCollection<StatusDevice>BunkStatuses
        {
            get
            {
                if (bunkStatuses == null)
                {
                    bunkStatuses = new ObservableCollection<StatusDevice>();
                    XML_data.GetStatus(BunkStatuses, "Bunker");
                    bunkStatuses.CollectionChanged += (obj, e) => ChangeStatus(obj, e, "Bunker"); ;
                    for (int i = 0; i < bunkStatuses.Count; i++)
                    {
                        bunkStatuses[i].ChangeValueHandlerDel = (status)=>XML_data.EditStatus(status,"Bunker");// делегат изменения свойства
                    }
                }
                return bunkStatuses;
            }
            set => bunkStatuses = value;
        }

        #endregion

        #region Статусы делителя
        private ObservableCollection<StatusDevice> delStatuses;
        public ObservableCollection<StatusDevice> DelStatuses
        {
            get
            {
                if (delStatuses == null)
                {
                    delStatuses = new ObservableCollection<StatusDevice>();
                    XML_data.GetStatus(DelStatuses, "Delitel");
                    delStatuses.CollectionChanged += (obj, e) => ChangeStatus(obj, e, "Delitel"); ;
                    for (int i = 0; i < delStatuses.Count; i++)
                    {
                        delStatuses[i].ChangeValueHandlerDel = (status) => XML_data.EditStatus(status, "Delitel");// делегат изменения свойства
                    }
                }
                return delStatuses;
            }
            set => delStatuses = value;
        }

        #endregion

        private void ChangeStatus(object sender, NotifyCollectionChangedEventArgs e, string name)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                var newStatus = e.NewItems[0] as StatusDevice;
                newStatus.Id = NakopStatuses.Count - 1;
                newStatus.ChangeValueHandlerDel = (status) => XML_data.EditStatus(status, name);// делегат изменения свойства
                XML_data.AddStatus(newStatus, name);
            }
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                for (int i = 0; i < e.OldItems.Count; i++)
                {
                    XML_data.RemoveStatus(e.OldItems[i] as StatusDevice, name);
                }
            }
        }
        #endregion

        #region Подписка на каждый делегат элемента коллеции конфигурации кнопок
        private void FollowChangeButton(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                (e.NewItems[0] as ButtonConfig).ChangeValueHandlerDel = EditButtonXML;
                (e.NewItems[0] as ButtonConfig).WriteValueHandlerDel = BitSwitch;
            }
        }
        #endregion

        #region Данные о конфигурации событий
        private ObservableCollection<EventConfig> eventConfigs;
        public ObservableCollection<EventConfig> EventConfigs
        {
            get
            {
                if (eventConfigs == null)
                {
                    eventConfigs = new ObservableCollection<EventConfig>();
                    eventConfigs.CollectionChanged += UpdateEventXML;
                    XML_data.GetEventConfig(EventConfigs);// Загрузка из XML конфигурации
                }
                return eventConfigs;
            }
            set => eventConfigs = value; 
        }
        #endregion

        #region Обновление конфигурации события
        private void UpdateEventXML(object sender, NotifyCollectionChangedEventArgs e)
        {

            XML_data.ClearEventConfig();
            for (int i = 0; i < EventConfigs.Count; i++)
            {
                EventConfigs[i].Id = i + 1;
                XML_data.AddEventConfig(EventConfigs[i]);
                EventConfigs[i].ChangeValueHandlerDel = EditEventXML;
            }
            UpdateDel += GetReadConfig;
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                (e.NewItems[0] as EventConfig).ChangeValueHandlerDel = EditEventXML;
                (e.NewItems[0] as EventConfig).EventIsActive += CallEventEvent;
            }
            
        }
        private void CallEventEvent(EventConfig eventConfig)
        {           
            EventEvent.Invoke(eventConfig);
        }
        private void EditEventXML(EventConfig config)
        {
            XML_data.EditEvent(config);
            UpdateDel += GetReadConfig;
        }
        #endregion

        #region Редактирование индикаторов в XML
        private void EditIndicatorXML(int Id, string property, string value)
        {
            XML_data.SetIndicatorConfig(Id, property, value);
            UpdateDel += GetReadConfig;
        }

        #endregion

        #region Редактирование кнопок в XML
        private void EditButtonXML(int Id, string property, string value)
        {
            XML_data.SetButtonConfig(Id, property, value);
            UpdateDel += GetReadConfig;
        }

        #endregion

        #region Словарь данных DictData
        Dictionary<int, DataBuffer> DictData = new Dictionary<int, DataBuffer>();
        #endregion

        

        #region Конструктор
        public Client()
        {    
           
            GetReadConfig();

        }
        #endregion

        #region Методы

        #region Прервать процесс
        public void CloseConnection()
        {
            StopProcessReq = true;
        }
        #endregion

        #region Старт процесса
        public void Start()
        {
            var threadProcess = new Thread(Process);
            threadProcess.Start();
        
        }
        #endregion

        #region Метод Process - основной поток
        private void Process()
        {
            while (!StopProcessReq)
            {
                int connResult = s7Client.ConnectTo(Ip, 0, 1);
                if (s7Client.Connected)
                {
                    ConnEvent?.Invoke(Ip, true);
                    Connected = true;
                }
                Thread.Sleep(1000);
                while (s7Client.Connected && !StopProcessReq)
                {
                    try
                    {
                        UpdateData();
                        ReadData();
                        ConvertData();
                        SetData();
                        
                    }
                    catch (Exception ex)
                    {
                        if (ConnEvent != null) ConnEvent(Ip, false);
                        s7Client.Disconnect();
                        Connected = false;

                    }
                }
                if (ConnEvent != null) ConnEvent(Ip, false);
                s7Client.Disconnect();
            Connected = false;
                
            }
        }
        #endregion

        #region Запись параметра
        public void WriteParameter(DataCell dataCell)
        {
            if (!Connected) return;
            WriteData writeData = new WriteData();
            writeData.DbNum = dataCell.DbNum;
            writeData.StartByte = dataCell.ByteOffset;

            switch (dataCell.DataType)
            {
                case "short":
                    writeData.Buffer = new byte[2];
                    S7.SetIntAt(writeData.Buffer, 0, (short)dataCell.WriteValue);
                    break;
                case "ushort":
                    writeData.Buffer = new byte[2];
                    S7.SetUIntAt(writeData.Buffer, 0, (ushort)dataCell.WriteValue);
                    break;
                case "int":
                    writeData.Buffer = new byte[4];
                    S7.SetDIntAt(writeData.Buffer, 0, (int)dataCell.WriteValue);
                    break;
                case "uint":
                    writeData.Buffer = new byte[4];
                    S7.SetUDIntAt(writeData.Buffer, 0, (uint)dataCell.WriteValue);
                    break;
                case "float":
                    writeData.Buffer = new byte[4];
                    S7.SetRealAt(writeData.Buffer, 0, (float)dataCell.WriteValue);
                    break;
                case "string":
                    writeData.Buffer = new byte[dataCell.Length];
                    S7.SetStringAt(writeData.Buffer, 0, dataCell.Length, dataCell.WriteValue.ToString());
                    break;
                default:
                    return;
            }
            WriteStack.Push(writeData);
        }
        #endregion

        #region Прочитать данные
        private void ReadData()
        {
            foreach (var item in DictData)
            {
                int result = s7Client.DBRead(item.Key, item.Value.startByte, item.Value.buffer.Length, item.Value.buffer);
                if (result != 0)
                {
                    throw new NotImplementedException();
                }
            }
        }
        #endregion

        #region Преобразовать данные
        public void ConvertData()
        {
            GetParams();//Получить данные параметров 
            GetEvents();//Получить данные событий
            GetIndicatorsValue(); // Получиьь данные индикаторов
            GetButtonsValue();// Получить данные кнопок
        }
        #endregion

        #region Получить данные параметров
        private void GetParams()
        {
            foreach (var item in DataCells)
            {
                item.GetValue(DictData[item.DbNum].buffer, DictData[item.DbNum].startByte);
            }
        }
        #endregion

        #region Получитьь данные событий
        private void GetEvents()
        {
            foreach (var item in EventConfigs)
            {
                item.GetEvent(DictData[item.DbNum].buffer, DictData[item.DbNum].startByte);
            }
        }
        #endregion

        #region Получить данные индикаторов
        private void GetIndicatorsValue()
        {
            foreach (var item in IndicatorConfigs)
            {
                item.GetValue(DictData[item.DbNum].buffer, DictData[item.DbNum].startByte);
            }
        }
        #endregion

        #region Получить данные кнопок
        private void GetButtonsValue()
        {
            foreach (var item in ButtonConfigs)
            {
                item.GetValue(DictData[item.DbNum].buffer, DictData[item.DbNum].startByte);
                item.GetValueWrite(DictData[item.DbNum].buffer, DictData[item.DbNum].startByte);
            }
        }
        #endregion

        #region Записать данные
        private void SetData()
        {
            while (WriteStack.Count!=0)
            {
                var writeData = WriteStack.Pop();
                int temp = s7Client.DBWrite(writeData.DbNum, writeData.StartByte, writeData.Buffer.Length, writeData.Buffer);
                if (temp != 0) throw new NotImplementedException();
            }
        }
        #endregion

        #region Операции с битами

        #region Переключить бит
        public void BitSwitch(ButtonConfig config, bool value)
        {
            if (Connected)
            {
                WriteData writeData = new WriteData();
                writeData.DbNum = config.DbNumWrite;
                writeData.StartByte = config.ByteNumWrite;
                int offset = DictData[config.DbNum].startByte;
                writeData.Buffer = new byte[1] { DictData[config.DbNumWrite].buffer[config.ByteNumWrite - offset] };
                S7.SetBitAt(ref writeData.Buffer, 0, config.BitNumWrite, value);
                WriteStack.Push(writeData); 
            }
        }
        #endregion

        #endregion

        #region Произвести обновление
        private void UpdateData()
        {
            if (UpdateDel != null) UpdateDel();
            UpdateDel = null;
        }
        #endregion

        #region Получить конфигурацию чтения  - записи 
        private void GetReadConfig()
        {
            DictData.Clear();
            foreach (var item in DataCells)
            {
                if (!DictData.ContainsKey(item.DbNum)) DictData[item.DbNum] = new DataBuffer
                {
                    startByte = item.ByteOffset,
                    finishByte = item.ByteOffset + DataCell.GetDataTypeSize(item.DataType) * (item.DataType == "string" ? item.Length : 1) - 1

                };
                else
                {                    
                    int startByte = item.ByteOffset;
                    int finishByte = item.ByteOffset + DataCell.GetDataTypeSize(item.DataType) * item.Length - 1;
                    // Переопределение нового стартового байта
                    if (DictData[item.DbNum].startByte > item.ByteOffset) DictData[item.DbNum].startByte = item.ByteOffset;
                    
                    // Переопределение нового конечного байта
                    if (DictData[item.DbNum].finishByte < finishByte) DictData[item.DbNum].finishByte = finishByte;                    
                }
            }
            foreach (var item in EventConfigs)
            {
                if (!DictData.ContainsKey(item.DbNum)) DictData[item.DbNum] = new DataBuffer
                {
                    startByte = item.ByteNum,
                    finishByte = item.ByteNum
                };
                else
                {
                    int startByte = item.ByteNum;
                    int finishByte = item.ByteNum;
                    // Переопределение нового стартового байта
                    if (DictData[item.DbNum].startByte > startByte) DictData[item.DbNum].startByte = startByte;

                    // Переопределение нового конечного байта
                    if (DictData[item.DbNum].finishByte < finishByte) DictData[item.DbNum].finishByte = finishByte;
                }
            }
            foreach (var item in IndicatorConfigs)
            {
                if (!DictData.ContainsKey(item.DbNum)) DictData[item.DbNum] = new DataBuffer
                {
                    startByte = item.ByteNum,
                    finishByte = item.ByteNum
                };
                else
                {
                    int startByte = item.ByteNum;
                    int finishByte = item.ByteNum;
                    // Переопределение нового стартового байта
                    if (DictData[item.DbNum].startByte > startByte) DictData[item.DbNum].startByte = startByte;

                    // Переопределение нового конечного байта
                    if (DictData[item.DbNum].finishByte < finishByte) DictData[item.DbNum].finishByte = finishByte;
                }
            }
            foreach (var item in ButtonConfigs)
            {
                if (!DictData.ContainsKey(item.DbNum)) DictData[item.DbNum] = new DataBuffer
                {
                    startByte = item.ByteNum,
                    finishByte = item.ByteNum
                };
                else
                {
                    int startByte = item.ByteNum;
                    int finishByte = item.ByteNum;
                    // Переопределение нового стартового байта
                    if (DictData[item.DbNum].startByte > startByte) DictData[item.DbNum].startByte = startByte;

                    // Переопределение нового конечного байта
                    if (DictData[item.DbNum].finishByte < finishByte) DictData[item.DbNum].finishByte = finishByte;
                }
                if (!DictData.ContainsKey(item.DbNumWrite)) DictData[item.DbNumWrite] = new DataBuffer
                {
                    startByte = item.ByteNumWrite,
                    finishByte = item.ByteNumWrite
                };
                else
                {
                    int startByte = item.ByteNumWrite;
                    int finishByte = item.ByteNumWrite;
                    // Переопределение нового стартового байта
                    if (DictData[item.DbNumWrite].startByte > startByte) DictData[item.DbNumWrite].startByte = startByte;

                    // Переопределение нового конечного байта
                    if (DictData[item.DbNumWrite].finishByte < finishByte) DictData[item.DbNumWrite].finishByte = finishByte;
                }
            }
            // Теперь необходимо заново инициализировать буфферы
            foreach (var item in DictData)
            {
                item.Value.buffer = new byte[item.Value.finishByte - item.Value.startByte + 1];
            }
        }
        #endregion

        

        #endregion

    }
}

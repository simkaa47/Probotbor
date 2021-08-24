using Probotbor.Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Probotbor.Models
{
    /// <summary>
    /// Представляет набор методов для загрузки из XML файла
    /// </summary>
    class XML_data
    {
        
        private static string Path { get; set; } = "controls.xml";
        #region Прочитать - установить адрес ПЛК из XML документа
        /// <summary>
        /// Метод чтения ip адреса ПЛК из xml документа
        /// </summary>
        /// <returns>ip адрес ПЛК</returns>
        public static string GetPlcAdress()
        {
            var xDoc = new XmlDocument();
            xDoc.Load(Path);
            return xDoc.DocumentElement.GetElementsByTagName("plcAddress")[0].InnerText;
        }
        /// <summary>
        /// Записать в xml адрес ПЛК
        /// </summary>
        /// <param name="ip">ip адрес ПЛК</param>
        public static void SetPlcAdress(string ip)
        {
            var xDoc = new XmlDocument();
            xDoc.Load(Path);
            xDoc.DocumentElement.GetElementsByTagName("device")[0]["plcAddress"].InnerText = ip;
            xDoc.Save(Path);
        }
        #endregion

        #region Прочитать из XML - записать в XML данные параметров
        public static void GetParamCellXML(ObservableCollection<DataCell> dataCells)
        {           
            var xDoc = new XmlDocument();
            xDoc.Load(Path);
            XmlNodeList xRoot = xDoc.DocumentElement.GetElementsByTagName("param");
            for (int i = 0; i < xRoot.Count; i++)
            {
                var cell = new DataCell();
                cell.Id = int.Parse(xRoot.Item(i).Attributes["Id"].Value);
                cell.Description = xRoot.Item(i).Attributes["Description"].Value;
                cell.DataType = xRoot.Item(i).Attributes["DataType"].Value;
                cell.DbNum = int.Parse(xRoot.Item(i).Attributes["DbNum"].Value);
                cell.ByteOffset = int.Parse(xRoot.Item(i).Attributes["ByteOffset"].Value);
                cell.Factor = float.Parse(xRoot.Item(i).Attributes["Factor"].Value.Replace(",", "."), CultureInfo.InvariantCulture);
                cell.Format = float.Parse(xRoot.Item(i).Attributes["Format"].Value.Replace(",", "."), CultureInfo.InvariantCulture);
                cell.Length = int.Parse(xRoot.Item(i).Attributes["Length"].Value);
                cell.WriteEnable = true ? xRoot.Item(i).Attributes["WriteEnable"].Value.ToLower() == "true" : false;
                dataCells.Add(cell); 
            }            
        }
        #endregion

        #region Записать в XML данные параметров
        /// <summary>
        /// Устанавливает что либо в XML по имени свойства
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="property"></param>
        public static void SetParamCell(int Id, string Property, string value)
        {
            var xDoc = new XmlDocument();
            xDoc.Load(Path);
            XmlNodeList xRoot = xDoc.DocumentElement.GetElementsByTagName("param");
            for (int i = 0; i < xRoot.Count; i++)
            {
                if (xRoot.Item(i).Attributes["Id"].Value == Id.ToString())
                {
                    xRoot.Item(i).Attributes[Property].Value = value;
                }
            }
            xDoc.Save(Path);
        }
        #endregion

        #region Прочитать из XML конфигурацию битовых индикаторов
        /// <summary>
        /// Прочитать из XML конфигурацию битовых индикаторов
        /// </summary>
        /// <param name="indicatorConfigs">Коллекция, куда добавлять</param>
        public static void GetIndicatorConfigXML(ObservableCollection<IndicatorConfig> indicatorConfigs)
        {
            var xDoc = new XmlDocument();
            xDoc.Load(Path);
            XmlNodeList xRoot = xDoc.DocumentElement.GetElementsByTagName("Indicator");
            for (int i = 0; i < xRoot.Count; i++)
            {
                var config = new IndicatorConfig();
                config.Id = int.Parse(xRoot.Item(i).Attributes["Id"].Value);
                config.Description = xRoot.Item(i).Attributes["Description"].Value;
                config.DbNum = int.Parse(xRoot.Item(i).Attributes["DbNum"].Value);
                config.ByteNum = int.Parse(xRoot.Item(i).Attributes["ByteNum"].Value);
                config.BitNum = int.Parse(xRoot.Item(i).Attributes["BitNum"].Value);
                config.Inverse = true ? xRoot.Item(i).Attributes["Inverse"].Value.ToLower() == "true" : false;
                indicatorConfigs.Add(config);
            }
        }
        #endregion

        #region Записать в XML измененное свойство конфигурации индикатора
        /// <summary>
        /// Записать в XML измененное свойство конфигурации индикатора
        /// </summary>
        /// <param name="Id">Идентификатор индикатора</param>
        /// <param name="Property">Изменяемое свойство</param>
        /// <param name="value">Значение свойства</param>
        public static void SetIndicatorConfig(int Id, string Property, string value)
        {
            try
            {
                var xDoc = new XmlDocument();
                xDoc.Load(Path);
                XmlNodeList xRoot = xDoc.DocumentElement.GetElementsByTagName("Indicator");
                for (int i = 0; i < xRoot.Count; i++)
                {
                    if (xRoot.Item(i).Attributes["Id"].Value == Id.ToString())
                    {
                        xRoot.Item(i).Attributes[Property].Value = value;
                    }
                }
                xDoc.Save(Path);
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region Прочитать из XML конфигурацию кнопок
        /// <summary>
        /// Прочитать из XML конфигурацию кнопок
        /// </summary>
        /// <param name="indicatorConfigs">Коллекция, куда добавлять</param>
        public static void GetButtonConfigXML(ObservableCollection<ButtonConfig> buttonConfigs)
        {
            try
            {
                var xDoc = new XmlDocument();
                xDoc.Load(Path);
                XmlNodeList xRoot = xDoc.DocumentElement.GetElementsByTagName("Button");
                for (int i = 0; i < xRoot.Count; i++)
                {
                    var config = new ButtonConfig();
                    config.Id = int.Parse(xRoot.Item(i).Attributes["Id"].Value);
                    config.Description = xRoot.Item(i).Attributes["Description"].Value;
                    config.DbNum = int.Parse(xRoot.Item(i).Attributes["DbNum"].Value);
                    config.ByteNum = int.Parse(xRoot.Item(i).Attributes["ByteNum"].Value);
                    config.BitNum = int.Parse(xRoot.Item(i).Attributes["BitNum"].Value);
                    config.Inverse = xRoot.Item(i).Attributes["Inverse"].Value.ToLower() == "true" ? true : false;
                    config.DbNumWrite = int.Parse(xRoot.Item(i).Attributes["DbNumWrite"].Value);
                    config.ByteNumWrite = int.Parse(xRoot.Item(i).Attributes["ByteNumWrite"].Value);
                    config.BitNumWrite = int.Parse(xRoot.Item(i).Attributes["BitNumWrite"].Value);                    
                    config.Action = xRoot.Item(i).Attributes["Action"].Value;
                    buttonConfigs.Add(config);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Записать в XML измененное свойство конфигурации кнопок
        /// <summary>
        /// Записать в XML измененное свойство конфигурации индикатора
        /// </summary>
        /// <param name="Id">Идентификатор индикатора</param>
        /// <param name="Property">Изменяемое свойство</param>
        /// <param name="value">Значение свойства</param>
        public static void SetButtonConfig(int Id, string Property, string value)
        {
            try
            {
                var xDoc = new XmlDocument();
                xDoc.Load(Path);
                XmlNodeList xRoot = xDoc.DocumentElement.GetElementsByTagName("Button");
                for (int i = 0; i < xRoot.Count; i++)
                {
                    if (xRoot.Item(i).Attributes["Id"].Value == Id.ToString())
                    {
                        xRoot.Item(i).Attributes[Property].Value = value;
                    }
                }
                xDoc.Save(Path);
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region Прочитать из ПЛК данные конфигурации событий
        public static void GetEventConfig(ObservableCollection<EventConfig> eventConfig)
        {                  
            var xDoc = new XmlDocument();
            xDoc.Load(Path);
            XmlNodeList xRoot = xDoc.DocumentElement.GetElementsByTagName("Event");
            for (int i = 0; i < xRoot.Count; i++)
            {
                var config = new EventConfig();
                config.Id = int.Parse(xRoot.Item(i).Attributes["Id"].Value);
                config.Description = xRoot.Item(i).Attributes["Description"].Value;
                config.DbNum = int.Parse(xRoot.Item(i).Attributes["DbNum"].Value);
                config.ByteNum = int.Parse(xRoot.Item(i).Attributes["ByteNum"].Value);
                config.BitNum = int.Parse(xRoot.Item(i).Attributes["BitNum"].Value);
                config.BitValue = int.Parse(xRoot.Item(i).Attributes["BitValue"].Value);
                config.EventType = xRoot.Item(i).Attributes["EventType"].Value;
                config.EventCode = xRoot.Item(i).Attributes["EventCode"].Value;
                eventConfig.Add(config);
            }            
        }

        #endregion

        #region Добавление записи в конфигурацию событий
        public static void AddEventConfig(EventConfig config)
        {
            var xDoc = new XmlDocument();
            xDoc.Load(Path);
            XmlNode xRoot = xDoc.DocumentElement.SelectSingleNode("Events");
            XmlElement eventElem = xDoc.CreateElement("Event");
            XmlAttribute idAttr = xDoc.CreateAttribute("Id");
            XmlAttribute descAttr = xDoc.CreateAttribute("Description");
            XmlAttribute dbNumAttr = xDoc.CreateAttribute("DbNum");
            XmlAttribute byteNumAttr = xDoc.CreateAttribute("ByteNum");
            XmlAttribute bitNumAttr = xDoc.CreateAttribute("BitNum");
            XmlAttribute bitValueAttr = xDoc.CreateAttribute("BitValue");
            XmlAttribute eventTypeAttr = xDoc.CreateAttribute("EventType");
            XmlAttribute eventCodeAttr = xDoc.CreateAttribute("EventCode");
            XmlText idText = xDoc.CreateTextNode(config.Id.ToString());
            XmlText descText = xDoc.CreateTextNode(config.Description);
            XmlText dbNumText = xDoc.CreateTextNode(config.DbNum.ToString());
            XmlText byteNumText = xDoc.CreateTextNode(config.ByteNum.ToString());
            XmlText bitNumText = xDoc.CreateTextNode(config.BitNum.ToString());
            XmlText biteValueText = xDoc.CreateTextNode(config.BitValue.ToString());
            XmlText eventTypeText = xDoc.CreateTextNode(config.EventType);
            XmlText eventCodeText = xDoc.CreateTextNode(config.EventCode);
            idAttr.AppendChild(idText);
            descAttr.AppendChild(descText);
            dbNumAttr.AppendChild(dbNumText);
            byteNumAttr.AppendChild(byteNumText);
            bitNumAttr.AppendChild(bitNumText);
            bitValueAttr.AppendChild(biteValueText);
            eventTypeAttr.AppendChild(eventTypeText);
            eventCodeAttr.AppendChild(eventCodeText);
            eventElem.Attributes.Append(idAttr);
            eventElem.Attributes.Append(descAttr);
            eventElem.Attributes.Append(dbNumAttr);
            eventElem.Attributes.Append(byteNumAttr);
            eventElem.Attributes.Append(bitNumAttr);
            eventElem.Attributes.Append(bitValueAttr);
            eventElem.Attributes.Append(eventTypeAttr);
            eventElem.Attributes.Append(eventCodeAttr);
            xRoot.AppendChild(eventElem);
            xDoc.Save(Path);
        }

        #endregion

        #region Очистить данные конфигурации событий в XML
        public static void ClearEventConfig()
        {
            var xDoc = new XmlDocument();
            xDoc.Load(Path);
            XmlNode xRoot = xDoc.DocumentElement.SelectSingleNode("Events");
            xRoot.RemoveAll();
            xDoc.Save(Path);
        }
        #endregion

        #region Редактирование события
        public static void EditEvent(EventConfig config)
        {
            var xDoc = new XmlDocument();
            xDoc.Load(Path);
            XmlNodeList xRoot = xDoc.DocumentElement.GetElementsByTagName("Event");
            for (int i = 0; i < xRoot.Count; i++)
            {
                if (xRoot.Item(i).Attributes["Id"].Value == config.Id.ToString())
                {
                    xRoot.Item(i).Attributes["Description"].Value = config.Description;
                    xRoot.Item(i).Attributes["DbNum"].Value = config.DbNum.ToString();
                    xRoot.Item(i).Attributes["ByteNum"].Value = config.ByteNum.ToString();
                    xRoot.Item(i).Attributes["BitNum"].Value = config.BitNum.ToString();
                    xRoot.Item(i).Attributes["BitValue"].Value = config.BitValue.ToString();
                    xRoot.Item(i).Attributes["EventType"].Value = config.EventType;
                    xRoot.Item(i).Attributes["EventCode"].Value = config.EventCode;
                }
            }
            xDoc.Save(Path);
        }
        #endregion

        #region Работа со статусами

        #region Прочитать из XML статусы
        public static void GetStatus(ObservableCollection<StatusDevice> statuses, string deviceName)
        {
            try
            {
                var xDoc = new XmlDocument();
                xDoc.Load(Path);
                XmlNode xNode = xDoc.DocumentElement.SelectSingleNode("Statuses");
                XmlNode xNode1 = xNode.SelectSingleNode(deviceName);
                XmlNodeList xRoot = xNode1.SelectNodes("Status");
                for (int i = 0; i < xRoot.Count; i++)
                {
                    var status = new StatusDevice();
                    status.Id = i;
                    xRoot.Item(i).Attributes["Id"].Value = i.ToString();
                    status.Status = xRoot.Item(i).Attributes["Description"].Value;
                    statuses.Add(status);

                }
                xDoc.Save(Path);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Редактирование статуса
        public static void EditStatus(StatusDevice status, string deviceName)
        {
            try
            {
                var xDoc = new XmlDocument();
                xDoc.Load(Path);
                XmlNode xNode = xDoc.DocumentElement.SelectSingleNode("Statuses");
                XmlNode xNode1 = xNode.SelectSingleNode(deviceName);
                XmlNodeList xRoot = xNode1.SelectNodes("Status");
                for (int i = 0; i < xRoot.Count; i++)
                {
                    if (xRoot.Item(i).Attributes["Id"].Value == status.Id.ToString())
                    {
                        xRoot.Item(i).Attributes["Description"].Value = status.Status.ToString();
                    }
                }
                xDoc.Save(Path);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Добавление статуса
        public static void AddStatus(StatusDevice status, string deviceName)
        {
            var xDoc = new XmlDocument();
            xDoc.Load(Path);
            XmlNode xNode = xDoc.DocumentElement.SelectSingleNode("Statuses");
            XmlNode xNode1 = xNode.SelectSingleNode(deviceName);            
            XmlElement deviceElem = xDoc.CreateElement("Status");
            XmlAttribute idAttr = xDoc.CreateAttribute("Id");
            XmlText idText = xDoc.CreateTextNode(status.Id.ToString());
            idAttr.AppendChild(idText);
            deviceElem.Attributes.Append(idAttr);
            XmlAttribute descAttr = xDoc.CreateAttribute("Description");
            XmlText descText = xDoc.CreateTextNode(status.Status);
            descAttr.AppendChild(descText);
            deviceElem.Attributes.Append(descAttr);
            xNode1.AppendChild(deviceElem);
            xDoc.Save(Path);

        }
        #endregion

        #region Удаление статуса
        public static void RemoveStatus(StatusDevice status, string deviceName)
        {
            var xDoc = new XmlDocument();
            xDoc.Load(Path);
            XmlNode xNode = xDoc.DocumentElement.SelectSingleNode("Statuses");
            XmlNode xNode1 = xNode.SelectSingleNode(deviceName);
            XmlNodeList xRoot = xNode1.SelectNodes("Status");
            for (int i = 0; i < xRoot.Count; i++)
            {
                if (xRoot.Item(i).Attributes["Id"].Value == status.Id.ToString())
                {
                    xNode1.RemoveChild(xRoot.Item(i));
                }
            }
            xDoc.Save(Path);
           
        }
        #endregion


        #endregion

    }
}

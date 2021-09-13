using Probotbor.Infrastructure;
using Probotbor.Infrastructure.Commands;
using Probotbor.Models;
using Probotbor.ViewModels.Base;
using Probotbor.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;

namespace Probotbor.ViewModels
{
    public class TaskWindowVM: ViewModelBase
    {
        #region View
        Client client;
        #endregion

        #region Статус соединения
        private bool connected;
        public bool Connected
        {
            set => Set(ref connected, value);
            get => connected;
        }
        private void ChangeStatusConnect(string Ip, bool status)
        {  
            Connected = status;
        }
        #endregion

        #region Команда записи параметра
        RelayCommand writeParamCommand;
        public RelayCommand WriteParamCommand
        {
            get => writeParamCommand ?? (writeParamCommand = new RelayCommand(OnWriteParamCommandExecuted, OnWriteParamCommandExecute));
        }
        private bool OnWriteParamCommandExecute(object p) => true;
        private void OnWriteParamCommandExecuted(object p)
        {
            DataCell cell = p as DataCell;

            if (cell != null) client.WriteParameter(cell);
        }


        #endregion

        #region Команды кнопок
        #region Нажатие кнопки
        RelayCommand buttonPressOnCommand;
        public RelayCommand ButtonPressOnCommand
        {
            get => buttonPressOnCommand ?? (buttonPressOnCommand = new RelayCommand(OnButtonPressOnCommandExecuted, OnButtonPressOnCommandExecute));
        }
        private bool OnButtonPressOnCommandExecute(object p) => client.Connected;
        private void OnButtonPressOnCommandExecuted(object p)
        {
            ButtonConfig config = p as ButtonConfig;
            if (config == null) return;
            switch (config.Action)
            {
                case "Мгновенно":
                    client.BitSwitch(config, true);
                    break;
                case "Переключатель":
                    client.BitSwitch(config, !config.BitValueWrite);
                    break;
                case "Включить":
                    client.BitSwitch(config, true);
                    break;
                case "Выключить":
                    client.BitSwitch(config, false);
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region Отжатие кнопки
        RelayCommand buttonPressOffCommand;
        public RelayCommand ButtonPressOffCommand
        {
            get => buttonPressOffCommand ?? (buttonPressOffCommand = new RelayCommand(OnButtonPressOffCommandExecuted, OnButtonPressOffCommandExecute));
        }
        private bool OnButtonPressOffCommandExecute(object p) => client.Connected;
        private void OnButtonPressOffCommandExecuted(object p)
        {
            ButtonConfig config = p as ButtonConfig;
            if (config == null) return;
            if (config.Action == "Мгновенно") client.BitSwitch(config, false);

        }
        #endregion

        #endregion

        #region Страница "Администрирование"
        ApplicationContext db;
        #region Вкладка "Управление пользователями"
        #region Команды
        #region Добавление пользователя
        RelayCommand addUserCommand;
        public RelayCommand AddUserCommand
        {
            get
            {
                return addUserCommand ??
                  (addUserCommand = new RelayCommand((o) =>
                  {
                      UserWindow userWindow = new UserWindow(new User());
                      if (userWindow.ShowDialog() == true && CheckLogin(userWindow.user.Login, userWindow.user.Password) && IsNotExistLogin(userWindow.user.Login))
                      {
                          Users.Add(userWindow.user);                         
                      }
                  }));
            }
        }
        #endregion
        #region Редактирование пользователя
        RelayCommand editUserCommand;
        // команда редактирования
        public RelayCommand EditUserCommand
        {
            get
            {
                return editUserCommand ??
                  (editUserCommand = new RelayCommand((selectedItem) =>
                  {
                      if (selectedItem == null) return;
                      // получаем выделенный объект
                      User user = selectedItem as User;

                      User vm = new User()
                      {
                          Id = user.Id,
                          Login = user.Login,
                          Name = user.Name,
                          Somename = user.Somename,
                          Post = user.Post,
                          Password = user.Password,
                          Level = user.Level

                      };
                      UserWindow userWindow = new UserWindow(vm);


                      if (userWindow.ShowDialog() == true && CheckLogin(userWindow.user.Login, userWindow.user.Password))
                      {
                          // получаем измененный объект                          
                          if (user != null)
                          {
                              user.Login = userWindow.user.Login;
                              user.Name = userWindow.user.Name;
                              user.Somename = userWindow.user.Somename;
                              user.Post = userWindow.user.Post;
                              user.Password = userWindow.user.Password;
                              user.Level = userWindow.user.Level;                              
                          }
                      }
                  }));
            }
        }
        #endregion
        #region Удаление пользователя
        RelayCommand deleteUserCommand;
        // команда удаления
        public RelayCommand DeleteUserCommand
        {
            get
            {
                return deleteUserCommand ??
                  (deleteUserCommand = new RelayCommand((selectedItem) =>
                  {
                      if (selectedItem == null) return;
                      // получаем выделенный объект
                      User user = selectedItem as User;
                      Users.Remove(user);
                  }));
            }
        }
        #endregion
        #endregion
        #region Методы
        #region Метод проверки правильного пользователя
        private bool CheckLogin(string login, string password)
        {
            return (login != "" && login != null && password != "" && password != null);
        }
        #endregion
        #region  Метод проверки существования пользователя
        private bool IsNotExistLogin(string login)
        {
            if (Users != null)
            {
                foreach (var item in Users)
                {
                    if (item.Login == login)
                    {
                        MessageBox.Show($"Пользователь с логином {login} уже существует!");
                        return false;
                    }
                }
            }
            return true;
        }
        #endregion
        #endregion
        #region Поля и свойства
        #region Коллекция пользователей
        ObservableCollection<User> users;
        /// <summary>
        /// Коллекция пользователей
        /// </summary>
        public ObservableCollection<User> Users { get => users ?? (users = client.Users); }
        #endregion

        #endregion
        #endregion

        #region Адресация
        #region Коллекция ячеек данных
        ObservableCollection<DataCell> dataCells;
        public ObservableCollection<DataCell> DataCells
        {
            get => dataCells;
            set => Set(ref dataCells, value);
        }
        #endregion
        
        

        #region IP адрес ПЛК
        private string ipAddress;
        public string IpAdress
        {
            get => client.Ip;
            set
            {
                client.Ip = value;
                ipAddress = client.Ip;
                Set(ref ipAddress, value);
            }
        }

        #endregion

        #endregion

        #region Конфигурация событий
        ObservableCollection<EventConfig> eventConfigs;
        public ObservableCollection<EventConfig> EventConfigs
        {
            get => eventConfigs;
            set
            {
                Set(ref eventConfigs, value);
            }
        }
        #endregion

        #region Конфигурация индикаторов
        ObservableCollection<IndicatorConfig> indicatorConfigs;
        public ObservableCollection<IndicatorConfig> IndicatorConfigs
        {
            get => indicatorConfigs;
            set
            {
                Set(ref indicatorConfigs, value);
            }
        }
        #endregion

        #region Конфигурация кнопок
        ObservableCollection<ButtonConfig> buttonConfigs;
        public ObservableCollection<ButtonConfig> ButtonConfigs
        {
            get => buttonConfigs;set => Set(ref buttonConfigs, value);
        }
        #endregion

        #endregion

        #region История событий
        #region Коллекция событий
        ObservableCollection<HistoryItem> historyItems;
        public ObservableCollection<HistoryItem> HistoryItems
        {
            get
            {
                return historyItems ?? (historyItems = client.HistoryItems);
            }
            set
            {
                if(!Set(ref historyItems, value))return;
                filteringEventCollection.Source = value;
            }
        }
        #endregion
        #region Коллеция для фильтрации
        private readonly CollectionViewSource filteringEventCollection = new CollectionViewSource();
        public ICollectionView FilteringEventCollection => filteringEventCollection.View;
        #endregion

        #region Метод фильтрации

        private void FilterHistoryEvent(object sender, FilterEventArgs e)
        {
            var filter_text = filterTextHistoryEvent;
            if (string.IsNullOrWhiteSpace(filter_text)) return;
            if (!(e.Item is HistoryItem historyItem))
            {
                e.Accepted = false;
                return;
            }
            if (historyItem.Date.Contains(filter_text)) return;
            e.Accepted = false;
        }
        #endregion

        #region Текст фильтрации 
        private string filterTextHistoryEvent;
        public string FilterTextHistoryEvent
        { 
            get => filterTextHistoryEvent ?? (filterTextHistoryEvent = DateTime.Today.ToString());
            set
            {
                if(!Set(ref filterTextHistoryEvent, value)) return;
                filteringEventCollection?.View?.Refresh();


            } 
        }
        #endregion
        
        #endregion

        #region Статусы

        #region Статусы накопителя
        ObservableCollection<StatusDevice> nakopStatuses;
        public ObservableCollection<StatusDevice> NakopStatuses
        {
            get
            {
                if (nakopStatuses == null)
                {
                    nakopStatuses = client.NakopStatuses;
                }
                return nakopStatuses;
            }
            set
            {
                Set(ref nakopStatuses, value);
            }
        }

        #endregion

        #region Статусы системы
        ObservableCollection<StatusDevice> sysStatuses;
        public ObservableCollection<StatusDevice> SysStatuses
        {
            get
            {
                if (sysStatuses == null)
                {
                    sysStatuses = client.SysStatuses;
                }
                return sysStatuses;
            }
            set
            {
                Set(ref sysStatuses, value);
            }
        }

        #endregion

        #region Статусы проботборников
        ObservableCollection<StatusDevice> probStatuses;
        public ObservableCollection<StatusDevice> ProbStatuses
        {
            get
            {
                if (probStatuses == null)
                {
                    probStatuses = client.ProbStatuses;
                }
                return probStatuses;
            }
            set
            {
                Set(ref probStatuses, value);
            }
        }

        #endregion

        #region Статусы бункера
        ObservableCollection<StatusDevice> bunStatuses;
        public ObservableCollection<StatusDevice> BunStatuses
        {
            get
            {
                if (bunStatuses == null)
                {
                    bunStatuses = client.BunkStatuses;
                }
                return bunStatuses;
            }
            set
            {
                Set(ref bunStatuses, value);
            }
        }

        #endregion

        #region Статусы Делителя
        ObservableCollection<StatusDevice> delStatuses;
        public ObservableCollection<StatusDevice> DelStatuses
        {
            get
            {
                if (delStatuses == null)
                {
                    delStatuses = client.DelStatuses;
                }
                return delStatuses;
            }
            set
            {
                Set(ref delStatuses, value);
            }
        }

        #endregion

        #endregion

        #region Верхний бар

        #region Команда "Закрыть приложение"
        RelayCommand closeAppCommand;
        public RelayCommand CloseAppCommand
        {
            get
            { 
                return closeAppCommand ?? (closeAppCommand = new RelayCommand(OnCloseAppCommandExecuted, OnCloseAppCommandExecute));
            }
        }

        private bool OnCloseAppCommandExecute(object p) => true;
        private void OnCloseAppCommandExecuted(object p) 
        { 
            Application.Current.Shutdown();
            client.CloseConnection(); 
        }
        #endregion       
        #region Имя пользователя
        string username;
        public string Username
        {
            set => Set(ref username, value);
            get => username;
        }
        private string GetUserName(User user)
        {
            string retStr = "";
            if (user.Somename != null) retStr = user.Somename;
            if (user.Name != null && user.Name.Length > 0) retStr = retStr + " " + user.Name.Substring(0, 1) + ".";
            return retStr;
        }
        #endregion
        #region текущий пользователь CurrentUser
        User currentUser;
        public User CurrentUser
        {
            set
            {
                Set(ref currentUser, value);
                Username = GetUserName(value);
            }
            get => currentUser;
        }


        #endregion
        #region Текущее время CurTime
        string curTime;
        public string CurTime
        {
            set => Set(ref curTime, value);
            get => curTime;
        }
        #region метод получения даты
        private void UpdateTime(object sender, object e)
        {
            CurTime = DateTime.Now.ToString("HH:mm:ss");
        }

        #endregion
        
        #endregion 
        #endregion        

        #region Таймер
        DispatcherTimer timer;
        #endregion

        

        #region Конструктор
        public TaskWindowVM()
        {            
            timer = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 1) };// Запуск таймера           
            timer.Start();
            timer.Tick += UpdateTime;
            client = new Client();
            DataCells = client.DataCells;
            EventConfigs = client.EventConfigs;
            IndicatorConfigs = client.IndicatorConfigs;
            ButtonConfigs = client.ButtonConfigs;
            client.Start();            
            filteringEventCollection.Filter += FilterHistoryEvent;
            client.ConnEvent += ChangeStatusConnect;
        } 
        #endregion
    }
}

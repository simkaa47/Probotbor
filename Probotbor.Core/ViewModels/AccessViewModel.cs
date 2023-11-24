using Castle.Core.Logging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using Probotbor.Core.Contracts.AccessControl;
using Probotbor.Core.Contracts.DataAccess;
using Probotbor.Core.Contracts.Dialog;
using Probotbor.Core.Models.AccessControl;
using Probotbor.Core.Services.AccessControl;

namespace Probotbor.Core.ViewModels
{
    public partial class AccessViewModel:ObservableObject
    {
        private readonly IRepository<User> _userRepository;
        private readonly ILogger<AccessViewModel> _logger;
        private readonly IAccessDialogService _accessDialog;
        private readonly IQuestionDialog _questionDialog;
        [ObservableProperty]
        private IEnumerable<User>? _users;
        [ObservableProperty]
        private User? _currentUser;

        public AccessViewModel(IRepository<User> userRepository, 
            ILogger<AccessViewModel> logger, 
            IAccessDialogService accessDialog, IQuestionDialog questionDialog)
        {
            _userRepository = userRepository;
            _logger = logger;
            _accessDialog = accessDialog;
            _questionDialog = questionDialog;
            InitAsync();
        }

        private async void InitAsync()
        {
            try
            {
                _logger.LogInformation($"Инициализация данных пользователей");
                Users = await _userRepository.InitAsync(UserDataFactory.GetUsers(), 3);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка при  инициализации данных пользователей - {ex.Message}");
            }
        }


        [RelayCommand]
        private async Task AddUserAsync()
        {
            var newUser = new User();
            if (!await _accessDialog.ShowDialog(newUser)) return;
            try
            {
                if (newUser.HasErrors)
                {
                    throw new Exception("New user adding: validation error");
                }
                _logger.LogInformation($"Выполняется добавление пользователя {newUser.FullName}");
                await _userRepository.AddAsync(newUser);
                Users = await _userRepository.ListAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка добавления пользователя {newUser.FullName} - {ex.Message}");
            }
        }


        [RelayCommand]
        private async Task UpdateUserAsync(object parameter)
        {
            if (!(parameter is User user)) return;
            _logger.LogInformation($"Выполняется редактирование пользователя {user.FullName}");
            if (!await _accessDialog.ShowDialog(user)) return;
            try
            {
                if (user.HasErrors)
                {
                    throw new Exception("New user adding: validation error");
                }
                
                await _userRepository.UpdateAsync(user);                
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка редактирования пользователя {user.FullName} - {ex.Message}");
            }
        }

        [RelayCommand]
        private async Task DeleteUserAsync(object parameter)
        {
            if (!(parameter is User user)) return;
            try
            {
                if (await _questionDialog.Ask($"Удаление пользователя {user.FullName}", $"Вы действительно хотите Удалить пользователя {user.FullName}?"))
                {
                    _logger.LogInformation($"Выполняется удаление пользователя {user.FullName}");
                    await _userRepository.DeleteAsync(user);
                    Users = await _userRepository.ListAllAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка удаления пользователя {user.FullName} - {ex.Message}");
            }

        }

        
        [RelayCommand]
        private async Task LoginAsync(object parameter)
        {
            if (!(parameter is Login login)) return;
            try
            {
                var user = await _userRepository.GetFirstWhere(u => u.Login == login.LoginName && u.Password == login.Password);
                if (user == null)
                {
                    login.FaliledLogin = true;
                    _logger.LogInformation($"Попытка авторизоваться с логином = {login.LoginName} и паролем {login.Password} была неуспешной");
                }
                else
                {
                    CurrentUser = user;
                    _logger.LogInformation($"Пользователь c логином {login.LoginName}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка при авторизации пользователя с логином  {login.LoginName} - {ex.Message}");
            }
        }

        [RelayCommand]
        public void Logout()
        {
            CurrentUser = null;
        }






    }
}

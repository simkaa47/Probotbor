using Probotbor.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Probotbor.ViewModels
{
    public class User: NotifyPropertyChanged
    {
            
        public  int Id { get; set; }
        #region Логин пользователя
        private string login;
        /// <summary>
        /// Логин пользователя
        /// </summary>
        public string Login { get => login; set => Set(ref login, value); }
        #endregion
        #region Имя пользователя
        private string name;
        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string Name { get => name; set => Set(ref name, value); }
        #endregion
        #region Фамилия пользователя
        private string somename;
        /// <summary>
        /// Фамилия пользователя
        /// </summary>
        public string Somename { get => somename; set => Set(ref somename, value); }
        #endregion
        #region Должность пользователя
        private string post;
        /// <summary>
        /// Должность пользователя
        /// </summary>
        public string Post { get => post; set => Set(ref post, value); }
        #endregion
        #region Пароль
        private string password;
        /// <summary>
        /// Пароль
        /// </summary>
        public string Password { get => password; set => Set(ref password, value); }
        #endregion
        #region Уровень доступа
        private string level = "Администратор";
        public string Level { get => level; set => Set(ref level, value); }
        #endregion
        
    }
}

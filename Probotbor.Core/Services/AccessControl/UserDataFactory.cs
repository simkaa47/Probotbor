using Probotbor.Core.Models.AccessControl;

namespace Probotbor.Core.Services.AccessControl
{
    public static class UserDataFactory
    {
        public static IEnumerable<User> GetUsers()
        {
            return new List<User>()
            {
                new User {Login = "user", Password = "user", FirstName = "User", LastName="User", AccessLevel = UserAccessLevel.None},
                new User {Login = "admin", Password = "0000", FirstName = "Админ", LastName="Админов", AccessLevel = UserAccessLevel.Admin},
                new User {Login = "service", Password = "service", FirstName = "Сервис", LastName="Сервисов", AccessLevel = UserAccessLevel.Service},
            };
        }
    }
}

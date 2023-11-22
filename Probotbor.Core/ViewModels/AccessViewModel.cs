using CommunityToolkit.Mvvm.ComponentModel;
using Probotbor.Core.Contracts.DataAccess;
using Probotbor.Core.Models.AccessControl;

namespace Probotbor.Core.ViewModels
{
    public class AccessViewModel:ObservableObject
    {
        private readonly IRepository<User> _userRepository;

        public AccessViewModel(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }
    }
}

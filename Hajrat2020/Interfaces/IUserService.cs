using Hajrat2020.ViewModel;

namespace Hajrat2020.Interfaces
{
    public interface IUserService
    {
        UserViewModel GetUsers(int? page);
        void DeActivateUser(string id);
        UserViewModel AddUser();
        UserViewModel EditUser(string id);
        UserViewModel GetUser(string id);
    }
}

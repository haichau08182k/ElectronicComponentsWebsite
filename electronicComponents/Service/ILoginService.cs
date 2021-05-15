using electronicComponents.DAL;

namespace electronicComponents.Service
{
    public interface ILoginService
    {
        Employee CheckLogin(string username, string password);
    }
}
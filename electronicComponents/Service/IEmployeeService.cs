using electronicComponents.DAL;
using System.Collections.Generic;

namespace electronicComponents.Service
{
    public interface IEmployeeService
    {
        Employee CheckLogin(string username, string password);
        IEnumerable<Employee> GetList();
        int GetTotalEmployee();
    }
}
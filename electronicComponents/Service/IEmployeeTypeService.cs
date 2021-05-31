using electronicComponents.DAL;
using System.Collections.Generic;

namespace electronicComponents.Service
{
    public interface IEmployeeTypeService
    {
        EmployeeType GetEmployeeTypeByID(int ID);
        IEnumerable<EmployeeType> GetListEmployeeType();
    }
}
using electronicComponents.DAL;
using electronicComponents.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace electronicComponents.Service
{
    public class LoginService : ILoginService
    {
        public GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();
        public Employee CheckLogin(string username, string password)
        {
            Employee employee = this._unitOfWork.GetRepositoryInstance<Employee>().GetAllRecords().SingleOrDefault(x => x.userName == username && x.passwordd == password);
            if (employee == null)
            {
                employee = this._unitOfWork.GetRepositoryInstance<Employee>().GetAllRecords().SingleOrDefault(x => x.email == username && x.passwordd == password);
            }
            return employee;
        }

    }
}
using electronicComponents.DAL;
using electronicComponents.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace electronicComponents.Service
{
    public class EmployeeService : IEmployeeService
    {
        private readonly GenericUnitOfWork _unitOfWork;
        public EmployeeService(GenericUnitOfWork repositoryContext)
        {
            this._unitOfWork = repositoryContext;
        }
        public Employee CheckLogin(string username, string password)
        {
            Employee emloyee = this._unitOfWork.GetRepositoryInstance<Employee>().GetAllRecords().SingleOrDefault(x => x.userName == username && x.passwordd == password);
            if (emloyee == null)
            {
                emloyee = this._unitOfWork.GetRepositoryInstance<Employee>().GetAllRecords().SingleOrDefault(x => x.email == username && x.passwordd == password);
            }
            return emloyee;
        }

        public IEnumerable<Employee> GetList()
        {
            return _unitOfWork.GetRepositoryInstance<Employee>().GetAllRecords();
        }

        public int GetTotalEmployee()
        {
            return _unitOfWork.GetRepositoryInstance<Employee>().GetAllRecords().Count();
        }
    }
}
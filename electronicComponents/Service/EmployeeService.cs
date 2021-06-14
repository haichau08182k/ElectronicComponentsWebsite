using electronicComponents.DAL;
using electronicComponents.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace electronicComponents.Service
{
    public interface IEmployeeService
    {
        Employee CheckLogin(string username, string password);
        IEnumerable<Employee> GetList();
        int GetTotalEmployee();
        Employee AddEmployee(Employee employee);
        List<SelectListItem> GetEmployeeType();
    }

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

        public Employee AddEmployee(Employee employee)
        {
            this._unitOfWork.GetRepositoryInstance<Employee>().Add(employee);
            return employee;
        }
        public IEnumerable<Employee> GetList()
        {
            return _unitOfWork.GetRepositoryInstance<Employee>().GetAllRecords();
        }

        public int GetTotalEmployee()
        {
            return _unitOfWork.GetRepositoryInstance<Employee>().GetAllRecords().Count();
        }

        public List<SelectListItem> GetEmployeeType()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            var cate = _unitOfWork.GetRepositoryInstance<EmployeeType>().GetAllRecords();
            foreach (var item in cate)
            {
                list.Add(new SelectListItem { Value = item.id.ToString(), Text = item.name });

            }
            return list;

        }
    }
}
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
        Employee GetByID(int ID);
        void Update(Employee employee);
        void ResetPassword(int EmloyeeID, string NewPassword);
        Employee CheckLogin1(int id, string password);
        bool CheckPhoneNumberEmployee(string PhoneNumber);
        bool CheckNameEmployee(string Name);
        bool CheckEmailEmployee(string Email);
        Employee GetByPhoneNumberEmployee(string PhoneNumber);
        Employee GetByNameEmployee(string Name);
        Employee GetByEmailEmployee(string Email);
        Employee GetByUserNameEmployee(string userName);
        bool CheckUserNameEmployee(string userName);
        void Active(Employee emloyee);
        void Block(Employee emloyee);
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

        public Employee CheckLogin1(int id, string password)
        {
            Employee emloyee = this._unitOfWork.GetRepositoryInstance<Employee>().GetAllRecords().SingleOrDefault(x => x.id == id && x.passwordd == password);
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
        public Employee GetByID(int ID)
        {
            return this._unitOfWork.GetRepositoryInstance<Employee>().GetFirstorDefault(ID);
        }

        public void Block(Employee emloyee)
        {
            emloyee.isActive = false;
            this._unitOfWork.GetRepositoryInstance<Employee>().Update(emloyee);
        }
        public void Active(Employee emloyee)
        {
            emloyee.isActive = true;
            this._unitOfWork.GetRepositoryInstance<Employee>().Update(emloyee);
        }

        public void Update(Employee employee)
        {
             this._unitOfWork.GetRepositoryInstance<Employee>().Update(employee);
           
        }
        public void ResetPassword(int EmloyeeID, string NewPassword)
        {
            Employee emloyee = GetByID(EmloyeeID);
            emloyee.passwordd = NewPassword;
            _unitOfWork.GetRepositoryInstance<Employee>().Update(emloyee);
        }

        public bool CheckPhoneNumberEmployee(string PhoneNumber)
        {
            var check = _unitOfWork.GetRepositoryInstance<Employee>().GetAllRecords(x => x.phoneNumber == PhoneNumber && x.isActive == true);
            if (check.Count() > 0)
            {
                return false;
            }
            return true;
        }

        public bool CheckNameEmployee(string Name)
        {
            var check = _unitOfWork.GetRepositoryInstance<Employee>().GetAllRecords(x => x.fullName == Name && x.isActive == true);
            if (check.Count() > 0)
            {
                return false;
            }
            return true;
        }
        public bool CheckUserNameEmployee(string userName)
        {
            var check = _unitOfWork.GetRepositoryInstance<Employee>().GetAllRecords(x => x.userName == userName && x.isActive == true);
            if (check.Count() > 0)
            {
                return false;
            }
            return true;
        }
        public bool CheckEmailEmployee(string Email)
        {
            var check = _unitOfWork.GetRepositoryInstance<Employee>().GetAllRecords(x => x.email == Email && x.isActive == true);
            if (check.Count() > 0)
            {
                return false;
            }
            return true;
        }
        public Employee GetByPhoneNumberEmployee(string PhoneNumber)
        {
            Employee emloyee = _unitOfWork.GetRepositoryInstance<Employee>().GetAllRecords().FirstOrDefault(x => x.phoneNumber == PhoneNumber);
            return emloyee;
        }

        public Employee GetByNameEmployee(string Name)
        {
            Employee emloyee = _unitOfWork.GetRepositoryInstance<Employee>().GetAllRecords().FirstOrDefault(x => x.fullName == Name);
            return emloyee;
        }
        public Employee GetByUserNameEmployee(string userName)
        {
            Employee emloyee = _unitOfWork.GetRepositoryInstance<Employee>().GetAllRecords().FirstOrDefault(x => x.userName == userName);
            return emloyee;
        }
        public Employee GetByEmailEmployee(string Email)
        {
            Employee emloyee = _unitOfWork.GetRepositoryInstance<Employee>().GetAllRecords().FirstOrDefault(x => x.email == Email);
            return emloyee;
        }

    }
}
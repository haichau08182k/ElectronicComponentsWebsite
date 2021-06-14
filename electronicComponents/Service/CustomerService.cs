using electronicComponents.DAL;
using electronicComponents.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace electronicComponents.Service
{
    public class CustomerService : ICustomerService
    {
        private readonly GenericUnitOfWork _unitOfWork;
        public CustomerService(GenericUnitOfWork repositoryContext)
        {
            this._unitOfWork = repositoryContext;
        }
        public Customer AddCustomer(Customer customer)
        {
            this._unitOfWork.GetRepositoryInstance<Customer>().Add(customer);
            return customer;
        }

        public IEnumerable<Customer> GetAll()
        {
            return _unitOfWork.GetRepositoryInstance<Customer>().GetAllRecords();
        }

        public string GetEmailByID(int ID)
        {
            return _unitOfWork.GetRepositoryInstance<Customer>().GetFirstorDefault(ID).email;
        }
        public void Update(Customer customer)
        {
            _unitOfWork.GetRepositoryInstance<Customer>().Update(customer);
        }
    }
}
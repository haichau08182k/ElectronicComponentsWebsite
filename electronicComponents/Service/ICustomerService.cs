using electronicComponents.DAL;
using System.Collections.Generic;

namespace electronicComponents.Service
{
    public interface ICustomerService
    {
        Customer AddCustomer(Customer customer);
        IEnumerable<Customer> GetAll();
        string GetEmailByID(int ID);
        void Update(Customer customer);
    }
}
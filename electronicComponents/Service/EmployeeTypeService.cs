using electronicComponents.DAL;
using electronicComponents.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace electronicComponents.Service
{

    public interface IEmployeeTypeService
    {
        EmployeeType GetEmployeeTypeByID(int ID);
        IEnumerable<EmployeeType> GetListEmployeeType();
    }
    public class EmloyeeTypeService : IEmployeeTypeService
    {
        private readonly GenericUnitOfWork _unitOfWork;
        public EmloyeeTypeService(GenericUnitOfWork repositoryContext)
        {
            this._unitOfWork = repositoryContext;
        }
        public EmployeeType GetEmployeeTypeByID(int ID)
        {
            return _unitOfWork.GetRepositoryInstance<EmployeeType>().GetFirstorDefault(ID);
        }

        public IEnumerable<EmployeeType> GetListEmployeeType()
        {
            return _unitOfWork.GetRepositoryInstance<EmployeeType>().GetAllRecords();
        }
    }
}
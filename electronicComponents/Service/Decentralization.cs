using electronicComponents.DAL;
using electronicComponents.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace electronicComponents.Service
{
    public class DecentralizationService : IDecentralizationService
    {
        private readonly GenericUnitOfWork _unitOfWork;
        public DecentralizationService(GenericUnitOfWork repositoryContext)
        {
            this._unitOfWork = repositoryContext;
        }

        public void Add(Decentralization decentralization)
        {
            _unitOfWork.GetRepositoryInstance<Decentralization>().Add(decentralization);
        }

        public IEnumerable<Decentralization> GetDecentralizationByEmloyeeTypeID(int ID)
        {
            return _unitOfWork.GetRepositoryInstance<Decentralization>().GetAllRecords(x => x.employeeTypeID == ID);
        }

        public void RemoveRange(IEnumerable<Decentralization> decentralizations)
        {
            foreach (var item in decentralizations)
            {
                _unitOfWork.GetRepositoryInstance<Decentralization>().Remove(item);
            }
        }
    }
}
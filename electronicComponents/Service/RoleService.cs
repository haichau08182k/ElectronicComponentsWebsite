using electronicComponents.DAL;
using electronicComponents.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace electronicComponents.Service
{
    public class RoleService : IRoleService
    {
        private readonly GenericUnitOfWork _unitOfWork;
        public RoleService(GenericUnitOfWork repositoryContext)
        {
            this._unitOfWork = repositoryContext;
        }

        public Rolee GetRoleByID(int ID)
        {
            return _unitOfWork.GetRepositoryInstance<Rolee>().GetFirstorDefault(ID);
        }

        public IEnumerable<Rolee> GetRoleList()
        {
            return _unitOfWork.GetRepositoryInstance<Rolee>().GetAllRecords();
        }
    }
}
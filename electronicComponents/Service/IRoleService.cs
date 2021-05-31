using electronicComponents.DAL;
using System.Collections.Generic;

namespace electronicComponents.Service
{
    public interface IRoleService
    {
        IEnumerable<Rolee> GetRoleList();
        Rolee GetRoleByID(int ID);
    }
}
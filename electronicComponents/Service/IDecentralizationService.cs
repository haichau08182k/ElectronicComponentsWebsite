using electronicComponents.DAL;
using System.Collections.Generic;

namespace electronicComponents.Service
{
    public interface IDecentralizationService
    {
        IEnumerable<Decentralization> GetDecentralizationByEmloyeeTypeID(int ID);
        void RemoveRange(IEnumerable<Decentralization> decentralizations);
        void Add(Decentralization decentralization);
    }
}
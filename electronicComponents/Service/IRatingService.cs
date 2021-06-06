using electronicComponents.DAL;
using System.Collections.Generic;

namespace electronicComponents.Service
{
    public interface IRatingService
    {
        void AddRating(Rating rating);
        int GetRating(int ProductID);
        IEnumerable<Rating> GetListRating(int ProductID);
        IEnumerable<Rating> GetListAllRating();
    }
}
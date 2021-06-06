using electronicComponents.DAL;
using electronicComponents.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace electronicComponents.Service
{
    public class RatingService : IRatingService
    {
        private readonly GenericUnitOfWork context;
        public RatingService(GenericUnitOfWork repositoryContext)
        {
            this.context = repositoryContext;
        }
        public void AddRating(Rating rating)
        {
            context.GetRepositoryInstance<Rating>().Add(rating);
        }

        public IEnumerable<Rating> GetListAllRating()
        {
            return context.GetRepositoryInstance<Rating>().GetAllRecords();
        }

        public IEnumerable<Rating> GetListRating(int ProductID)
        {
            return context.GetRepositoryInstance<Rating>().GetAllRecords(x => x.productID == ProductID);
        }

        public int GetRating(int ProductID)
        {
            IEnumerable<Rating> ratings = context.GetRepositoryInstance<Rating>().GetAllRecords(x => x.productID == ProductID);
            List<int> list = ratings.Select(x => x.star.Value).ToList();
            int sum = 0;
            foreach (int item in list)
            {
                sum += item;
            }
            if (sum > 0)
                return sum / list.Count;
            else
                return 0;
        }
    }
}
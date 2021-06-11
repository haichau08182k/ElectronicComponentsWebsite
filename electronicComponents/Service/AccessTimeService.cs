using electronicComponents.DAL;
using electronicComponents.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace electronicComponents.Service
{
    public interface IAccessTimesCountService
    {
        int GetSum();
        void AddCount(DateTime Date);
        IEnumerable<AccessTimeCount> GetListAccessTimeCountStatistic(DateTime from, DateTime to);
    }
    public class AccessTimesCountService : IAccessTimesCountService
    {
        private readonly GenericUnitOfWork _unitOfWork;
        public AccessTimesCountService(GenericUnitOfWork repositoryContext)
        {
            this._unitOfWork = repositoryContext;
        }

        public void AddCount(DateTime Date)
        {
            AccessTimeCount accessTimesCount = (AccessTimeCount)_unitOfWork.GetRepositoryInstance<AccessTimeCount>().GetAllRecords(x => x.datee.Value.Date == Date.Date);
            accessTimesCount.acessTime += 1;
            _unitOfWork.GetRepositoryInstance<AccessTimeCount>().Update(accessTimesCount);
        }

        public IEnumerable<AccessTimeCount> GetListAccessTimeCountStatistic(DateTime from, DateTime to)
        {
            IEnumerable<AccessTimeCount> accessTimesCounts = _unitOfWork.GetRepositoryInstance<AccessTimeCount>().GetAllRecords(x => DbFunctions.TruncateTime(x.datee) >= from.Date && DbFunctions.TruncateTime(x.datee) <= to.Date);
            return accessTimesCounts;
        }
        public int GetSum()
        {
            return _unitOfWork.GetRepositoryInstance<AccessTimeCount>().GetAllRecords().Sum(x => x.acessTime.Value);
        }
    }
}
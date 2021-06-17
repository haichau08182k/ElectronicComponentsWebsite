using electronicComponents.DAL;
using System.Collections.Generic;

namespace electronicComponents.Service
{
    public interface IQAService
    {
        QA AddQA(QA qA);
        IEnumerable<QA> GetQAByProductID(int ID);
        QA GetQAByID(int ID);
        void UpdateQA(QA qA);
        IEnumerable<QA> GetQAList();
        IEnumerable<QA> GetQAListInHome();
    }
}
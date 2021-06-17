using electronicComponents.DAL;
using electronicComponents.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace electronicComponents.Service
{
    public class QAService : IQAService
    {
        private readonly GenericUnitOfWork _unitOfWork;
        public QAService(GenericUnitOfWork repository_unitOfWork)
        {
            this._unitOfWork = repository_unitOfWork;
        }
        public QA AddQA(QA qA)
        {
            this._unitOfWork.GetRepositoryInstance<QA>().Add(qA);
            return qA;
        }

        public QA GetQAByID(int ID)
        {
            QA qA = this._unitOfWork.GetRepositoryInstance<QA>().GetFirstorDefault(ID);
            return qA;
        }

        public IEnumerable<QA> GetQAByProductID(int ID)
        {
            IEnumerable<QA> listQA = this._unitOfWork.GetRepositoryInstance<QA>().GetAllRecords(x => x.productID == ID);
            return listQA;
        }

        public IEnumerable<QA> GetQAList()
        {
            IEnumerable<QA> listQA = this._unitOfWork.GetRepositoryInstance<QA>().GetAllRecords();
            return listQA;
        }

        public void UpdateQA(QA qA)
        {
            qA.dateAnswer = DateTime.Now;
            qA.statuss = true;
            this._unitOfWork.GetRepositoryInstance<QA>().Update(qA);
        }
        public IEnumerable<QA> GetQAListInHome()
        {
            IEnumerable<QA> listQA = this._unitOfWork.GetRepositoryInstance<QA>().GetAllRecords(x=>x.question!=null).Skip(3).OrderByDescending(x=>x.dateQuestion);
            return listQA;
        }
    }
}
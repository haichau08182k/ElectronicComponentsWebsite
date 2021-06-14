using electronicComponents.Repository;
using System;
using electronicComponents.DAL;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace electronicComponents.Service
{
    public class CommentService : ICommentService
    {
        private readonly GenericUnitOfWork _unitOfWork;
        public CommentService(GenericUnitOfWork repositoryContext)
        {
            this._unitOfWork = repositoryContext;
        }
        public Comment AddComment(Comment comment)
        {
            this._unitOfWork.GetRepositoryInstance<Comment>().Add(comment);
            return comment;
        }
        public IEnumerable<Comment> GetCommentByProductID(int ID)
        {
            IEnumerable<Comment> listComment = this._unitOfWork.GetRepositoryInstance<Comment>().GetAllRecords(x => x.productID == ID);
            return listComment;
        }
        public IEnumerable<Comment> GetCommentByMember(int ID)
        {
            IEnumerable<Comment> listComment = this._unitOfWork.GetRepositoryInstance<Comment>().GetAllRecords(x => x.memberID == ID);
            return listComment;
        }
    }
}
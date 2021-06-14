using System.Collections.Generic;
using electronicComponents.DAL;

namespace electronicComponents.Service
{
    public interface ICommentService
    {
        Comment AddComment(Comment comment);
        IEnumerable<Comment> GetCommentByProductID(int ID);
        IEnumerable<Comment> GetCommentByMember(int ID);

        
    }
}
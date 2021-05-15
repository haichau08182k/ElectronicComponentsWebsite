using electronicComponents.DAL;

namespace electronicComponents.Service
{
    public interface IMemberService
    {
        Member AddMember(Member member);
        Member CheckCapcha(int ID, string capcha);
        Member CheckLogin(string username, string password);
        Member GetByID(int ID);
        void UpdateCapcha(int ID, string capcha);
        void UpdateMember(Member member);
    }
}
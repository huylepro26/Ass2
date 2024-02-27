using BusinessObject;
using System.Collections.Generic;

namespace DataAccess
{
    public interface IMemberRepository
    {
        IEnumerable<MemberObject> GetMembers();
        MemberObject GetMemberByID(int memberID);
        void InsertMember(MemberObject member);
        void DeleteMember(int memberID);
        void UpdateMember(MemberObject member);
    }
}

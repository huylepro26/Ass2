using System;

namespace BusinessObject
{
    public class MemberObject
    {
        public int MemberID { get; set; }
        
        public String Email { get; set; }

        public String CompanyName { get; set; }
        public String Password { get; set; }
        public String City { get; set; }
        public String Country { get; set; }

        public override string ToString()
        {
            return $"{{{nameof(MemberID)}={MemberID.ToString()}, {nameof(CompanyName)}={CompanyName}, {nameof(Email)}={Email}, {nameof(Password)}={Password}, {nameof(City)}={City}, {nameof(Country)}={Country}, }}";
        }
    }
}

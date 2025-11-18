using Ardalis.Specification;
using ChatInRealTime.Core.Entites;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatInRealTime.Core.Specififcation
{
    public class UserSpecification
    {
        public class FindByEmail : Specification<AppUser>
        {
            public FindByEmail(string email)
            {
                Query.Where(b => b.Email == email);
            }
        }
    }
}

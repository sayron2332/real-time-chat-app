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
    public class TokenSpecification
    {
        public class GetByToken : Specification<RefreshToken>
        {
            public GetByToken(string token)
            {
                Query.Where(t => t.Token == token);
            }
        }
    }
}

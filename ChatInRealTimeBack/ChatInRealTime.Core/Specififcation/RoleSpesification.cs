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
    public class RoleSpesification
    {
        public class GetByName : Specification<AppRole>
        {

            public GetByName(string name)
            {
                Query.Where(b => b.Name == name);
            }
        }
    }
}

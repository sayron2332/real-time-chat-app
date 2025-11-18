using Ardalis.Specification;
using ChatInRealTime.Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatInRealTime.Core.Specififcation
{
    public class MessageSpecification
    {
        public class TakeFewMessages : Specification<AppMessage>
        {
            public TakeFewMessages(int count)
            {
                Query.OrderBy(m => m.Timestamp)
                .Take(count);
            }
        }
    }
}

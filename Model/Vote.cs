using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PickapicBackend.Model
{
    public class Vote
    {
        // own fields
        public long VoteId { get; set; }

        // foreign key
        public long ImageId { get; set; }
        public virtual Image Image { get; set; }
    }
}

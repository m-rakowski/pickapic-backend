using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PickapicBackend.Model
{
    public class Image
    {
        // own fields
        public long ImageId { get; set; }
        public string Url { get; set; }

        // foreign key
        public long PostId { get; set; }
        public virtual Post Post { get; set; }
    
        // list of votes
        public virtual List<Vote> Votes { get; set; }
    }
}

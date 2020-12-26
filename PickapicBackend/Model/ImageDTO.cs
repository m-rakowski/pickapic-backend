using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PickapicBackend.Model
{
    public class ImageDTO
    {
        public long ImageId { get; set; }
        public string Url { get; set; }
        public virtual List<VoteDTO> Votes { get; set; }

    }
}

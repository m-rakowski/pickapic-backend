using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PickapicBackend.Model
{
    public class Image
    {
        public long ImageId { get; set; }
        public long PostId { get; set; }
        public string Url { get; set; }
        public virtual Post Post { get; set; }
    }
}

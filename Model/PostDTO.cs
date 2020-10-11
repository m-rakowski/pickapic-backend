using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PickapicBackend.Model
{
    public class PostDTO
    {
        public long Id { get; set; }
        public string Question { get; set; }
        public DateTime AdditionDate { get; set; }
    }
}

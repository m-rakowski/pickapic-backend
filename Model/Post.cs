using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PickapicBackend.Model
{
    public class Post
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public DateTime AdditionDate { get; set; }
    }
}

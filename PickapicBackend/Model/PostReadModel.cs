using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PickapicBackend.Model
{
    public class PostReadModel
    {
        // own fields
        public string Question { get; set; }
        
        // list of images
        public virtual List<Image> Images { get; set; }


    }
}

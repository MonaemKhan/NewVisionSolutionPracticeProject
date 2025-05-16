using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Model
{
    public class APIReponce<T>
    {
        public T ResponceData { get; set; }
        public string ErrorMsg { get; set; }
    }
}

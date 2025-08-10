using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonModels
{
    public class OrderEvent
    {
        public Guid OrderId { get; set; }
        public int CustomerId { get; set; }
        public int OrderAmount { get; set; }
    }
}

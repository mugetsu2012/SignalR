using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoffeOrders.Web.Models
{
    public class CheckResultsDto
    {
        public bool Finished { get; set; }

        public string Update { get; set; }

        public bool New { get; set; }

    }
}

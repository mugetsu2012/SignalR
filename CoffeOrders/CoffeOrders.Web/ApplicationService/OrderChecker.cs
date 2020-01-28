using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoffeOrders.Web.Models;

namespace CoffeOrders.Web.ApplicationService
{
    public class OrderChecker: IOrderChecker
    {
        private readonly Random _random;
        private int index;

        private readonly string[] _status =
        {
            "Grinding Beans", "Steaming milk", "Takin a sip (quality control)", "On transit to encounter", "Picked up"
        };

        public OrderChecker(Random random)
        {
            _random = random;
        }

        public CheckResultsDto GetUpdate(int orderNo)
        {
            if (_random.Next(1, 5) == 4)
            {
                if (_status.Length - 1 > index)
                {
                    index++;
                    var result = new CheckResultsDto
                    {
                        Finished = _status.Length - 1 == index,
                        New = true,
                        Update = _status[index]
                    };

                    return result;
                }
            }

            return new CheckResultsDto()
            {
                New = false
            };
        }
    }
}

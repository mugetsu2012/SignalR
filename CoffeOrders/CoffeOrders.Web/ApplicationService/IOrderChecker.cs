using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoffeOrders.Web.Models;

namespace CoffeOrders.Web.ApplicationService
{
    public interface IOrderChecker
    {
        CheckResultsDto GetUpdate(int orderNo);
    }
}

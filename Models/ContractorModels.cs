using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Trading.Models
{
    public partial class ContractedOrder
    {
        public decimal ContractedCost
        {
            get
            {
                if (ContractorCost.HasValue && ContractorCostType.HasValue)
                {
                    if (ContractorCostType.Value == 0)
                    {
                        return Order.TotalSum * ((100 - ContractorCost.Value) / 100);
                    }
                    else if (ContractorCostType == 1)
                    {
                        return Order.TotalSum - ContractorCost.Value;
                    }
                }
                return 0;
            }
        }
    }
}
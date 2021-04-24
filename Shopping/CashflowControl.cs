using System;
using System.Collections.Generic;
using System.Text;

namespace Shopping
{
    public class CashFlowControl : ICashflowControl
    {
        public double LatestPurchase { get; set; }

        public void RecordPurchase(double amountPaid)
        {
            LatestPurchase = amountPaid;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Shopping
{
    public interface ICashflowControl
    {
        public double LatestPurchase { get; set; }
        void RecordPurchase(double amountPaid);
    }
}

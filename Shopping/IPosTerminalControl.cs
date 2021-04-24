using System;
using System.Collections.Generic;
using System.Text;

namespace Shopping
{
    public interface IPosTerminalControl
    {
        public bool ValidatePayment();
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Shopping
{
    public interface IWeightScale
    {
        // teszteles vegett
        public int LastMeasuredValue { get; set; }

        /// <summary>
        /// A termek sulya grammban
        /// </summary>
        /// <returns></returns>
        public int GetCurrentWeight();        
    }
}

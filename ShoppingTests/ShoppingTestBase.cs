using Shopping;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingTests
{
    public abstract class ShoppingTestBase
    {
        protected readonly Shop s = new Shop(new InMemoryInventory(), new WeightScaleMock());

        /**
         * Termékek ABC sorrendben, betűnként 10-el növekvő értékben.
         * Opcionális paraméterek nem kerülnek kitöltésre.
         * A -> 10
         * B -> 20
         * C -> 30 ...
         * 
         * Param:
         * @nrOfProducts, ennyi darab terméket regisztrál be, A-tól kezdve.
         */
        protected void RegisterProducts(int nrOfProducts)
        {
            int currentValue = 10;
            int charCode = 65; // 'A' ASCI karakterkódja
            for (int i = 0; i < nrOfProducts; i++)
            {
                s.RegisterProduct((char)charCode, currentValue);
                currentValue += 10;
                charCode++;
            }
        }
    }
}

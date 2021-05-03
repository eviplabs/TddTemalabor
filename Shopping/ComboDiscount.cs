using System.Linq;
using System.Collections.Generic;

namespace Shopping
{
    public class ComboDiscount : Discount
    {
        #region Variables
        private List<Product> dcProducts;
        private uint newPrice;
        #endregion

        #region Init
        public ComboDiscount(List<Product> discountedProducts, uint newPrice, bool membershipRequired = false) : base(membershipRequired)
        {
            dcProducts = discountedProducts;
            this.newPrice = newPrice;
        }
        #endregion

        #region Calculations

        public override double getDiscount(ref Dictionary<char, uint> productsInCart, bool hasMembership)
        {
            if (CheckIfEligible(hasMembership))
            {
                return 0;
            }
            if (!areConditionsFulfilled(productsInCart, hasMembership))
            {
                return 0;
            }

            uint maxOccurence = productsInCart.Max(i => i.Value);
            foreach (var product in dcProducts)
            {
                uint currentOccurence = productsInCart[product.name];
                if (maxOccurence > currentOccurence)
                {
                    maxOccurence = currentOccurence;
                }
            }
            removeFromCart(ref productsInCart, maxOccurence);
            return (dcProducts.Sum(p => p.price) - newPrice) * maxOccurence;
        }

        private void removeFromCart(ref Dictionary<char, uint> productsInCart, uint occurence)
        {
            foreach(var product in dcProducts)
            {
                if (productsInCart.Keys.Contains(product.name))
                {
                   productsInCart[product.name] -= occurence;
                }
            }
        }

        private bool areConditionsFulfilled(Dictionary<char, uint> productsInCart, bool hasMembership)
        {
            if (membershipRequired && (!hasMembership)
                || dcProducts.Where(i => !productsInCart.Keys.Contains(i.name)).Any())
            {
                return false;
            }
            return true;
        }
        #endregion
    }
}

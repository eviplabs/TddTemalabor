using System;
using System.Collections.Generic;
using System.Text;

namespace Shopping
{
    public static partial class CartProcessor
    {
        private static CartProcessorEvents readingState;
        public static void processData(string cart, out string userID, out Dictionary<char, int> productsInCart, out bool SSpay, out string code)
        {
            
            Dictionary<char, int> cartManager = new Dictionary<char, int>();
            string ID = null;
            bool SSpaymentReading = false;
            string coupon = null;

            for (int i = 0; i < cart.Length; i++)
            {
                readingState = getReadingEvent(readingState, cart[i]);
                if(readingState == CartProcessorEvents.ProductReading)
                {
                    cartManager = addProduct(cartManager, cart[i]);
                }
                else if(readingState == CartProcessorEvents.MassProductReading)
                {
                    int mass = Convert.ToInt32(cart[i].ToString()); // horrible conversion but you can't directly convert from char to int
                    cartManager[cart[i - 1]] += mass - 1;
                }
                else if(readingState == CartProcessorEvents.UserIDReading)
                {
                    if(cart[i] == 'v')
                    {
                        continue;
                    }
                    ID += cart[i];
                }
                else if(readingState == CartProcessorEvents.SuperShopPayment)
                {
                    SSpaymentReading = true;
                }
                else if(readingState == CartProcessorEvents.CouponReading)
                {
                    if (cart[i] == 'k')
                    {
                        continue;
                    }
                    coupon += cart[i];
                }
            }
            userID = ID;
            SSpay = SSpaymentReading;
            productsInCart = cartManager;
            code = coupon;
        }
        private static CartProcessorEvents getReadingEvent(CartProcessorEvents state, char element)
        {
            if(char.IsDigit(element))
            {
                if(state == CartProcessorEvents.ProductReading)
                {
                    return CartProcessorEvents.MassProductReading;
                }
                else if(state == CartProcessorEvents.UserIDReading)
                {
                    // returns itself since we are still reading the SS ID
                    return state;
                }
            }
            else if (char.IsUpper(element))
            {
                return CartProcessorEvents.ProductReading;
            }
            // signal for userIDReading
            else if(element == 'v')
            {
                return CartProcessorEvents.UserIDReading;
            }
            // signal for SuperShop payments
            else if (element == 'p')
            {
                return CartProcessorEvents.SuperShopPayment;
            }
            else if(element == 'k')
            {
                return CartProcessorEvents.CouponReading;
            }
            // default setting
            return CartProcessorEvents.ProductReading;
        }
        private static Dictionary<char, int> addProduct(Dictionary<char, int> cartManager, char element)
        {
            if (!cartManager.ContainsKey(element))
            {
                cartManager[element] = 1;
            }
            else
            {
                cartManager[element]++;
            }
            return cartManager;
        }
    }
}

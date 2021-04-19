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
            string numberSubstring = "";
            char currentProduct = '0';

            for (int i = 0; i < cart.Length; i++)
            {
                readingState = getReadingEvent(readingState, cart[i]);
                if(readingState == CartProcessorEvents.ProductReading)
                {
                    currentProduct = cart[i];
                    cartManager = addProduct(cartManager, currentProduct);
                }
                else if(readingState == CartProcessorEvents.MassProductReading)
                {
                    numberSubstring += cart[i];
                    if(!char.IsDigit(cart[i + 1]))
                    {
                        int mass = Convert.ToInt32(numberSubstring); // horrible conversion but you can't directly convert from char to int
                        cartManager[currentProduct] += mass - 1;
                        numberSubstring = "";
                    }
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
            return state;
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

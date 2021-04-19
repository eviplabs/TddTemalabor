using System;
using System.Collections.Generic;
using System.Text;

namespace Shopping
{
    public static partial class CartProcessor
    {
        private static CartProcessorEvents readingState;
        public static void processData(string cart, out string userID, out Dictionary<char, int> productsInCart, out bool SSpay)
        {
            
            Dictionary<char, int> cartManager = new Dictionary<char, int>();
            string ID = null;
            bool SSpaymentReading = false;

            for (int i = 0; i < cart.Length; i++)
            {
                readingState = getReadingEvent(readingState, cart[i]);
                if(readingState == CartProcessorEvents.ProductReading)
                {
                    cartManager = addProduct(cartManager, cart[i]);
                }
                else if(readingState == CartProcessorEvents.MassProductReading)
                {
                    cartManager = massAddProduct(cartManager, Convert.ToInt32(cart[i]), cart[i - 1]);
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
            }
            userID = ID;
            SSpay = SSpaymentReading;
            productsInCart = cartManager;
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
        private static Dictionary<char, int> massAddProduct(Dictionary<char, int> cartManager, int mass, char product)
        {
            cartManager[product] += mass - 1; // the -1 is added in the previous state
            return cartManager;
        }
    }
}

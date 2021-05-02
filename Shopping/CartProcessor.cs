using System;
using System.Collections.Generic;

namespace Shopping
{
    public partial class CartProcessor
    {
        public void processData(string cart, out string userID, out Dictionary<char, int> productsInCart, out bool SSpay, out string code, Dictionary<char, Product> products)
        {
            
            Dictionary<char, int> cartManager = new Dictionary<char, int>();
            string ID = null;
            bool SSpaymentReading = false;
            string coupon = null;
            string numberSubstring = "";
            char currentProduct = '0';

            foreach(var element in cart)
            {
                readingState = getReadingEvent(readingState, element);
                if (numberSubstring != "" && readingState != CartProcessorEvents.MassProductReading)
                {
                    int mass = Convert.ToInt32(numberSubstring); // horrible conversion but you can't directly convert from char to int
                    if (products[currentProduct].priceInKilo)
                    {
                        double count = mass / 10.0;
                        cartManager[currentProduct] += (int)Math.Round(count, MidpointRounding.AwayFromZero) - 1;
                    }
                    else
                    {
                        cartManager[currentProduct] += mass - 1;
                    }
                    numberSubstring = "";
                }
                switch (readingState)
                {
                    case CartProcessorEvents.ProductReading:
                        currentProduct = element;
                        cartManager = addProduct(cartManager, currentProduct);
                        break;
                    case CartProcessorEvents.MassProductReading:
                        numberSubstring += element;
                        break;
                    case CartProcessorEvents.UserIDReading:
                        if (element == userIDKey)
                        {
                            continue;
                        }
                        ID += element;
                        break;
                    case CartProcessorEvents.SuperShopPayment:
                        SSpaymentReading = true;
                        break;
                    case CartProcessorEvents.CouponReading:
                        if (element == couponKey)
                        {
                            continue;
                        }
                        coupon += element;
                        break;
                }
            }
            if(numberSubstring != "")
            {
                int mass = Convert.ToInt32(numberSubstring); // horrible conversion but you can't directly convert from char to int
                if (products[currentProduct].priceInKilo)
                {
                    double count = mass / 10.0;
                    cartManager[currentProduct] += (int)Math.Round(count, MidpointRounding.AwayFromZero) - 1;
                }
                else
                {
                    cartManager[currentProduct] += mass - 1;
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
            else if(element == userIDKey)
            {
                return CartProcessorEvents.UserIDReading;
            }
            // signal for SuperShop payments
            else if (element == superShopPaymentKey)
            {
                return CartProcessorEvents.SuperShopPayment;
            }
            else if(element == couponKey)
            {
                return CartProcessorEvents.CouponReading;
            }
            // default setting
            return state;
        }
        private Dictionary<char, int> addProduct(Dictionary<char, int> cartManager, char element)
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

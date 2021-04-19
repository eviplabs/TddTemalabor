﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Shopping
{
    public partial class CartProcessor
    {
        public static void processData(string cart, out string userID, out Dictionary<char, int> productsInCart, out bool SSpay, out string code)
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
                    cartManager[currentProduct] += mass - 1;
                    numberSubstring = "";
                }
                if (readingState == CartProcessorEvents.ProductReading)
                {
                    currentProduct = element;
                    cartManager = addProduct(cartManager, currentProduct);
                }
                else if(readingState == CartProcessorEvents.MassProductReading)
                {
                    numberSubstring += element;
                }
                else if(readingState == CartProcessorEvents.UserIDReading)
                {
                    if(element == userIDKey)
                    {
                        continue;
                    }
                    ID += element;
                }
                else if(readingState == CartProcessorEvents.SuperShopPayment)
                {
                    SSpaymentReading = true;
                }
                else if(readingState == CartProcessorEvents.CouponReading)
                {
                    if (element == couponKey)
                    {
                        continue;
                    }
                    coupon += element;
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

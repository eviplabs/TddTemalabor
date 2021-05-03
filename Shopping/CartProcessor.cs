using System;
using System.Collections.Generic;

namespace Shopping
{
    public partial class CartProcessor
    {
        public void processData(string cart, out string userID, out Dictionary<char, uint> productsInCart, 
                                        out bool SSpay, out string code, Dictionary<char, Product> products)
        {
            foreach (var element in cart)
            {
                readingState = getReadingEvent(readingState, element);
                if (dh.numberSubstring != "" && readingState != CartProcessorEvents.MassProductReading)
                {
                    uint mass = Convert.ToUInt32(dh.numberSubstring); // horrible conversion but you can't directly convert from char to int
                    if (products[dh.currentProduct].priceInKilo)
                    {
                        double count = mass / 10.0;
                        dh.cartManager[dh.currentProduct] += (uint)Math.Round(count, MidpointRounding.AwayFromZero) - 1;
                    }
                    else
                    {
                        dh.cartManager[dh.currentProduct] += mass - 1;
                    }
                    dh.numberSubstring = "";
                }
                switch (readingState)
                {
                    case CartProcessorEvents.ProductReading:
                        dh.currentProduct = element;
                        addProduct();
                        break;
                    case CartProcessorEvents.MassProductReading:
                        dh.numberSubstring += element;
                        break;
                    case CartProcessorEvents.UserIDReading:
                        if (element == userIDKey)
                        {
                            continue;
                        }
                        dh.ID += element;
                        break;
                    case CartProcessorEvents.SuperShopPayment:
                        dh.SSpaymentReading = true;
                        break;
                    case CartProcessorEvents.CouponReading:
                        if (element == couponKey)
                        {
                            continue;
                        }
                        dh.coupon += element;
                        break;
                }
            }
            if(dh.numberSubstring != "")
            {
                uint mass = Convert.ToUInt32(dh.numberSubstring); // horrible conversion but you can't directly convert from char to int
                if (products[dh.currentProduct].priceInKilo)
                {
                    double count = mass / 10.0;
                    dh.cartManager[dh.currentProduct] += (uint)Math.Round(count, MidpointRounding.AwayFromZero) - 1;
                }
                else
                {
                    dh.cartManager[dh.currentProduct] += mass - 1;
                }
            }
            userID = dh.ID;
            SSpay = dh.SSpaymentReading;
            productsInCart = dh.cartManager;
            code = dh.coupon;
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
        private void addProduct()
        {
            if (!dh.cartManager.ContainsKey(dh.currentProduct))
            {
                dh.cartManager[dh.currentProduct] = 1;
            }
            else
            {
                dh.cartManager[dh.currentProduct]++;
            }
        }
    }
}

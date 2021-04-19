using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Shopping;

namespace ShoppingTests
{
    public class ComboDcTests
    {
        #region Variables
        private readonly Shop sh = new Shop();
        #endregion

        #region Init
        public ComboDcTests()
        {
            sh.RegisterProduct('A', 10);
            sh.RegisterProduct('B', 20);
            sh.RegisterProduct('C', 50);
            sh.RegisterProduct('D', 100);
        }
        #endregion

        #region Helper Methods
        private void AssertPrice(double expected, string cart)
        {
            int result = sh.GetPrice(cart);
            Assert.Equal(expected, result);
        }
        #endregion
        [Fact]
        public void RegisterComboDiscount()
        {
            List<Product> productList = new List<Product>();
            productList.Add(sh.products['A']);
            productList.Add(sh.products['B']);
            productList.Add(sh.products['C']);
            sh.RegisterDiscount("ABC", new ComboDiscount(productList, 60));
            AssertPrice(110, "CAAAABB");
        }

        [Fact]
        public void RegisterComboDiscountWithFalseDiscount()
        {
            List<Product> productList = new List<Product>();
            productList.Add(sh.products['A']);
            productList.Add(sh.products['B']);
            productList.Add(sh.products['C']);
            productList.Add(sh.products['D']);
            sh.RegisterDiscount("ABCD", new ComboDiscount(productList, 60));
            AssertPrice(130, "CAAAABB");
        }

        [Fact]
        public void RegisterComboDiscountWithMultipleAppliableDiscounts()
        {
            List<Product> productList = new List<Product>();
            productList.Add(sh.products['A']);
            productList.Add(sh.products['B']);
            productList.Add(sh.products['C']);
            sh.RegisterDiscount("ABC", new ComboDiscount(productList, 60));
            AssertPrice(130, "AABBCCA");
        }
        [Fact]
        public void MultipleComboDiscounts()
        {
            List<Product> productList1 = new List<Product>();
            List<Product> productList2 = new List<Product>();
            List<Product> productList3 = new List<Product>();
            productList1.Add(sh.products['A']);
            productList1.Add(sh.products['B']);
            sh.RegisterDiscount("AB", new ComboDiscount(productList1, 20));
            productList2.Add(sh.products['A']);
            productList2.Add(sh.products['B']);
            productList2.Add(sh.products['C']);
            sh.RegisterDiscount("ABC", new ComboDiscount(productList2, 20));
            productList3.Add(sh.products['A']);
            productList3.Add(sh.products['C']);
            sh.RegisterDiscount("AC", new ComboDiscount(productList3, 20));
            AssertPrice(120, "ABCD"); // 180 - 60
        }
        [Fact]
        public void MultipleAppliableComboDiscounts()
        {
            List<Product> productList1 = new List<Product>();
            List<Product> productList2 = new List<Product>();
            List<Product> productList3 = new List<Product>();
            productList1.Add(sh.products['A']);
            productList1.Add(sh.products['B']);
            sh.RegisterDiscount("AB", new ComboDiscount(productList1, 20));
            productList2.Add(sh.products['A']);
            productList2.Add(sh.products['B']);
            productList2.Add(sh.products['C']);
            sh.RegisterDiscount("ABC", new ComboDiscount(productList2, 20));
            productList3.Add(sh.products['A']);
            productList3.Add(sh.products['C']);
            sh.RegisterDiscount("AC", new ComboDiscount(productList3, 20));
            AssertPrice(140, "AABBCD"); // 210 - 60 - 10
        }
        [Fact]
        public void ComboDiscountWithMemberShip()
        {
            List<Product> productList = new List<Product>();
            productList.Add(sh.products['A']);
            productList.Add(sh.products['B']);
            productList.Add(sh.products['C']);
            sh.RegisterSuperShopCard("1");
            sh.RegisterDiscount("ABC", new ComboDiscount(productList, 60, true)); // 3. taggal (bool) megadható hogy a kedvezmény csak klubtagoknak jár-e
            //20+40+100=160 (comboDiscount csak tagoknak)
            AssertPrice(160, "AABBCC");
            //(60+60)*0,9  comboDiscount és MemberShipDiscount is
            AssertPrice(108, "AABBCCv1");
        }
        [Fact]
        public void ToggleDiscountOnlyForClubMembersCombo()
        {
            List<Product> productList1 = new List<Product>();
            productList1.Add(sh.products['A']);
            productList1.Add(sh.products['B']);
            sh.RegisterDiscount("AB", new ComboDiscount(productList1, 20, true));
            sh.RegisterSuperShopCard("1");
            AssertPrice(18, "ABv1");
            AssertPrice(30, "AB");
        }
    }
}

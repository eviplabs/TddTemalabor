namespace Shopping
{
    public class Product
    {
        public char name { get; private set; }
        public int price { get; private set; }
        //if priceInKilo has been set to true --> price means the product's price for each 100gramm;
        //Which means the lowest price is 1 / 100g , and if the amount is under 100 it only takes the integer form price / 100 with a simple math.round() (for example 1299 equals to 1300)
        public bool priceInKilo { get; private set; }
        public Product(char name, int price, bool priceInKilo = false)
        {
            this.name = name;
            this.price = price;
            this.priceInKilo = priceInKilo;
        }
    }
}

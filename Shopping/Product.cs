namespace Shopping
{
    public class Product
    {
        public char name { get; private set; }
        public int price { get; private set; }
        //if priceInKilo has been set to true --> price means the product's price for each 10gramm;
        //Which means the lowest price is 1 / 10g , and if the amount is under 10 it only takes the integer form price / 10.0 with a simple math.round() (for example 1209g equals to 1210g)
        public bool priceInKilo { get; private set; }
        public Product(char name, int price, bool priceInKilo = false)
        {
            this.name = name;
            this.price = price;
            this.priceInKilo = priceInKilo;
        }
    }
}

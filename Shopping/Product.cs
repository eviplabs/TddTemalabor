namespace Shopping
{
    public class Product
    {
        public char name { get; private set; }
        public int price { get; private set; }
        //if priceInKilo has been set to true --> price means the product's price for each 100gramm;
        //Which means the lowest price is 1 / 100g !
        public bool priceInKilo { get; private set; }
        public Product(char name, int price, bool priceInKilo = false)
        {
            this.name = name;
            this.price = price;
            this.priceInKilo = priceInKilo;
        }
    }
}

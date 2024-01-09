namespace ST10090758_FarmCentral.Models
{
    public class Product
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public double Price { get; set; }
         public DateTime SupplyDate { get; set; }

        public int UserID { get; set; }

        public Product(int productID, string productName,string description, string type, double price, DateTime supplyDate, int userID)
        {
            this.ProductID = productID;
            this.ProductName = productName;
            this.Description = description;
            this.Type = type;
            this.Price = price;
            this.SupplyDate = supplyDate;
            this.UserID= userID;
        }

        public Product(string productName, string type, string description, double price, DateTime supplyDate, int userID)
        {
            ProductName = productName;
            Description = description;
            Type = type;
            Price = price;
            SupplyDate = supplyDate;
            UserID = userID;
        }

        public Product()
        {

        }
    }
}

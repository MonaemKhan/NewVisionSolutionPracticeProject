namespace Product.Model
{
    public class ProductModel
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public decimal price { get; set; }
        public int StockQuentity {  get; set; }
        public string LastUpdate { get; set; }
    }
}

namespace FourthWallCafe.MVC.Models
{
    public class CafeOrderViewModel
    {
        public int ServerId { get; set; }             
        public string ServerName { get; set; }
        public List<ItemPrice> AvailableItems { get; set; } = new List<ItemPrice>();
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public int SelectedItemPriceID { get; set; }
        public int Quantity { get; set; }
    }
}


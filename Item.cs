using System;
using System.ComponentModel.DataAnnotations;

namespace FourthWallCafe.MVC.Models;

public class Item
{
    [Key]
    public int ItemID { get; set; }
    public Category Category { get; set; } = new Category();

    public string ItemName { get; set; }
    public string ItemDescription { get; set; }
    public List<ItemPrice> Prices { get; set; } = new List<ItemPrice>();
    public ICollection<OrderItem> OrderItems { get; set; }
}

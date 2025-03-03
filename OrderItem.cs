using System;

namespace FourthWallCafe.MVC.Models;

public class OrderItem
{
    public int OrderItemID {get; set;}
    public int OrderID {get; set;}
    public int ItemPriceID {get; set;}
    public int Quantity {get; set;}
    public decimal ExtendedPrice {get; set;}
    public CafeOrder Order { get; set; }
    public ItemPrice ItemPrice { get; set; }
}

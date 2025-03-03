using System;
using System.ComponentModel.DataAnnotations;


namespace FourthWallCafe.MVC.Models;

public class CafeOrder
{
    [Key]
    public int OrderID {get; set;}
    public int ServerID {get; set;}
    public int? PaymentTypeID {get; set;}
    public PaymentType PaymentType { get; set; }
    public DateTime OrderDate {get; set;}
    public decimal SubTotal {get; set;}
     public Server Server { get; set; } 
    public decimal Tax {get; set;}
    public decimal Tip {get; set;}
    public decimal AmountDue {get; set;}
    public ICollection<OrderItem> OrderItems { get; set; }
}

using System;

namespace FourthWallCafe.MVC.Models;

public class PaymentType
{
    public int PaymentTypeID {get; set;}
    public string PaymentTypeName {get; set;}
    public ICollection<CafeOrder> Orders { get; set; }
}

using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FourthWallCafe.MVC.Models;
using Microsoft.EntityFrameworkCore;

namespace FourthWallCafe.MVC.Controllers;

public class CafeController : Controller
{
    private readonly YourDbContext _context;


    public CafeController(YourDbContext context)
    {
        _context = context;
    }



    public IActionResult Index()
    {
         var openOrders = _context.CafeOrders
            .Where(o => o.PaymentTypeID == null)
            .ToList();
        return View(openOrders);



    }



    public IActionResult CreateNewOrder()
    {
        var activeServers = _context.Servers
            .Where(s => s.HireDate <= DateTime.Now && (s.TermDate == null || s.TermDate > DateTime.Now))
            .ToList();

        return View(activeServers);
    }

    public IActionResult CreateOrder(int serverId)
    {
        var server = _context.Servers.FirstOrDefault(s => s.ServerId == serverId);
        if (server == null)
        {
            return NotFound();
        }

        var items = _context.ItemPrices
            .Include(ip => ip.Item)
            .Include(ip => ip.TimeOfDay)
            .Select(ip => new ItemPrice
            {
                ItemPriceID = ip.ItemPriceID,
                Price = ip.Price,
                Item = ip.Item,               // Ensure Item is loaded
                TimeOfDay = ip.TimeOfDay      // Ensure TimeOfDay is loaded
            })
            .ToList();

        var viewModel = new CafeOrderViewModel
        {
            ServerId = server.ServerId,
            ServerName = $"{server.FirstName} {server.LastName}",
            AvailableItems = items
        };

        return View(viewModel);
    }




    [HttpPost]
    public IActionResult CreateOrderConfirmed(int serverId, int[] SelectedItemPriceIDs, int[] Quantities)
    {
        if (SelectedItemPriceIDs.Length != Quantities.Length)
        {
            return BadRequest("Mismatched items and quantities.");
        }

        var newOrder = new CafeOrder
        {
            ServerID = serverId,
            OrderDate = DateTime.Now,
            OrderItems = new List<OrderItem>()
        };

        for (int i = 0; i < SelectedItemPriceIDs.Length; i++)
        {
            newOrder.OrderItems.Add(new OrderItem
            {
                ItemPriceID = SelectedItemPriceIDs[i],
                Quantity = Quantities[i],
                Order = newOrder
            });
        }

        _context.CafeOrders.Add(newOrder);
        _context.SaveChanges();

        return RedirectToAction("Index");
    }




    public IActionResult OrderDetails(int id)
    {
        var order = _context.CafeOrders
       .Include(o => o.Server)
       .Include(o => o.OrderItems)
           .ThenInclude(oi => oi.ItemPrice)
               .ThenInclude(ip => ip.Item)
       .FirstOrDefault(o => o.OrderID == id);

        if (order == null)
        {
            return NotFound();
        }



        ViewBag.Order = order;
        return View(order);
    }

    public IActionResult AddItem(int orderId)
    {
        ViewBag.OrderId = orderId;
        var items = _context.ItemPrices
             .Include(i => i.Item) // Assuming Items have a Prices navigation property
             .ToList();
        return View(items);
    }

    public IActionResult AddItemToOrder(int orderId, int itemPriceId, int quantity)
    {
        var orderItem = new OrderItem
        {
            OrderID = orderId,
            ItemPriceID = itemPriceId,
            Quantity = quantity,

        };

        _context.OrderItems.Add(orderItem);
        _context.SaveChanges();

        return RedirectToAction("OrderDetails", new { id = orderId });
    }

    public IActionResult ProcessPayment(int orderId)
    {
        ViewBag.OrderId = orderId;
        var paymentTypes = _context.PaymentTypes.ToList();
        return View(paymentTypes);
    }



[HttpPost]
public IActionResult SubmitPayment(int orderId, int paymentTypeId, string cardNumber, string expirationDate, string cvv)
{
    var order = _context.CafeOrders.Find(orderId);
    if (order == null)
    {
        return NotFound();
    }

    // Assign payment type
    order.PaymentTypeID = paymentTypeId;

    // Example: Optional simple card validation logic (skip if unnecessary)
    if (paymentTypeId == 1) // Assuming 1 = Card Payment
    {
        if (string.IsNullOrWhiteSpace(cardNumber) || cardNumber.Length < 12)
        {
            ModelState.AddModelError("CardNumber", "Invalid card number.");
            return View("ProcessPayment", _context.PaymentTypes.ToList());
        }
    }

    // Assume payment successful
    order.AmountDue = 0;

    _context.SaveChanges();

    return RedirectToAction("Index");
}





    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

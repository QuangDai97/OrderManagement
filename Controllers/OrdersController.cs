using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Data;
using OrderManagement.Models;
using System.Linq;

namespace OrderManagement.Controllers
{
    public class OrdersController(ApplicationDbContext context) : Controller
    {
        public async Task<IActionResult> Index(string search, DateTime? startDate, DateTime? endDate)
        {
            IQueryable<Order> orders = context.Orders.AsQueryable();

            if (!string.IsNullOrEmpty(search))
                orders = orders.Where(o => o.OrderNo.Contains(search) ||
                o.DeptCode.Contains(search) ||
                o.CustCode.Contains(search) ||
                o.EmpCode.Contains(search) ||
                o.WhCode.Contains(search));
            if (startDate.HasValue)
                orders = orders.Where(o => o.OrderDate >= startDate.Value);
            if (endDate.HasValue)
                orders = orders.Where(o => o.OrderDate <= endDate.Value);

            return View(await orders.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Order order)
        {
            var existingOrder = await context.Orders.FirstOrDefaultAsync(o => o.OrderNo == order.OrderNo);
            if (existingOrder != null)
            {
                ModelState.AddModelError("OrderNo", "OrderNo đã tồn tại. Vui lòng nhập OrderNo khác.");
                return View(order);
            }

            context.Add(order);
            await context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Order created successfully!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Order order)
        {
            try
            {
                context.Update(order);
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(order.OrderNo))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(string OrderNo)
        {
            if (OrderNo == null)
            {
                return NotFound();
            }

            var order = await context.Orders
                .FirstOrDefaultAsync(m => m.OrderNo == OrderNo);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string OrderNo)
        {
            if (OrderNo == null)
            {
                return NotFound();
            }
            var order = await context.Orders
                .FirstOrDefaultAsync(m => m.OrderNo == OrderNo);
            if (order == null)
            {
                return NotFound();
            }

            var orderDetails = context.OrderDetails.Where(od => od.OrderNo == OrderNo);
            context.OrderDetails.RemoveRange(orderDetails);

            context.Orders.Remove(order);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Statistics(DateTime? startDate, DateTime? endDate, string textSearch)
        {
            var orders = context.Orders.AsQueryable();

            if (startDate.HasValue)
                orders = orders.Where(o => o.OrderDate >= startDate.Value);
            if (endDate.HasValue)
                orders = orders.Where(o => o.OrderDate <= endDate.Value);
            if (!string.IsNullOrWhiteSpace(textSearch))
            {
                orders = orders.Where(o => o.DeptCode.Contains(textSearch) ||
                o.CustCode.Contains(textSearch) ||
                o.EmpCode.Contains(textSearch) ||
                o.WhCode.Contains(textSearch));
            }
            
            var orderStatistics = await orders
                .Select(o => new
                {
                    o.OrderNo,
                    o.OrderDate,
                    o.DeptCode,
                    o.CustCode,
                    o.EmpCode,
                    o.WhCode,
                    TotalAmount = context.OrderDetails
                        .Where(od => od.OrderNo == o.OrderNo)
                        .Sum(od => od.Quantity * od.UnitPrice * (1 - od.Discount / 100.0))
                })
                .ToListAsync();

            return View(orderStatistics);
        }

        private bool OrderExists(string id)
        {
            return context.Orders.Any(e => e.OrderNo == id);
        }


        void AddDummyData()
        {
            var orders = new List<Order>
    {
        new Order
        {
            OrderNo = "ORD001",
            OrderDate = DateTime.Parse("2024-07-25"),
            DeptCode = "DEP001",
            CustCode = "CUST001",
            EmpCode = "EMP001",
            RequiredDate = "2024-07-30",
            CustOrderNo = "CUST_ORDER_001",
            WhCode = "WH001",
            CmpTax = 10,
            SlipComment = "Urgent delivery needed",
            UpdateDate = DateTime.Now,
            Updater = "admin",
            OrderDetails = new List<OrderDetail>
            {
                new OrderDetail
                {
                    OrderNo = "ORD001",
                    SoRowNo = 1,
                    ProdCode = "PROD001",
                    ProdName = "Product 1",
                    UnitPrice = 100,
                    Quantity = 10,
                    CmpTaxRate = 10,
                    ReserveQty = 0,
                    DeliveryOrderQty = 0,
                    DeliveredQty = 0,
                    CompleteFlg = 0,
                    Discount = 0,
                    DeliveryDate = "2024-07-30",
                    UpdateDate = DateTime.Now,
                    Updater = "admin"
                },
                new OrderDetail
                {
                    OrderNo = "ORD001",
                    SoRowNo = 2,
                    ProdCode = "PROD002",
                    ProdName = "Product 2",
                    UnitPrice = 150,
                    Quantity = 5,
                    CmpTaxRate = 10,
                    ReserveQty = 0,
                    DeliveryOrderQty = 0,
                    DeliveredQty = 0,
                    CompleteFlg = 0,
                    Discount = 0,
                    DeliveryDate = "2024-07-30",
                    UpdateDate = DateTime.Now,
                    Updater = "admin"
                }
            }
        },
        new Order
        {
            OrderNo = "ORD002",
            OrderDate = DateTime.Parse("2024-07-24"),
            DeptCode = "DEP002",
            CustCode = "CUST002",
            EmpCode = "EMP002",
            RequiredDate = "2024-07-29",
            CustOrderNo = "CUST_ORDER_002",
            WhCode = "WH002",
            CmpTax = 8,
            SlipComment = "Standard delivery",
            UpdateDate = DateTime.Now,
            Updater = "admin",
            OrderDetails = new List<OrderDetail>
            {
                new OrderDetail
                {
                    OrderNo = "ORD002",
                    SoRowNo = 1,
                    ProdCode = "PROD003",
                    ProdName = "Product 3",
                    UnitPrice = 80,
                    Quantity = 8,
                    CmpTaxRate = 8,
                    ReserveQty = 0,
                    DeliveryOrderQty = 0,
                    DeliveredQty = 0,
                    CompleteFlg = 0,
                    Discount = 0,
                    DeliveryDate = "2024-07-29",
                    UpdateDate = DateTime.Now,
                    Updater = "admin"
                }
            }
        }
    };
            context.Orders.AddRange(orders);
            context.SaveChanges();
        }
    }
}

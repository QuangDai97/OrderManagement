﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Data;
using OrderManagement.Models;
using static System.Formats.Asn1.AsnWriter;

namespace OrderManagement.Controllers
{
    public class OrdersController(ApplicationDbContext context) : Controller
    {
        // GET: Order
        public async Task<IActionResult> Index(string search , DateTime? startDate, DateTime? endDate)
        {
            // Gọi 1 lần để update data
            //AddDummyData();
            IQueryable<Order> orders = context.Orders.AsQueryable();

            if (!string.IsNullOrEmpty(search))
                orders = orders.Where(o => o.OrderNo.Contains(search)
                || o.DeptCode.Contains(search) || o.CustCode.Contains(search) || o.EmpCode.Contains(search)
                 || o.WhCode.Contains(search));
            if (startDate.HasValue)
                orders = orders.Where(o => o.OrderDate >= startDate.Value);
            if (endDate.HasValue)
                orders = orders.Where(o => o.OrderDate <= endDate.Value);

            return View(await orders.ToListAsync());
        }

        // GET: Order/Create
        public IActionResult Create()
        {
            return View();
        }


        // POST: Order/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderNo,OrderDate,DeptCode,CustCode,EmpCode,RequiredDate,CustOrderNo,WhCode,CmpTax,SlipComment")] Order order)
        {
            // Kiểm tra nếu OrderNo đã tồn tại
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
        // GET: Order/Edit/5
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

        // POST: Order/Edit/5
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
            //}
            //return View(order);
        }

        // GET: Order/Delete/5
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

        // POST: Order/Delete/5
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

            // Xóa các OrderDetail liên quan
            var orderDetails = context.OrderDetails.Where(od => od.OrderNo == OrderNo);
            context.OrderDetails.RemoveRange(orderDetails);

            // Xóa Order
            context.Orders.Remove(order);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Order/Statistics
        public async Task<IActionResult> Statistics(DateTime? startDate, DateTime? endDate, string deptCode, string custCode, string empCode, string whCode)
        {
            var orders = context.Orders.AsQueryable();

            if (startDate.HasValue)
                orders = orders.Where(o => o.OrderDate >= startDate.Value);
            if (endDate.HasValue)
                orders = orders.Where(o => o.OrderDate <= endDate.Value);
            if (!string.IsNullOrEmpty(deptCode))
                orders = orders.Where(o => o.DeptCode.Contains(deptCode));
            if (!string.IsNullOrEmpty(custCode))
                orders = orders.Where(o => o.CustCode.Contains(custCode));
            if (!string.IsNullOrEmpty(empCode))
                orders = orders.Where(o => o.EmpCode.Contains(empCode));
            if (!string.IsNullOrEmpty(whCode))
                orders = orders.Where(o => o.WhCode.Contains(whCode));

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
            // Tạo và thêm các đối tượng Order vào cơ sở dữ liệu
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

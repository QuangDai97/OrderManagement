using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Data;
using OrderManagement.Models;

namespace OrderManagement.Controllers
{
    public class OrdersController(ApplicationDbContext context) : Controller
    {
        // GET: Order
        public async Task<IActionResult> Index(string orderNo, string deptCode, string custCode, string empCode, string whCode, DateTime? startDate, DateTime? endDate)
        {
            IQueryable<Order> orders = context.Orders.AsQueryable();

            if (!string.IsNullOrEmpty(orderNo))
                orders = orders.Where(o => o.OrderNo.Contains(orderNo));
            if (!string.IsNullOrEmpty(deptCode))
                orders = orders.Where(o => o.DeptCode.Contains(deptCode));
            if (!string.IsNullOrEmpty(custCode))
                orders = orders.Where(o => o.CustCode.Contains(custCode));
            if (!string.IsNullOrEmpty(empCode))
                orders = orders.Where(o => o.EmpCode.Contains(empCode));
            if (!string.IsNullOrEmpty(whCode))
                orders = orders.Where(o => o.WhCode.Contains(whCode));
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
            if (ModelState.IsValid)
            {
                context.Add(order);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(order);
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
        public async Task<IActionResult> Edit(string id, [Bind("OrderNo,OrderDate,DeptCode,CustCode,EmpCode,RequiredDate,CustOrderNo,WhCode,CmpTax,SlipComment")] Order order)
        {
            if (id != order.OrderNo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
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
            return View(order);
        }

        // GET: Order/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await context.Orders
                .FirstOrDefaultAsync(m => m.OrderNo == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var order = await context.Orders.FindAsync(id);
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
    }
}

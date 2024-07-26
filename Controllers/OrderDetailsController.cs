using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Data;
using OrderManagement.Models;

namespace OrderManagement.Controllers
{
    public class OrderDetailsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderDetailsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string parentId)
        {
            if (string.IsNullOrEmpty(parentId))
            {
                return NotFound();
            }
            ViewBag.ParentId = parentId;
            var orderDetails = await _context.OrderDetails
                                             .Where(od => od.Order.OrderNo == parentId)
                                             .ToListAsync();

            return View(orderDetails);
        }
        // GET: OrderDetails/Details/5
        public async Task<IActionResult> Details(string OrderNo)
        {
            if (OrderNo == null)
            {
                return NotFound();
            }

            var orderDetail = await _context.OrderDetails
                .FirstOrDefaultAsync(m => m.OrderNo == OrderNo);
            if (orderDetail == null)
            {
                return NotFound();
            }

            return View(orderDetail);
        }

        // GET: OrderDetails/Create
        public async Task<IActionResult> Create(string id )
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewBag.OrderId = id;
            return View();
        }

        // POST: OrderDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string OrderNoParent, [Bind("OrderNo,SoRowNo,ProdCode,ProdName,UnitPrice,Quantity,CmpTaxRate,ReserveQty,DeliveryOrderQty,DeliveredQty,CompleteFlg,Discount,DeliveryDate,UpdateDate,Updater")] OrderDetail orderDetail)
        {
            var order = await _context.Orders.FindAsync(OrderNoParent);
            orderDetail.Order = order;
            //if (ModelState.IsValid)
            //{
                _context.Add(orderDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { parentId = OrderNoParent });
            //}
            return View(orderDetail);
        }

        // GET: OrderDetails/Edit/5
        public async Task<IActionResult> Edit(string OrderNo)
        {
            if (OrderNo == null)
            {
                return NotFound();
            }

            var orderDetail =  _context.OrderDetails
                                             .Where(od => od.OrderNo == OrderNo).FirstOrDefault();
            if (orderDetail == null)
            {
                return NotFound();
            }
            return View(orderDetail);
        }

        // POST: OrderDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("OrderNo,SoRowNo,ProdCode,ProdName,UnitPrice,Quantity,CmpTaxRate,ReserveQty,DeliveryOrderQty,DeliveredQty,CompleteFlg,Discount,DeliveryDate,UpdateDate,Updater")] OrderDetail orderDetail)
        {
            //if (OrderNo != orderDetail.OrderNo)
            //{
            //    return NotFound();
            //}

            //if (ModelState.IsValid)
            //{
                try
                {
                    _context.Update(orderDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderDetailExists(orderDetail.OrderNo))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { parentId = orderDetail.Order.OrderNo });
            //}
            return View(orderDetail);
        }

        // GET: OrderDetails/Delete/5
        public async Task<IActionResult> Delete(string OrderNo)
        {
            if (OrderNo == null)
            {
                return NotFound();
            }

            var orderDetail = await _context.OrderDetails
                .FirstOrDefaultAsync(m => m.OrderNo == OrderNo);
            if (orderDetail == null)
            {
                return NotFound();
            }

            return View(orderDetail);
        }

        // POST: OrderDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string OrderNo)
        {
            var orderDetail = await _context.OrderDetails.FindAsync(OrderNo);
            if (orderDetail != null)
            {
                _context.OrderDetails.Remove(orderDetail);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderDetailExists(string id)
        {
            return _context.OrderDetails.Any(e => e.OrderNo == id);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
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
        public async Task<IActionResult> Create(string id)
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
        public async Task<IActionResult> Create([Bind("OrderNo,SoRowNo,ProdCode,ProdName,UnitPrice,Quantity,CmpTaxRate,ReserveQty,DeliveryOrderQty,DeliveredQty,CompleteFlg,Discount,DeliveryDate,UpdateDate,Updater")] OrderDetail orderDetail)
        {
            var orderCheck = _context.OrderDetails
                           .Include(od => od.Order) // Eager load the Order navigation property
                           .FirstOrDefault(od => od.OrderNo == orderDetail.OrderNo && od.SoRowNo == orderDetail.SoRowNo);
            if (orderCheck != null)
            {
                // Nếu OrderDetail đã tồn tại
                ModelState.AddModelError("SoRowNo", "SoRowNo đã tồn tại. Vui lòng nhập SoRowNo khác.");
                ViewBag.OrderId = orderDetail.OrderNo;
                return View(orderDetail);
            }

            var order = _context.Orders.FirstOrDefault(od => od.OrderNo == orderDetail.OrderNo);
            orderDetail.Order = order;
            orderDetail.UpdateDate = DateTime.Now;
            _context.Add(orderDetail);
            await _context.SaveChangesAsync();

            // Lưu thông báo thành công vào TempData
            TempData["SuccessMessage"] = "Order detail created successfully!";
            return RedirectToAction(nameof(Index), new { parentId = orderDetail.OrderNo });
        }

        // GET: OrderDetails/Edit/5
        public async Task<IActionResult> Edit(string OrderNo, int SoRowNo)
        {
            if (OrderNo == null)
            {
                return NotFound();
            }

            var orderDetail = _context.OrderDetails
                             .Include(od => od.Order) // Eager load the Order navigation property
                             .FirstOrDefault(od => od.OrderNo == OrderNo && od.SoRowNo == SoRowNo);
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
            orderDetail.UpdateDate = DateTime.Now;
            try
            {
                _context.Update(orderDetail);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderDetailExists(orderDetail.OrderNo, orderDetail.SoRowNo))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index), new { parentId = orderDetail.OrderNo });
            //}
        }

        // GET: OrderDetails/Delete/5
        public async Task<IActionResult> Delete(string OrderNo, int SoRowNo)
        {
            if (OrderNo == null)
            {
                return NotFound();
            }

            var orderDetail = await _context.OrderDetails
                .FirstOrDefaultAsync(m => m.OrderNo == OrderNo && m.SoRowNo == SoRowNo);
            if (orderDetail == null)
            {
                return NotFound();
            }

            return View(orderDetail);
        }

        // POST: OrderDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string OrderNo, int SoRowNo)
        {

            var orderDetail = _context.OrderDetails.FirstOrDefault(od => od.OrderNo == OrderNo && od.SoRowNo == SoRowNo); ;
            if (orderDetail != null)
            {
                _context.OrderDetails.Remove(orderDetail);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { parentId = orderDetail.OrderNo });
        }

        private bool OrderDetailExists(string id, int soRowNo)
        {
            return _context.OrderDetails.Any(e => e.OrderNo == id && e.SoRowNo == soRowNo);
        }
    }
}

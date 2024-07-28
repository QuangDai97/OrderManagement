using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> Details(string OrderNo)
        {
            if (OrderNo == null)
            {
                return NotFound();
            }

            var orderDetail = await _context.OrderDetails.FirstOrDefaultAsync(m => m.OrderNo == OrderNo);
            if (orderDetail == null)
            {
                return NotFound();
            }

            return View(orderDetail);
        }

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderDetail orderDetail)
        {
            var orderCheck = _context.OrderDetails
                           .Include(od => od.Order)
                           .FirstOrDefault(od => od.OrderNo == orderDetail.OrderNo && od.SoRowNo == orderDetail.SoRowNo);
            if (orderCheck != null)
            {
                ModelState.AddModelError("SoRowNo", "SoRowNo đã tồn tại. Vui lòng nhập SoRowNo khác.");
                ViewBag.OrderId = orderDetail.OrderNo;
                return View(orderDetail);
            }

            var order = _context.Orders.FirstOrDefault(od => od.OrderNo == orderDetail.OrderNo);
            orderDetail.Order = order;
            orderDetail.UpdateDate = DateTime.Now;
            _context.Add(orderDetail);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Order detail created successfully!";
            return RedirectToAction(nameof(Index), new { parentId = orderDetail.OrderNo });
        }

        public async Task<IActionResult> Edit(string OrderNo, int SoRowNo)
        {
            if (OrderNo == null)
            {
                return NotFound();
            }

            var orderDetail = _context.OrderDetails
                             .Include(od => od.Order)
                             .FirstOrDefault(od => od.OrderNo == OrderNo && od.SoRowNo == SoRowNo);
            if (orderDetail == null)
            {
                return NotFound();
            }
            return View(orderDetail);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(OrderDetail orderDetail)
        {
            
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
        }

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

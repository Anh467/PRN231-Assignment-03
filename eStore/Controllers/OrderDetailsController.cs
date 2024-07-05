using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Models;
using DataAccess.DataBase;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using BusinessObject.Utils.MyContansts;
using Microsoft.EntityFrameworkCore.Query;
using eBookStore.Utils;

namespace eStore.Controllers
{
    [Authorize]
    public class OrderDetailsController : Controller
    {
        private readonly ApplicationDBContext _context;
        private readonly string _url;

        public OrderDetailsController(ApplicationDBContext context)
        {
            _context = context;
            this._url = "https://localhost:7221/api/OrderDetails";
        }

        public async Task<IActionResult> Statistic(DateTime? start, DateTime? end)
        {
            var urlTemp = this._url;
/*             urlTemp = $"{this._url}?$filter= Order.OrderDate gt {start} and Order.OrderDate lt {end}";
            if(start == null || end == null)
            {
                urlTemp = this._url;
            }*/
            var orderDetails = await ApiHandler.DeserializeApiResponse<IEnumerable<OrderDetail>>(urlTemp, HttpMethod.Get);
            if (!(start == null || end == null))
            {
                orderDetails = orderDetails.Where(a=> DateTime.Compare( a.Order.OrderDate, start.Value) > 0 && DateTime.Compare(a.Order.OrderDate, end.Value) < 0).ToList();
            }
            return View(orderDetails);
        }

        // GET: OrderDetails
        public async Task<IActionResult> Index(int id)
        {
            var urlTemp = $"{this._url}?$filter= OrderId eq {id}";

            var orderDetails = await ApiHandler.DeserializeApiResponse<IEnumerable<OrderDetail>>(urlTemp, HttpMethod.Get);
            
            return View(orderDetails);
        }

        // GET: OrderDetails/Details/5
        public async Task<IActionResult> Details(int? orderid, int? productid)
        {
            if (orderid == null || productid == null)
            {
                return NotFound();
            }

            var orderDetail = await _context.OrderDetails
                .Include(o => o.Order)
                .Include(o => o.Product)
                .FirstOrDefaultAsync(m => m.ProductId == productid && m.OrderId == orderid);

            if (orderDetail == null)
            {
                return NotFound();
            }

            return View(orderDetail);
        }

        // GET: OrderDetails/Create
        public IActionResult Create()
        {
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "MemberId");
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductName");
            return View();
        }

        // POST: OrderDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderId,ProductId,UnitPrice,Quantity,UnitsInStock")] OrderDetail orderDetail)
        {
            if (ModelState.IsValid)
            {
                _context.Add(orderDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "MemberId", orderDetail.OrderId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductName", orderDetail.ProductId);
            return View(orderDetail);
        }

        // GET: OrderDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderDetail = await _context.OrderDetails.FindAsync(id);
            if (orderDetail == null)
            {
                return NotFound();
            }
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "MemberId", orderDetail.OrderId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductName", orderDetail.ProductId);
            return View(orderDetail);
        }

        // POST: OrderDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,ProductId,UnitPrice,Quantity,UnitsInStock")] OrderDetail orderDetail)
        {
            if (id != orderDetail.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orderDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderDetailExists(orderDetail.ProductId))
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
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "MemberId", orderDetail.OrderId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductName", orderDetail.ProductId);
            return View(orderDetail);
        }

        // GET: OrderDetails/Delete/5
        public async Task<IActionResult> Delete(int? orderid, int? productid)
        {
            if (productid == null || orderid == null)
            {
                return NotFound();
            }

            var urlTemp = $"{this._url}/{orderid}/{productid}";
            var orderDetail = await ApiHandler.DeserializeApiResponse<OrderDetail>(urlTemp, HttpMethod.Get);

            if (orderDetail == null)
            {
                return NotFound();
            }

            return View(orderDetail);
        }

        // POST: OrderDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? orderid, int? productid)
        {
            var urlTemp = $"{this._url}/{orderid}/{productid}";

            var orderDetail = await ApiHandler.DeserializeApiResponse<OrderDetail>(urlTemp, HttpMethod.Get);
            if (orderDetail != null)
            {
                await ApiHandler.DeserializeApiResponse<OrderDetail>(urlTemp, HttpMethod.Delete);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderDetailExists(int id)
        {
            return _context.OrderDetails.Any(e => e.ProductId == id);
        }
    }
}

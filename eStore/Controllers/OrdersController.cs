using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Models;
using DataAccess.DataBase;
using System.Security.Claims;
using eBookStore.Utils;
using BusinessObject.Utils.MyContansts;
using Microsoft.AspNetCore.Authorization;

namespace eStore.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly ApplicationDBContext _context;
        private readonly string _url;

        public OrdersController(ApplicationDBContext context)
        {
            _context = context;
            this._url = "https://localhost:7221/api/Orders";
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var urlTemp = this._url;
            var role = User.Claims.FirstOrDefault(a => ClaimTypes.Role == a.Type)?.Value ?? "User";
            var userid = User.Claims.FirstOrDefault(a => ClaimTypes.NameIdentifier == a.Type)?.Value ?? "0";

            if (role.Equals(Roles.User.ToString()))
            {
                urlTemp = $"{this._url}?$filter={nameof(Order.MemberId)} eq '{userid}'";
            }

            IEnumerable<Order> orders = await ApiHandler.DeserializeApiResponse<IEnumerable<Order>>(urlTemp, HttpMethod.Get) ?? [];
            return View(orders);
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var urlTemp = $"{this._url}/{id}";

            var order = await ApiHandler.DeserializeApiResponse<Order>(urlTemp, HttpMethod.Get) ?? null;
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            ViewData["MemberId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderId,MemberId,OrderDate,RequiredDate,ShippedDate,Freight")] Order order)
        {
            if (ModelState.IsValid)
            {
                await ApiHandler.DeserializeApiResponse<Order>(this._url, HttpMethod.Post, order);
/*                _context.Add(order);
                await _context.SaveChangesAsync();*/
                return RedirectToAction(nameof(Index));
            }
            ViewData["MemberId"] = new SelectList(_context.Users, "Id", "Id", order.MemberId);
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var urlTemp = $"{this._url}/{id}";
            var order = await ApiHandler.DeserializeApiResponse<Order>(urlTemp, HttpMethod.Get) ?? null;

            if (order == null)
            {
                return NotFound();
            }
            ViewData["MemberId"] = new SelectList(_context.Users, "Id", "Id", order.MemberId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,MemberId,OrderDate,RequiredDate,ShippedDate,Freight")] Order order)
        {
            if (id != order.OrderId)
            {
                return NotFound();
            }

            var urlTemp = $"{this._url}/{id}";

            if (ModelState.IsValid)
            {
                try
                {
                    await ApiHandler.DeserializeApiResponse<Order>(urlTemp, HttpMethod.Put, order);
/*                    _context.Update(order);
                    await _context.SaveChangesAsync();*/
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderId))
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
            ViewData["MemberId"] = new SelectList(_context.Users, "Id", "Id", order.MemberId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var urlTemp = $"{this._url}/{id}";
            var order = await ApiHandler.DeserializeApiResponse<Order>(urlTemp, HttpMethod.Get) ?? null;

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                var urlTemp = $"{this._url}/{id}";
                await ApiHandler.DeserializeApiResponse<Order>(urlTemp, HttpMethod.Delete);
/*                _context.Orders.Remove(order);*/
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderId == id);
        }
    }
}

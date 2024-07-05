using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Models;
using DataAccess.DataBase;
using Microsoft.AspNetCore.OData.Query;

namespace eStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailsController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public OrderDetailsController(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET: api/OrderDetails
        [HttpGet]
        [EnableQuery]
        public async Task<ActionResult<IEnumerable<OrderDetail>>> GetOrderDetails()
        {
            var a = _context.OrderDetails.ToList().OrderByDescending(a => a.Price);
            return Ok(a.AsQueryable());
        }

        // GET: api/OrderDetails/5
        [HttpGet("{orderid}/{productid}")]
        public async Task<ActionResult<OrderDetail>> GetOrderDetail(int orderid, int productid)
        {
            var orderDetail = await _context.OrderDetails.FindAsync( orderid, productid);

            if (orderDetail == null)
            {
                return NotFound();
            }

            return orderDetail;
        }

        // PUT: api/OrderDetails/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{orderid}/{productid}")]
        public async Task<IActionResult> PutOrderDetail(int orderid, int productid, OrderDetail orderDetail)
        {
            if (productid != orderDetail.ProductId && orderid != orderDetail.OrderId)
            {
                return BadRequest();
            }

            _context.Entry(orderDetail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderDetailExists(orderid, productid))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/OrderDetails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<OrderDetail>> PostOrderDetail(OrderDetail orderDetail)
        {
            _context.OrderDetails.Add(orderDetail);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (OrderDetailExists(orderDetail.OrderId, orderDetail.ProductId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetOrderDetail", new { id =  new { orderDetail.OrderId , orderDetail.ProductId } }, orderDetail);
        }

        // DELETE: api/OrderDetails/5/2
        [HttpDelete("{orderid}/{productid}")]
        public async Task<IActionResult> DeleteOrderDetail(int orderid, int productid)
        {
            var orderDetail = await _context.OrderDetails.FindAsync( orderid , productid);
            if (orderDetail == null)
            {
                return NotFound();
            }

            _context.OrderDetails.Remove(orderDetail);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderDetailExists(int orderid, int productid)
        {
            return _context.OrderDetails.Any(e => e.ProductId == productid && e.OrderId == orderid);
        }
    }
}

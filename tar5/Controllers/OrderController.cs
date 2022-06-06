using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using tar5.Models;

namespace tar5.Controllers
{
    public class OrderController : ApiController
    {
        // Reading all order by user id
        // GET api/<controller>/5
        public List<OrderWithDetails> Get(int id)
        {
            return Order.ReadAllOrders(id);
        }

        // Cancel order with order id & user id
        [HttpDelete]
        [Route("api/CancelOrder/{id}/{userId}")]
        public List<OrderWithDetails> CancelOrder(int id, int userId)
        {
            return Order.CancelOrder(id, userId);
        }

        // Placing order
        // POST api/<controller>
        public int Post([FromBody] Order value)
        {
            return value.CreateOrder();
        }

    }
}
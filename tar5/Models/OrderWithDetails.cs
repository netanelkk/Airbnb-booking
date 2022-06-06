using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tar5.Models
{
    // Order With Apartment Details (name & picture)
    public class OrderWithDetails : Order
    {
        private string picture;
        private string name;

        public OrderWithDetails(int id, int userId, int apartmentId, string fromDate, string toDate, string orderDate, float totalPrice, string cancelDate, string picture, string name) : base(id, userId, apartmentId, fromDate, toDate, orderDate, totalPrice, cancelDate)
        {
            this.picture = picture;
            this.name = name;
        }

        public string Picture { get => picture; set => picture = value; }
        public string Name { get => name; set => name = value; }
    }
}
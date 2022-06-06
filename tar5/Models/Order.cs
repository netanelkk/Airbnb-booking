using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using tar5.Models.DAL;

namespace tar5.Models
{
    public class Order
    {
        private int id;
        private int userId;
        private int apartmentId;
        private string fromDate;
        private string toDate;
        private string orderDate;
        private float totalPrice;
        private string cancelDate;

        public Order(int id, int userId, int apartmentId, string fromDate, string toDate, string orderDate, float totalPrice, string cancelDate)
        {
            this.id = id;
            this.userId = userId;
            this.apartmentId = apartmentId;
            this.fromDate = fromDate;
            this.toDate = toDate;
            this.orderDate = orderDate;
            this.totalPrice = totalPrice;
            this.cancelDate = cancelDate;
        }

        public int Id { get => id; set => id = value; }
        public int UserId { get => userId; set => userId = value; }
        public int ApartmentId { get => apartmentId; set => apartmentId = value; }
        public string FromDate { get => fromDate; set => fromDate = value; }
        public string ToDate { get => toDate; set => toDate = value; }
        public string OrderDate { get => orderDate; set => orderDate = value; }
        public float TotalPrice { get => totalPrice; set => totalPrice = value; }
        public string CancelDate { get => cancelDate; set => cancelDate = value; }

        public int CreateOrder()
        {
            DataServices ds = new DataServices();
            return ds.NewOrder(this);
        }

        public static List<OrderWithDetails> ReadAllOrders(int userId)
        {
            DataServices ds = new DataServices();
            return ds.ReadUsersOrders(userId);
        }

        public static List<OrderWithDetails> CancelOrder(int id, int userId)
        {
            DataServices ds = new DataServices();
            return ds.CancelOrder(id, userId);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tar5.Models
{
    public class AdminUsers
    {
        private int id;
        private string email;
        private string username;
        private string registerDate;
        private int orders;
        private int totalProfit;
        private int orderCancellations;

        public AdminUsers(int id, string email, string username, string registerDate, int orders, int totalProfit, int orderCancellations)
        {
            this.id = id;
            this.email = email;
            this.username = username;
            this.orders = orders;
            this.totalProfit = totalProfit;
            this.orderCancellations = orderCancellations;
            this.registerDate = registerDate;
        }

        public int Id { get => id; set => id = value; }
        public string Email { get => email; set => email = value; }
        public string Username { get => username; set => username = value; }
        public int Orders { get => orders; set => orders = value; }
        public int TotalProfit { get => totalProfit; set => totalProfit = value; }
        public int OrderCancellations { get => orderCancellations; set => orderCancellations = value; }
        public string RegisterDate { get => registerDate; set => registerDate = value; }
    }
}
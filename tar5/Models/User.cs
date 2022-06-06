using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using tar5.Models.DAL;

namespace tar5.Models
{
    public class User
    {
        private int id;
        private string email;
        private string username;
        private string password;
        private string registerDate;

        public User(int id, string email, string username, string password, string registerDate)
        {
            this.id = id;
            this.email = email;
            this.username = username;
            this.password = password;
            this.registerDate = registerDate;
        }

        public int Register()
        {
            DataServices ds = new DataServices();
            return ds.Register(this);
        }

        public User Login()
        {
            DataServices ds = new DataServices();
            return ds.Login(this);
        }

        public int Id { get => id; set => id = value; }
        public string Email { get => email; set => email = value; }
        public string Username { get => username; set => username = value; }
        public string Password { get => password; set => password = value; }
        public string RegisterDate { get => registerDate; set => registerDate = value; }

        public static List<AdminUsers> AdminReadUsers()
        {
            DataServices ds = new DataServices();
            return ds.AdminReadUsers();
        }

    }
}
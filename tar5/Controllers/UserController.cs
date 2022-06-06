using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using tar5.Models;

namespace tar5.Controllers
{
    public class UserController : ApiController
    {
        // Login method with email & password
        [HttpGet]
        [Route("api/LoginUser/{email}/{password}")]
        public User LoginUser(string email, string password)
        {
            User loginUser = new User(0, email, "", password, "");
            return loginUser.Login();
        }

        // Users data for admin
        [HttpGet]
        [Route("api/AdminUsers")]
        public List<AdminUsers> AdminUsers()
        {
            return tar5.Models.User.AdminReadUsers();
        }

        // Register method
        // POST api/<controller>
        public int Post([FromBody] User value)
        {
            return value.Register();
        }

    }
}

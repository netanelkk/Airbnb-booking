using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using tar5.Models;
using tar5.Models.DAL;

namespace tar5.Controllers
{
    public class ApartmentController : ApiController
    {

        // Apartments data for admin
        [HttpGet]
        [Route("api/AdminApartments")]
        public List<AdminApartments> AdminApartments()
        {
            return tar5.Models.AdminApartments.AdminReadApartments();
        }

        // Hosts data for admin
        [HttpGet]
        [Route("api/AdminHosts")]
        public List<AdminHosts> AdminHosts()
        {
            return tar5.Models.AdminHosts.AdminReadHosts();
        }

        // Method for reading info from excel files
        // GET api/<controller>
        public HttpResponseMessage Get()
        {
            DataServices ds = new DataServices();
            string value = ds.ReadExcel();
            if (value.Equals("ok"))
            {
                return Request.CreateResponse(HttpStatusCode.OK, "Data Inserted Successfuly!");
            }
            return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Couldn't Import Data: " + value);
        }

        // Get full apartment data by apartment id
        // GET api/<controller>/5
        public Apartment Get(int id)
        {
            DataServices ds = new DataServices();
            return ds.ReadApartment(id);
        }

        // Read apartments with filtering option, and returning number of rows and list of apartments
        // POST api/<controller>
        public Dictionary<int, List<Apartment>> Post([FromBody] Filter value)
        {
            DataServices ds = new DataServices();
            return ds.ReadApartments(value);
        }

    }

}
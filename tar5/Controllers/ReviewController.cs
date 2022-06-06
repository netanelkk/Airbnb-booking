using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using tar5.Models.DAL;

namespace tar5.Controllers
{
    public class ReviewController : ApiController
    {
        // Read reviews for apartment id & page number
        [HttpGet]
        [Route("api/ReadReviews/{id}/{page}")]
        public List<Review> ReadReviews(int id, int page)
        {
            DataServices ds = new DataServices();
            return ds.ReadReviews(id, page);
        }

    }
}
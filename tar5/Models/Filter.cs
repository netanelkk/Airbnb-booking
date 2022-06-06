using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tar5.Models
{
    public class Filter
    {
        private int priceFrom;
        private int priceTo;
        private float rating;
        private int numOfRooms;
        private float distFromCenter;
        private string fromDate;
        private string toDate;
        private int page;
        private string keywords;
        public Filter(int priceFrom, int priceTo, float rating, int numOfRooms, float distFromCenter, string fromDate, string toDate, int page, string keywords)
        {
            this.priceFrom = priceFrom;
            this.priceTo = priceTo;
            this.rating = rating;
            this.numOfRooms = numOfRooms;
            this.distFromCenter = distFromCenter;
            this.fromDate = fromDate;
            this.toDate = toDate;
            this.page = page;
            this.keywords = keywords;
        }

        public int PriceFrom { get => priceFrom; set => priceFrom = value; }
        public int PriceTo { get => priceTo; set => priceTo = value; }
        public float Rating { get => rating; set => rating = value; }
        public int NumOfRooms { get => numOfRooms; set => numOfRooms = value; }
        public float DistFromCenter { get => distFromCenter; set => distFromCenter = value; }
        public string FromDate { get => fromDate; set => fromDate = value; }
        public string ToDate { get => toDate; set => toDate = value; }
        public int Page { get => page; set => page = value; }
        public string Keywords { get => keywords; set => keywords = value; }
    }
}
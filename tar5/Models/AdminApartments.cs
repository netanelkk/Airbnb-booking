using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using tar5.Models.DAL;

namespace tar5.Models
{
    public class AdminApartments
    {
        private int id;
        private string picture_url;
        private string name;
        private int totalDaysRented;
        private int totalCancellations;

        public AdminApartments(int id, string picture_url, string name, int totalDaysRented, int totalCancellations)
        {
            this.id = id;
            this.picture_url = picture_url;
            this.name = name;
            this.totalDaysRented = totalDaysRented;
            this.totalCancellations = totalCancellations;
        }

        public int Id { get => id; set => id = value; }
        public string Picture_url { get => picture_url; set => picture_url = value; }
        public string Name { get => name; set => name = value; }
        public int TotalDaysRented { get => totalDaysRented; set => totalDaysRented = value; }
        public int TotalCancellations { get => totalCancellations; set => totalCancellations = value; }


        public static List<AdminApartments> AdminReadApartments()
        {
            DataServices ds = new DataServices();
            return ds.AdminReadApartments();
        }


    }
}
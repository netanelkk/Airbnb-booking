using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using tar5.Models.DAL;

namespace tar5.Models
{
    public class AdminHosts
    {
        private int host_id;
        private string host_name;
        private string host_since;
        private string host_picture_url;
        private int apartmentsCount;
        private int totalProfit;
        private int totalCancellations;

        public AdminHosts(int host_id, string host_name, string host_since, string host_picture_url, int apartmentsCount, int totalProfit, int totalCancellations)
        {
            this.host_id = host_id;
            this.host_name = host_name;
            this.host_since = host_since;
            this.host_picture_url = host_picture_url;
            this.apartmentsCount = apartmentsCount;
            this.totalProfit = totalProfit;
            this.totalCancellations = totalCancellations;
        }

        public int Host_id { get => host_id; set => host_id = value; }
        public string Host_name { get => host_name; set => host_name = value; }
        public string Host_since { get => host_since; set => host_since = value; }
        public string Host_picture_url { get => host_picture_url; set => host_picture_url = value; }
        public int ApartmentsCount { get => apartmentsCount; set => apartmentsCount = value; }
        public int TotalProfit { get => totalProfit; set => totalProfit = value; }
        public int TotalCancellations { get => totalCancellations; set => totalCancellations = value; }


        public static List<AdminHosts> AdminReadHosts()
        {
            DataServices ds = new DataServices();
            return ds.AdminReadHosts();
        }
    }
}
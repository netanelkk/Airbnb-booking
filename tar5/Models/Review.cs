using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tar5.Models.DAL
{
    public class Review
    {
        private string date;
        private string reviewer_name;
        private string comments;

        public Review(string date, string reviewer_name, string comments)
        {
            this.date = date;
            this.reviewer_name = reviewer_name;
            this.comments = comments;
        }

        public string Date { get => date; set => date = value; }
        public string Reviewer_name { get => reviewer_name; set => reviewer_name = value; }
        public string Comments { get => comments; set => comments = value; }
    }
}
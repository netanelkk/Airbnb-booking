using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using tar5.Models.DAL;

namespace tar5.Models
{
    public class Apartment
    {
        private int id;
        private string name;
        private string description;
        private string neighborhood_overview;
        private string picture_url;
        private int host_id;
        private string host_name;
        private string host_since;
        private string host_picture_url;
        private float latitude;
        private float longitude;
        private string property_type;
        private string room_type;
        private short accommodates;
        private string bathrooms_text;
        private short bedrooms;
        private short beds;
        private string[] amenities;
        private float price;
        private short minimum_nights;
        private short maximum_nights;
        private short number_of_reviews;
        private float review_scores_rating;
        private float review_scores_accuracy;
        private float review_scores_cleanliness;
        private float review_scores_checkin;
        private float review_scores_communication;
        private float review_scores_location;
        private float review_scores_value;


        
        public Apartment(int id, string name, string picture_url, string host_name, float latitude, float longitude, string property_type, short accommodates, short bedrooms, float price, short number_of_reviews, float review_scores_rating)
        {
            this.id = id;
            this.name = name;
            this.picture_url = picture_url;
            this.host_name = host_name;
            this.latitude = latitude;
            this.longitude = longitude;
            this.property_type = property_type;
            this.accommodates = accommodates;
            this.bedrooms = bedrooms;
            this.price = price;
            this.number_of_reviews = number_of_reviews;
            this.review_scores_rating = review_scores_rating;
        }

        // Full detailed apartment
        public Apartment(int id, string name, string description, string neighborhood_overview, string picture_url, int host_id, string host_name, string host_since, string host_picture_url, float latitude, float longitude, string property_type, string room_type, short accommodates, string bathrooms_text, short bedrooms, short beds, string[] amenities, float price, short minimum_nights, short maximum_nights, short number_of_reviews, float review_scores_rating, float review_scores_accuracy, float review_scores_cleanliness, float review_scores_checkin, float review_scores_communication, float review_scores_location, float review_scores_value)
        {
            this.id = id;
            this.name = name;
            this.description = description;
            this.neighborhood_overview = neighborhood_overview;
            this.picture_url = picture_url;
            this.host_id = host_id;
            this.host_name = host_name;
            this.host_since = host_since;
            this.host_picture_url = host_picture_url;
            this.latitude = latitude;
            this.longitude = longitude;
            this.property_type = property_type;
            this.room_type = room_type;
            this.accommodates = accommodates;
            this.bathrooms_text = bathrooms_text;
            this.bedrooms = bedrooms;
            this.beds = beds;
            this.amenities = amenities;
            this.price = price;
            this.minimum_nights = minimum_nights;
            this.maximum_nights = maximum_nights;
            this.number_of_reviews = number_of_reviews;
            this.review_scores_rating = review_scores_rating;
            this.review_scores_accuracy = review_scores_accuracy;
            this.review_scores_cleanliness = review_scores_cleanliness;
            this.review_scores_checkin = review_scores_checkin;
            this.review_scores_communication = review_scores_communication;
            this.review_scores_location = review_scores_location;
            this.review_scores_value = review_scores_value;
        }

        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string Description { get => description; set => description = value; }
        public string Neighborhood_overview { get => neighborhood_overview; set => neighborhood_overview = value; }
        public string Picture_url { get => picture_url; set => picture_url = value; }
        public string Host_name { get => host_name; set => host_name = value; }
        public float Latitude { get => latitude; set => latitude = value; }
        public float Longitude { get => longitude; set => longitude = value; }
        public string Property_type { get => property_type; set => property_type = value; }
        public string Room_type { get => room_type; set => room_type = value; }
        public short Accommodates { get => accommodates; set => accommodates = value; }
        public string Bathrooms_text { get => bathrooms_text; set => bathrooms_text = value; }
        public short Bedrooms { get => bedrooms; set => bedrooms = value; }
        public short Beds { get => beds; set => beds = value; }
        public string[] Amenities { get => amenities; set => amenities = value; }
        public float Price { get => price; set => price = value; }
        public short Minimum_nights { get => minimum_nights; set => minimum_nights = value; }
        public short Maximum_nights { get => maximum_nights; set => maximum_nights = value; }
        public short Number_of_reviews { get => number_of_reviews; set => number_of_reviews = value; }
        public float Review_scores_rating { get => review_scores_rating; set => review_scores_rating = value; }
        public float Review_scores_accuracy { get => review_scores_accuracy; set => review_scores_accuracy = value; }
        public float Review_scores_cleanliness { get => review_scores_cleanliness; set => review_scores_cleanliness = value; }
        public float Review_scores_checkin { get => review_scores_checkin; set => review_scores_checkin = value; }
        public float Review_scores_communication { get => review_scores_communication; set => review_scores_communication = value; }
        public float Review_scores_location { get => review_scores_location; set => review_scores_location = value; }
        public float Review_scores_value { get => review_scores_value; set => review_scores_value = value; }
        public string Host_since { get => host_since; set => host_since = value; }
        public string Host_picture_url { get => host_picture_url; set => host_picture_url = value; }
        public int Host_id { get => host_id; set => host_id = value; }
    }
}
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.Configuration;

namespace tar5.Models.DAL
{
    public class DataServices
    {
        // Admin panel - fetch apartments details
        public List<AdminApartments> AdminReadApartments()
        {
            SqlConnection con = null;
            try
            {
                con = Connect();

                SqlCommand command = new SqlCommand();

                command.CommandText = "spAdminReadApartments";
                command.Connection = con;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 10; // in seconds

                List<AdminApartments> apartmentsList = new List<AdminApartments>();
                using (SqlDataReader dr = command.ExecuteReader(CommandBehavior.Default))
                {
                    while (dr.Read())
                    {
                        apartmentsList.Add(new AdminApartments(int.Parse(dr["id"].ToString()),
                                                 dr["picture_url"].ToString(),
                                                 dr["name"].ToString(),
                                                 int.Parse(dr["totalDaysRented"].ToString()),
                                                 int.Parse(dr["totalCancellations"].ToString())
                                                 ));
                    }
                }
                con.Close();
                return apartmentsList;
            }
            catch (Exception)
            {
                return null;
            } 
            finally
            {
                if(con != null)
                {
                    con.Close();
                }
            }
        }

        // Admin panel - fetch hosts details
        public List<AdminHosts> AdminReadHosts()
        {
            SqlConnection con = null;
            try
            {
                con = Connect();

                SqlCommand command = new SqlCommand();

                command.CommandText = "spAdminReadHosts";
                command.Connection = con;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 10; // in seconds

                List<AdminHosts> hostsList = new List<AdminHosts>();
                using (SqlDataReader dr = command.ExecuteReader(CommandBehavior.Default))
                {
                    while (dr.Read())
                    {
                        hostsList.Add(new AdminHosts(int.Parse(dr["host_id"].ToString()),
                                                 dr["host_name"].ToString(),
                                                 dr["host_since"].ToString(),
                                                 dr["host_picture_url"].ToString(),
                                                 int.Parse(dr["apartmentsCount"].ToString()),
                                                 int.Parse(dr["totalProfit"].ToString()),
                                                 int.Parse(dr["totalCancellations"].ToString())
                                              ));
                    }
                }
                con.Close();
                return hostsList;
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }

        // Admin panel - fetch users details
        public List<AdminUsers> AdminReadUsers()
        {
            SqlConnection con = null;
            try
            {
                con = Connect();

                SqlCommand command = new SqlCommand();

                command.CommandText = "spAdminReadUsers";
                command.Connection = con;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 10; // in seconds

                List<AdminUsers> usersList = new List<AdminUsers>();
                using (SqlDataReader dr = command.ExecuteReader(CommandBehavior.Default))
                {
                    while (dr.Read())
                    {
                        usersList.Add(new AdminUsers(int.Parse(dr["id"].ToString()),
                                                 dr["email"].ToString(),
                                                 dr["username"].ToString(),
                                                 dr["registerDate"].ToString(),
                                                 int.Parse(dr["orders"].ToString()),
                                                 int.Parse(dr["totalProfit"].ToString()),
                                                 int.Parse(dr["orderCancellations"].ToString())
                                              ));
                    }
                }
                con.Close();
                return usersList;
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }

        // Cancel user's order, by order id and user id
        // Cancelling is implemented by adding date to cancelDate parameter
        public List<OrderWithDetails> CancelOrder(int id, int userId)
        {
            SqlConnection con = null;
            try
            {
                con = Connect();

                SqlCommand command = new SqlCommand();
                command.Parameters.AddWithValue("userId", userId);
                command.Parameters.AddWithValue("id", id);

                command.CommandText = "spCancelOrder";
                command.Connection = con;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 10; // in seconds

                command.ExecuteNonQuery();
                con.Close();

                return ReadUsersOrders(userId);
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }

        // Fetching all orders for user
        public List<OrderWithDetails> ReadUsersOrders(int userId)
        {
            SqlConnection con = null;
            try
            {
                con = Connect();

                SqlCommand command = new SqlCommand();
                command.Parameters.AddWithValue("userId", userId);

                command.CommandText = "spReadUsersOrders";
                command.Connection = con;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 10; // in seconds

                List<OrderWithDetails> ordersList = new List<OrderWithDetails>();
                using (SqlDataReader dr = command.ExecuteReader(CommandBehavior.Default))
                {
                    while (dr.Read())
                    {
                        ordersList.Add(new OrderWithDetails(int.Parse(dr["id"].ToString()),
                                                 userId,
                                                 int.Parse(dr["apartmentId"].ToString()),
                                                 dr["fromDate"].ToString().Split(' ')[0],
                                                 dr["toDate"].ToString().Split(' ')[0],
                                                 dr["orderDate"].ToString().Split(' ')[0],
                                                 float.Parse(dr["totalPrice"].ToString()),
                                                 dr["cancelDate"].ToString(),
                                                 dr["picture_url"].ToString(),
                                                 dr["name"].ToString()
                                              ));
                    }
                }
                con.Close();
                return ordersList;
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }

        // Placing new apartment order
        public int NewOrder(Order order)
        {
            SqlConnection con = null;
            try
            {
                con = Connect();
                SqlCommand command = new SqlCommand();

                if(!isApartmentAvailable(con, order))
                {
                    con.Close();
                    return -1;
                }

                command.Parameters.AddWithValue("userId", order.UserId);
                command.Parameters.AddWithValue("apartmentId", order.ApartmentId);
                command.Parameters.AddWithValue("fromDate", order.FromDate);
                command.Parameters.AddWithValue("toDate", order.ToDate);
                command.Parameters.AddWithValue("totalPrice", order.TotalPrice);

                command.CommandText = "spOrder";
                command.Connection = con;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 10; // in seconds

                command.ExecuteNonQuery();
                con.Close();
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }

        // Checking if apartment available between date range
        private bool isApartmentAvailable(SqlConnection con, Order order)
        {
            try
            {
                SqlCommand command = new SqlCommand();
                command.Parameters.AddWithValue("from", order.FromDate);
                command.Parameters.AddWithValue("to", order.ToDate);
                command.Parameters.AddWithValue("apartmentId", order.ApartmentId);

                command.CommandText = "spCheckAvailability";
                command.Connection = con;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 10; // in seconds

                using (SqlDataReader dr = command.ExecuteReader(CommandBehavior.Default))
                {
                    if (dr.Read())
                    {
                        return (int.Parse(dr["rows"].ToString()) > 0 ? false : true);
                    }

                    return false;
                }
            } catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }

        // Verifing login with email and password
        public User Login(User user)
        {
            SqlConnection con = null;
            try
            {
                User loggedUser = null;
                con = Connect();
                SqlCommand command = new SqlCommand();

                command.Parameters.AddWithValue("email", user.Email);
                command.Parameters.AddWithValue("password", user.Password);

                command.CommandText = "spLogin";
                command.Connection = con;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 10; // in seconds

                SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

                if (dr.Read())
                {
                    loggedUser = new User(int.Parse(dr["id"].ToString()),
                        user.Email,
                        dr["username"].ToString(),
                        user.Password,
                        "");
                }
                con.Close();
                return loggedUser;
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }

        // Registration of new user
        public int Register(User user)
        {
            SqlConnection con = null;
            try
            {
                con = Connect();
                SqlCommand command = new SqlCommand();

                command.Parameters.AddWithValue("email", user.Email);
                command.Parameters.AddWithValue("username", user.Username);
                command.Parameters.AddWithValue("password", user.Password);

                command.CommandText = "spRegister";
                command.Connection = con;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 10; // in seconds

                command.ExecuteNonQuery();
                con.Close();
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }

        // Fetching reviews for apartment by apartment id and page number
        public List<Review> ReadReviews(int id, int page)
        {
            SqlConnection con = null;
            try
            {
                con = Connect();

                SqlCommand command = new SqlCommand();
                command.Parameters.AddWithValue("id", id);
                command.Parameters.AddWithValue("page", page);

                command.CommandText = "spReadReviews";
                command.Connection = con;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 10; // in seconds

                List<Review> reviewsList = new List<Review>();
                using (SqlDataReader dr = command.ExecuteReader(CommandBehavior.Default))
                {
                    while (dr.Read())
                    {
                        reviewsList.Add(new Review(dr["date"].ToString(),
                                                   dr["reviewer_name"].ToString(),
                                                   dr["comments"].ToString()
                                              ));
                    }
                }

                con.Close();

                return reviewsList;
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }

        // Fetch full apartment details by apartment id
        public Apartment ReadApartment(int id)
        {
            SqlConnection con = null;
            try
            {
                con = Connect();

                SqlCommand command = new SqlCommand();
                command.Parameters.AddWithValue("id", id);

                command.CommandText = "spViewApartment";
                command.Connection = con;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 10; // in seconds

                Apartment apartment = null;
                using (SqlDataReader dr = command.ExecuteReader(CommandBehavior.Default))
                {

                    if (dr.Read())
                    {
                        apartment = new Apartment(int.Parse(dr["id"].ToString()),
                                                            dr["name"].ToString(),
                                                            dr["description"].ToString(),
                                                            dr["neighborhood_overview"].ToString(),
                                                            dr["picture_url"].ToString(),
                                                            int.Parse(dr["host_id"].ToString()),
                                                            dr["host_name"].ToString(),
                                                            dr["host_since"].ToString(),
                                                            dr["host_picture_url"].ToString(),
                                                            float.Parse(dr["latitude"].ToString()),
                                                            float.Parse(dr["longitude"].ToString()),
                                                            dr["property_type"].ToString(),
                                                            dr["room_type"].ToString(),
                                                            short.Parse(dr["accommodates"].ToString()),
                                                            dr["bathrooms_text"].ToString(),
                                                            short.Parse(dr["bedrooms"].ToString()),
                                                            short.Parse(dr["beds"].ToString()),
                                                            StringArrayParse(dr["amenities"].ToString()),
                                                            float.Parse(dr["price"].ToString().Substring(1)),
                                                            short.Parse(dr["minimum_nights"].ToString()),
                                                            short.Parse(dr["maximum_nights"].ToString()),
                                                            short.Parse(dr["number_of_reviews"].ToString()),
                                                            float.Parse(dr["review_scores_rating"].ToString()),
                                                            float.Parse(dr["review_scores_accuracy"].ToString()),
                                                            float.Parse(dr["review_scores_cleanliness"].ToString()),
                                                            float.Parse(dr["review_scores_checkin"].ToString()),
                                                            float.Parse(dr["review_scores_communication"].ToString()),
                                                            float.Parse(dr["review_scores_location"].ToString()),
                                                            float.Parse(dr["review_scores_value"].ToString())
                                                  );
                    }
                }
                con.Close();

                return apartment;
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }


       // parsing string format like: ["a","b"] to array
       // used for `Amenities` field
        public string[] StringArrayParse(string arr)
        {
            arr = arr.Replace("[", string.Empty);
            arr = arr.Replace("]", string.Empty);
            string[] newarr = arr.Split(',');

            // removing extra "" in every string
            for(int i = 0; i < newarr.Length; i++)
            {
                newarr[i] = newarr[i].Replace("\"", string.Empty);
            }
            return newarr;
        }

        // Fetching filtered apartments details for main page
        // returns dictionary with number of rows, and a list of 15 apartments
        public Dictionary<int, List<Apartment>> ReadApartments(Filter filter)
        {
            SqlConnection con = null;
            try
            {
            con = Connect();
            SqlCommand command = new SqlCommand();
            command.Parameters.AddWithValue("page", filter.Page);
            command.Parameters.AddWithValue("priceFrom", filter.PriceFrom);
            command.Parameters.AddWithValue("priceTo", filter.PriceTo);
            command.Parameters.AddWithValue("rating", filter.Rating);
            command.Parameters.AddWithValue("numOfRooms", filter.NumOfRooms);
            command.Parameters.AddWithValue("keywords", filter.Keywords);
            command.Parameters.AddWithValue("dateFrom", filter.FromDate);
            command.Parameters.AddWithValue("dateTo", filter.ToDate);

            command.CommandText = "spReadApartments";
            command.Connection = con;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = 10; // in seconds

            List<Apartment> apartmentsList = new List<Apartment>();
            using (SqlDataReader dr = command.ExecuteReader(CommandBehavior.Default))
            {
                // countAll - counting all filtered rows
                // countResult - counting all results that will be returned
                int countAll = 0, countResults = 0;

                while (dr.Read() && countResults < 15)
                {
                        // Calculating distance of the apartment from center (distance 0 means no distance requirement)
                    if((calculateDistance(new double[] { double.Parse(dr["latitude"].ToString()), double.Parse(dr["longitude"].ToString()) }) <= filter.DistFromCenter
                        || filter.DistFromCenter == 0))
                    {
                        // Paging - page 1: first 15 rows, page 2: skip first 15 rows, fetch next 15, page 3: ...
                        if (countAll < 15 * filter.Page && countAll >= 15 * (filter.Page - 1))
                        {
                            apartmentsList.Add(new Apartment(int.Parse(dr["id"].ToString()),
                                                                dr["name"].ToString(),
                                                                dr["picture_url"].ToString(),
                                                                dr["host_name"].ToString(),
                                                                float.Parse(dr["latitude"].ToString()),
                                                                float.Parse(dr["longitude"].ToString()),
                                                                dr["property_type"].ToString(),
                                                                short.Parse(dr["accommodates"].ToString()),
                                                                short.Parse(dr["bedrooms"].ToString()),
                                                                float.Parse(dr["price"].ToString().Substring(1)),
                                                                short.Parse(dr["number_of_reviews"].ToString()),
                                                                float.Parse(dr["review_scores_rating"].ToString())
                                              ));
                            countResults++;
                        }
                        countAll++;
                    }

                }
            }



            // returning number of rows, and list of apartments
            Dictionary<int, List<Apartment>> result = new Dictionary<int, List<Apartment>>();
            result.Add(CountApartmentRows(con,filter), apartmentsList);
            con.Close();

            return result;
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }

        // Counting how many reviews an apartment has
        // Used for creating pagination
        public int CountReviewRows(SqlConnection con, int id)
        {
            try
            {
                SqlCommand command = new SqlCommand();
                command.Parameters.AddWithValue("id", id);
                command.CommandText = "spCountReviews";
                command.Connection = con;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 10; // in seconds

                SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);
                if (dr.Read())
                {
                    return int.Parse(dr["rows"].ToString());
                }

                return 0;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        // Counting the number of apartments after filtering
        // Used for creating pagination
        public int CountApartmentRows(SqlConnection con, Filter filter)
        {
            try
            {
                SqlCommand command = new SqlCommand();
                command.Parameters.AddWithValue("priceFrom", filter.PriceFrom);
                command.Parameters.AddWithValue("priceTo", filter.PriceTo);
                command.Parameters.AddWithValue("rating", filter.Rating);
                command.Parameters.AddWithValue("numOfRooms", filter.NumOfRooms);
                command.Parameters.AddWithValue("keywords", filter.Keywords);
                command.Parameters.AddWithValue("dateFrom", filter.FromDate);
                command.Parameters.AddWithValue("dateTo", filter.ToDate);

                command.CommandText = "spCountApartments";
                command.Connection = con;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 10; // in seconds

                SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);
                int count = 0;
                while (dr.Read())
                {
                    if (calculateDistance(new double[] { double.Parse(dr["latitude"].ToString()), double.Parse(dr["longitude"].ToString()) }) <= filter.DistFromCenter
                        || filter.DistFromCenter == 0)
                    {
                        count++;
                    }
                }

                return count;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        // Calling the method for importing data from excel, and returning "ok" if succeeded
        public string ReadExcel()
        {
            SqlConnection conn = null;
            try
            {
                conn = Connect();
                if (ImportData(conn, "apartments") && ImportData(conn, "reviews") && UpdateReviews(conn))
                {
                    return "ok";
                }
                return "error";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }

        public bool UpdateReviews(SqlConnection con)
        {
            try
            {
                con = Connect();
                SqlCommand command = new SqlCommand();

                command.CommandText = "spUpdateReviews";
                command.Connection = con;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 10; // in seconds

                command.ExecuteNonQuery();
                con.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }
        // Importing data from excel files, building sql table and inserting data
        // name: "apartments" for apartments data, "reviews" for reviews data
        private bool ImportData(SqlConnection conn, string name)
        {
            string[] columnsApartments = new string[] { "@id","@name","@description",
                                                              "@neighborhood_overview","@picture_url","@host_id","@host_name",
                                                              "@host_since","@host_picture_url","@latitude",
                                                              "@longitude","@property_type","@room_type","@accommodates","@bathrooms_text",
                                                              "@bedrooms","@beds","@amenities","@price","@minimum_nights","@maximum_nights",
                                                              "@number_of_reviews","@review_scores_rating","@review_scores_accuracy","@review_scores_cleanliness",
                                                              "@review_scores_checkin", "@review_scores_communication","@review_scores_location","@review_scores_value" };
            string[] columnsReviews = new string[] { "@listing_id",
                                                             "@id",
                                                             "@date",
                                                             "@reviewer_id",
                                                             "@reviewer_name",
                                                             "@comments" };
            string filename = (name.Equals("apartments") ? "listing100.xlsx" : "reviewFrom100.xlsx");
            string[] columns = (name.Equals("apartments") ? columnsApartments : columnsReviews);
            string commandText = (name.Equals("apartments") ? "spInsertApartments" : "spInsertReviews");

            try
            {
                using (var stream = File.Open(AppDomain.CurrentDomain.BaseDirectory + "Content/" + filename, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        do
                        {
                            reader.Read();
                            while (reader.Read())
                            {
                                SqlCommand command = new SqlCommand();


                                for (int i = 0; i < columns.Length; i++)
                                {
                                    command.Parameters.AddWithValue(columns[i], (reader[i] == null) ? "" : reader[i]);
                                }

                                command.CommandText = commandText;
                                command.Connection = conn;
                                command.CommandType = CommandType.StoredProcedure;
                                command.CommandTimeout = 10; // in seconds

                                command.ExecuteNonQuery();
                            }
                        } while (reader.NextResult());
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // Mathematical function that calculates distance of marker from center
        private double calculateDistance(double[] location)
        {
            double[] center = new double[] { 52.3799259, 4.893605 }; // lat & lan of center of Amsterdam
            double R = 6371e3; // metres
            double φ1 = location[0] * Math.PI / 180; // φ, λ in radians
            double φ2 = center[0] * Math.PI / 180;
            double Δφ = (center[0] - location[0]) * Math.PI / 180;
            double Δλ = (center[1] - location[1]) * Math.PI / 180;

            double a = Math.Sin(Δφ / 2) * Math.Sin(Δφ / 2) +
                      Math.Cos(φ1) * Math.Cos(φ2) *
                      Math.Sin(Δλ / 2) * Math.Sin(Δλ / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return (R * c)/1000; // in metres
        }


         private SqlConnection Connect() {


            // read the connection string from the web.config file
            string connectionString = WebConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;

            // create the connection to the db
            SqlConnection con = new SqlConnection(connectionString);

            // open the database connection
            con.Open();

            return con;

        }
    }
}
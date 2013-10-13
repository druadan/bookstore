using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace Bookstore_Service.DBClasses
{
    [DataContract]
    public class Review
    {

        public Review(int id, string customer_login, int book_id, string title, string content, double score){
            this.id = id;
            this.customer_login = customer_login;
            this.book_id = book_id;
            this.title = title;
            this.content = content;
            this.score = score;
        }

        [DataMember]
        public int id { get; set; }
        [DataMember]
        public String customer_login { get; set; }
        [DataMember]
        public int book_id { get; set; }
        [DataMember]
        public String title { get; set; }
        [DataMember]
        public String content { get; set; }
        [DataMember]
        public double score { get; set; }

        static public Review[] getReviews(int book_id)
        {
              try
            {
                SqlConnection con = new SqlConnection(Bookstore.sqlConnectionString);
                con.Open();
                
                SqlCommand cmd = new SqlCommand("SELECT r.* FROM bookstore.dbo.Review R JOIN bookstore.dbo.Book B ON R.book_id = B.id WHERE book_id = @bookID ; ", con);
                cmd.Parameters.AddWithValue("@bookID", book_id);

                SqlDataReader rdr = cmd.ExecuteReader();
                
                List<Review> reviewList = new List<Review>();

                while (rdr.Read())
                {
                    Review r = new Review(rdr.GetInt32(0), rdr.GetString(1), rdr.GetInt32(2), rdr.GetString(3),  rdr.GetString(4), rdr.GetDouble(5));
                    reviewList.Add(r);         
                }
                return reviewList.ToArray();
            }
            catch (Exception e)
            {
                InternalError fault = new InternalError();
                fault.Result = 1;
                fault.ErrorMessage = "Błąd podczas wyszukiwania komentarzy";
                throw new FaultException<InternalError>(fault, new FaultReason(fault.ErrorMessage));
            }
        }
       
    }
}

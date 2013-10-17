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

        public Review(Review r)
        {
            this.id = r.id;
            this.customer_login = r.customer_login;
            this.book_id = r.book_id;
            this.title = r.title;
            this.content = r.content;
            this.score = r.score;
        }

        public Review(){
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
            catch (Exception)
            {
                InternalError fault = new InternalError();
                fault.Result = 1;
                fault.ErrorMessage = "Błąd podczas wyszukiwania komentarzy";
                throw new FaultException<InternalError>(fault, new FaultReason(fault.ErrorMessage));
            }
        }

      
       
    }


    public class ReviewS : Review
    {
        public ReviewS(Review r):base(r)
        {
        }
/* 
    id int NOT NULL PRIMARY KEY IDENTITY(1,1), 
    customer_login varchar(30) NOT NULL, 
    book_id int NOT NULL,
    title varchar(50) NOT NULL,
    content varchar(8000) NOT NULL,
    score float NOT NULL,
 */

        public void save()
        {

            String s;
            if (this.id != 0)
            {
                s = "UPDATE bookstore.dbo.Review SET customer_login=@customerLogin, book_id=@bookID, title=@title, content=@content, score=@score WHERE id = @reviewID ; ";

            }
            else
            {
                s = "INSERT INTO bookstore.dbo.Review (customer_login, book_id, title, content, score) VALUES (@customerLogin, @bookID, @title, @content, @score );";
            }

            try
            {
                SqlConnection con = new SqlConnection(Bookstore.sqlConnectionString + " Asynchronous Processing=true;");
                con.Open();

                SqlCommand cmd = new SqlCommand(s, con);
                cmd.Parameters.AddWithValue("@customerLogin", customer_login);
                cmd.Parameters.AddWithValue("@bookID", book_id);
                cmd.Parameters.AddWithValue("@title", title);
                cmd.Parameters.AddWithValue("@content", content);
                cmd.Parameters.AddWithValue("@score", score);

                if (this.id != 0)
                {
                    cmd.Parameters.AddWithValue("@reviewID", id);
                }

                cmd.BeginExecuteNonQuery();

            }
            catch (Exception)
            {
                InternalError fault = new InternalError();
                fault.Result = 1;
                fault.ErrorMessage = "Błąd podczas dodawania komentarza";
                throw new FaultException<InternalError>(fault, new FaultReason(fault.ErrorMessage));
            }

        }
    }
}

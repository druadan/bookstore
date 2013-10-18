using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace Bookstore_Service.DBClasses
{
    [DataContract]
    public class Book
    {
        public Book(int id, string title, string author, string category, double price)
        {
            this.id = id;
            this.title = title;
            this.author = author;
            this.category = category;
            this.price = price;
        }

        [DataMember]
        public int id { get; set; }
        [DataMember]
        public String title { get; set; }
        [DataMember]
        public String author { get; set; }
        [DataMember]
        public String category { get; set; }
        [DataMember]
        public double price { get; set; }
        

        static public Book[] getBooks(string title, string author, string category, string tag, double minScore, double maxScore, double minAge, double maxAge, string education, int allOrAny)
        {
            try
            {
                SqlConnection con = new SqlConnection(Bookstore.sqlConnectionString);

                con.Open();

                string s = allOrAny == 0 ? " AND " : " OR ";
                bool minScoreB = !Double.IsNaN(minScore);
                bool maxScoreB = !Double.IsNaN(maxScore);
                bool minAgeB = !Double.IsNaN(minAge);
                bool maxAgeB = !Double.IsNaN(maxAge);
                bool scoresBothB = minScoreB && maxScoreB;
                bool scoresAnyB = minScoreB || maxScoreB;
                bool agesBothB = minAgeB && maxAgeB;
                bool agesAnyB = minAgeB || maxAgeB;


                bool tagB = !tag.Equals("") ;
                bool titleB = !title.Equals("");
                bool categoryB = !category.Equals("");
                bool authorB = !author.Equals("");
                bool educationB = !education.Equals("");


                string scoreSubquery = String.Format(
                    " b.id in (SELECT DISTINCT b.id " +
                    "FROM bookstore.dbo.Book b " +
                    "JOIN Bookstore.dbo.Review r ON r.book_id = b.id " +
                    " {0} {1} {2} {3} {4} {5} " +
                    "GROUP BY b.id " +
                    "{6} {7} {8} {9} ) ",
                    agesAnyB || educationB ? " JOIN Bookstore.dbo.Client c on r.customer_login = c.[login] WHERE " : "",
                    minAgeB ? " c.age >= @minAge " : "", minAgeB && ( maxAgeB || educationB) ? " AND " : "",
                    maxAgeB ? " c.age <= @maxAge " : "", maxAgeB && educationB ? " AND " : "", 
                    educationB ? " c.education = @education " : "",
                    scoresAnyB ? " HAVING " : "",
                    minScoreB ? " AVG(r.score) >= @minScore" : "", scoresBothB ? s : "",
                    maxScoreB ? " AVG(r.score) <= @maxScore" : "" 
                    );


            
                string query = String.Format(
                    "SELECT DISTINCT b.* FROM bookstore.dbo.Book b {0} {1} {2} {3} {4} {5} {6} {7} {8} {9} {10} ;",
                    tagB ? " JOIN bookstore.dbo.Tag_association ta ON ta.book_id = b.id " : "", 
                    titleB || authorB || categoryB || tagB || educationB || agesAnyB || scoresAnyB ? " WHERE " : "",
                    titleB ? "title LIKE @titleCond" : "", titleB && (authorB || categoryB || tagB || scoresAnyB || agesAnyB || educationB) ? s : "",
                    authorB ? "author LIKE @authorCond" : "", authorB && (categoryB || tagB || scoresAnyB || agesAnyB || educationB) ? s : "",
                    categoryB ? "category LIKE @categoryCond" : "", categoryB && (tagB || scoresAnyB || agesAnyB || educationB) ? s : "",
                    tagB ? "tag_id LIKE @tagCond " : "", tagB && (scoresAnyB || agesAnyB || educationB) ? s : "",
                    scoresAnyB || agesAnyB || educationB? scoreSubquery : ""
                    );



                SqlCommand cmd = new SqlCommand(query,con);

                if (titleB)
                {
                    cmd.Parameters.AddWithValue("@titleCond", "%" + title + "%");
                }
                if (authorB)
                {
                    cmd.Parameters.AddWithValue("@authorCond", "%" + author + "%" );
                }
                if (categoryB)
                {
                    cmd.Parameters.AddWithValue("@categoryCond", "%" + category + "%" );
                }
                if (tagB)
                {
                    cmd.Parameters.AddWithValue("@tagCond", "%" + tag + "%");
                }
                if (minScoreB)
                {
                    cmd.Parameters.AddWithValue("@minScore", minScore );
                }
                if (maxScoreB)
                {
                    cmd.Parameters.AddWithValue("@maxScore", maxScore);
                }
                if (minAgeB)
                {
                    cmd.Parameters.AddWithValue("@minAge", minAge);
                }
                if (maxAgeB)
                {
                    cmd.Parameters.AddWithValue("@maxAge", maxAge);
                }
                if (educationB)
                {
                    cmd.Parameters.AddWithValue("@education", education);
                }
                
                SqlDataReader rdr = cmd.ExecuteReader();
                
                List<Book> booksList = new List<Book>();

                while (rdr.Read())
                {
                    Book b = new Book(rdr.GetInt32(0), rdr.GetString(1), rdr.GetString(2), rdr.GetString(3), rdr.GetDouble(4));
                    booksList.Add(b);         
                }
                return booksList.ToArray();
            }
            catch (Exception)
            {
                InternalError fault = new InternalError();
                fault.Result = 1;
                fault.ErrorMessage = "Błąd podczas pobierania listy książek";
                throw new FaultException<InternalError>(fault, new FaultReason(fault.ErrorMessage));
            }

            


        }
       
    }
}

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Runtime.Serialization;
using System.Windows.Forms;

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
        

        static public Book[] getBooks(string title, string author, string category, string tag, double minScore, double maxScore, int allOrAny)
        {
            try
            {
                SqlConnection con = new SqlConnection(Bookstore.sqlConnectionString);

                con.Open();

                string s = allOrAny == 0 ? " AND " : " OR ";
                bool minScoreB = !Double.IsNaN(minScore);
                bool maxScoreB = !Double.IsNaN(maxScore);
                bool scoresBothB = minScoreB && maxScoreB;
                bool scoreAnyB = minScoreB || maxScoreB;

                string scoreSubquery = String.Format(
                    " b.id in (SELECT DISTINCT b.id " +
                    "FROM bookstore.dbo.Book b " +
                    "JOIN Bookstore.dbo.Review r ON r.book_id = b.id " +
                    "GROUP BY b.id " +
                    "HAVING {0} {1} {2} ) ",
                    minScoreB ? " AVG(r.score) >= @minScore" : "", scoresBothB ? s : "",
                    maxScoreB ? " AVG(r.score) <= @maxScore" : ""
                    );


            
                string query = String.Format(
                    "SELECT DISTINCT b.* FROM bookstore.dbo.Book b {0} {1} {2} {3} {4} {5} {6} {7} {8} {9} {10} ;",
                    !tag.Equals("") ? " JOIN bookstore.dbo.Tag_association ta ON ta.book_id = b.id " : "", 
                    title.Equals("") && author.Equals("") && category.Equals("") && tag.Equals("") && !scoreAnyB ? "" : " WHERE ",
                    !title.Equals("")    ? "title LIKE @titleCond" : "",       !title.Equals("") &&  ( !author.Equals("") || !category.Equals("") || !tag.Equals("") || scoreAnyB ) ? s : "",
                    !author.Equals("") ? "author LIKE @authorCond" : "",       !author.Equals("") && (!category.Equals("") || !tag.Equals("") || scoreAnyB) ? s : "",
                    !category.Equals("") ? "category LIKE @categoryCond" : "", !category.Equals("") && (!tag.Equals("") || scoreAnyB) ? s : "",
                    !tag.Equals("") ? "tag_id LIKE @tagCond " : "", !tag.Equals("") && scoreAnyB ? s : "",
                    scoreAnyB ? scoreSubquery : ""
                    );



                SqlCommand cmd = new SqlCommand(query,con);

                if (!title.Equals(""))
                {
                    cmd.Parameters.AddWithValue("@titleCond", "%" + title + "%");
                }
                if (!author.Equals(""))
                {
                    cmd.Parameters.AddWithValue("@authorCond", "%" + author + "%" );
                }
                if (!category.Equals(""))
                {
                    cmd.Parameters.AddWithValue("@categoryCond", "%" + category + "%" );
                }
                if (!tag.Equals(""))
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
                
                SqlDataReader rdr = cmd.ExecuteReader();
                
                List<Book> booksList = new List<Book>();

                while (rdr.Read())
                {
                    Book b = new Book(rdr.GetInt32(0), rdr.GetString(1), rdr.GetString(2), rdr.GetString(3), rdr.GetDouble(4));
                    booksList.Add(b);         
                }
                return booksList.ToArray();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }

            return null;


        }
       
    }
}

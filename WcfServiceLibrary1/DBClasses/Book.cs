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
        

        static public Book[] getBooks(string title, string author, string category, string tag, int allOrAny)
        {
            try
            {
                SqlConnection con = new SqlConnection(Bookstore.sqlConnectionString);
                con.Open();

                string s = allOrAny == 0 ? " AND " : " OR ";

                string query = String.Format(
                    "SELECT DISTINCT b.* FROM bookstore.dbo.Book b {0} {1} {2} {3} {4} {5} {6} {7} {8} ;",
                    !tag.Equals("") ? " JOIN bookstore.dbo.Tag_association ta on ta.book_id = b.id " : "", 
                    title.Equals("") && author.Equals("") && category.Equals("") && tag.Equals("") ? "" : " WHERE ",
                    !title.Equals("")    ? "title LIKE @titleCond" : "",       !title.Equals("") &&  ( !author.Equals("") || !category.Equals("") || !tag.Equals("") ) ? s : "",
                    !author.Equals("")   ? "author LIKE @authorCond" : "",     !author.Equals("") && ( !category.Equals("") || !tag.Equals("") ) ? s : "", 
                    !category.Equals("") ? "category LIKE @categoryCond" : "", !category.Equals("") && !tag.Equals("") ? s : "", 
                    !tag.Equals("")      ? "tag_id LIKE @tagCond " : ""
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

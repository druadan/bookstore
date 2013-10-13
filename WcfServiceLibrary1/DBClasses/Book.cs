using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bookstore_Service;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
namespace Bookstore_Service.DBClasses
{
    public class Book
    {


        public int id { get; set; }
        public String title { get; set; }
        public String author { get; set; }
        public String category { get; set; }
        public double price { get; set; }
        
        override public string ToString()
        {
            return id + " " + title;
        }

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
                    Book b = new Book();
                    b.id = rdr.GetInt32(0);
                    b.title = rdr.GetString(1);
                    b.author = rdr.GetString(2);
                    b.category = rdr.GetString(3);
                    b.price = rdr.GetDouble(4);
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

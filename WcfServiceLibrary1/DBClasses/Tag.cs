using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace Bookstore_Service.DBClasses
{

    [DataContract]
    public class TagsCount
    {
        [DataMember]
        public Tag tag { get; set; }

        [DataMember]
        public int count { get; set; }


        public override String ToString()
        {
            return tag.ToString() + " " + count ;
        }



    }

    [DataContract]
    public class Tag
    {
        [DataMember]
        public String tag_id { get; set; }

        public Tag(){
        }

        public Tag(string id){
            tag_id = id;
        }

        public Tag(Tag t)
        {
            tag_id = t.tag_id;
        }

        public override String ToString(){
            return tag_id;
        }

  
       
    }

    public class TagS : Tag
    {

        public TagS()
        {
        }

        public TagS(Tag t)
            : base(t)
        {
        }

        public static List<KeyValuePair<Tag, int>> getTagsForBook(int book_id)
        {
            try
            {
                SqlConnection con = new SqlConnection(Bookstore.sqlConnectionString);
                con.Open();

                SqlCommand cmd = new SqlCommand(
                    "SELECT TOP 5 T.tag_id, COUNT(*)" +
                    "FROM Bookstore.dbo.Tag T JOIN Bookstore.dbo.Tag_association TA on t.tag_id = TA.tag_id " +
                    "WHERE TA.book_id = @bookID " +
                    "GROUP BY T.tag_id " +
                    "ORDER BY 2 DESC ;"
                    , con);

                cmd.Parameters.AddWithValue("@bookID", book_id);

                SqlDataReader rdr = cmd.ExecuteReader();

                List<KeyValuePair<Tag, int>> tagsList = new List<KeyValuePair<Tag, int>>();

                while (rdr.Read())
                {
                   
                    Tag t = new Tag(rdr.GetString(0));
                    int count = rdr.GetInt32(1);
                   
                    KeyValuePair<Tag, int> kv = new KeyValuePair<Tag, int>(t, count);
                    tagsList.Add(kv);
                }
                return tagsList;

            }
            catch (Exception)
            {
                InternalError fault = new InternalError();
                fault.Result = 1;
                fault.ErrorMessage = "Błąd podczas pobierania tagów";
                throw new FaultException<InternalError>(fault, new FaultReason(fault.ErrorMessage));
            }
        }

        public int addToBook(int book_id)
        {
            try
            {
                SqlConnection con = new SqlConnection(Bookstore.sqlConnectionString  + "MultipleActiveResultSets=True;");
                con.Open();

                String insert1 = "INSERT INTO bookstore.dbo.Tag (tag_id) VALUES (@tagID );";
                String insert2 = "INSERT INTO bookstore.dbo.Tag_association (tag_id, book_id) VALUES (@tagID, @bookID );";

                SqlCommand cmd1 = new SqlCommand(insert1, con);
                SqlCommand cmd2 = new SqlCommand(insert2, con);

                cmd1.Parameters.AddWithValue("@tagID", tag_id);
                cmd2.Parameters.AddWithValue("@tagID", tag_id);
                cmd2.Parameters.AddWithValue("@bookID", book_id);

                // such tag may already exists, so exception here is expected
                try
                {
                    cmd1.ExecuteNonQuery();
                }
                catch (System.Data.SqlClient.SqlException)
                {
                }

                cmd2.ExecuteNonQuery();
                return 0;

            }
            catch (Exception)
            {
                InternalError fault = new InternalError();
                fault.Result = 1;
                fault.ErrorMessage = "Błąd podczas dodawania tagu";
                throw new FaultException<InternalError>(fault, new FaultReason(fault.ErrorMessage));
            }
        }
    }

}

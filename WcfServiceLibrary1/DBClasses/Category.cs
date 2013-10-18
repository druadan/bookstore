using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace Bookstore_Service.DBClasses
{
    [DataContract]
    public class Category
    {

        [DataMember]
        public string name { get; set; }

        public Category(string n)
        {
            this.name = n;
        }

        public Category()
        {
        }

        public override string ToString()
        {
            return name;
        }
    
    }

    public class CategoryS : Category
    {
        public static Category[] getCategories()
        {
            try
            {
                SqlConnection con = new SqlConnection(Bookstore.sqlConnectionString);
                con.Open();

                SqlCommand cmd = new SqlCommand(
                   "SELECT DISTINCT * " +
                    "FROM bookstore.dbo.Category ;"
                    , con);


                SqlDataReader rdr = cmd.ExecuteReader();

                List<Category> catList = new List<Category>();

                while (rdr.Read())
                {
                    Category c = new Category();
                    c.name = rdr.GetString(0);
                    catList.Add(c);
                }
                return catList.ToArray();

            }
            catch (Exception)
            {
                InternalError fault = new InternalError();
                fault.Result = 1;
                fault.ErrorMessage = "Błąd podczas pobierania kategorii";
                throw new FaultException<InternalError>(fault, new FaultReason(fault.ErrorMessage));
            }
        }
    }

  

}

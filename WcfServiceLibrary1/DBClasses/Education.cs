using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace Bookstore_Service.DBClasses
{
    [DataContract]
    public class Education
    {
        [DataMember]
        public String name { get; set; }

        public Education(){
        }

        public Education(string id){
            name= id;
        }

        public Education(Education e)
        {
            name = e.name;
        }

        public override String ToString(){
            return name;
        }

        public static Education[] getEducationDegrees()
        {
            try
            {
                SqlConnection con = new SqlConnection(Bookstore.sqlConnectionString);
                con.Open();

                SqlCommand cmd = new SqlCommand(
                   "SELECT DISTINCT * " +
                    "FROM bookstore.dbo.Education ;"
                    , con);


                SqlDataReader rdr = cmd.ExecuteReader();

                List<Education> eduList = new List<Education>();

                while (rdr.Read())
                {
                    Education e = new Education();
                    e.name = rdr.GetString(0);
                    eduList.Add(e);
                }
                return eduList.ToArray();

            }
            catch (Exception)
            {
                InternalError fault = new InternalError();
                fault.Result = 1;
                fault.ErrorMessage = "Błąd podczas pobierania listy stopni edukacji";
                throw new FaultException<InternalError>(fault, new FaultReason(fault.ErrorMessage));
            }
        }
       
    }

  

}

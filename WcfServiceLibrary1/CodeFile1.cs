using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Data.SqlClient;
using System.Data;
namespace Bookstore_Service
{
    [DataContract]
    public class Maths
    {

        [DataMember]
        public int Number1 { get; set; }
        [DataMember]
        public int Number2 { get; set; }
    }

    [ServiceContract]
    public interface IBookstore
    {
        /*      [OperationContract]
               int Addition(Maths obj1);
              [OperationContract]
               int Subtraction(Maths obj2);
              [OperationContract]
               int Multiplication(Maths obj3);
              [OperationContract]
               int Division(Maths obj4);*/
        [OperationContract]
        int Login(string login, string password);
    }

    class Bookstore : IBookstore
    {
        public int Login(string login, string password)
        {
            try
            {

               // SqlConnection customerConnection = new SqlConnection("Data Source=(local);Initial Catalog=AdventureWorks;Integrated Security=SSPI;");
                SqlConnection customerConnection = new SqlConnection("Data Source=DRUADAN-DESKTOP\\SQLEXPRESS; User ID=adm; Password=adm");
               
                customerConnection.Open();
                SqlDataAdapter custAdapter = new SqlDataAdapter(
                "SELECT * FROM dbo.Clients", customerConnection);
                DataSet customerOrders = new DataSet();
            }
            catch (SqlException e)
            {
                System.Console.WriteLine(e.Message);
            }
            return 0;
        }
    }
}
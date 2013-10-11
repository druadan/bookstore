using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Data.SqlClient;
using System.Data;
using System.Security.Cryptography;
using System.IdentityModel.Selectors;
using System.Windows.Forms;

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
        string Login(string login, string password);
        int Logout(string login, string sessionToken);
    }

    public class CustomUserNameValidator : UserNamePasswordValidator
    {
         public override void Validate(string userName, string password)
         {
            if(userName.Equals("pr1")){
                return;
            }
         }
    }

    class Bookstore : IBookstore
    {
        String sqlConnectionString = "Data Source=DRUADAN-DESKTOP\\SQLEXPRESS; User ID=adm; Password=adm";
        Dictionary<String, List<String>> loggedUsers = new Dictionary<string, List<string>>();

        public string Login(string login, string password)
        {
            try
            {
              
                SqlConnection con = new SqlConnection(sqlConnectionString);
                con.Open();

                SqlDataAdapter loginAdapter = new SqlDataAdapter("SELECT * FROM bookstore.dbo.Client", con);
                DataSet logins = new DataSet();
                loginAdapter.Fill(logins,"Client");

                DataTable clientTable = logins.Tables["Client"];
                DataColumn[] pk = new DataColumn[1];
                pk[0] = clientTable.Columns["login"];
                clientTable.PrimaryKey = pk;

                DataRow userRow = clientTable.Rows.Find(login);

                if (userRow != null && userRow["password"].Equals(password))
                {
                    var s = new StringBuilder();
                    SHA1 hasher = SHA1.Create();
                    foreach (byte b in hasher.ComputeHash(Encoding.UTF8.GetBytes(login)))
                        s.Append(b.ToString("x2").ToLower());

                    // adding new token to logged in users
                    if ( !loggedUsers.ContainsKey(login) )
                    {
                        loggedUsers.Add(login, new List<String>());
                    }
                    loggedUsers[login].Add(s.ToString());

                    return s.ToString();
                    
                }
                return "";
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            return "";
        }


        public int Logout(string login, string sessionToken)
        {
            try
            {
                if (loggedUsers[login] == null)
                {
                    return 1;
                }else if( loggedUsers[login].Remove(sessionToken) ){
                    return 0;
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            return 1;
        }
    }
}
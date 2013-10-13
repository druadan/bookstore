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
    class Client
    {


        public String login;
        public String password;

        static public Client getClient(string login){
               SqlConnection con = new SqlConnection(Bookstore.sqlConnectionString);
               con.Open();
                

                SqlDataAdapter loginAdapter = new SqlDataAdapter("SELECT * FROM bookstore.dbo.Client", con);
                DataSet logins = new DataSet();
                loginAdapter.Fill(logins,"Client");

                DataTable clientTable = logins.Tables["Client"];
                DataColumn[] pk = new DataColumn[1];
                pk[0] = clientTable.Columns["login"];
                clientTable.PrimaryKey = pk;

                DataRow userRow = clientTable.Rows.Find(login);

                if (userRow == null)
                {
                    return null;
                }

                Client c = new Client();
                c.login = login;
                c.password = userRow["password"].ToString();

                return c;

        }
       
    }
}

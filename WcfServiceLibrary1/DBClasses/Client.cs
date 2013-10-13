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
    class ClientS
    {

        /*       	
           age int not null,
           education varchar(30) NOT NULL,
           preferredCat varchar(30) not null,
           preferredCat2 varchar(30) not null,
               */


        public String login;
        public String name;
        public String surname;
        public String address;
        public int loyal_client;
        public String password;
        public int age;
        public String education;
        public String preferredCat;
        public String preferredCat2;

        public ClientS(string login){
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
                    throw new Exception("Couldn't find such user"); 
                }

                this.login = login;
            this.name = 
        }
       
       
    }

    class Client
    {

        public String login;
        public String name;
        public String surname;
        public String address;
        public int loyal_client;

        public Client(string login, string name, string surname, string address, int loyal_client)
        {
            this.login = login;
            this.name = name;
            this.surname = surname;
            this.address = address;
            this.loyal_client = loyal_client;
        }

        public Client()
        {
        }

    }

    class OtherClient
    {

        public String login;
        public String name;

        public OtherClient(string login, string name)
        {
            this.login = login;
            this.name = name;
        }
    }
}

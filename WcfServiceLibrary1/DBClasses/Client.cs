using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bookstore_Service;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Runtime.Serialization;
using System.ServiceModel;


namespace Bookstore_Service.DBClasses
{
    class ClientS : Client
    {
        public String password;

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
                this.name = userRow["name"].ToString();
                this.surname = userRow["surname"].ToString();
                this.address = userRow["address"].ToString();
                this.loyal_client = Convert.ToInt32(userRow["loyal_client"].ToString());
                this.password = userRow["password"].ToString();
                this.age = Convert.ToInt32(userRow["age"].ToString());
                this.education = userRow["education"].ToString();
                this.preferredCat = userRow["preferredCat"].ToString();
                this.preferredCat2 = userRow["preferredCat2"].ToString();
                
        }
       
       
    }

    [DataContract]
    public class Client : OtherClient
    {
         [DataMember]
        public String surname;
         [DataMember]
        public String address;
         [DataMember]
        public int loyal_client;


        public Client(string login, string name, string surname, string address, int loyal_client, int age, string education, string preferredCat, string preferredCat2) : base( login,  name, age, education,  preferredCat,  preferredCat2)
        {
            this.login = login;
            this.name = name;
            this.surname = surname;
            this.address = address;
            this.loyal_client = loyal_client;
            this.age = age;
            this.education = education;
            this.preferredCat = preferredCat;
            this.preferredCat2 = preferredCat2;
        }

        public Client()
        {
        }



    }

    [DataContract]
    public class OtherClient
    {
        [DataMember]
        public String login;
        [DataMember]
        public String name;
        [DataMember]
        public int age;
        [DataMember]
        public String education;
        [DataMember]
        public String preferredCat;
        [DataMember]
        public String preferredCat2;

        public OtherClient(string login, string name, int age, string education, string preferredCat, string preferredCat2)
        {
            this.login = login;
            this.name = name;
            this.age = age;
            this.education = education;
            this.preferredCat = preferredCat;
            this.preferredCat2 = preferredCat2;
        }

        public OtherClient() { }
    }
}

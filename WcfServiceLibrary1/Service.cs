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
using Bookstore_Service.DBClasses;

namespace Bookstore_Service
{
    [DataContract]
    public class InternalError
    {
        [DataMember]
        public int Result { get; set; }
        [DataMember]
        public string ErrorMessage { get; set; }

        override public string ToString()
        {
            return "Result: " + this.Result.ToString() + "\n\nError message: " + this.ErrorMessage.ToString();
        }
    }

    [ServiceContract]
    public interface IBookstore
    {
        [OperationContract]
        [FaultContract(typeof(InternalError))]
        string Login(string login, string password);

        [OperationContract]
        int Logout(string login, string sessionToken);
    }


    class Bookstore : IBookstore
    {
        static public String sqlConnectionString = "Data Source=DRUADAN-DESKTOP\\SQLEXPRESS; User ID=adm; Password=adm";
        static Dictionary<String, List<String>> loggedUsers = new Dictionary<string, List<string>>();

        public string Login(string login, string password)
        {
            try
            {

                Client c = Client.getClient(login);
                if (c == null)
                {
                    return "";
                }

                if (c.password.Equals(password))
                {
                    var s = new StringBuilder();
                    SHA1 hasher = SHA1.Create();
                    foreach (byte b in hasher.ComputeHash(Encoding.UTF8.GetBytes(login + DateTime.Now.ToString("M/d/yyyy") + new Random().Next())))
                        s.Append(b.ToString("x2").ToLower());

                    // adding new token to logged in users
                    if (!loggedUsers.ContainsKey(login))
                    {
                        loggedUsers.Add(login, new List<String>());
                    }
                    loggedUsers[login].Add(s.ToString());

                    return s.ToString();

                }
                else
                {
                    return "";
                }
            }
            catch (Exception e)
            {
                InternalError fault = new InternalError();
                fault.Result = 1;
                fault.ErrorMessage = "Logowanie nie powiodło się z powodu wewnętrznego błędu serwera";
                throw new FaultException<InternalError>(fault, new FaultReason(fault.ErrorMessage));
            }

            
        }


        public int Logout(string login, string sessionToken)
        {
            try
            {
                if (!loggedUsers.ContainsKey(login) || loggedUsers[login] == null)
                {
                    return 1;
                }else if( loggedUsers[login].Remove(sessionToken) ){
                    return 0;
                }

            }
            catch (Exception e)
            {
                InternalError fault = new InternalError();
                fault.Result = 1;
                fault.ErrorMessage = "Wylogowywanie nie powiodło się z powodu wewnętrznego błędu serwera";
                throw new FaultException<InternalError>(fault, new FaultReason(fault.ErrorMessage));
            }

            return 1;
        }
    }
}
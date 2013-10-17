﻿using System;
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

        [OperationContract]
        Book[] GetBooks(string title, string author, string category, string tag, double minScore, double maxScore, int allOrAny);

        [OperationContract]
        [FaultContract(typeof(InternalError))]
        Review[] GetReviews(int book_id);

        [OperationContract]
        String GetAverageScore(int book_id);

        [OperationContract]
        OtherClient GetOtherClient(string login);

        [OperationContract]
        int AddReview(Review r);

        [OperationContract]
        Tag[] GetTopTagsForBook(int book_id);

        [OperationContract]
        int AddTag(Tag t, int book_id);

        [OperationContract]
        Category[] GetCategories();
    }


    class Bookstore : IBookstore
    {
        static public String sqlConnectionString = "Data Source=DRUADAN-DESKTOP\\SQLEXPRESS; User ID=adm; Password=adm;";
        static Dictionary<String, List<String>> loggedUsers = new Dictionary<string, List<string>>();

        public string Login(string login, string password)
        {
            try
            {

                ClientS c = new ClientS(login);
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
            catch (Exception)
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
            catch (Exception)
            {
                InternalError fault = new InternalError();
                fault.Result = 1;
                fault.ErrorMessage = "Wylogowywanie nie powiodło się z powodu wewnętrznego błędu serwera";
                throw new FaultException<InternalError>(fault, new FaultReason(fault.ErrorMessage));
            }

            return 1;
        }

        public Book[] GetBooks(string title, string author, string category, string tag, double minScore, double maxScore, int allOrAny)
        {
            return Book.getBooks(title, author, category, tag, minScore, maxScore, allOrAny);
            
        }

        public Review[] GetReviews(int book_id)
        {
            return Review.getReviews(book_id);

        }

        public String GetAverageScore(int book_id)
        {
            Review[] reviews = Review.getReviews(book_id);
            double sum = 0.0;
            foreach (Review r in reviews)
            {
                sum += r.score;
            }

            try
            {
                double res = sum / reviews.Length;
                if (Double.NaN.Equals(res))
                {
                    res = 0.0;
                }
                return String.Format("{0:0.##}", res); 

            }
            catch (Exception)
            {
                return String.Format("{0:0.##}", 0.0); 
            }

        }

        public OtherClient GetOtherClient(string login)
        {
            ClientS cs = new ClientS(login);
            return new OtherClient(cs.login, cs.name,cs.age,cs.education,cs.preferredCat,cs.preferredCat2);
        }

        public int AddReview(Review r)
        {
            ReviewS rs = new ReviewS(r);
            rs.save();
            return 0;
        }

        public Tag[] GetTopTagsForBook(int book_id)
        {
            return Tag.getTagsForBook(book_id);
        }

        public int AddTag(Tag t, int book_id)
        {
            TagS ts = new TagS(t);
            return ts.addToBook(book_id);  
        }

        public Category[] GetCategories()
        {
            return Category.getCategories();
        }
    }
}
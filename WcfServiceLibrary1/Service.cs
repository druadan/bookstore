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
    public class BookstoreError
    {
        [DataMember]
        public int Result { get; set; }
        [DataMember]
        public string ErrorMessage { get; set; }

        override public string ToString()
        {
            return  "Error message: " + this.ErrorMessage.ToString();
        }
    }

    [DataContract]
    public class LoginError : BookstoreError
    {
    }

    [DataContract]
    public class InvalidSessionTokenError : BookstoreError
    {
    }

    [DataContract]
    public class InternalError: BookstoreError
    {
    }



    [ServiceContract]
    public interface IBookstore
    {
        [OperationContract]
        [FaultContract(typeof(InternalError))]
        [FaultContract(typeof(LoginError))]
        string Login(string login, string password);

        [OperationContract]
        [FaultContract(typeof(InternalError))]
        [FaultContract(typeof(InvalidSessionTokenError))]
        int Logout(string login, string sessionToken);

        [OperationContract]
        [FaultContract(typeof(InternalError))]
        [FaultContract(typeof(InvalidSessionTokenError))]
        Book[] GetBooks(string title, string author, string category, string tag, double minScore, double maxScore, double minAge, double maxAge, string education, int allOrAny, string login, string sessionToken);

        [OperationContract]
        [FaultContract(typeof(InternalError))]
        [FaultContract(typeof(InvalidSessionTokenError))]
        Review[] GetReviews(int book_id, string login, string sessionToken);

        [OperationContract]
        [FaultContract(typeof(InternalError))]
        [FaultContract(typeof(InvalidSessionTokenError))]
        String GetAverageScore(int book_id, string login, string sessionToken);

        [OperationContract]
        [FaultContract(typeof(InternalError))]
        [FaultContract(typeof(InvalidSessionTokenError))]
        OtherClient GetOtherClient(string otherUserlogin, string login, string sessionToken);

        [OperationContract]
        [FaultContract(typeof(InternalError))]
        [FaultContract(typeof(InvalidSessionTokenError))]
        void AddReview(Review r, string login, string sessionToken);

        [OperationContract]
        [FaultContract(typeof(InternalError))]
        [FaultContract(typeof(InvalidSessionTokenError))]
        Tag[] GetTopTagsForBook(int book_id, string login, string sessionToken);

        [OperationContract]
        [FaultContract(typeof(InternalError))]
        [FaultContract(typeof(InvalidSessionTokenError))]
        int AddTag(Tag t, int book_id, string login, string sessionToken);

        [OperationContract]
        [FaultContract(typeof(InternalError))]
        [FaultContract(typeof(InvalidSessionTokenError))]
        Category[] GetCategories(string login, string sessionToken);

        [OperationContract]
        [FaultContract(typeof(InternalError))]
        [FaultContract(typeof(InvalidSessionTokenError))]
        Education[] GetEducationDegrees(string login, string sessionToken);
    }


    public class Bookstore : IBookstore
    {
        static public String sqlConnectionString = "Data Source=DRUADAN-DESKTOP\\SQLEXPRESS; User ID=adm; Password=adm;";
        static Dictionary<String, List<String>> loggedUsers = new Dictionary<string, List<string>>();

        public string Login(string login, string password)
        {
            bool loginNotFound = false;
            try
            {
                ClientS c = null;
                try
                {
                   c = new ClientS(login);
                }
                catch (Exception)
                {
                    loginNotFound = true;
                }

                SHA1 sha1 = new SHA1CryptoServiceProvider();
                byte[] hashedPasswd = sha1.ComputeHash(Encoding.UTF8.GetBytes(password));


                if (loginNotFound == false && c.password.SequenceEqual(hashedPasswd))
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

            }
            catch (Exception)
            {
                InternalError ie = new InternalError();
                ie.Result = 1;
                ie.ErrorMessage = "Logowanie nie powiodło się z powodu wewnętrznego błędu serwera";
                throw new FaultException<InternalError>(ie, new FaultReason(ie.ErrorMessage));
            }

            // login failed
            LoginError fault = new LoginError();
            fault.Result = 1;
            fault.ErrorMessage = "Zły login lub hasło";
            throw new FaultException<LoginError>(fault, new FaultReason(fault.ErrorMessage));
            
        }


        public int Logout(string login, string sessionToken)
        {
            validateSessionToken(login, sessionToken);
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

        public Book[] GetBooks(string title, string author, string category, string tag, double minScore, double maxScore, double minAge, double maxAge, string education, int allOrAny, string login, string sessionToken)
        {
            validateSessionToken(login, sessionToken);
            return BookS.getBooks(title, author, category, tag, minScore, maxScore, minAge, maxAge, education, allOrAny);
            
        }

        public Review[] GetReviews(int book_id, string login, string sessionToken)
        {
            validateSessionToken(login, sessionToken);
            return ReviewS.getReviews(book_id);

        }

        public String GetAverageScore(int book_id, string login, string sessionToken)
        {
            validateSessionToken(login, sessionToken);
            try
            {
                Review[] reviews = ReviewS.getReviews(book_id);
                double sum = 0.0;
                foreach (Review r in reviews)
                {
                    sum += r.score;
                }

            
                double res = sum / reviews.Length;
                if (Double.NaN.Equals(res))
                {
                    res = 0.0;
                }
                return String.Format("{0:0.##}", res); 

            }
            catch (Exception)
            {
                InternalError fault = new InternalError();
                fault.Result = 1;
                fault.ErrorMessage = "Wystąpił błąd podczas pobierania średniej ocen";
                throw new FaultException<InternalError>(fault, new FaultReason(fault.ErrorMessage)); 
            }

        }

        public OtherClient GetOtherClient(string otherUserlogin, string login, string sessionToken)
        {
            validateSessionToken(login, sessionToken);
            try
            {
                ClientS cs = new ClientS(otherUserlogin);
                return new OtherClient(cs.login, cs.name,cs.age,cs.education,cs.preferredCat,cs.preferredCat2);
            }
            catch (Exception)
            {
                InternalError fault = new InternalError();
                fault.Result = 1;
                fault.ErrorMessage = "Wystąpił błąd podczas pobierania profilu użytkownika";
                throw new FaultException<InternalError>(fault, new FaultReason(fault.ErrorMessage));
            }
        }

        public void AddReview(Review r, string login, string sessionToken)
        {
            validateSessionToken(login, sessionToken);
            try
            {
                ReviewS rs = new ReviewS(r);
                rs.save();
            }
            catch (Exception)
            {
                InternalError fault = new InternalError();
                fault.Result = 1;
                fault.ErrorMessage = "Wystąpił błąd podczas dodawania recenzji";
                throw new FaultException<InternalError>(fault, new FaultReason(fault.ErrorMessage));
            }
        }

        public Tag[] GetTopTagsForBook(int book_id, string login, string sessionToken)
        {
            validateSessionToken(login, sessionToken);
            try
            {
                return TagS.getTagsForBook(book_id);
            }
            catch (Exception)
            {
                InternalError fault = new InternalError();
                fault.Result = 1;
                fault.ErrorMessage = "Wystąpił błąd podczas pobierania tagów";
                throw new FaultException<InternalError>(fault, new FaultReason(fault.ErrorMessage));
            }
        }

        public int AddTag(Tag t, int book_id, string login, string sessionToken)
        {
            validateSessionToken(login, sessionToken);
            try
            {
                TagS ts = new TagS(t);
                return ts.addToBook(book_id);
            }
            catch (Exception)
            {
                InternalError fault = new InternalError();
                fault.Result = 1;
                fault.ErrorMessage = "Wystąpił błąd podczas pobierania tagów";
                throw new FaultException<InternalError>(fault, new FaultReason(fault.ErrorMessage));
            }
        }

        public Category[] GetCategories(string login, string sessionToken)
        {
            validateSessionToken(login, sessionToken);
            return CategoryS.getCategories();
        }

        public Education[] GetEducationDegrees(string login, string sessionToken)
        {
            validateSessionToken(login, sessionToken);
            return EducationS.getEducationDegrees();
        }


        bool validateSessionToken(string login, string sessionToken)
        {
            if (loggedUsers.ContainsKey(login) && loggedUsers[login] != null && loggedUsers[login].Contains(sessionToken))
            {
                return true;
            }
            else
            {
                InvalidSessionTokenError fault = new InvalidSessionTokenError();
                fault.Result = 1;
                fault.ErrorMessage = "Użytego nieważnego tokenu sesji";
                throw new FaultException<InvalidSessionTokenError>(fault, new FaultReason(fault.ErrorMessage));
            }
             
        }
    }
}
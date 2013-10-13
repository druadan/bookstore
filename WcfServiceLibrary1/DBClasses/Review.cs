using System;
using System.Runtime.Serialization;

namespace Bookstore_Service.DBClasses
{
    [DataContract]
    public class Review
    {

        public Review(int id, string customer_login, int book_id, string title, string content, double score){
            this.id = id;
            this.customer_login = customer_login;
            this.book_id = book_id;
            this.title = title;
            this.content = content;
            this.score = score;
        }

        [DataMember]
        public int id { get; set; }
        [DataMember]
        public String customer_login { get; set; }
        [DataMember]
        public int book_id { get; set; }
        [DataMember]
        public String title { get; set; }
        [DataMember]
        public String content { get; set; }
        [DataMember]
        public double score { get; set; }


       
    }
}

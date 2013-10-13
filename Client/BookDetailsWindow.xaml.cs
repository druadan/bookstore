using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Bookstore_Service.DBClasses;
using Bookstore_Service;
using System.ServiceModel;

namespace Client
{
    /// <summary>
    /// Interaction logic for BookDetailsWindow.xaml
    /// </summary>
    public partial class BookDetailsWindow : Window
    {
        Book book;
        List<Review> reviewsList;
        public BookDetailsWindow(Book b)
        {
            InitializeComponent();
            book = b;

            titleTextBlock.Text = b.title;
            authorTextBlock.Text = b.author;
            categoryTextBlock.Text = b.category;
            priceTextBlock.Text = b.price.ToString();

            using (ChannelFactory<IBookstore> factory = new ChannelFactory<IBookstore>("BookstoreClient"))
            {
                try
                {
                    IBookstore proxy = factory.CreateChannel();
                    reviewsList = new List<Review>(proxy.GetReviews(b.id));

                }
                catch (FaultException<InternalError> err)
                {
                    MessageBox.Show(err.Detail.ToString());
                }
            }

        }
    }
}

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
        int reviewInd;
       
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

                    avgScoreTextBlock.Text = proxy.GetAverageScore(b.id).ToString();
                }
                catch (FaultException<InternalError> err)
                {
                    MessageBox.Show(err.Detail.ToString());
                }
            }

            

            reviewInd = 0;
            refreshReview();

        }

        private void refreshReview()
        {
            Review r = reviewsList[reviewInd];

            reviewAuthorTextBlock.Text = r.customer_login;
            reviewTitleTextBlock.Text = r.title;
            reviewContentTextBlock.Text = r.content;
            scoreTextBlock.Text = r.score.ToString();

            prevReviewButton.IsEnabled = reviewInd > 0;
            nextReviewButton.IsEnabled = reviewsList.Count - 1 > reviewInd;
        }

        private void prevReviewButton_Click(object sender, RoutedEventArgs e)
        {
            reviewInd--;
            refreshReview();
        }

        private void nextReviewButton_Click(object sender, RoutedEventArgs e)
        {
            reviewInd++;
            refreshReview();
        }

        private void reviewAuthorTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Window nextWindow = new OtherUserDetailsWindow(reviewAuthorTextBlock.Text);
            App.Current.MainWindow = nextWindow;
            this.Close();
            nextWindow.Show();
        }
    }
}

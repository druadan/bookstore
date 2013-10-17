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
                    avgScoreTextBlock.Text = proxy.GetAverageScore(b.id);
                }
                catch (FaultException<InternalError> err)
                {
                    MessageBox.Show(err.Detail.ToString());
                }
            }

            List<Double> scoresList = new List<double>(new double[]{1.0, 1.5, 2.0, 2.5, 3.0, 3.5, 4.0, 4.5, 5.0});
            newReviewScoreCB.ItemsSource = scoresList;

            reviewInd = 0;
            refreshReviewView();

        }

        private void refreshReviewView()
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
            refreshReviewView();
        }

        private void nextReviewButton_Click(object sender, RoutedEventArgs e)
        {
            reviewInd++;
            refreshReviewView();
        }

        private void reviewAuthorTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Window nextWindow = new OtherUserDetailsWindow(reviewAuthorTextBlock.Text);
            App.Current.MainWindow = nextWindow;
            this.Close();
            nextWindow.Show();
        }

        private void sendReviewBtn_Click(object sender, RoutedEventArgs e)
        {
            using (ChannelFactory<IBookstore> factory = new ChannelFactory<IBookstore>("BookstoreClient"))
            {
                try
                {
                    IBookstore proxy = factory.CreateChannel();

                    Review r = new Review();
                    r.content = newReviewContentTB.Text;
                    r.title = newReviewTitleTB.Text;
                    r.score = Double.Parse(newReviewScoreCB.Text);
                    // r.customer_login = App.login;
                    r.customer_login = "pr";
                    r.book_id = book.id;

                    proxy.AddReview(r);
                    reviewsList = new List<Review>(proxy.GetReviews(book.id));
                    avgScoreTextBlock.Text = proxy.GetAverageScore(book.id);
                    reviewInd = 0;
                    refreshReviewView();


                    newReviewContentTB.Text = "";
                    newReviewTitleTB.Text = "";
                    newReviewScoreCB.Text = null;
                }
                catch (FaultException<InternalError> err)
                {
                    MessageBox.Show(err.Detail.ToString());
                }
            }
        }
    }
}

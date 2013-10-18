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
       
        public BookDetailsWindow()
        {
            InitializeComponent();
        }


        public void setBook(Book b)
        {
            this.book = b;

            titleTextBlock.Text = b.title;
            authorTextBlock.Text = b.author;
            categoryTextBlock.Text = b.category;
            priceTextBlock.Text = b.price.ToString();

            try
             {
                using (ChannelFactory<IBookstore> factory = new ChannelFactory<IBookstore>("BookstoreClient"))
                {
               
                        IBookstore proxy = factory.CreateChannel();
                        reviewsList = new List<Review>(proxy.GetReviews(b.id, App.login, App.sessionToken));
                        avgScoreTextBlock.Text = proxy.GetAverageScore(b.id, App.login, App.sessionToken);
                        tagsLB.ItemsSource = new List<Tag>(proxy.GetTopTagsForBook(b.id, App.login, App.sessionToken));
               
                }
             }
             catch (FaultException<BookstoreError> err)
             {
                 MessageBox.Show(err.Detail.ToString());
             }

            List<Double> scoresList = new List<double>(new double[] { 1.0, 1.5, 2.0, 2.5, 3.0, 3.5, 4.0, 4.5, 5.0 });
            newReviewScoreCB.ItemsSource = scoresList;

            reviewInd = 0;
            refreshReviewView();
        }

        private void refreshReviewView()
        {
            if (reviewsList.Count > 0)
            {
                Review r = reviewsList[reviewInd];

                reviewAuthorBtn.Content = r.customer_login;
                reviewTitleTextBlock.Text = r.title;
                reviewContentTextBlock.Text = r.content;
                scoreTextBlock.Text = r.score.ToString();

                prevReviewButton.IsEnabled = reviewInd > 0;
                nextReviewButton.IsEnabled = reviewsList.Count - 1 > reviewInd;
                reviewAuthorBtn.IsEnabled = true;
            }
            else
            {
                reviewAuthorBtn.Content = reviewTitleTextBlock.Text = reviewContentTextBlock.Text = scoreTextBlock.Text = "";
                prevReviewButton.IsEnabled = nextReviewButton.IsEnabled = reviewAuthorBtn.IsEnabled = false ;
               
            }
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


        private void sendReviewBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (ChannelFactory<IBookstore> factory = new ChannelFactory<IBookstore>("BookstoreClient"))
                {
                    if (newReviewContentTB.Text.Equals("") || newReviewTitleTB.Text.Equals("") || newReviewScoreCB.Text.Equals(""))
                    {
                        MessageBox.Show("Nie wypełniłeś wszystkich potrzebnych pól do wystawienia komentarza");
                    }
                    else
                    {


                        IBookstore proxy = factory.CreateChannel();

                        Review r = new Review();
                        r.content = newReviewContentTB.Text;
                        r.title = newReviewTitleTB.Text;
                        r.score = Double.Parse(newReviewScoreCB.Text);
                        // r.customer_login = App.login;
                        r.customer_login = "pr";
                        r.book_id = book.id;

                        proxy.AddReview(r, App.login, App.sessionToken);
                        reviewsList = new List<Review>(proxy.GetReviews(book.id, App.login, App.sessionToken));
                        avgScoreTextBlock.Text = proxy.GetAverageScore(book.id, App.login, App.sessionToken);
                        reviewInd = 0;
                        refreshReviewView();


                        newReviewContentTB.Text = "";
                        newReviewTitleTB.Text = "";
                        newReviewScoreCB.Text = null;
                    }
                }
            }
            catch (FaultException<BookstoreError> err)
            {
                MessageBox.Show(err.Detail.ToString());
            }
        }
            

         
        

        private void reviewAuthorBtn_Click(object sender, RoutedEventArgs e)
        {
            App.nextWindow(this, App.otherUserDetailsWindow);
            App.otherUserDetailsWindow.setUserLogin((String)reviewAuthorBtn.Content);
        }

        private void addTagBtn_Click(object sender, RoutedEventArgs e)
        {
            if (addTagTB.Text.Equals(""))
            {
                MessageBox.Show("Tag musi mieć treść!");
            }
            else
            {
                 try
                 {
                    using (ChannelFactory<IBookstore> factory = new ChannelFactory<IBookstore>("BookstoreClient"))
                    {
                   
                        IBookstore proxy = factory.CreateChannel();

                        Tag t = new Tag();
                        t.tag_id = addTagTB.Text;


                        proxy.AddTag(t, book.id, App.login, App.sessionToken);
                        tagsLB.ItemsSource = new List<Tag>(proxy.GetTopTagsForBook(book.id, App.login, App.sessionToken));


                        addTagTB.Text = "";
                   
                    }
                 }
                 catch (FaultException<BookstoreError> err)
                 {
                     MessageBox.Show(err.Detail.ToString());
                 }
            }
        }

        private void tagsLB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tagsLB.SelectedItem != null)
            {
                addTagTB.Text = tagsLB.SelectedItem.ToString();
            }
        }

        private void prevWindowBtn_Click(object sender, RoutedEventArgs e)
        {
            App.prevWindow(this);
        }
    }
}

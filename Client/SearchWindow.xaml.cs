using System.ServiceModel;
using System.Windows;
using System.Data;
using Bookstore_Service;
using Bookstore_Service.DBClasses;
using System.Reflection;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using System.Text.RegularExpressions;

namespace Client
{
    /// <summary>
    /// Interaction logic for SearchWindow.xaml
    /// </summary>
    public partial class SearchWindow : Window
    {

        List<Category> catList = null;
        List<Education> eduList = null;

        public SearchWindow()
        {
            InitializeComponent();
        }

        // need to call it later, because 
        public void init(){
            if (catList == null || eduList == null)
            {
                try
                {
                    using (ChannelFactory<IBookstore> factory = new ChannelFactory<IBookstore>("BookstoreClient"))
                    {

                        IBookstore proxy = factory.CreateChannel();
                        catList = new List<Category>();
                        catList.Add(new Category(""));
                        catList.AddRange(proxy.GetCategories(App.login, App.sessionToken));

                        eduList = new List<Education>();
                        eduList.Add(new Education(""));
                        eduList.AddRange(proxy.GetEducationDegrees(App.login, App.sessionToken));

                        categoryCB.ItemsSource = catList;
                        reviewerEducationCB.ItemsSource = eduList;
                    }

                }
                catch (FaultException<BookstoreError> err)
                {
                    MessageBox.Show(err.Detail.ToString());
                }
            }
        }

        static Book[] booksList;

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (ChannelFactory<IBookstore> factory = new ChannelFactory<IBookstore>("BookstoreClient"))
                {

                    IBookstore proxy = factory.CreateChannel();

                    bool? allOrAny = allRadioButton.IsChecked;
                    if (!allOrAny.HasValue) //check for a value
                    {
                        allOrAny = false;
                    }

                    double minScore = minScoreTB.Text.Equals("") ? double.NaN : double.Parse(minScoreTB.Text);
                    double maxScore = maxScoreTB.Text.Equals("") ? double.NaN : double.Parse(maxScoreTB.Text);

                    double minAge = reviewerAgeMinTB.Text.Equals("") ? double.NaN : double.Parse(reviewerAgeMinTB.Text);
                    double maxAge = reviewerAgeMaxTB.Text.Equals("") ? double.NaN : double.Parse(reviewerAgeMaxTB.Text);

                    booksList = proxy.GetBooks(titleTextBox.Text, authorTextBox.Text, categoryCB.Text, tagTextBox.Text, minScore, maxScore, minAge, maxAge, reviewerEducationCB.Text, (bool)allOrAny ? 0 : 1, App.login, App.sessionToken);

                    booksDataGrid.ItemsSource = new List<Book>(booksList);

                }


            }
            catch (FaultException<BookstoreError> err)
            {
                MessageBox.Show(err.Detail.ToString());
            }
        }

        private void booksDataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

        private void booksDatagrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Book b = (Book)booksDataGrid.SelectedItem;
            App.nextWindow(this, App.bookDetailsWindow);
            App.bookDetailsWindow.setBook(b);
        }



      
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^[0-9]*[,.]?[0-9]*$");
            e.Handled = !regex.IsMatch(e.Text);
        }



    }
}

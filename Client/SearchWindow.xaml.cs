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
        public SearchWindow()
        {
            InitializeComponent();
            using (ChannelFactory<IBookstore> factory = new ChannelFactory<IBookstore>("BookstoreClient"))
            {

                try
                {
                    IBookstore proxy = factory.CreateChannel();
                    List<Category> catList = new List<Category>(proxy.GetCategories());
                    catList.Add(new Category(""));
                    categoryCB.ItemsSource = catList;
                }
                catch (FaultException<InternalError> err)
                {
                    MessageBox.Show(err.Detail.ToString());
                }

            }
        }

        static Book[] booksList;

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            using (ChannelFactory<IBookstore> factory = new ChannelFactory<IBookstore>("BookstoreClient"))
            {
               
                try
                {

                    IBookstore proxy = factory.CreateChannel();

                    bool? allOrAny = allRadioButton.IsChecked;
                    if (!allOrAny.HasValue) //check for a value
                    {
                        allOrAny = false;
                    }

                    double minScore = minScoreTB.Text.Equals("") ? double.NaN : double.Parse(minScoreTB.Text);
                    double maxScore = maxScoreTB.Text.Equals("") ? double.NaN : double.Parse(maxScoreTB.Text);

                    booksList = proxy.GetBooks(titleTextBox.Text, authorTextBox.Text, categoryCB.Text, tagTextBox.Text, minScore, maxScore, (bool)allOrAny ? 0 : 1 );

                    booksDataGrid.ItemsSource = new List<Book>(booksList);
                   
                    
                }
                catch (FaultException<InternalError> err)
                {
                    MessageBox.Show(err.Detail.ToString());
                }

            }
        }

        private void booksDataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

        private void booksDatagrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Book b = (Book)booksDataGrid.SelectedItem;
            App.changeWindow(this, App.bookDetailsWindow);
            App.bookDetailsWindow.setBook(b);
        }



      
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^[0-9]*[,.]?[0-9]*$");
            e.Handled = !regex.IsMatch(e.Text);
        }

    }
}

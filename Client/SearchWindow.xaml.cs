using System.ServiceModel;
using System.Windows;
using System.Data;
using Bookstore_Service;
using Bookstore_Service.DBClasses;
using System.Reflection;
using System.Data.SqlClient;
using System.Data;
using System.Data;
using System.Collections.Generic;
using System.Windows.Controls;

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

                    booksList = proxy.GetBooks(titleTextBox.Text, authorTextBox.Text, categoryTextBox.Text, tagTextBox.Text, (bool)allOrAny ? 0 : 1 );

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

            Window nextWindow = new BookDetailsWindow(b);
            App.Current.MainWindow = nextWindow;
            this.Close();
            nextWindow.Show();
        }



    }
}

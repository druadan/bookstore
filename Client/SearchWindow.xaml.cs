using System.ServiceModel;
using System.Windows;
using System.Data;
using Bookstore_Service;
using Bookstore_Service.DBClasses;
using System.Reflection;
using System.Data.SqlClient;
using System.Data;

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

                    booksList = proxy.Search(titleTextBox.Text, authorTextBox.Text, categoryTextBox.Text, tagTextBox.Text, (bool)allOrAny ? 0 : 1 );

                    PropertyInfo[] pi = typeof(Book).GetProperties();
                    
                    DataTable dt = new DataTable();

                    foreach (PropertyInfo p in pi)
                    {
                        string s = p.Name;
                        if(p.Name.Equals("id")){
                            continue;
                        }
                        dt.Columns.Add(p.Name);

                    }
                    dt.Rows.Clear();
                    foreach(Book b in booksList){
                        DataRow newRow = dt.NewRow();
                        foreach (PropertyInfo p in pi)
                        {
                            if (p.Name.Equals("id"))
                            {
                                continue;
                            }
                            newRow[p.Name] = p.GetValue(b,null);
                        }
                        dt.Rows.Add(newRow);
                    }

                    /*
                    string conStr = "Data Source=DRUADAN-DESKTOP\\SQLEXPRESS; User ID=adm; Password=adm";
                    SqlConnection conn = new SqlConnection(conStr);
                    conn.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM bookstore.dbo.Book", conn);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds, "Book");
                    MessageBox.Show(ds.GetXml());
                    booksDataGrid.DataContext = ds.Tables["Book"].DefaultView;*/


                    booksDataGrid.DataContext = dt.DefaultView;
                    
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



    }
}

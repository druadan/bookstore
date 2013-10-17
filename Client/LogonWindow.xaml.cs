using System.ServiceModel;
using System.Windows;
using Bookstore_Service;

namespace Client
{
    /// <summary>
    /// Interaction logic for LogonWindow.xaml
    /// </summary>
    public partial class LogonWindow : Window
    {
        public LogonWindow()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            using (ChannelFactory<IBookstore> factory = new ChannelFactory<IBookstore>("BookstoreClient"))
            {
                string obtaintedToken = null;
                try
                {
                    IBookstore proxy = factory.CreateChannel();
                    obtaintedToken = proxy.Login(loginTextBox.Text, passwordTextBox.Password);

                    if ("".Equals(obtaintedToken))
                    {
                        MessageBox.Show("Błędny login lub hasło");
                    }
                    else
                    {
                        App.sessionToken = obtaintedToken;
                        App.login = loginTextBox.Text;

                        App.changeWindow(this, App.mainWindow);
                    }

                }
                catch (FaultException<InternalError> err)
                {
                    MessageBox.Show( err.Detail.ToString() );
                }

            }
        }
    }
}

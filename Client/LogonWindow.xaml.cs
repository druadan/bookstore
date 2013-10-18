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

                    App.sessionToken = obtaintedToken;
                    App.login = loginTextBox.Text;

                    App.nextWindow(this, App.mainWindow);
                    passwordTextBox.Password = "";
                    
                }
                catch (FaultException<LoginError> err)
                {
                    MessageBox.Show(err.Detail.ToString());
                }
                catch (FaultException<InternalError> err)
                {
                    MessageBox.Show( err.Detail.ToString() );
                }

            }
        }
    }
}

using System.ServiceModel;
using System.Windows;
using Bookstore_Service;
using Bookstore_Service.DBClasses;

namespace Client
{
    /// <summary>
    /// Interaction logic for OtherUserDetailsWindow.xaml
    /// </summary>
    public partial class OtherUserDetailsWindow : Window
    {

       
        public OtherUserDetailsWindow(string userLogin)
        {
            InitializeComponent();

            using (ChannelFactory<IBookstore> factory = new ChannelFactory<IBookstore>("BookstoreClient"))
            {
                try
                {
                    IBookstore proxy = factory.CreateChannel();

                    OtherClient oc = proxy.GetOtherClient(userLogin);
                    ageTB.Text = oc.age.ToString();
                    loginTB.Text = oc.login;
                    educationTB.Text = oc.education;
                    nameTB.Text = oc.name;
                    preferredTB.Text = oc.preferredCat + ", " + oc.preferredCat2;


                }
                catch (FaultException<InternalError> err)
                {
                    MessageBox.Show(err.Detail.ToString());
                }
            }
        }
    }
}

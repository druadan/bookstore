﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Bookstore_Service;
using System.ServiceModel.Description;
using System.ServiceModel;
using System.ServiceModel.Security;

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }


        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            App.nextWindow(this, App.searchWindow);
        }

        private void logoutButtion_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (ChannelFactory<IBookstore> factory = new ChannelFactory<IBookstore>("BookstoreClient"))
                {
                    IBookstore proxy = factory.CreateChannel();
                    int result = proxy.Logout(App.login, App.sessionToken);

                    if (result != 0)
                    {
                        MessageBox.Show("Błąd podczas wylogowywania!");
                    }
                    else
                    {
                        App.sessionToken = "";
                        App.login = "";

                        App.nextWindow(this, App.logonWindow);
                    }
                }


             }
            catch (FaultException<BookstoreError> err)
            {
                MessageBox.Show(err.Detail.ToString());
            }
        }

           
    }
}

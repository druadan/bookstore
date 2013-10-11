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
using System.Windows.Shapes;
using Bookstore_Service;
using System.ServiceModel;

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
                    obtaintedToken = proxy.Login(loginTextBox.Text, passwordTextBox.Text);

                    if ("".Equals(obtaintedToken))
                    {
                        MessageBox.Show("Błędny login lub haslo");
                    }
                    else
                    {
                        App.sessionToken = obtaintedToken;
                        MessageBox.Show("W pytę!");
                        MainWindow main = new MainWindow();
                        App.Current.MainWindow = main;
                        this.Close();
                        main.Show();
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
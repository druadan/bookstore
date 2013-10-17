using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static String sessionToken = "";
        public static String login = "";

        public static BookDetailsWindow bookDetailsWindow = new BookDetailsWindow();
        public static LogonWindow logonWindow = new LogonWindow();
        public static MainWindow mainWindow = new MainWindow();
        public static OtherUserDetailsWindow otherUserDetailsWindow = new OtherUserDetailsWindow();
        public static SearchWindow searchWindow = new SearchWindow();
        public static UserDetailsWindow userDetailsWindow = new UserDetailsWindow();

        public static Window prevWindow = null;

        public static void changeWindow(Window currentWindow, Window nextWindow)
        {
            currentWindow.Visibility = System.Windows.Visibility.Hidden;
            prevWindow = currentWindow;
            nextWindow.Visibility = System.Windows.Visibility.Visible;

        }
    }
}

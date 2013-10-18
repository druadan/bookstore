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
        public static Dictionary<Window, Window> prevWindows = new Dictionary<Window, Window>();

        static App()
        {
            
            prevWindows.Add(bookDetailsWindow, null);
            prevWindows.Add(logonWindow, null);
            prevWindows.Add(mainWindow, null);
            prevWindows.Add(otherUserDetailsWindow, null);
            prevWindows.Add(searchWindow, null);
            prevWindows.Add(userDetailsWindow, null);

        }

        public static void nextWindow(Window current, Window next)
        {
            current.Visibility = System.Windows.Visibility.Hidden;
            prevWindows[next] = current;
            next.Visibility = System.Windows.Visibility.Visible;
        }


        public static void prevWindow(Window current)
        {
            current.Visibility = System.Windows.Visibility.Hidden;
            prevWindows[current].Visibility = System.Windows.Visibility.Visible;
        }
    }

}

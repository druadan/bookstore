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

        public static BookDetailsWindow bookDetailsWindow;
        public static LogonWindow logonWindow;
        public static MainWindow mainWindow;
        public static OtherUserDetailsWindow otherUserDetailsWindow ;
        public static SearchWindow searchWindow ;
        public static UserDetailsWindow userDetailsWindow ;

        public static Dictionary<Window, Window> prevWindows = new Dictionary<Window, Window>();

        

 

       public static void init()
       {
           bookDetailsWindow = new BookDetailsWindow();
           logonWindow = (LogonWindow)App.Current.MainWindow;
           mainWindow = new MainWindow();
           otherUserDetailsWindow = new OtherUserDetailsWindow();
           searchWindow = new SearchWindow();
           userDetailsWindow = new UserDetailsWindow();

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
            App.Current.MainWindow = next;
            next.Visibility = System.Windows.Visibility.Visible;
        }


        public static void prevWindow(Window current)
        {
            current.Visibility = System.Windows.Visibility.Hidden;
            App.Current.MainWindow = prevWindows[current];
            prevWindows[current].Visibility = System.Windows.Visibility.Visible;
        }
    }

}

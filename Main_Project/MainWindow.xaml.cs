using System;
using System.Windows;

namespace Examp_System
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {    
        public MainWindow()
        {
            InitializeComponent();
            _data_grid.AutoGenerateColumns = true;                                
        }

        private void Chrome_click(object sender, RoutedEventArgs e)
        {
            try { _data_grid.ItemsSource = Cookies_Finder.Table_Chrome_cookies(); }
            catch (Exception ex){ MessageBox.Show(ex.Message); }
        }
        private void Opera_click(object sender, RoutedEventArgs e)
        {
            try { _data_grid.ItemsSource = Cookies_Finder.Table_Opera_cookies(); }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void FireFox_click(object sender, RoutedEventArgs e)
        {
            try { _data_grid.ItemsSource = Cookies_Finder.Table_FireFoxe_cookies(); }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
       


    }
}

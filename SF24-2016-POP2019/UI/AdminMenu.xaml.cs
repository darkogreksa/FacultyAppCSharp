using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SF24_2016_POP2019.UI
{
    /// <summary>
    /// Interaction logic for AdminMenu.xaml
    /// </summary>
    public partial class AdminMenu : Window
    {
        public AdminMenu()
        {
            InitializeComponent();
        }

        private void BtnUstanove_Click(object sender, RoutedEventArgs e)
        {
            UstanoveWindow uw = new UstanoveWindow();
            uw.Show();
        }

        private void BtnUcionice_Click(object sender, RoutedEventArgs e)
        {
            UcioniceWindow ucw = new UcioniceWindow();
            ucw.Show();
        }

        private void BtnKorisnici_Click(object sender, RoutedEventArgs e)
        {
            KorisniciWindow kw = new KorisniciWindow();
            kw.Show();
        }

        private void BtnTermini_Click(object sender, RoutedEventArgs e)
        {
            TerminiWindow tw = new TerminiWindow();
            tw.Show();
        }

        private void ZatvoriProzor_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void IzlogujSe_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Da li ste sigurni?", "Izlazak", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                var MainWindow = new MainWindow();
                MainWindow.Show();
                this.Hide();
            };
        }

    }
}

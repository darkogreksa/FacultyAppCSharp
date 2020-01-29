using SF24_2016_POP2019.Database;
using SF24_2016_POP2019.UI;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using static SF24_2016_POP2019.Model.Korisnik;

namespace SF24_2016_POP2019
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string TrenutnoLogovan { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            var korisnickoIme = this.tbKorisnickoIme.Text;
            var lozinka = this.tbLozinka.Password;
            ETipKorisnika tipKorisnika;
            bool pronasao = false;
            foreach (var korisnik in Data.Instance.Korisnici)
            {
                if (!korisnik.Obrisano && korisnik.Username == korisnickoIme && korisnik.Password == lozinka)
                {
                    tipKorisnika = korisnik.TipKorisnika;
                    switch (tipKorisnika)
                    {
                        case ETipKorisnika.Administrator:

                            this.Hide();
                            TrenutnoLogovan = korisnickoIme;
                            AdminMenu am = new AdminMenu();
                            am.Show();
                            pronasao = true;
                            break;

                        case ETipKorisnika.Profesor:

                            this.Hide();
                            TrenutnoLogovan = korisnickoIme;
                            PAWindow am1 = new PAWindow(korisnik);
                            am1.Show();
                            pronasao = true;
                            break;
                        case ETipKorisnika.Asistent:
                            this.Hide();
                            TrenutnoLogovan = korisnickoIme;
                            PAWindow am2 = new PAWindow(korisnik);
                            am2.Show();
                            pronasao = true;
                            break;
                    }

                    break;
                }
                else if (korisnik.Username == "" || korisnik.Password == "")
                {
                    MessageBox.Show("Unesite korisnicko ime i lozinku");
                }
            }
            if (!pronasao)
            {
                MessageBox.Show("Niste uspeli da se ulogujete!");
            }
        }

        private void Odustani_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Guest_Click(object sender, RoutedEventArgs e)
        {
            GuestWindow gw = new GuestWindow();
            gw.Show();
            this.Close();
        }
    }
}

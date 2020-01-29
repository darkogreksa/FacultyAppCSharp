using SF24_2016_POP2019.Database;
using SF24_2016_POP2019.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using static SF24_2016_POP2019.Model.Korisnik;

namespace SF24_2016_POP2019.UI
{
    /// <summary>
    /// Interaction logic for IzmeniKorisnikeWindow.xaml
    /// </summary>
    public partial class IzmeniKorisnikeWindow : Window
    {
        private ICollectionView viewUstanova;
        public enum Operacija
        {
            DODAVANJE,
            IZMENA
        };

        private Operacija operacija;
        private Korisnik korisnik;

        public IzmeniKorisnikeWindow(Korisnik korisnik, Operacija operacija)
        {
            InitializeComponent();
            viewUstanova = CollectionViewSource.GetDefaultView(Data.Instance.Ustanove);

            this.operacija = operacija;
            this.korisnik = korisnik;

            cbUstanova.ItemsSource = viewUstanova;
            cbTipKorisnika.ItemsSource = Enum.GetValues(typeof(ETipKorisnika)).Cast<ETipKorisnika>();

            tbIme.DataContext = korisnik;
            tbPrezime.DataContext = korisnik;
            tbEmail.DataContext = korisnik;
            tbKorisnickoIme.DataContext = korisnik;
            tbLozinka.DataContext = korisnik;
            cbTipKorisnika.DataContext = korisnik;
            cbUstanova.DataContext = korisnik;
        }

        private void IzlazIzProzora(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SacuvajIzmene(object sender, RoutedEventArgs e)
        {
            switch (operacija)
            {
                case Operacija.DODAVANJE:

                    Korisnik k = new Korisnik()
                    {
                        Ime = tbIme.Text,
                        Prezime = tbPrezime.Text,
                        Email = tbEmail.Text,
                        Username = tbKorisnickoIme.Text,
                        Password = tbLozinka.Text,
                        TipKorisnika = (ETipKorisnika)cbTipKorisnika.SelectedIndex,
                        UstanovaId = cbUstanova.SelectedIndex + 1
                    };
                    Korisnik.Create(k);
                    break;

                case Operacija.IZMENA:
                    korisnik.Ime = tbIme.Text;
                    korisnik.Prezime = tbPrezime.Text;
                    korisnik.Email = tbEmail.Text;
                    korisnik.Username = tbKorisnickoIme.Text;
                    korisnik.Password = tbLozinka.Text;
                    korisnik.TipKorisnika = (ETipKorisnika)cbTipKorisnika.SelectedIndex;
                    korisnik.UstanovaId = cbUstanova.SelectedIndex + 1;
                    Korisnik.Update(korisnik);
                    break;
            }
            Close();
        }
    }
}

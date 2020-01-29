using SF24_2016_POP2019.Database;
using SF24_2016_POP2019.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace SF24_2016_POP2019.UI
{
    /// <summary>
    /// Interaction logic for NeprijavljenKorisnikWindow.xaml
    /// </summary>
    public partial class NeprijavljenKorisnikWindow : Window
    {
        ICollectionView q;

        public NeprijavljenKorisnikWindow()
        {
            InitializeComponent();
            Inicijalizuj();
        }

        private void Inicijalizuj()
        {
            var korisnici = new ObservableCollection<Korisnik>();
            var ustanove = new ObservableCollection<Ustanova>();
            var q = (from k in korisnici
                     join u in ustanove on k.UstanovaId equals u.Id
                     select new
                     {
                         k.Ime,
                         k.Prezime,
                         k.TipKorisnika,
                         u.Naziv
                     }).ToList();

            dgNeprijavljeniKorisnik.IsSynchronizedWithCurrentItem = true;
            dgNeprijavljeniKorisnik.DataContext = this;
            dgNeprijavljeniKorisnik.ItemsSource = q;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

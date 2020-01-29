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
using static SF24_2016_POP2019.Model.Termin;

namespace SF24_2016_POP2019.UI
{
    /// <summary>
    /// Interaction logic for PADodajTermin.xaml
    /// </summary>
    public partial class PADodajTermin : Window
    {
        private ICollectionView viewKorisnik;
        private Termin termin;
        private Korisnik korisnik;
        public PADodajTermin(Termin termin, Korisnik korisnik)
        {
            InitializeComponent();
            viewKorisnik = CollectionViewSource.GetDefaultView(Data.Instance.Korisnici);
            
            this.termin = termin;
            this.korisnik = korisnik;
            
            cbTipNastave.ItemsSource = Enum.GetValues(typeof(ETipNastave)).Cast<ETipNastave>();

            cbTipNastave.DataContext = termin;
            tbVremeOd.DataContext = termin;
            tbVremeDo.DataContext = termin;
            tbDan.DataContext = termin;
        }

        private void Sacuvaj_Click(object sender, RoutedEventArgs e)
        {
            Termin t = new Termin()
            {
                TipNastave = (ETipNastave)cbTipNastave.SelectedIndex,
                VremeZauzecaOd = DateTime.Parse(tbVremeOd.Text),
                VremeZauzecaDo = DateTime.Parse(tbVremeDo.Text),
                Dan = tbDan.Text,
                KorisnikId =  korisnik.Id
            };
            Termin.Create(t);
        }

        private void Izadji_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

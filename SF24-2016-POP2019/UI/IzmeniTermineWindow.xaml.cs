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
    /// Interaction logic for IzmeniTermineWindow.xaml
    /// </summary>
    public partial class IzmeniTermineWindow : Window
    {
        private ICollectionView viewKorisnik;

        public enum Operacija
        {
            DODAVANJE,
            IZMENA
        };

        private Operacija operacija;
        private Termin termin;

        public IzmeniTermineWindow(Termin termin, Operacija operacija)
        {
            InitializeComponent();
            viewKorisnik = CollectionViewSource.GetDefaultView(Data.Instance.Korisnici);

            this.operacija = operacija;
            this.termin = termin;

            cbKorisnik.ItemsSource = viewKorisnik;
            cbTipNastave.ItemsSource = Enum.GetValues(typeof(ETipNastave)).Cast<ETipNastave>();

            cbTipNastave.DataContext = termin;
            tbVremeOd.DataContext = termin;
            tbVremeDo.DataContext = termin;
            tbDan.DataContext = termin;
            cbKorisnik.DataContext = termin;
        }

        private void Sacuvaj_Click(object sender, RoutedEventArgs e)
        {
            switch (operacija)
            {
                case Operacija.DODAVANJE:
                    Termin t = new Termin()
                    {
                        TipNastave = (ETipNastave)cbTipNastave.SelectedIndex,
                        VremeZauzecaOd = DateTime.Parse(tbVremeOd.Text),
                        VremeZauzecaDo = DateTime.Parse(tbVremeDo.Text),
                        Dan = tbDan.Text,
                        KorisnikId = cbKorisnik.SelectedIndex + 1
                    };
                    Termin.Create(t);
                    break;

                case Operacija.IZMENA:
                    termin.TipNastave = (ETipNastave)cbTipNastave.SelectedIndex;
                    termin.VremeZauzecaOd = DateTime.Parse(tbVremeOd.Text);
                    termin.VremeZauzecaDo = DateTime.Parse(tbVremeDo.Text);
                    termin.Dan = tbDan.Text;
                    termin.KorisnikId = cbKorisnik.SelectedIndex + 1;
                    Termin.Update(termin);
                    break;
            }
            Close();
        }

        private void Izadji_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

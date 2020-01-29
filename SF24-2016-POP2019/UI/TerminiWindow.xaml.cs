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

namespace SF24_2016_POP2019.UI
{
    /// <summary>
    /// Interaction logic for TerminiWindow.xaml
    /// </summary>
    public partial class TerminiWindow : Window
    {
        ICollectionView view;
        ICollectionView viewPretraga;

        public Termin IzabraniTermin { get; set; }

        public TerminiWindow()
        {
            InitializeComponent();
            InicijalizujTermin();
        }

        private void InicijalizujTermin()
        {
            view = CollectionViewSource.GetDefaultView(Data.Instance.Termini);

            view.Filter = PrikazFilter;

            dgTermin.IsSynchronizedWithCurrentItem = true;
            dgTermin.DataContext = true;
            dgTermin.ItemsSource = view;
        }

        private bool PrikazFilter(object obj)
        {
            return !((Termin)obj).Obrisano;
        }

        private void DodajTermin(object sender, RoutedEventArgs e)
        {
            var noviTermin = new Termin()
            {
                TipNastave = Termin.ETipNastave.Predavanje,
                VremeZauzecaOd = default(DateTime),
                VremeZauzecaDo = default(DateTime),
                Dan = "",
                KorisnikId = 0
            };
            var terminProzor = new IzmeniTermineWindow(noviTermin, IzmeniTermineWindow.Operacija.DODAVANJE);
            terminProzor.ShowDialog();
        }

        private void IzmeniTermin(object sender, RoutedEventArgs e)
        {
            try
            {
                IzabraniTermin = (Termin)dgTermin.SelectedItem;
                var kopija = (Termin)IzabraniTermin.Clone();
                var terminProzor = new IzmeniTermineWindow(kopija, IzmeniTermineWindow.Operacija.IZMENA);

                terminProzor.Show();
            }
            catch
            {
                MessageBox.Show("Morate obeleziti red koji zelite da menjate", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ObrisiTermin(object sender, RoutedEventArgs e)
        {
            var listaTermina = Data.Instance.Termini;
            IzabraniTermin = (Termin)dgTermin.SelectedItem;

            if (MessageBox.Show($"Da li zelite da obrisete ovaj termin?", "Brisanje", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                foreach (var termin in listaTermina)
                {
                    if (termin.Id == IzabraniTermin.Id)
                    {
                        Termin.Delete(termin);
                        view.Refresh();
                        break;
                    }
                }
            }
        }

        private void ZatvoriProzor_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Pretrazi_Click(object sender, RoutedEventArgs e)
        {
            if (cbPretraga.SelectedIndex == 0)
            {
                string tipNastave = tbPretraga.Text;
                viewPretraga = CollectionViewSource.GetDefaultView(Termin.PretragaTermina(tipNastave, Termin.TipPretrage.TIPNASTAVE));
                dgTermin.ItemsSource = viewPretraga;
            }
            else if (cbPretraga.SelectedIndex == 1)
            {
                string vremeOd = tbPretraga.Text;
                viewPretraga = CollectionViewSource.GetDefaultView(Termin.PretragaTermina(vremeOd, Termin.TipPretrage.VREMEZAUZECAOD));
                dgTermin.ItemsSource = viewPretraga;
            }
            else if (cbPretraga.SelectedIndex == 2)
            {
                string vremeDo = tbPretraga.Text;
                viewPretraga = CollectionViewSource.GetDefaultView(Termin.PretragaTermina(vremeDo, Termin.TipPretrage.VREMEZAUZECADO));
                dgTermin.ItemsSource = viewPretraga;
            }
            else if (cbPretraga.SelectedIndex == 3)
            {
                string dan = tbPretraga.Text;
                viewPretraga = CollectionViewSource.GetDefaultView(Termin.PretragaTermina(dan, Termin.TipPretrage.DAN));
                dgTermin.ItemsSource = viewPretraga;
            }
            else if (cbPretraga.SelectedIndex == 4)
            {
                string korisnikId = tbPretraga.Text;
                viewPretraga = CollectionViewSource.GetDefaultView(Termin.PretragaTermina(korisnikId, Termin.TipPretrage.KORISNIKID));
                dgTermin.ItemsSource = viewPretraga;
            }
        }

    }
}

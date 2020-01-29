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
    /// Interaction logic for KorisniciWindow.xaml
    /// </summary>
    public partial class KorisniciWindow : Window
    {
        ICollectionView view;
        ICollectionView viewPretraga;

        public Korisnik IzabraniKorisnik { get; set; }
        public KorisniciWindow()
        {
            InitializeComponent();
            InicijalizujKorisnika();
        }

        private void InicijalizujKorisnika()
        {
            view = CollectionViewSource.GetDefaultView(Data.Instance.Korisnici);

            view.Filter = PrikazFilter;

            dgKorisnik.IsSynchronizedWithCurrentItem = true;
            dgKorisnik.DataContext = this;
            dgKorisnik.ItemsSource = view;
        }

        private bool PrikazFilter(object obj)
        {
            return !((Korisnik)obj).Obrisano;
        }

        private void DodajKorisnika(object sender, RoutedEventArgs e)
        {
            var noviKorisnik = new Korisnik()
            {
                Ime = "",
                Prezime = "",
                Email = "",
                Username = "",
                Password = "",
                TipKorisnika = Korisnik.ETipKorisnika.Administrator,
                UstanovaId = 0
            };
            var korisnikProzor = new IzmeniKorisnikeWindow(noviKorisnik, IzmeniKorisnikeWindow.Operacija.DODAVANJE);
            korisnikProzor.ShowDialog();
        }

        private void IzmeniKorisnika(object sender, RoutedEventArgs e)
        {
            try
            {
                IzabraniKorisnik = (Korisnik)dgKorisnik.SelectedItem;
                var kopija = (Korisnik)IzabraniKorisnik.Clone();
                var korisnikProzor = new IzmeniKorisnikeWindow(kopija, IzmeniKorisnikeWindow.Operacija.IZMENA);

                korisnikProzor.Show();
            }
            catch
            {
                MessageBox.Show("Morate obeleziti red koji zelite da menjate", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ObrisiKorisnika_Click(object sender, RoutedEventArgs e)
        {
            var listaKorisnika = Data.Instance.Korisnici;

            if (MessageBox.Show($"Da li zelite da obrisete {IzabraniKorisnik.Ime} {IzabraniKorisnik.Prezime}?", "Brisanje", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                foreach (var korisnik in listaKorisnika)
                {
                    if (korisnik.Id == IzabraniKorisnik.Id)
                    {
                        Korisnik.Delete(korisnik);
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

        private void PretraziKorisnika_Click(object sender, RoutedEventArgs e)
        {
            if (cbPretraga.SelectedIndex == 0)
            {
                string ime = tbPretraga.Text;
                viewPretraga = CollectionViewSource.GetDefaultView(Korisnik.PretragaKorisnika(ime, Korisnik.TipPretrage.IME));
                dgKorisnik.ItemsSource = viewPretraga;
            }
            else if (cbPretraga.SelectedIndex == 1)
            {
                string prezime = tbPretraga.Text;
                viewPretraga = CollectionViewSource.GetDefaultView(Korisnik.PretragaKorisnika(prezime, Korisnik.TipPretrage.PREZIME));
                dgKorisnik.ItemsSource = viewPretraga;
            }
            else if (cbPretraga.SelectedIndex == 2)
            {
                string korisnickoIme = tbPretraga.Text;
                viewPretraga = CollectionViewSource.GetDefaultView(Korisnik.PretragaKorisnika(korisnickoIme, Korisnik.TipPretrage.USERNAME));
                dgKorisnik.ItemsSource = viewPretraga;
            }
            else if (cbPretraga.SelectedIndex == 3)
            {
                string email = tbPretraga.Text;
                viewPretraga = CollectionViewSource.GetDefaultView(Korisnik.PretragaKorisnika(email, Korisnik.TipPretrage.EMAIL));
                dgKorisnik.ItemsSource = viewPretraga;
            }
            else if (cbPretraga.SelectedIndex == 4)
            {
                string tipKorisnika = tbPretraga.Text;
                viewPretraga = CollectionViewSource.GetDefaultView(Korisnik.PretragaKorisnika(tipKorisnika, Korisnik.TipPretrage.TIPKORISNIKA));
                dgKorisnik.ItemsSource = viewPretraga;
            }
            else if (cbPretraga.SelectedIndex == 5)
            {
                string ustanovaId = tbPretraga.Text;
                viewPretraga = CollectionViewSource.GetDefaultView(Korisnik.PretragaKorisnika(ustanovaId, Korisnik.TipPretrage.USTANOVAID));
                dgKorisnik.ItemsSource = viewPretraga;
            }
        }

        private void SortirajKorisnika_Click(object sender, RoutedEventArgs e)
        {
            if (cbSortiranje.SelectedIndex == 0)
            {
                if (cbSortiraj.SelectedIndex == 0)
                {
                    dgKorisnik.Items.SortDescriptions.Clear();
                    dgKorisnik.Items.SortDescriptions.Add(new SortDescription("Ime", ListSortDirection.Descending));
                    dgKorisnik.ItemsSource = view;
                }
                else if (cbSortiraj.SelectedIndex == 1)
                {
                    dgKorisnik.Items.SortDescriptions.Clear();
                    dgKorisnik.Items.SortDescriptions.Add(new SortDescription("Ime", ListSortDirection.Ascending));
                    dgKorisnik.ItemsSource = view;
                }
            }
            else if (cbSortiranje.SelectedIndex == 1)
            {
                if (cbSortiraj.SelectedIndex == 0)
                {
                    dgKorisnik.Items.SortDescriptions.Clear();
                    dgKorisnik.Items.SortDescriptions.Add(new SortDescription("Prezime", ListSortDirection.Descending));
                    dgKorisnik.ItemsSource = view;
                }
                else if (cbSortiraj.SelectedIndex == 1)
                {
                    dgKorisnik.Items.SortDescriptions.Clear();
                    dgKorisnik.Items.SortDescriptions.Add(new SortDescription("Prezime", ListSortDirection.Ascending));
                    dgKorisnik.ItemsSource = view;
                }
            }
            else if (cbSortiranje.SelectedIndex == 2)
            {
                if (cbSortiraj.SelectedIndex == 0)
                {
                    dgKorisnik.Items.SortDescriptions.Clear();
                    dgKorisnik.Items.SortDescriptions.Add(new SortDescription("Username", ListSortDirection.Descending));
                    dgKorisnik.ItemsSource = view;
                }
                else if (cbSortiraj.SelectedIndex == 1)
                {
                    dgKorisnik.Items.SortDescriptions.Clear();
                    dgKorisnik.Items.SortDescriptions.Add(new SortDescription("Username", ListSortDirection.Ascending));
                    dgKorisnik.ItemsSource = view;
                }
            }
            else if (cbSortiranje.SelectedIndex == 3)
            {
                if (cbSortiraj.SelectedIndex == 0)
                {
                    dgKorisnik.Items.SortDescriptions.Clear();
                    dgKorisnik.Items.SortDescriptions.Add(new SortDescription("Email", ListSortDirection.Descending));
                    dgKorisnik.ItemsSource = view;
                }
                else if (cbSortiraj.SelectedIndex == 1)
                {
                    dgKorisnik.Items.SortDescriptions.Clear();
                    dgKorisnik.Items.SortDescriptions.Add(new SortDescription("Email", ListSortDirection.Ascending));
                    dgKorisnik.ItemsSource = view;
                }
            }
            else if (cbSortiranje.SelectedIndex == 4)
            {
                if (cbSortiraj.SelectedIndex == 0)
                {
                    dgKorisnik.Items.SortDescriptions.Clear();
                    dgKorisnik.Items.SortDescriptions.Add(new SortDescription("TipKorisnika", ListSortDirection.Descending));
                    dgKorisnik.ItemsSource = view;
                }
                else if (cbSortiraj.SelectedIndex == 1)
                {
                    dgKorisnik.Items.SortDescriptions.Clear();
                    dgKorisnik.Items.SortDescriptions.Add(new SortDescription("TipKorisnika", ListSortDirection.Ascending));
                    dgKorisnik.ItemsSource = view;
                }
            }
            else if (cbSortiranje.SelectedIndex == 5)
            {
                if (cbSortiraj.SelectedIndex == 0)
                {
                    dgKorisnik.Items.SortDescriptions.Clear();
                    dgKorisnik.Items.SortDescriptions.Add(new SortDescription("UstanovaId", ListSortDirection.Descending));
                    dgKorisnik.ItemsSource = view;
                }
                else if (cbSortiraj.SelectedIndex == 1)
                {
                    dgKorisnik.Items.SortDescriptions.Clear();
                    dgKorisnik.Items.SortDescriptions.Add(new SortDescription("UstanovaId", ListSortDirection.Ascending));
                    dgKorisnik.ItemsSource = view;
                }
            }
        }

    }
}

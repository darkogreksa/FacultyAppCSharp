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
    /// Interaction logic for UstanoveWindow.xaml
    /// </summary>
    public partial class UstanoveWindow : Window
    {
        ICollectionView view;
        ICollectionView viewPretraga;

        public Ustanova IzabranaUstanova { get; set; }

        public UstanoveWindow()
        {
            InitializeComponent();
            InicijalizujUstanovu();
        }

        private void InicijalizujUstanovu()
        {
            view = CollectionViewSource.GetDefaultView(Data.Instance.Ustanove);

            view.Filter = PrikazFilter;

            dgUstanova.IsSynchronizedWithCurrentItem = true;
            dgUstanova.DataContext = this;
            dgUstanova.ItemsSource = view;
        }

        private bool PrikazFilter(object obj)
        {
            return !((Ustanova)obj).Obrisano;
        }

        private void DodajUstanovu(object sender, RoutedEventArgs e)
        {
            var novaUstanova = new Ustanova()
            {
                SifraUstanove = "",
                Naziv = "",
                Lokacija = ""
            };
            var ustanovaProzor = new IzmeniUstanoveWindow(novaUstanova, IzmeniUstanoveWindow.Operacija.DODAVANJE);
            ustanovaProzor.ShowDialog();
        }

        private void IzmeniUstanovu(object sender, RoutedEventArgs e)
        {
            try
            {
                IzabranaUstanova = (Ustanova)dgUstanova.SelectedItem;
                var kopija = (Ustanova)IzabranaUstanova.Clone();
                var ustanovaProzor = new IzmeniUstanoveWindow(kopija, IzmeniUstanoveWindow.Operacija.IZMENA);

                ustanovaProzor.Show();
            }
            catch
            {
                MessageBox.Show("Morate obeleziti red koji zelite da menjate", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ObrisiUstanovu_Click(object sender, RoutedEventArgs e)
        {
            var listaUstanova = Data.Instance.Ustanove;

            if (MessageBox.Show($"Da li zelite da obrisete {IzabranaUstanova.Naziv} ?", "Brisanje", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                foreach (var ustanova in listaUstanova)
                {
                    if (ustanova.Id == IzabranaUstanova.Id)
                    {
                        Ustanova.Delete(ustanova);
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

        private void PretraziUstanovu_Click(object sender, RoutedEventArgs e)
        {
            if (cbPretraga.SelectedIndex == 0)
            {
                string sifra = tbPretraga.Text;
                viewPretraga = CollectionViewSource.GetDefaultView(Ustanova.PretragaUstanove(sifra, Ustanova.TipPretrage.SIFRAUSTANOVE));
                dgUstanova.ItemsSource = viewPretraga;
            }
            else if (cbPretraga.SelectedIndex == 1)
            {
                string naziv = tbPretraga.Text;
                viewPretraga = CollectionViewSource.GetDefaultView(Ustanova.PretragaUstanove(naziv, Ustanova.TipPretrage.NAZIV));
                dgUstanova.ItemsSource = viewPretraga;
            }
            else if (cbPretraga.SelectedIndex == 2)
            {
                string lokacija = tbPretraga.Text;
                viewPretraga = CollectionViewSource.GetDefaultView(Ustanova.PretragaUstanove(lokacija, Ustanova.TipPretrage.LOKACIJA));
                dgUstanova.ItemsSource = viewPretraga;
            }
        }

        private void SortirajUstanovu_Click(object sender, RoutedEventArgs e)
        {
            if (cbSortiranje.SelectedIndex == 0)
            {
                if (cbSortiraj.SelectedIndex == 0)
                {
                    dgUstanova.Items.SortDescriptions.Clear();
                    dgUstanova.Items.SortDescriptions.Add(new SortDescription("SifraUstanove", ListSortDirection.Descending));
                    dgUstanova.ItemsSource = view;
                }
                else if (cbSortiraj.SelectedIndex == 1)
                {
                    dgUstanova.Items.SortDescriptions.Clear();
                    dgUstanova.Items.SortDescriptions.Add(new SortDescription("SifraUstanove", ListSortDirection.Ascending));
                    dgUstanova.ItemsSource = view;
                }
            }
            else if (cbSortiranje.SelectedIndex == 1)
            {
                if (cbSortiraj.SelectedIndex == 0)
                {
                    dgUstanova.Items.SortDescriptions.Clear();
                    dgUstanova.Items.SortDescriptions.Add(new SortDescription("Naziv", ListSortDirection.Descending));
                    dgUstanova.ItemsSource = view;
                }
                else if (cbSortiraj.SelectedIndex == 1)
                {
                    dgUstanova.Items.SortDescriptions.Clear();
                    dgUstanova.Items.SortDescriptions.Add(new SortDescription("Naziv", ListSortDirection.Ascending));
                    dgUstanova.ItemsSource = view;
                }
            }
            else if (cbSortiranje.SelectedIndex == 2)
            {
                if (cbSortiraj.SelectedIndex == 0)
                {
                    dgUstanova.Items.SortDescriptions.Clear();
                    dgUstanova.Items.SortDescriptions.Add(new SortDescription("Lokacija", ListSortDirection.Descending));
                    dgUstanova.ItemsSource = view;
                }
                else if (cbSortiraj.SelectedIndex == 1)
                {
                    dgUstanova.Items.SortDescriptions.Clear();
                    dgUstanova.Items.SortDescriptions.Add(new SortDescription("Lokacija", ListSortDirection.Ascending));
                    dgUstanova.ItemsSource = view;
                }
            }

        }
    }
}

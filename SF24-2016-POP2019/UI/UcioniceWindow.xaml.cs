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
    /// Interaction logic for UcioniceWindow.xaml
    /// </summary>
    public partial class UcioniceWindow : Window
    {
        ICollectionView view;
        ICollectionView viewPretraga;

        public Ucionica IzabranaUcionica { get; set; }

        public UcioniceWindow()
        {
            InitializeComponent();
            InicijalizujUcionicu();
        }

        private void InicijalizujUcionicu()
        {
            view = CollectionViewSource.GetDefaultView(Data.Instance.Ucionice);

            view.Filter = PrikazFilter;

            dgUcionica.IsSynchronizedWithCurrentItem = true;
            dgUcionica.DataContext = this;
            dgUcionica.ItemsSource = view;
        }

        private bool PrikazFilter(object obj)
        {
            return !((Ucionica)obj).Obrisano;
        }

        private void DodajUcionicu(object sender, RoutedEventArgs e)
        {
            var novaUcionica = new Ucionica()
            {
                BrojUcionice = 0,
                BrojMesta = 0,
                TipUcionice = Ucionica.ETipUcionice.SaRacunarima,
                UstanovaId = 0
            };
            var ucionicaProzor = new IzmeniUcioniceWindow(novaUcionica, IzmeniUcioniceWindow.Operacija.DODAVANJE);
            ucionicaProzor.ShowDialog();
        }

        private void IzmeniUcionicu(object sender, RoutedEventArgs e)
        {
            try
            {
                IzabranaUcionica = (Ucionica)dgUcionica.SelectedItem;
                var kopija = (Ucionica)IzabranaUcionica.Clone();
                var ucionicaProzor = new IzmeniUcioniceWindow(kopija, IzmeniUcioniceWindow.Operacija.IZMENA);

                ucionicaProzor.Show();
            }
            catch
            {
                MessageBox.Show("Morate obeleziti red koji zelite da menjate", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ObrisiUcionicu_Click(object sender, RoutedEventArgs e)
        {
            var listaUcionoca = Data.Instance.Ucionice;

            if (MessageBox.Show($"Da li zelite da obrisete ucionicu br. {IzabranaUcionica.BrojUcionice} ?", "Brisanje", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                foreach (var ucionica in listaUcionoca)
                {
                    if (ucionica.Id == IzabranaUcionica.Id)
                    {
                        Ucionica.Delete(ucionica);
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

        private void PretraziUcionicu_Click(object sender, RoutedEventArgs e)
        {
            if (cbPretraga.SelectedIndex == 0)
            {
                string brUcionice = tbPretraga.Text;
                viewPretraga = CollectionViewSource.GetDefaultView(Ucionica.PretragaUcionice(brUcionice, Ucionica.TipPretrage.BROJUCIONICE));
                dgUcionica.ItemsSource = viewPretraga;
            }
            else if (cbPretraga.SelectedIndex == 1)
            {
                string brMesta = tbPretraga.Text;
                viewPretraga = CollectionViewSource.GetDefaultView(Ucionica.PretragaUcionice(brMesta, Ucionica.TipPretrage.BROJMESTA));
                dgUcionica.ItemsSource = viewPretraga;
            }
            else if (cbPretraga.SelectedIndex == 2)
            {
                string tipUcionice = tbPretraga.Text;
                viewPretraga = CollectionViewSource.GetDefaultView(Ucionica.PretragaUcionice(tipUcionice, Ucionica.TipPretrage.TIPUCIONICE));
                dgUcionica.ItemsSource = viewPretraga;
            }
            else if (cbPretraga.SelectedIndex == 3)
            {
                string ustanovaId = tbPretraga.Text;
                viewPretraga = CollectionViewSource.GetDefaultView(Ucionica.PretragaUcionice(ustanovaId, Ucionica.TipPretrage.USTANOVAID));
                dgUcionica.ItemsSource = viewPretraga;
            }
        }

        private void SortirajUcionicu_Click(object sender, RoutedEventArgs e)
        {
            if (cbSortiranje.SelectedIndex == 0)
            {
                if (cbSortiraj.SelectedIndex == 0)
                {
                    dgUcionica.Items.SortDescriptions.Clear();
                    dgUcionica.Items.SortDescriptions.Add(new SortDescription("BrojUcionice", ListSortDirection.Descending));
                    dgUcionica.ItemsSource = view;
                }
                else if (cbSortiraj.SelectedIndex == 1)
                {
                    dgUcionica.Items.SortDescriptions.Clear();
                    dgUcionica.Items.SortDescriptions.Add(new SortDescription("BrojUcionice", ListSortDirection.Ascending));
                    dgUcionica.ItemsSource = view;
                }
            }
            else if (cbSortiranje.SelectedIndex == 1)
            {
                if (cbSortiraj.SelectedIndex == 0)
                {
                    dgUcionica.Items.SortDescriptions.Clear();
                    dgUcionica.Items.SortDescriptions.Add(new SortDescription("BrojMesta", ListSortDirection.Descending));
                    dgUcionica.ItemsSource = view;
                }
                else if (cbSortiraj.SelectedIndex == 1)
                {
                    dgUcionica.Items.SortDescriptions.Clear();
                    dgUcionica.Items.SortDescriptions.Add(new SortDescription("BrojMesta", ListSortDirection.Ascending));
                    dgUcionica.ItemsSource = view;
                }
            }
            else if (cbSortiranje.SelectedIndex == 2)
            {
                if (cbSortiraj.SelectedIndex == 0)
                {
                    dgUcionica.Items.SortDescriptions.Clear();
                    dgUcionica.Items.SortDescriptions.Add(new SortDescription("TipUcionice", ListSortDirection.Descending));
                    dgUcionica.ItemsSource = view;
                }
                else if (cbSortiraj.SelectedIndex == 1)
                {
                    dgUcionica.Items.SortDescriptions.Clear();
                    dgUcionica.Items.SortDescriptions.Add(new SortDescription("TipUcionice", ListSortDirection.Ascending));
                    dgUcionica.ItemsSource = view;
                }
            }
            else if (cbSortiranje.SelectedIndex == 3)
            {
                if (cbSortiraj.SelectedIndex == 0)
                {
                    dgUcionica.Items.SortDescriptions.Clear();
                    dgUcionica.Items.SortDescriptions.Add(new SortDescription("UstanovaId", ListSortDirection.Descending));
                    dgUcionica.ItemsSource = view;
                }
                else if (cbSortiraj.SelectedIndex == 1)
                {
                    dgUcionica.Items.SortDescriptions.Clear();
                    dgUcionica.Items.SortDescriptions.Add(new SortDescription("UstanovaId", ListSortDirection.Ascending));
                    dgUcionica.ItemsSource = view;
                }
            }
        }

    }
}

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
using static SF24_2016_POP2019.Model.Ucionica;

namespace SF24_2016_POP2019.UI
{
    /// <summary>
    /// Interaction logic for IzmeniUcioniceWindow.xaml
    /// </summary>
    public partial class IzmeniUcioniceWindow : Window
    {
        private ICollectionView viewUstanova;
        public enum Operacija
        {
            DODAVANJE,
            IZMENA
        };

        private Operacija operacija;
        private Ucionica ucionica;

        public IzmeniUcioniceWindow(Ucionica ucionica, Operacija operacija)
        {
            InitializeComponent();
            viewUstanova = CollectionViewSource.GetDefaultView(Data.Instance.Ustanove);

            this.operacija = operacija;
            this.ucionica = ucionica;

            cbUstanova.ItemsSource = viewUstanova;
            cbTipUcionice.ItemsSource = Enum.GetValues(typeof(ETipUcionice)).Cast<ETipUcionice>();

            tbBrUcionice.DataContext = ucionica;
            tbBrMesta.DataContext = ucionica;
            cbTipUcionice.DataContext = ucionica;
            cbUstanova.DataContext = ucionica;
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

                    Ucionica u = new Ucionica()
                    {
                        BrojUcionice = int.Parse(tbBrUcionice.Text),
                        BrojMesta = int.Parse(tbBrMesta.Text),
                        TipUcionice = (ETipUcionice)cbTipUcionice.SelectedIndex,
                        UstanovaId = cbUstanova.SelectedIndex + 1
                    };
                    Ucionica.Create(u);
                    break;

                case Operacija.IZMENA:
                    ucionica.BrojUcionice = int.Parse(tbBrUcionice.Text);
                    ucionica.BrojMesta = int.Parse(tbBrMesta.Text);
                    ucionica.TipUcionice = (ETipUcionice)cbTipUcionice.SelectedIndex;
                    ucionica.UstanovaId = cbUstanova.SelectedIndex + 1;
                    Ucionica.Update(ucionica);
                    break;
            }
            Close();
        }
    }
}

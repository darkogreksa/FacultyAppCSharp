using SF24_2016_POP2019.Model;
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
using System.Windows.Shapes;

namespace SF24_2016_POP2019.UI
{
    /// <summary>
    /// Interaction logic for IzmeniUstanoveWindow.xaml
    /// </summary>
    public partial class IzmeniUstanoveWindow : Window
    {
        public enum Operacija
        {
            DODAVANJE,
            IZMENA
        };

        private Operacija operacija;
        private Ustanova ustanova;

        public IzmeniUstanoveWindow(Ustanova ustanova, Operacija operacija)
        {
            InitializeComponent();

            this.operacija = operacija;
            this.ustanova = ustanova;

            tbSifra.DataContext = ustanova;
            tbNaziv.DataContext = ustanova;
            tbLokacija.DataContext = ustanova;
        }

        private void Sacuvaj_Click(object sender, RoutedEventArgs e)
        {
            switch (operacija)
            {
                case Operacija.DODAVANJE:
                    Ustanova u = new Ustanova()
                    {
                        SifraUstanove = tbSifra.Text,
                        Naziv = tbNaziv.Text,
                        Lokacija = tbLokacija.Text
                    };
                    Ustanova.Create(u);
                    break;

                case Operacija.IZMENA:
                    ustanova.SifraUstanove = tbSifra.Text;
                    ustanova.Naziv = tbNaziv.Text;
                    ustanova.Lokacija = tbLokacija.Text;
                    Ustanova.Update(ustanova);
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

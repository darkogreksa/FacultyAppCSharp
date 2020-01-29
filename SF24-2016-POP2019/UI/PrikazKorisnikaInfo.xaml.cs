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
    /// Interaction logic for PrikazKorisnikaInfo.xaml
    /// </summary>
    public partial class PrikazKorisnikaInfo : Window
    {
        private Korisnik korisnik;

        public PrikazKorisnikaInfo(Korisnik k)
        {
            this.korisnik = k;
            InitializeComponent();
            tbKorisnikInfo.Text = KorisnikInfo();
        }

        private string KorisnikInfo()
        {
            string tekst = "";
            string linija = new String('-', 44);

            tekst += linija +
                    "Ime: " + korisnik.Ime + "\n" +
                     linija + "\n" +
                "Prezime: " + korisnik.Prezime + "\n" +
                     linija + "\n" +
                "Email: " + korisnik.Email + "\n" +
                linija + "\n" +
                "Korisnicko ime: " + korisnik.Username + "\n" +
                linija + "\n" +
                "Tip korisnika: " + korisnik.TipKorisnika + "\n" +
                linija + "\n";

            return tekst;
        }
    }
}

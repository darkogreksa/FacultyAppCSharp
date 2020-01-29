using SF24_2016_POP2019.Database;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SF24_2016_POP2019.Model
{
    public class Termin : INotifyPropertyChanged
    {

        private int id;
        public int Id
        {
            get { return id; }
            set { id = value; OnPropertyChanged("Id"); }
        }

        public enum ETipNastave
        {
            Predavanje,
            Vezbe
        }

        private ETipNastave tipNastave;

        public ETipNastave TipNastave
        {
            get { return tipNastave; }
            set { tipNastave = value; OnPropertyChanged("TipNastave"); }
        }

        private DateTime vremeZauzecaOd;

        public DateTime VremeZauzecaOd
        {
            get { return vremeZauzecaOd; }
            set { vremeZauzecaOd = value; OnPropertyChanged("VremeZauzecaOd"); }
        }

        private DateTime vremeZauzecaDo;

        public DateTime VremeZauzecaDo
        {
            get { return vremeZauzecaDo; }
            set { vremeZauzecaDo = value; OnPropertyChanged("VremeZauzecaDo"); }
        }

        private string dan;

        public string Dan
        {
            get { return dan; }
            set { dan = value; OnPropertyChanged("Dan"); }
        }

        private int korisnikId;

        public int KorisnikId
        {
            get { return korisnikId; }
            set { korisnikId = value; OnPropertyChanged("KorisnikId"); }
        }

        private Korisnik korisnik;

        public Korisnik Korisnik
        {
            get
            {
                if (korisnik == null)
                {
                    korisnik = Korisnik.GetKorisnik(KorisnikId);
                }
                return korisnik;
            }
            set
            {
                try
                {
                    korisnik = value;
                    KorisnikId = korisnik.Id;
                    OnPropertyChanged("Korisnik");
                }
                catch (Exception)
                {

                    MessageBox.Show("Greska");
                }
            }
        }


        private bool obrisano;

        public bool Obrisano
        {
            get { return obrisano; }
            set { obrisano = value; OnPropertyChanged("Obrisano"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        //Ispisuje korisnikovo ime i prezime umesto id-a
        public override string ToString()
        {
            return $"{Korisnik.GetKorisnik(korisnikId)?.Ime} {Korisnik.GetKorisnik(korisnikId)?.Prezime}\n";
        }

        public object Clone()
        {
            return new Termin()
            {
                Id = id,
                TipNastave = tipNastave,
                VremeZauzecaOd = vremeZauzecaOd,
                VremeZauzecaDo = vremeZauzecaDo,
                Dan = dan,
                KorisnikId = korisnikId,
                Obrisano = obrisano
            };
        }
        
        #region CRUD

        public static ObservableCollection<Termin> GetAll()
        {
            var termini = new ObservableCollection<Termin>();
            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["POP"].ConnectionString))
            {
                SqlCommand cmd = conn.CreateCommand();
                SqlDataAdapter da = new SqlDataAdapter();
                cmd.CommandText = "SELECT * FROM Termin WHERE Obrisano = 0;";
                da.SelectCommand = cmd;
                DataSet ds = new DataSet();
                da.Fill(ds, "Termin"); // Izvrsavanje upita

                foreach (DataRow row in ds.Tables["Termin"].Rows)
                {
                    var t = new Termin();
                    t.Id = int.Parse(row["Id"].ToString());
                    t.TipNastave = (ETipNastave)Enum.Parse(typeof(ETipNastave), (row["TipNastave"].ToString()));
                    t.VremeZauzecaOd = DateTime.Parse(row["VremeZauzecaOd"].ToString());
                    t.VremeZauzecaDo = DateTime.Parse(row["VremeZauzecaDo"].ToString());
                    t.Dan = row["Dan"].ToString();
                    t.KorisnikId = int.Parse(row["KorisnikId"].ToString());
                    t.Obrisano = bool.Parse(row["Obrisano"].ToString());

                    termini.Add(t);
                }
            }
            return termini;
        }

        public static Termin Create(Termin t)
        {
            try
            {
                using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["POP"].ConnectionString))
                {
                    conn.Open();

                    SqlCommand cmd = conn.CreateCommand();

                    cmd.CommandText = "INSERT INTO Termin (TipNastave,VremeZauzecaOd,VremeZauzecaDo,Dan,KorisnikId,Obrisano) VALUES (@TipNastave,@VremeZauzecaOd,@VremeZauzecaDo,@Dan,@KorisnikId,@Obrisano);";
                    cmd.CommandText += "SELECT SCOPE_IDENTITY()";

                    cmd.Parameters.AddWithValue("@TipNastave", t.TipNastave.ToString());
                    cmd.Parameters.AddWithValue("@VremeZauzecaOd", t.VremeZauzecaOd);
                    cmd.Parameters.AddWithValue("@VremeZauzecaDo", t.VremeZauzecaDo);
                    cmd.Parameters.AddWithValue("@Dan", t.Dan);
                    cmd.Parameters.AddWithValue("@KorisnikId", t.KorisnikId);
                    cmd.Parameters.AddWithValue("@Obrisano", t.Obrisano);

                    t.Id = int.Parse(cmd.ExecuteScalar().ToString()); //executeScalar izvrsava upit
                }

                Data.Instance.Termini.Add(t);
                return t;
            }
            catch (Exception)
            {
                MessageBox.Show("Upis u bazu nije uspeo.\n Pokusajte ponovo.", "Greska", MessageBoxButton.OK, MessageBoxImage.Warning);
                return null;
            }
        }

        public static void Update(Termin t)
        {
            try
            {
                using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["POP"].ConnectionString))
                {
                    conn.Open();

                    SqlCommand cmd = conn.CreateCommand();

                    cmd.CommandText = "UPDATE Termin SET TipNastave = @TipNastave,VremeZauzecaOd = @VremeZauzecaOd, VremeZauzecaDo = @VremeZauzecaDo, Dan = @Dan, KorisnikId = @KorisnikId, Obrisano= @Obrisano WHERE Id = @Id";
                    cmd.Parameters.AddWithValue("@Id", t.Id);
                    cmd.Parameters.AddWithValue("@TipNastave", t.TipNastave.ToString());
                    cmd.Parameters.AddWithValue("@VremeZauzecaOd", t.VremeZauzecaOd);
                    cmd.Parameters.AddWithValue("@VremeZauzecaDo", t.VremeZauzecaDo);
                    cmd.Parameters.AddWithValue("@Dan", t.Dan);
                    cmd.Parameters.AddWithValue("@KorisnikId", t.KorisnikId);
                    cmd.Parameters.AddWithValue("@Obrisano", t.Obrisano);
                    cmd.ExecuteNonQuery();
                }
                //azuriranje modela
                foreach (var termin in Data.Instance.Termini)
                {
                    if (t.Id == termin.Id)
                    {
                        termin.TipNastave = t.TipNastave;
                        termin.VremeZauzecaOd = t.VremeZauzecaOd;
                        termin.VremeZauzecaDo = t.VremeZauzecaDo;
                        termin.Dan = t.Dan;
                        termin.KorisnikId = t.KorisnikId;
                        termin.Obrisano = t.Obrisano;
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Upis u bazu nije uspeo.\n Molim da pokusate ponovo!", "Greska", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        public static void Delete(Termin t)
        {
            t.Obrisano = true;
            Update(t);
        }

        #endregion

        #region SEARCH

        public enum TipPretrage
        {
            TIPNASTAVE,
            VREMEZAUZECAOD,
            VREMEZAUZECADO,
            DAN,
            KORISNIKID
        }

        public static ObservableCollection<Termin> PretragaTermina(string unos, TipPretrage tipPretrage)
        {
            var termini = new ObservableCollection<Termin>();

            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["POP"].ConnectionString))
            {
                SqlCommand cmd = conn.CreateCommand();
                SqlDataAdapter da = new SqlDataAdapter();

                switch (tipPretrage)
                {
                    case TipPretrage.TIPNASTAVE:
                        cmd.CommandText = "SELECT * FROM Termin WHERE TipNastave LIKE @unos AND Obrisano = 0;";
                        break;

                    case TipPretrage.VREMEZAUZECAOD:
                        cmd.CommandText = "SELECT * FROM Termin WHERE VremeZauzecaOd LIKE @unos AND Obrisano = 0;";
                        break;
                    case TipPretrage.VREMEZAUZECADO:
                        cmd.CommandText = "SELECT * FROM Termin WHERE VremeZauzecaDo LIKE @unos AND Obrisano = 0;";
                        break;
                    case TipPretrage.DAN:
                        cmd.CommandText = "SELECT * FROM Termin WHERE Dan LIKE @unos AND Obrisano = 0;";
                        break;
                    case TipPretrage.KORISNIKID:
                        cmd.CommandText = "SELECT * FROM Termin WHERE KorisnikId LIKE @unos AND Obrisano = 0;";
                        break;
                }
                cmd.Parameters.AddWithValue("unos", "%" + unos.Trim() + "%");
                da.SelectCommand = cmd;
                DataSet ds = new DataSet();
                da.Fill(ds, "Termin");

                foreach (DataRow row in ds.Tables["Termin"].Rows)
                {
                    var t = new Termin();
                    t.Id = int.Parse(row["Id"].ToString());
                    t.TipNastave = (ETipNastave)Enum.Parse(typeof(ETipNastave), (row["TipNastave"].ToString()));
                    t.VremeZauzecaOd = DateTime.Parse(row["VremeZauzecaOd"].ToString());
                    t.VremeZauzecaDo = DateTime.Parse(row["VremeZauzecaDo"].ToString());
                    t.Dan = row["Dan"].ToString();
                    t.KorisnikId = int.Parse(row["KorisnikId"].ToString());
                    t.Obrisano = bool.Parse(row["Obrisano"].ToString());

                    termini.Add(t);
                }
            }
            return termini;
        }

        #endregion
        
    }
}

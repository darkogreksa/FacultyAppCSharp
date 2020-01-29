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
    public class Korisnik : INotifyPropertyChanged, IDataErrorInfo
    {
        private int id;
        public int Id
        {
            get { return id; }
            set { id = value; OnPropertyChanged("Id"); }
        }

        private string ime;

        public string Ime
        {
            get { return ime; }
            set { ime = value; OnPropertyChanged("Ime"); }
        }

        private string prezime;

        public string Prezime
        {
            get { return prezime; }
            set { prezime = value; OnPropertyChanged("Prezime"); }
        }

        private string email;

        public string Email
        {
            get { return email; }
            set { email = value; OnPropertyChanged("Email"); }
        }

        private string username;

        public string Username
        {
            get { return username; }
            set { username = value; OnPropertyChanged("Username"); }
        }

        private string password;

        public string Password
        {
            get { return password; }
            set { password = value; OnPropertyChanged("Password"); }
        }

        public enum ETipKorisnika
        {
            Administrator,
            Profesor,
            Asistent
        }

        private ETipKorisnika tipKorisnika;

        public ETipKorisnika TipKorisnika
        {
            get { return tipKorisnika; }
            set { tipKorisnika = value; OnPropertyChanged("TipKorisnika"); }
        }


        private int ustanovaId;

        public int UstanovaId
        {
            get { return ustanovaId; }
            set { ustanovaId = value; OnPropertyChanged("UstanovaId"); }
        }

        private bool obrisano;

        public bool Obrisano
        {
            get { return obrisano; }
            set { obrisano = value; OnPropertyChanged("Obrisano"); }
        }


        private Ustanova ustanova;

        public Ustanova Ustanova
        {
            get
            {
                if (ustanova == null)
                {
                    ustanova = Ustanova.GetById(UstanovaId);
                }
                return ustanova;
            }
            set
            {
                try
                {
                    ustanova = value;
                    UstanovaId = ustanova.Id;
                    OnPropertyChanged("Ustanova");
                }
                catch (Exception)
                {

                    MessageBox.Show("Greska");
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public object Clone()
        {
            return new Korisnik()
            {
                Id = id,
                Ime = ime,
                Prezime = prezime,
                Email = email,
                Username = username,
                Password = password,
                TipKorisnika = tipKorisnika,
                UstanovaId = ustanovaId,
                Obrisano = obrisano
            };
        }

        //Preko korisnikovog id-a vraca 'korisnik' kao objekat
        public static Korisnik GetKorisnik(int id)
        {
            foreach(var korisnik in Data.Instance.Korisnici)
            {
                if(korisnik.Id == id)
                {
                    return korisnik;
                }
            }
            return null;
        }

        //Ispisuje ime i prezime umesto korisnikovog id-a i naziv ustanove umesto ustanovaId
        public override string ToString()
        {
            return $"{Ime} {Prezime}, {Ustanova.GetById(UstanovaId)?.Naziv}\n";
        }


        #region CRUD

        public static ObservableCollection<Korisnik> GetAll()
        {
            var korisnici = new ObservableCollection<Korisnik>();
            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["POP"].ConnectionString))
            {
                SqlCommand cmd = conn.CreateCommand();
                SqlDataAdapter da = new SqlDataAdapter();
                cmd.CommandText = "SELECT * FROM Korisnik WHERE Obrisano = 0;";
                da.SelectCommand = cmd;
                DataSet ds = new DataSet();
                da.Fill(ds, "Korisnik"); // Izvrsavanje upita

                foreach (DataRow row in ds.Tables["Korisnik"].Rows)
                {
                    var k = new Korisnik();
                    k.Id = int.Parse(row["Id"].ToString());
                    k.Ime = row["Ime"].ToString();
                    k.Prezime = row["Prezime"].ToString();
                    k.Email = row["Email"].ToString();
                    k.Username = row["Username"].ToString();
                    k.Password = row["Password"].ToString();
                    k.TipKorisnika = (ETipKorisnika)Enum.Parse(typeof(ETipKorisnika), (row["TipKorisnika"].ToString()));
                    k.UstanovaId = int.Parse(row["UstanovaId"].ToString());
                    k.Obrisano = bool.Parse(row["Obrisano"].ToString());

                    korisnici.Add(k);
                }
            }
            return korisnici;
        }

        public static Korisnik Create(Korisnik k)
        {
            try
            {
                using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["POP"].ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();

                    cmd.CommandText = "INSERT INTO Korisnik (Ime,Prezime,Email,Username,Password,TipKorisnika,UstanovaId,Obrisano) VALUES (@Ime,@Prezime,@Email,@Username,@Password,@TipKorisnika,@UstanovaId,@Obrisano);";
                    cmd.CommandText += "SELECT SCOPE_IDENTITY()";

                    cmd.Parameters.AddWithValue("@Ime", k.Ime);
                    cmd.Parameters.AddWithValue("@Prezime", k.Prezime);
                    cmd.Parameters.AddWithValue("@Email", k.Email);
                    cmd.Parameters.AddWithValue("@Username", k.Username);
                    cmd.Parameters.AddWithValue("@Password", k.Password);
                    cmd.Parameters.AddWithValue("@TipKorisnika", k.TipKorisnika.ToString());
                    cmd.Parameters.AddWithValue("@UstanovaId", k.UstanovaId);
                    cmd.Parameters.AddWithValue("@Obrisano", k.Obrisano);

                    k.Id = int.Parse(cmd.ExecuteScalar().ToString());
                    //cmd.ExecuteNonQuery();
                }

                Data.Instance.Korisnici.Add(k);
                return k;
            }
            catch (Exception)
            {
                MessageBox.Show("Upis u bazu nije uspeo.\n Pokusajte ponovo.", "Greska", MessageBoxButton.OK, MessageBoxImage.Warning);
                return null;
            }
        }

        public static void Update(Korisnik k)
        {
            try
            {
                using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["POP"].ConnectionString))
                {
                    conn.Open();

                    SqlCommand cmd = conn.CreateCommand();

                    cmd.CommandText = "UPDATE Korisnik SET Ime = @Ime,Prezime = @Prezime, Email = @Email, Username = @Username, Password = @Password, TipKorisnika = @TipKorisnika, UstanovaId = @UstanovaId,Obrisano= @Obrisano WHERE Id = @Id";
                    cmd.Parameters.AddWithValue("@Id", k.Id);
                    cmd.Parameters.AddWithValue("@Ime", k.Ime);
                    cmd.Parameters.AddWithValue("@Prezime", k.Prezime);
                    cmd.Parameters.AddWithValue("@Email", k.Email);
                    cmd.Parameters.AddWithValue("@Username", k.Username);
                    cmd.Parameters.AddWithValue("@Password", k.Password);
                    cmd.Parameters.AddWithValue("@TipKorisnika", k.TipKorisnika.ToString());
                    cmd.Parameters.AddWithValue("@UstanovaId", k.UstanovaId);
                    cmd.Parameters.AddWithValue("@Obrisano", k.Obrisano);
                    cmd.ExecuteNonQuery();
                }
                //azuriranje modela
                foreach (var korisnik in Data.Instance.Korisnici)
                {
                    if (k.Id == korisnik.Id)
                    {
                        korisnik.Ime = k.Ime;
                        korisnik.Prezime = k.Prezime;
                        korisnik.Email = k.Email;
                        korisnik.Username = k.Username;
                        korisnik.Password = k.Password;
                        korisnik.TipKorisnika = k.TipKorisnika;
                        korisnik.UstanovaId = k.UstanovaId;
                        korisnik.Obrisano = k.Obrisano;
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Upis u bazu nije uspeo.\n Pokusajte ponovo!", "Greska", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        public static void Delete(Korisnik k)
        {
            k.Obrisano = true;
            Update(k);
        }

        #endregion

        #region SEARCH

        public enum TipPretrage
        {
            IME,
            PREZIME,
            EMAIL,
            USERNAME,
            TIPKORISNIKA,
            USTANOVAID
        }

        public static ObservableCollection<Korisnik> PretragaKorisnika(string unos, TipPretrage tipPretrage)
        {
            var korisnici = new ObservableCollection<Korisnik>();

            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["POP"].ConnectionString))
            {
                SqlCommand cmd = conn.CreateCommand();
                SqlDataAdapter da = new SqlDataAdapter();

                switch (tipPretrage)
                {
                    case TipPretrage.IME:
                        cmd.CommandText = "SELECT * FROM Korisnik WHERE Ime LIKE @unos AND Obrisano = 0;";
                        break;
                    case TipPretrage.PREZIME:
                        cmd.CommandText = "SELECT * FROM Korisnik WHERE Prezime LIKE @unos AND Obrisano = 0;";
                        break;
                    case TipPretrage.EMAIL:
                        cmd.CommandText = "SELECT * FROM Korisnik WHERE Email LIKE @unos AND Obrisano = 0;";
                        break;
                    case TipPretrage.USERNAME:
                        cmd.CommandText = "SELECT * FROM Korisnik WHERE Username LIKE @unos AND Obrisano = 0;";
                        break;
                    case TipPretrage.TIPKORISNIKA:
                        cmd.CommandText = "SELECT * FROM Korisnik WHERE TipKorisnika LIKE @unos AND Obrisano = 0;";
                        break;
                    case TipPretrage.USTANOVAID:
                        cmd.CommandText = "SELECT * FROM Korisnik WHERE UstanovaId LIKE @unos AND Obrisano = 0;";
                        break;
                }
                cmd.Parameters.AddWithValue("unos", "%" + unos.Trim() + "%");
                da.SelectCommand = cmd;
                DataSet ds = new DataSet();
                da.Fill(ds, "Korisnik");

                foreach (DataRow row in ds.Tables["Korisnik"].Rows)
                {
                    var k = new Korisnik();
                    k.Id = int.Parse(row["Id"].ToString());
                    k.Ime = row["Ime"].ToString();
                    k.Prezime = row["Prezime"].ToString();
                    k.Email = row["Email"].ToString();
                    k.Username = row["Username"].ToString();
                    k.Password = row["Password"].ToString();
                    k.TipKorisnika = (ETipKorisnika)Enum.Parse(typeof(ETipKorisnika), (row["TipKorisnika"].ToString()));
                    k.UstanovaId = int.Parse(row["UstanovaId"].ToString());
                    k.Obrisano = bool.Parse(row["Obrisano"].ToString());

                    korisnici.Add(k);
                }
            }
            return korisnici;
        }

        #endregion

        #region VALIDATION

        public string Error
        {
            get
            {
                return "Neispravni podaci o korisniku";
            }
        }

        public string this[string propertyName]
        {
            get
            {
                switch (propertyName)
                {
                    case "Ime":
                        if (string.IsNullOrEmpty(Ime))
                            return "Polje ne sme biti prazno";
                        break;
                    case "Prezime":
                        if (string.IsNullOrEmpty(Prezime))
                        {
                            return "Polje ne sme biti prazno";
                        }
                        break;
                    case "Email":
                        if (string.IsNullOrEmpty(Email))
                        {
                            return "Polje ne sme biti prazno";
                        }
                        break;
                    case "Username":
                        if (string.IsNullOrEmpty(Username))
                        {
                            return "Polje ne sme biti prazno";
                        }
                        break;
                    case "Password":
                        if (string.IsNullOrEmpty(Password))
                        {
                            return "Polje ne sme biti prazno";
                        }
                        break;
                }
                return "";
            }
        }

        #endregion
    }
}

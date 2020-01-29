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
    public class Ustanova : INotifyPropertyChanged, IDataErrorInfo
    {

        private int id;
        public int Id
        {
            get { return id; }
            set { id = value; OnPropertyChanged("Id"); }
        }

        private string sifraUstanove;

        public string SifraUstanove
        {
            get { return sifraUstanove; }
            set { sifraUstanove = value; OnPropertyChanged("SifraUstanove"); }
        }

        private string naziv;

        public string Naziv
        {
            get { return naziv; }
            set { naziv = value; OnPropertyChanged("Naziv"); }
        }

        private string lokacija;

        public string Lokacija
        {
            get { return lokacija; }
            set { lokacija = value; OnPropertyChanged("Lokacija"); }
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

        public override string ToString()
        {
            return $"{Naziv} \n";
        }

        public static Ustanova GetById(int id)
        {
            foreach (var ustanova in Data.Instance.Ustanove)
            {
                if (ustanova.Id == id)
                {
                    return ustanova;
                }
            }
            return null;
        }


        public object Clone()
        {
            return new Ustanova()
            {
                Id = id,
                SifraUstanove = sifraUstanove,
                Naziv = naziv,
                Lokacija = lokacija,
                Obrisano = obrisano
            };
        }

        #region CRUD

        public static ObservableCollection<Ustanova> GetAll()
        {
            var ustanove = new ObservableCollection<Ustanova>();
            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["POP"].ConnectionString))
            {
                SqlCommand cmd = conn.CreateCommand();
                SqlDataAdapter da = new SqlDataAdapter();
                cmd.CommandText = "SELECT * FROM Ustanova WHERE Obrisano = 0;";
                da.SelectCommand = cmd;
                DataSet ds = new DataSet();
                da.Fill(ds, "Ustanova"); // Izvrsavanje upita

                foreach (DataRow row in ds.Tables["Ustanova"].Rows)
                {
                    var u = new Ustanova();
                    u.Id = int.Parse(row["Id"].ToString());
                    u.SifraUstanove = row["SifraUstanove"].ToString();
                    u.Naziv = row["Naziv"].ToString();
                    u.Lokacija = row["Lokacija"].ToString();
                    u.Obrisano = bool.Parse(row["Obrisano"].ToString());

                    ustanove.Add(u);
                }
            }
            return ustanove;
        }

        public static Ustanova Create(Ustanova u)
        {
            try
            {
                using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["POP"].ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();

                    cmd.CommandText = "INSERT INTO Ustanova (SifraUstanove,Naziv,Lokacija,Obrisano) VALUES (@SifraUstanove,@Naziv,@Lokacija,@Obrisano);";
                    cmd.CommandText += "SELECT SCOPE_IDENTITY()";

                    cmd.Parameters.AddWithValue("@SifraUstanove", u.SifraUstanove);
                    cmd.Parameters.AddWithValue("@Naziv", u.Naziv);
                    cmd.Parameters.AddWithValue("@Lokacija", u.Lokacija);
                    cmd.Parameters.AddWithValue("@Obrisano", u.Obrisano);

                    u.Id = int.Parse(cmd.ExecuteScalar().ToString());
                    //cmd.ExecuteNonQuery();
                }

                Data.Instance.Ustanove.Add(u);
                return u;
            }
            catch (Exception)
            {
                MessageBox.Show("Upis u bazu nije uspeo.\n Pokusajte ponovo.", "Greska", MessageBoxButton.OK, MessageBoxImage.Warning);
                return null;
            }
        }

        public static void Update(Ustanova u)
        {
            try
            {
                using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["POP"].ConnectionString))
                {
                    conn.Open();

                    SqlCommand cmd = conn.CreateCommand();

                    cmd.CommandText = "UPDATE Ustanova SET SifraUstanove = @SifraUstanove,Naziv = @Naziv, Lokacija = @Lokacija, Obrisano= @Obrisano WHERE Id = @Id";
                    cmd.Parameters.AddWithValue("@Id", u.Id);
                    cmd.Parameters.AddWithValue("@SifraUstanove", u.SifraUstanove);
                    cmd.Parameters.AddWithValue("@Naziv", u.Naziv);
                    cmd.Parameters.AddWithValue("@Lokacija", u.Lokacija);
                    cmd.Parameters.AddWithValue("@Obrisano", u.Obrisano);
                    cmd.ExecuteNonQuery();
                }
                //azuriranje modela
                foreach (var ustanova in Data.Instance.Ustanove)
                {
                    if (u.Id == ustanova.Id)
                    {
                        ustanova.SifraUstanove = u.SifraUstanove;
                        ustanova.Naziv = u.Naziv;
                        ustanova.Lokacija = u.Lokacija;
                        ustanova.Obrisano = u.Obrisano;
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Upis u bazu nije uspeo.\n Pokusajte ponovo!", "Greska", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        public static void Delete(Ustanova u)
        {
            u.Obrisano = true;
            Update(u);
        }

        #endregion

        #region SEARCH

        public enum TipPretrage
        {
            SIFRAUSTANOVE,
            NAZIV,
            LOKACIJA
        }

        public static ObservableCollection<Ustanova> PretragaUstanove(string unos, TipPretrage tipPretrage)
        {
            var ustanove = new ObservableCollection<Ustanova>();

            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["POP"].ConnectionString))
            {
                SqlCommand cmd = conn.CreateCommand();
                SqlDataAdapter da = new SqlDataAdapter();

                switch (tipPretrage)
                {
                    case TipPretrage.SIFRAUSTANOVE:
                        cmd.CommandText = "SELECT * FROM Ustanova WHERE SifraUstanove LIKE @unos AND Obrisano = 0;";
                        break;
                    case TipPretrage.NAZIV:
                        cmd.CommandText = "SELECT * FROM Ustanova WHERE Naziv LIKE @unos AND Obrisano = 0;";
                        break;
                    case TipPretrage.LOKACIJA:
                        cmd.CommandText = "SELECT * FROM Ustanova WHERE Lokacija LIKE @unos AND Obrisano = 0;";
                        break;
                }
                cmd.Parameters.AddWithValue("unos", "%" + unos.Trim() + "%");
                da.SelectCommand = cmd;
                DataSet ds = new DataSet();
                da.Fill(ds, "Ustanova");

                foreach (DataRow row in ds.Tables["Ustanova"].Rows)
                {
                    var u = new Ustanova();
                    u.Id = int.Parse(row["Id"].ToString());
                    u.SifraUstanove = row["SifraUstanove"].ToString();
                    u.Naziv = row["Naziv"].ToString();
                    u.Lokacija = row["Lokacija"].ToString();
                    u.Obrisano = bool.Parse(row["Obrisano"].ToString());

                    ustanove.Add(u);
                }
            }
            return ustanove;
        }

        #endregion

        #region VALIDATION

        public string Error
        {
            get
            {
                return "Neispravni podaci o ustanovi";
            }
        }

        public string this[string propertyName]
        {
            get
            {
                switch (propertyName)
                {
                    case "SifraUstanove":
                        if (string.IsNullOrEmpty(SifraUstanove))
                            return "Polje ne sme biti prazno";
                        break;
                    case "Naziv":
                        if (string.IsNullOrEmpty(Naziv))
                        {
                            return "Polje ne sme biti prazno";
                        }
                        break;
                    case "Lokacija":
                        if (string.IsNullOrEmpty(Lokacija))
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

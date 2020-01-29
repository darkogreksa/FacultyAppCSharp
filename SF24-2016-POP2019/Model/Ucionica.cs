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
    public class Ucionica : INotifyPropertyChanged, IDataErrorInfo
    {
        private int id;
        public int Id
        {
            get { return id; }
            set { id = value; OnPropertyChanged("Id"); }
        }

        private int brojUcionice;

        public int BrojUcionice
        {
            get { return brojUcionice; }
            set { brojUcionice = value; OnPropertyChanged("BrojUcionice"); }
        }

        private int brojMesta;

        public int BrojMesta
        {
            get { return brojMesta; }
            set { brojMesta = value; OnPropertyChanged("BrojMesta"); }
        }

        public enum ETipUcionice
        {
            SaRacunarima,
            BezRacunara
        }

        private ETipUcionice tipUcionice;

        public ETipUcionice TipUcionice
        {
            get { return tipUcionice; }
            set { tipUcionice = value; OnPropertyChanged("TipUcionice"); }
        }

        private bool obrisano;

        public bool Obrisano
        {
            get { return obrisano; }
            set { obrisano = value; OnPropertyChanged("Obrisano"); }
        }

        private int ustanovaId;

        public int UstanovaId
        {
            get { return ustanovaId; }
            set { ustanovaId = value; OnPropertyChanged("UstanovaId"); }
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

        public static Ucionica GetById(int id)
        {
            foreach (var ucionica in Data.Instance.Ucionice)
            {
                if (ucionica.Id == id)
                {
                    return ucionica;
                }
            }
            return null;
        }

        public object Clone()
        {
            return new Ucionica()
            {
                Id = id,
                BrojUcionice = brojUcionice,
                BrojMesta = brojMesta,
                TipUcionice = tipUcionice,
                Obrisano = obrisano,
                UstanovaId = ustanovaId
            };
        }


        //Ispisuje naziv ustanove umesto ustanovaId
        public override string ToString()
        {
            return $"{Ustanova.GetById(UstanovaId)?.Naziv}\n";
        }

        #region CRUD

        public static ObservableCollection<Ucionica> GetAll()
        {
            var ucionice = new ObservableCollection<Ucionica>();
            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["POP"].ConnectionString))
            {
                SqlCommand cmd = conn.CreateCommand();
                SqlDataAdapter da = new SqlDataAdapter();
                cmd.CommandText = "SELECT * FROM Ucionica WHERE Obrisano = 0;";
                da.SelectCommand = cmd;
                DataSet ds = new DataSet();
                da.Fill(ds, "Ucionica"); // Izvrsavanje upita

                foreach (DataRow row in ds.Tables["Ucionica"].Rows)
                {
                    var u = new Ucionica();
                    u.Id = int.Parse(row["Id"].ToString());
                    u.BrojUcionice = int.Parse(row["BrojUcionice"].ToString());
                    u.BrojMesta = int.Parse(row["BrojMesta"].ToString());
                    u.TipUcionice = (ETipUcionice)Enum.Parse(typeof(ETipUcionice), (row["TipUcionice"].ToString()));
                    u.Obrisano = bool.Parse(row["Obrisano"].ToString());
                    u.UstanovaId = int.Parse(row["UstanovaId"].ToString());

                    ucionice.Add(u);
                }
            }
            return ucionice;
        }

        public static Ucionica Create(Ucionica u)
        {
            try
            {
                using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["POP"].ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();

                    cmd.CommandText = "INSERT INTO Ucionica (BrojUcionice,BrojMesta,TipUcionice,Obrisano) VALUES (@BrojUcionice,@BrojMesta,@TipUcionice,@Obrisano);";
                    cmd.CommandText += "SELECT SCOPE_IDENTITY()";

                    cmd.Parameters.AddWithValue("@BrojUcionice", u.BrojUcionice);
                    cmd.Parameters.AddWithValue("@BrojMesta", u.BrojMesta);
                    cmd.Parameters.AddWithValue("@TipUcionice", u.TipUcionice.ToString());
                    cmd.Parameters.AddWithValue("@Obrisano", u.Obrisano);
                    cmd.Parameters.AddWithValue("@UstanovaId", u.UstanovaId);

                    u.Id = int.Parse(cmd.ExecuteScalar().ToString());
                    //cmd.ExecuteNonQuery();


                }

                Data.Instance.Ucionice.Add(u);
                return u;
            }
            catch (Exception)
            {
                MessageBox.Show("Upis u bazu nije uspeo.\n Pokusajte ponovo.", "Greska", MessageBoxButton.OK, MessageBoxImage.Warning);
                return null;
            }
        }

        public static void Update(Ucionica u)
        {
            try
            {
                using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["POP"].ConnectionString))
                {
                    conn.Open();

                    SqlCommand cmd = conn.CreateCommand();

                    cmd.CommandText = "UPDATE Ucionica SET BrojUcionice = @BrojUcionice,BrojMesta = @BrojMesta, TipUcionice = @TipUcionice, Obrisano= @Obrisano WHERE Id = @Id";
                    cmd.Parameters.AddWithValue("@Id", u.Id);
                    cmd.Parameters.AddWithValue("@BrojUcionice", u.BrojUcionice);
                    cmd.Parameters.AddWithValue("@BrojMesta", u.BrojMesta);
                    cmd.Parameters.AddWithValue("@TipUcionice", u.TipUcionice.ToString());
                    cmd.Parameters.AddWithValue("@Obrisano", u.Obrisano);
                    cmd.Parameters.AddWithValue("@UstanovaId", u.UstanovaId);

                    cmd.ExecuteNonQuery();
                }
                //azuriranje modela
                foreach (var ucionica in Data.Instance.Ucionice)
                {
                    if (u.Id == ucionica.Id)
                    {
                        ucionica.BrojUcionice = u.BrojUcionice;
                        ucionica.BrojMesta = u.BrojMesta;
                        ucionica.TipUcionice = u.TipUcionice;
                        ucionica.Obrisano = u.Obrisano;
                        ucionica.UstanovaId = u.UstanovaId;
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Upis u bazu nije uspeo.\n Molim da pokusate ponovo!", "Greska", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        public static void Delete(Ucionica u)
        {
            u.Obrisano = true;
            Update(u);
        }

        #endregion

        #region SEARCH

        public enum TipPretrage
        {
            BROJUCIONICE,
            BROJMESTA,
            TIPUCIONICE,
            USTANOVAID
        }

        public static ObservableCollection<Ucionica> PretragaUcionice(string unos, TipPretrage tipPretrage)
        {
            var ucionice = new ObservableCollection<Ucionica>();

            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["POP"].ConnectionString))
            {
                SqlCommand cmd = conn.CreateCommand();
                SqlDataAdapter da = new SqlDataAdapter();

                switch (tipPretrage)
                {
                    case TipPretrage.BROJUCIONICE:
                        cmd.CommandText = "SELECT * FROM Ucionica WHERE BrojUcionice LIKE @unos AND Obrisano = 0;";
                        break;

                    case TipPretrage.BROJMESTA:
                        cmd.CommandText = "SELECT * FROM Ucionica WHERE BrojMesta LIKE @unos AND Obrisano = 0;";
                        break;
                    case TipPretrage.TIPUCIONICE:
                        cmd.CommandText = "SELECT * FROM Ucionica WHERE TipUcionice LIKE @unos AND Obrisano = 0;";
                        break;
                    case TipPretrage.USTANOVAID:
                        cmd.CommandText = "SELECT * FROM Ucionica WHERE UstanovaId LIKE @unos AND Obrisano = 0;";
                        break;
                }
                cmd.Parameters.AddWithValue("unos", "%" + unos.Trim() + "%");
                da.SelectCommand = cmd;
                DataSet ds = new DataSet();
                da.Fill(ds, "Ucionica");

                foreach (DataRow row in ds.Tables["Ucionica"].Rows)
                {
                    var u = new Ucionica();
                    u.Id = int.Parse(row["Id"].ToString());
                    u.BrojUcionice = int.Parse(row["BrojUcionice"].ToString());
                    u.BrojMesta = int.Parse(row["BrojMesta"].ToString());
                    u.TipUcionice = (ETipUcionice)Enum.Parse(typeof(ETipUcionice), (row["TipUcionice"].ToString()));
                    u.Obrisano = bool.Parse(row["Obrisano"].ToString());
                    u.UstanovaId = int.Parse(row["UstanovaId"].ToString());

                    ucionice.Add(u);
                }
            }
            return ucionice;
        }

        #endregion

        #region VALIDATION

        public string Error
        {
            get
            {
                return "Neispravni podaci o ucionici";
            }
        }

        public string this[string propertyName]
        {
            get
            {
                switch (propertyName)
                {
                    case "BrojUcionice":
                        if (BrojUcionice <= 0)
                            return "Polje ne sme biti prazno";
                        break;
                    case "BrojMesta":
                        if (BrojMesta <= 0)
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

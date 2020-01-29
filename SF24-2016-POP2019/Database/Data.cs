using SF24_2016_POP2019.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SF24_2016_POP2019.Database
{
    public class Data
    {

        public ObservableCollection<Ustanova> Ustanove { get; set; }
        public ObservableCollection<Ucionica> Ucionice { get; set; }
        public ObservableCollection<Korisnik> Korisnici { get; set; }
        public ObservableCollection<Termin> Termini { get; set; }

        private static Data _instance = null;
        public static Data Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Data();
                return _instance;
            }
        }

        private Data()
        {
            Ustanove = Ustanova.GetAll();
            Ucionice = Ucionica.GetAll();
            Korisnici = Korisnik.GetAll();
            Termini = Termin.GetAll();
        }

    }
}

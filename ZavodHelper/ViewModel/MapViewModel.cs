using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ZavodHelper
{
    public class MapViewModel : ViewModelBase, IDisposable
    {
        #region Private Members

        private ObservableCollection<Instrument> instruments;

        private Instrument selectedInstrument;

        private Location selectedLocation;

        private string imageMap;

        private int floor;

        private ObservableCollection<Location> table;

        #endregion

        #region Public Members

        public ObservableCollection<Location> Table
        {
            get
            {
                return table;
            }

            set
            {
                if (table == value)
                {
                    return;
                }

                table = value;
                OnPropertyChanged("Table");
            }
        }

        public ObservableCollection<Instrument> Instruments
        {
            get => instruments;
            set
            {
                instruments = value;
                OnPropertyChanged("Instruments");
            }
        }
        public string ImageMap
        {
            get => imageMap;
            set
            {
                imageMap = value;
                OnPropertyChanged("ImageMap");
            }
        }

        public Location SelectedLocation
        {
            get
            {
                return selectedLocation;
            }

            set
            {
                if (selectedLocation == value)
                {
                    return;
                }

                selectedLocation = value;
                OnPropertyChanged("SelectedLocation");
            }
        }
        public Instrument SelectedInstrument
        {
            get
            {
                return selectedInstrument;
            }

            set
            {
                if (selectedInstrument == value)
                {
                    return;
                }

                selectedInstrument = value;
                OnPropertyChanged("SelectedInstrument");
            }
        }

        public int Floor
        {
            get
            {
                return floor;
            }

            set
            {
                if (floor == value)
                {
                    return;
                }

                floor = value;
                OnPropertyChanged("Floor");
            }
        }

        #endregion

        #region Commands

        private RelayCommand addButton;
        public RelayCommand AddButtonCommand
        {
            get
            {
                return addButton ??
                        (addButton = new RelayCommand(x =>
                        {
                            using (ZavodContext db = new ZavodContext())
                            {
                                Location location = new Location();
                                location.Floor = selectedInstrument.Floor;
                                location.InstrumentId = selectedInstrument.IdInstrument;
                                location.NumberPlace = selectedLocation.IdLocation;
                                if (db.Locations.Where(y => y.InstrumentId == selectedInstrument.IdInstrument).FirstOrDefault() != null)
                                {

                                    Location loc = db.Locations.Where(y => y.InstrumentId == selectedInstrument.IdInstrument).FirstOrDefault();

                                    Table.Where(z => z.IdLocation == loc.NumberPlace).FirstOrDefault().Opacity = 0.1;

                                    loc.NumberPlace = location.NumberPlace;

                                    db.Entry<Location>(loc).State = System.Data.Entity.EntityState.Modified;

                                    Table.Where(z => z.IdLocation == loc.NumberPlace).FirstOrDefault().Opacity = 1;
                                }
                                else
                                {
                                    db.Locations.Add(location);
                                    Table.Where(z => z.IdLocation == location.NumberPlace).FirstOrDefault().Opacity = 1;

                                }
                                db.SaveChanges();
                            }
                        },
                        x => SelectedLocation != null && SelectedInstrument != null));
            }
        }

        private RelayCommand infoCommand;
        public RelayCommand InfoCommand
        {
            get
            {
                return infoCommand ??
                        (infoCommand = new RelayCommand(x =>
                        {
                            Table = Singleton.getInstance().table;
                            foreach (var item in Table)
                            {
                                item.Opacity = 0.1;
                            }
                            Singleton.getInstance(null).MainViewModel.CurrentViewModel = new InfoViewModel();
                        }));
            }
        }
        private RelayCommand selectionChangedCommand;
        public RelayCommand SelectionChangedCommand
        {
            get
            {
                return selectionChangedCommand ??
                        (selectionChangedCommand = new RelayCommand(x =>
                        {
                            try
                            {
                                using (ZavodContext db = new ZavodContext())
                                {
                                    Location loc;
                                    if (SelectedInstrument != null)
                                    {
                                        loc = db.Locations.Where(y => y.InstrumentId == SelectedInstrument.IdInstrument).FirstOrDefault();
                                        if (loc != null)
                                            Table.Where(z => z.IdLocation == loc.NumberPlace).FirstOrDefault().Opacity = 0.1;
                                    }
                                    SelectedInstrument = x as Instrument;

                                    loc = db.Locations.Where(y => y.InstrumentId == SelectedInstrument.IdInstrument).FirstOrDefault();
                                    if (loc != null)
                                        Table.Where(z => z.IdLocation == loc.NumberPlace).FirstOrDefault().Opacity = 1;
                                }
                                SetImage();
                                SelectedLocation = null;
                            }
                            catch (Exception ee)
                            {
                                MessageBox.Show("Я ошибка которая происходит из-за какой то фигни :_)\n" + ee.Message);
                            }

                        }));
            }
        }
        private RelayCommand floorCommand;
        public RelayCommand FloorCommand
        {
            get
            {
                return floorCommand ??
                        (floorCommand = new RelayCommand(x =>
                        {
                            using (ZavodContext db = new ZavodContext())
                            {
                                switch (x.ToString())
                                {
                                    case "NextMonth":
                                        {
                                            int nextMonth = DateTime.Now.Month + 1;
                                            int currentYear = DateTime.Now.Year;
                                            Instruments = new ObservableCollection<Instrument>(db.Instruments.
                                                                                                              Where(i => i.NextCheckDate.Month == nextMonth
                                                                                                              && i.NextCheckDate.Year == currentYear).ToList());
                                            break;
                                        }
                                    case "CurrentMonth":
                                        {
                                            int currentMonth = DateTime.Now.Month;
                                            int currentYear = DateTime.Now.Year;
                                            Instruments = new ObservableCollection<Instrument>(db.Instruments.Where(i => i.NextCheckDate.Month == currentMonth
                                                                                                                    && i.NextCheckDate.Year == currentYear).ToList());
                                            break;
                                        }
                                    default:
                                        break;
                                }
                            }
                        }));
            }
        }
        #endregion

        #region ctor

        public MapViewModel()
        {
            using (ZavodContext db = new ZavodContext())
            {
                Instruments = new ObservableCollection<Instrument>(db.Instruments.ToList());
            }
            SetImage("reb");
            Table = Singleton.getInstance().table;
            SelectedInstrument = Instruments.First();
            Floor = SelectedInstrument.Floor;

        }
        public MapViewModel(Instrument SelectedInstrument)
        {
            Table = Singleton.getInstance().table;
            foreach (var item in Table)
            {
                item.Opacity = 0.1;
            }
            this.SelectedInstrument = SelectedInstrument;
            Floor = selectedInstrument.Floor;
            using (ZavodContext db = new ZavodContext())
            {
                Instruments = new ObservableCollection<Instrument>();
                Instruments.Add(SelectedInstrument);
                Location loc = db.Locations.Where(y => y.InstrumentId == selectedInstrument.IdInstrument).FirstOrDefault();
                if (loc != null)
                    Table.Where(x => x.IdLocation == loc.NumberPlace).FirstOrDefault().Opacity = 1;
            }
            SetImage();
        }

        #endregion
        public void Dispose()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
        public void SetImage()
        {
            switch (SelectedInstrument.Floor)
            {
                case 1:
                    SetImage("reb");
                    break;
                case 3:
                    SetImage("third");
                    break;
                case 4:
                    SetImage("forth");
                    break;
                case 5:
                    SetImage("five");
                    break;
                case 6:
                    SetImage("six");
                    break;
            }
        }
        public void SetImage(string value)
        {
            ImageMap = value;
        }
    }
}

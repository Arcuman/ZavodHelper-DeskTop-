using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ZavodHelper
{
    public class InfoViewModel : ViewModelBase
    {
        private ObservableCollection<Instrument> instruments;

        private Instrument selectedInstrument;

        public ObservableCollection<Instrument> Instruments
        {
            get => instruments;
            set
            {
                instruments = value;
                OnPropertyChanged("Instruments");
            }
        }
        public Instrument SelectedInstrument
        {
            get => selectedInstrument;
            set
            {
                selectedInstrument = value;
                OnPropertyChanged("SelectedInstrument");
            }
        }

        private RelayCommand addButton;
        public RelayCommand AddButtonCommand
        {
            get
            {
                return addButton ??
                        (addButton = new RelayCommand(x =>
                        {
                            Singleton.getInstance(null).MainViewModel.CurrentViewModel = new AddViewModel();

                        }));
            }
        }
        private RelayCommand deleteCommand;
        public RelayCommand DeleteCommand
        {
            get
            {
                return deleteCommand ??
                        (deleteCommand = new RelayCommand(x =>
                        {
                            using (ZavodContext db = new ZavodContext())
                            {
                                db.Entry(SelectedInstrument).State = System.Data.Entity.EntityState.Deleted;
                                db.SaveChanges();
                                Instruments.Remove(SelectedInstrument);
                            }
                        },
                         x => SelectedInstrument != null));
            }
        }
        private RelayCommand editButton;
        public RelayCommand EditButtonCommand
        {
            get
            {
                return editButton ??
                        (editButton = new RelayCommand(x =>
                        {

                            Singleton.getInstance(null).MainViewModel.CurrentViewModel = new EditViewModel(SelectedInstrument);

                        },
                        x => SelectedInstrument != null));
            }
        }

        private RelayCommand mapButton;
        public RelayCommand MapButtonCommand
        {
            get
            {
                return mapButton ??
                        (mapButton = new RelayCommand(x =>
                        {

                            Singleton.getInstance(null).MainViewModel.CurrentViewModel = new MapViewModel();

                        }));
            }
        }
        private RelayCommand XMLCommandButton;
        public RelayCommand XMLCommand
        {
            get
            {
                return XMLCommandButton ??
                        (XMLCommandButton = new RelayCommand(x =>
                        {
                            XmlSerializer formatter = new XmlSerializer(typeof(ObservableCollection<Instrument>));
                            // получаем поток, куда будем записывать сериализованный объект
                            using (FileStream fs = new FileStream("Instruments.xml", FileMode.OpenOrCreate))
                            {
                                formatter.Serialize(fs, Instruments);
                            }

                        }));
            }
        }


         private RelayCommand mapSelectionButtonCommand;
        public RelayCommand MapSelectionButtonCommand
        {
            get
            {
                return mapSelectionButtonCommand ??
                        (mapSelectionButtonCommand = new RelayCommand(x =>
                        {

                            Singleton.getInstance(null).MainViewModel.CurrentViewModel = new MapViewModel(SelectedInstrument);

                        },
                         x => SelectedInstrument != null));
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
                                    case "Six":
                                        {
                                            Instruments = new ObservableCollection<Instrument>(db.Instruments.Where(i => i.Floor == 6).ToList());
                                            break;
                                        }
                                    case "Five":
                                        {
                                            Instruments = new ObservableCollection<Instrument>(db.Instruments.Where(i => i.Floor == 5).ToList());
                                            break;
                                        }
                                    case "Four":
                                        {
                                            Instruments = new ObservableCollection<Instrument>(db.Instruments.Where(i => i.Floor == 4).ToList());
                                            break;
                                        }
                                    case "Three":
                                        {
                                            Instruments = new ObservableCollection<Instrument>(db.Instruments.Where(i => i.Floor == 3).ToList());
                                            break;
                                        }
                                    case "First":
                                        {
                                            Instruments = new ObservableCollection<Instrument>(db.Instruments.Where(i => i.Floor == 1).ToList());
                                            break;
                                        }
                                    case "All":
                                        {
                                            Instruments = new ObservableCollection<Instrument>(db.Instruments.ToList());
                                            break;
                                        }
                                    case "NextMonth":
                                        {
                                            int nextMonth = DateTime.Now.Month + 1;
                                            int currentYear = DateTime.Now.Year;
                                            Instruments = new ObservableCollection<Instrument>(db.Instruments.
                                                                                                              Where(i=>i.NextCheckDate.Month == nextMonth 
                                                                                                              && i.NextCheckDate.Year== currentYear).ToList());
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

        public InfoViewModel()
        {
            
            using (ZavodContext db = new ZavodContext())
            {
                Instruments = new ObservableCollection<Instrument>(db.Instruments.ToList());
            }
        }

    }
}

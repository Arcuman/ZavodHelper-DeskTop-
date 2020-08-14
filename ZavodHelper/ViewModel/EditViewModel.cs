using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZavodHelper
{
    public class EditViewModel:InstrumentViewModelBase
    {
        private Instrument instrument;
        private RelayCommand infoCommand;
        public RelayCommand InfoCommand
        {
            get
            {
                return infoCommand ??
                        (infoCommand = new RelayCommand(x =>
                        {
                            Singleton.getInstance(null).MainViewModel.CurrentViewModel = new InfoViewModel();

                        }));
            }
        }
        private RelayCommand editCommand;
        public RelayCommand EditCommand
        {
            get
            {
                return editCommand ??
                        (editCommand = new RelayCommand(x =>
                        {
                            using (ZavodContext db = new ZavodContext())
                            {
                                instrument.Floor = Floor;
                                instrument.InstrumentName = InstrumentName;
                                instrument.InstrumentType = InstrumentType;
                                instrument.FactoryNumber = FactoryNumber;
                                instrument.AccuracyClassInstrument = AccuracyClassInstrument;
                                instrument.MinValue = MinValue;
                                instrument.MaxValue = MaxValue;
                                instrument.UnitValue = UnitValue;
                                instrument.PlaceInstrument = PlaceInstrument;
                                instrument.PositionInstrument = PositionInstrument;
                                instrument.PeriodCheck = PeriodCheck;
                                if (instrument.LastCheckDate != LastCheckDate)
                                {
                                    instrument.LastCheckDate = LastCheckDate;
                                    NextCheckDate = instrument.CountNextDate(LastCheckDate, PeriodCheck);
                                }
                                instrument.NextCheckDate = NextCheckDate;
                                db.Entry<Instrument>(instrument).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                        }));
            }
        }

        public EditViewModel(Instrument instrument)
        {
            this.instrument = instrument;
            Floor = instrument.Floor;
            InstrumentName = instrument.InstrumentName;
            InstrumentType = instrument.InstrumentType;
            FactoryNumber = instrument.FactoryNumber;
            AccuracyClassInstrument = instrument.AccuracyClassInstrument;
            MinValue = instrument.MinValue;
            MaxValue = instrument.MaxValue;
            UnitValue = instrument.UnitValue;
            PlaceInstrument = instrument.PlaceInstrument;
            PositionInstrument = instrument.PositionInstrument;
            PeriodCheck = instrument.PeriodCheck;
            LastCheckDate = instrument.LastCheckDate;
            NextCheckDate = instrument.NextCheckDate;
        }
    }
}

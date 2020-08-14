using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace ZavodHelper
{
    public class AddViewModel : InstrumentViewModelBase
    {
        Instrument tempInstrument = new Instrument();
        Instrument lastInstrument = new Instrument();

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
                                Instrument instrument = new Instrument(
                                    Floor,
                                    InstrumentName,
                                    InstrumentType,
                                    FactoryNumber,
                                    AccuracyClassInstrument,
                                    MinValue,
                                    MaxValue,
                                    UnitValue,
                                    PlaceInstrument,
                                    PositionInstrument,
                                    PeriodCheck,
                                    LastCheckDate
                                );
                                db.Instruments.Add(instrument);
                                db.SaveChanges();
                                Floor = 1;
                                InstrumentName = InstrumentType= AccuracyClassInstrument = PlaceInstrument = PositionInstrument = FactoryNumber = "";
                                 MinValue = MaxValue = 0;
                                UnitValue = "Null";
                                PeriodCheck = 12;
                                LastCheckDate = DateTime.Now;
                            }
                        }));
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
                            Singleton.getInstance(null).MainViewModel.CurrentViewModel = new InfoViewModel();

                        }));
            }
        }
        private RelayCommand excelCommand;
        public RelayCommand ExcelCommand
        {
            get
            {
                return excelCommand ??
                        (excelCommand = new RelayCommand(x =>
                        {
                            Excel excel = new Excel(@"Zavod.xlsx", 1);
                            using (var db = new ZavodContext())
                            {
                                for (int i = 0; i < 550; i++)
                                {
                                    Instrument instrument = new Instrument();
                                    instrument.Floor = 1;
                                    string temp = excel.ReadCell(i, 0);
                                    if (temp == "")
                                    {
                                        if (excel.ReadCell(i, 1) == @"-//-")
                                            instrument.InstrumentName = lastInstrument.InstrumentName;
                                        else
                                        {
                                            instrument.InstrumentName = excel.ReadCell(i, 1) == "" ? tempInstrument.InstrumentName : excel.ReadCell(i, 1);
                                        }
                                        instrument.InstrumentType = excel.ReadCell(i, 2) == ""? tempInstrument.InstrumentType : excel.ReadCell(i, 2);

                                        instrument.FactoryNumber = excel.ReadCell(i, 3) == ""? tempInstrument.FactoryNumber : excel.ReadCell(i, 3);

                                        instrument.AccuracyClassInstrument = excel.ReadCell(i, 4) == "" ? tempInstrument.AccuracyClassInstrument : excel.ReadCell(i, 4);

                                        instrument.MinValue = excel.ReadCell(i, 5) == ""? tempInstrument.MinValue : Convert.ToDouble(excel.ReadCell(i, 5));

                                        instrument.MaxValue = excel.ReadCell(i, 6) == ""? tempInstrument.MaxValue : Convert.ToDouble(excel.ReadCell(i, 6));

                                        instrument.UnitValue = excel.ReadCell(i, 7) == ""? tempInstrument.UnitValue : excel.ReadCell(i, 7);

                                        instrument.PlaceInstrument = excel.ReadCell(i, 8) == ""? tempInstrument.PlaceInstrument : excel.ReadCell(i, 8);

                                        if (excel.ReadCell(i, 9) == "демонтирован")
                                            continue;
                                        instrument.Floor = excel.ReadCell(i, 9) == "" ? tempInstrument.Floor : Convert.ToInt32(excel.ReadCell(i, 9));

                                        instrument.PositionInstrument = excel.ReadCell(i, 10) == ""? tempInstrument.PositionInstrument : excel.ReadCell(i, 10);

                                        instrument.PeriodCheck = excel.ReadCell(i, 11) == ""? tempInstrument.PeriodCheck : Convert.ToInt32(excel.ReadCell(i, 11));

                                        instrument.LastCheckDate = excel.ReadCell(i, 12) == ""? tempInstrument.LastCheckDate : Convert.ToDateTime(excel.ReadCell(i, 12));
                                        
                                        instrument.NextCheckDate = instrument.CountNextDate(instrument.LastCheckDate, instrument.PeriodCheck);

                                        db.Instruments.Add(instrument);
                                        db.SaveChanges();

                                    }
                                    else
                                    {
                                        if (excel.ReadCell(i, 1) == @"-//-")
                                            instrument.InstrumentName = lastInstrument.InstrumentName;
                                        else
                                        {
                                            instrument.InstrumentName = excel.ReadCell(i, 1) == "" ? "" : excel.ReadCell(i, 1);
                                        }
                                        instrument.InstrumentType = excel.ReadCell(i, 2) == "" ? "" : excel.ReadCell(i, 2);

                                        instrument.FactoryNumber = excel.ReadCell(i, 3) == "" ? "" : excel.ReadCell(i, 3);

                                        instrument.AccuracyClassInstrument = excel.ReadCell(i, 4) == "" ? "" : excel.ReadCell(i, 4);

                                        instrument.MinValue = excel.ReadCell(i, 5) == "" ? 0 : Convert.ToDouble(excel.ReadCell(i, 5));

                                        instrument.MaxValue = excel.ReadCell(i, 6) == "" ? 0 : Convert.ToDouble(excel.ReadCell(i, 6));

                                        instrument.UnitValue = excel.ReadCell(i, 7) == "" ? "" : excel.ReadCell(i, 7);

                                        instrument.PlaceInstrument = excel.ReadCell(i, 8) == "" ? "" : excel.ReadCell(i, 8);

                                        if (excel.ReadCell(i, 9) == "демонтирован")
                                            continue;
                                        instrument.Floor = excel.ReadCell(i, 9) == "" ? 0 : Convert.ToInt32(excel.ReadCell(i, 9));

                                        instrument.PositionInstrument = excel.ReadCell(i, 10) == "" ? "" : excel.ReadCell(i, 10);

                                        instrument.PeriodCheck = excel.ReadCell(i, 11) == "" ? 12 : Convert.ToInt32(excel.ReadCell(i, 11));

                                        if (excel.ReadCell(i, 12) == "")
                                            instrument.LastCheckDate = Convert.ToDateTime("01.01.2000");
                                        else
                                        {
                                            string date = excel.ReadCell(i, 12);
                                            if (date.Length == 5)
                                                date = "01." + date;
                                            instrument.LastCheckDate = Convert.ToDateTime(date);
                                        }
                                        instrument.NextCheckDate = instrument.CountNextDate(instrument.LastCheckDate, instrument.PeriodCheck);
                                        db.Instruments.Add(instrument);
                                        db.SaveChanges();
                                        tempInstrument = instrument;
                                    }
                                    lastInstrument = instrument;
                                }
                            }
                        }));
            }
        }

        public AddViewModel()
        {
            
            LastCheckDate = DateTime.Now.Date;
            UnitValue = "Null";
            PeriodCheck = 12;
            Floor = 1;
        }

        #region Helpers

        #endregion
    }
}

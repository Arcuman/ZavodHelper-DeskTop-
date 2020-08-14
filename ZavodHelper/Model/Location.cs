using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ZavodHelper
{
    public class Location : INotifyPropertyChanged
    {
        [Key]
        public int IdLocation { get; set; }

        public int Floor { get; set; }
        

        public int NumberPlace { get; set; }

        [Required]
        public int InstrumentId { get; set; }

        [ForeignKey("InstrumentId")]
        public Instrument Instrument { get; set; }

        [NotMapped]
        private double opacity;
        [NotMapped]
        public double Opacity
        {
            get => opacity;
            set
            {
                opacity = value;
                OnPropertyChanged("Opacity");
            }
        }

        public Location()
        {
            Opacity = 0.1;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZavodHelper
{
    public class Singleton
    {
        public ObservableCollection<Location> table;
        private static Singleton instance;
        private Singleton(MainViewModel mainViewModel)
        {
            MainViewModel = mainViewModel;
            table = new ObservableCollection<Location>();
            for (int i = 0; i < 544; i++)
                table.Add(new Location() { IdLocation = i});
        }
        public MainViewModel MainViewModel { get; set; }
        public static Singleton getInstance(MainViewModel mainViewModel = null)
        {
            return instance ?? (instance = new Singleton(mainViewModel));
        }
    }
}


using CarServicePolomka.Database;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace CarService
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static CarService_Osipov_420Entities1 db = new CarService_Osipov_420Entities1();
        public static Client selectedClient { get; set; }
        public static ClientService selectedVisit { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServicePolomka.Database
{
    public partial class Client
    {
        public string FullName
        {
            get
            {
                return $"{FirstName} {LastName} {Patronymic}";
            }
        }
    }
}

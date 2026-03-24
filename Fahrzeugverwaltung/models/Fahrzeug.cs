using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fahrzeugverwaltung.Models
{
    public class Fahrzeug
    {
        public string Kennzeichen { get; set; }
        public string Marke { get; set; }
        public string Modell { get; set; }
        public int Kilometerstand { get; set; }
        public DateTime TuvDatum { get; set; }
        public decimal Preis { get; set; }
    }
}

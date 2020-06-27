using System;
using System.Collections.Generic;

namespace OZO.Models
{
    public partial class Obrazovanje
    {
        public int IdObrazovanje { get; set; }
        public string NazivŠkole { get; set; }
        public string StručnaSprema { get; set; }
        public string PoloženiTečaji { get; set; }
        public int IdZaposlenici { get; set; }

        public virtual Zaposlenici IdZaposleniciNavigation { get; set; }
    }
}

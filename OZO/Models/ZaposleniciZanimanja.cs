using System;
using System.Collections.Generic;

namespace OZO.Models
{
    public partial class ZaposleniciZanimanja
    {
        public int IdZaposleniciZanimanja { get; set; }
        public string Naziv { get; set; }
        public int IdZaposlenici { get; set; }

        public virtual Zaposlenici IdZaposleniciNavigation { get; set; }
    }
}

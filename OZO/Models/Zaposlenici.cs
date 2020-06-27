using System;
using System.Collections.Generic;

namespace OZO.Models
{
    public partial class Zaposlenici
    {
        public Zaposlenici()
        {
            Obrazovanje = new HashSet<Obrazovanje>();
            ZaposleniciZanimanja = new HashSet<ZaposleniciZanimanja>();
        }

        public int IdZaposlenici { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public DateTime? DatumRođenja { get; set; }
        public decimal? TrošakZaposlenika { get; set; }
        public int IdPoslovi { get; set; }

        public virtual Poslovi IdPosloviNavigation { get; set; }
        public virtual ICollection<Obrazovanje> Obrazovanje { get; set; }
        public virtual ICollection<ZaposleniciZanimanja> ZaposleniciZanimanja { get; set; }
    }
}

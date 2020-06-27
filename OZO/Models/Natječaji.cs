using System;
using System.Collections.Generic;

namespace OZO.Models
{
    public partial class Natječaji
    {
        public Natječaji()
        {
            NatječajPoslodavac = new HashSet<NatječajPoslodavac>();
            Poslovi = new HashSet<Poslovi>();
        }

        public int IdNatječaji { get; set; }
        public string Naziv { get; set; }
        public string Opis { get; set; }
        public decimal Cijena { get; set; }
        public int? IdReferentniTip { get; set; }
        public DateTime VremenskiRok { get; set; }

        public virtual ReferentniTip IdReferentniTipNavigation { get; set; }
        public virtual ICollection<NatječajPoslodavac> NatječajPoslodavac { get; set; }
        public virtual ICollection<Poslovi> Poslovi { get; set; }
    }
}

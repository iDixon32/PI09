using System;
using System.Collections.Generic;

namespace OZO.Models
{
    public partial class ReferentniTip
    {
        public ReferentniTip()
        {
            Natječaji = new HashSet<Natječaji>();
            Oprema = new HashSet<Oprema>();
            Usluge = new HashSet<Usluge>();
        }

        public int IdReferentniTip { get; set; }
        public string Naziv { get; set; }

        public virtual ICollection<Natječaji> Natječaji { get; set; }
        public virtual ICollection<Oprema> Oprema { get; set; }
        public virtual ICollection<Usluge> Usluge { get; set; }
    }
}

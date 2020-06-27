using System;
using System.Collections.Generic;

namespace OZO.Models
{
    public partial class Oprema
    {
        public Oprema()
        {
            OpremaIzvještaji = new HashSet<OpremaIzvještaji>();
            PosaoOprema = new HashSet<PosaoOprema>();
        }

        public int IdOprema { get; set; }
        public string Naziv { get; set; }
        public string Status { get; set; }
        public bool Dostupnost { get; set; }
        public int IdReferentniTip { get; set; }

        public virtual ReferentniTip IdReferentniTipNavigation { get; set; }
        public virtual ICollection<OpremaIzvještaji> OpremaIzvještaji { get; set; }
        public virtual ICollection<PosaoOprema> PosaoOprema { get; set; }
    }
}

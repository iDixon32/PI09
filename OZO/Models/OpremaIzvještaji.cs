using System;
using System.Collections.Generic;

namespace OZO.Models
{
    public partial class OpremaIzvještaji
    {
        public int IdOpremaIzvještaji { get; set; }
        public string Sadržaj { get; set; }
        public int IdOprema { get; set; }
        public decimal? Cijena { get; set; }

        public virtual Oprema IdOpremaNavigation { get; set; }
    }
}

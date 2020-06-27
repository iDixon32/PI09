using System;
using System.Collections.Generic;

namespace OZO.Models
{
    public partial class PosaoOprema
    {
        public int IdPosaoOprema { get; set; }
        public int IdPoslovi { get; set; }
        public int IdOprema { get; set; }

        public virtual Oprema IdOpremaNavigation { get; set; }
        public virtual Poslovi IdPosloviNavigation { get; set; }
    }
}

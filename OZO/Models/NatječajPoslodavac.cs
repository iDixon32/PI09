using System;
using System.Collections.Generic;

namespace OZO.Models
{
    public partial class NatječajPoslodavac
    {
        public int IdNatječajPoslodavac { get; set; }
        public int IdNatječaja { get; set; }
        public int IdPoslodavca { get; set; }

        public virtual Natječaji IdNatječajaNavigation { get; set; }
        public virtual Poslodavac IdPoslodavcaNavigation { get; set; }
    }
}

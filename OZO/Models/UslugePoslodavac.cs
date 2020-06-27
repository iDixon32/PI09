using System;
using System.Collections.Generic;

namespace OZO.Models
{
    public partial class UslugePoslodavac
    {
        public int IdUslugePoslodavac { get; set; }
        public int IdUsluge { get; set; }
        public int IdPoslodavac { get; set; }

        public virtual Poslodavac IdPoslodavacNavigation { get; set; }
        public virtual Usluge IdUslugeNavigation { get; set; }
    }
}

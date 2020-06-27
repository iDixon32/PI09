using System;
using System.Collections.Generic;

namespace OZO.Models
{
    public partial class Poslodavac
    {
        public Poslodavac()
        {
            NatječajPoslodavac = new HashSet<NatječajPoslodavac>();
            UslugePoslodavac = new HashSet<UslugePoslodavac>();
        }

        public int IdPoslodavac { get; set; }
        public string ImeFirme { get; set; }

        public virtual ICollection<NatječajPoslodavac> NatječajPoslodavac { get; set; }
        public virtual ICollection<UslugePoslodavac> UslugePoslodavac { get; set; }
    }
}

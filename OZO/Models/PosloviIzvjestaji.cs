using System;
using System.Collections.Generic;

namespace OZO.Models
{
    public partial class PosloviIzvjestaji
    {
        public int IdPosloviIzvještaji { get; set; }
        public int IdPoslovi { get; set; }
        public string Sadržaj { get; set; }

        public virtual Poslovi IdPosloviNavigation { get; set; }
    }
}

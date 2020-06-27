using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OZO.Models
{
    public partial class Poslovi
    {
        public Poslovi()
        {
            PosaoOprema = new HashSet<PosaoOprema>();
            PosloviIzvjestaji = new HashSet<PosloviIzvjestaji>();
            Zaposlenici = new HashSet<Zaposlenici>();
        }
  
        public int IdPoslovi { get; set; }
         [Required(ErrorMessage = "Naziv je obvezno polje")]
        [Display(Name = "Naziv", Prompt = "Unesite naziv posla")]
        public string Naziv { get; set; }
        [Required(ErrorMessage = "Mjesto je obvezno polje")]
        [Display(Name = "Mjesto", Prompt = "Unesite mjesto održavanja")]
        public string Mjesto { get; set; }
        public int? IdNatječaji { get; set; }
        public int? IdUsluge { get; set; }
        [Display(Name = "Vrijeme trajanja")]
        public DateTime? VrijemeTrajanja { get; set; }

        public virtual Natječaji IdNatječajiNavigation { get; set; }
        public virtual Usluge IdUslugeNavigation { get; set; }
        public virtual ICollection<PosaoOprema> PosaoOprema { get; set; }
        public virtual ICollection<PosloviIzvjestaji> PosloviIzvjestaji { get; set; }
        public virtual ICollection<Zaposlenici> Zaposlenici { get; set; }
    }
}

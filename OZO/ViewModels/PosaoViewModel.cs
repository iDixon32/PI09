using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OZO.ViewModels
{
  public class PosaoViewModel
  {
    public int IdPoslovi  { get; set; }
    public string Naziv  { get; set; }
    public string Mjesto { get; set; }
    public string NazivNatječaja{ get; set; }
    public  string NazivUsluge { get; set; }
    public DateTime? VrijemeTrajanja { get; set; }
  }
}

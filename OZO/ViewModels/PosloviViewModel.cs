using OZO.Models;
using System.Collections.Generic;


namespace OZO.ViewModels
{
  public class PosloviViewModel
  {
    public IEnumerable<PosaoViewModel> Poslovi { get; set; }
    public PagingInfo PagingInfo { get; set; }
  }
}

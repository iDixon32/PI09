using System;

namespace OZO.ViewModels
{
  public class PagingInfo
  {
    public int TotalItems { get; set; } //upisemo prop i na tipkovnici tab tab
    public int ItemsPerPage { get; set; }
    public int CurrentPage { get; set; }
    public bool Ascending { get; set; } //uzlazno ili silazno sortiranje
    public int TotalPages //broj  stranica
    {
      get
      {
        return (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage);
      }
    }
    public int Sort { get; set; }
  }
}

using OZO.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;
using System;
using System.Linq;

namespace OZO.TagHelpers
{
  /// <summary>
  /// Tag helper za kreiranje vlastitih poveznica na stranice u rezultatu nekog upravljača
  /// Upotrebljava se kao atribut HTML oznake *pager* koju mijenja u div
  /// <example>
  /// Primjer upotrebe
  /// ```
  /// <pager page-info="@Model.PagingInfo" page-action="Index" page-title="Unesite željenu stranicu" class="float-right">
  /// </pager>
  /// ```
  /// U datoteku *_ViewImports.cshtml* potrebno dodati `@addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers`  
  /// te u pogled uključiti vlastitu javascript datoteku *gotopage.js*
  /// </example>
  /// </summary>  
  [HtmlTargetElement(Attributes = "page-info")] //jedino što je obavezno za naš pager
  public class PagerTagHelper : TagHelper
  {

    private readonly IUrlHelperFactory urlHelperFactory;
    private readonly AppSettings appData;
    public PagerTagHelper(IUrlHelperFactory helperFactory, IOptionsSnapshot<AppSettings> options)
    {
      this.urlHelperFactory = helperFactory;
      appData = options.Value;
    }

    [ViewContext]
    [HtmlAttributeNotBound]
    public ViewContext ViewContext { get; set; }

    /// <summary>
    /// Serijalizirani string koji sadrži informacije o trenutnoj i ukupnom broju stranicu 
    /// </summary>
    public PagingInfo PageInfo { get; set; } //mora imati ime kao page-info atribut samo bez crte

    /// <summary>
    /// Serijalizirani string kojim se prenose informacije o aktivnom filtriranju podataka
    /// </summary>
   // public IPageFilter PageFilter { get; set; }

    /// <summary>
    /// Akcija na koju poveznica treba voditi
    /// </summary>
    public string PageAction { get; set; }

    /// <summary>
    /// Tekst za tooltip za trenutni broj stranice i unos ciljane stranice
    /// </summary>
    public string PageTitle { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
      output.TagName = "nav"; //nav iz bootstrapa
      int offset = appData.PageOffset; //broj stranica u nav prije i poslije stranice
      TagBuilder paginationList = new TagBuilder("ul"); //unutar dodajemo elemente
      paginationList.AddCssClass("pagination"); //predstavljaju stranjičenje u bootstrapu

      if (PageInfo.CurrentPage - offset > 1) //create list item for the first page
      {
        var tag = BuildListItemForPage(1, "1.."); //stvaramo vlastiti tag
        paginationList.InnerHtml.AppendHtml(tag); 
      }

      for (int i = Math.Max(1, PageInfo.CurrentPage - offset); //uzmemu većeg od ove dvije funkcije kako ne bi išli u negativno ako je stranica do 2 nps
               i <= Math.Min(PageInfo.TotalPages, PageInfo.CurrentPage + offset); //ili idemo za offset ili broj ukupno stranica
               i++)
      {
        var tag = i == PageInfo.CurrentPage ? BuildListItemForCurrentPage(i) : BuildListItemForPage(i);
        paginationList.InnerHtml.AppendHtml(tag);
      }

      if (PageInfo.CurrentPage + offset < PageInfo.TotalPages) //create list item for the last page
      {
        var tag = BuildListItemForPage(PageInfo.TotalPages, ".. " + PageInfo.TotalPages);
        paginationList.InnerHtml.AppendHtml(tag);
      }

      output.Content.AppendHtml(paginationList);
    }

    /// <summary>
    /// Stvara oznaku za i-tu stranicu koristeći *i* kao sadržaj poveznice
    /// <seealso cref="BuildListItemForPage(int, string)"/>
    /// </summary>
    /// <param name="i">broj stranice</param>
    /// <returns>TagBuilder s pripremljenom poveznicom</returns>
    private TagBuilder BuildListItemForPage(int i) //broj stranice
    {
      return BuildListItemForPage(i, i.ToString());
    }

    /// <summary>
    ///  Stvara oznaku za i-tu stranicu koristeći argument text kao sadržaj poveznice
    /// </summary>
    /// <param name="i">broj stranice</param>
    /// <param name="text">tekst poveznice</param>
    /// <returns>TagBuilder s pripremljenom poveznicom</returns>
    private TagBuilder BuildListItemForPage(int i, string text)
    {
      IUrlHelper urlHelper = urlHelperFactory.GetUrlHelper(ViewContext);

      TagBuilder a = new TagBuilder("a");
      a.InnerHtml.Append(text);
      a.Attributes["href"] = urlHelper.Action(PageAction, new
      {
        page = i,
        sort = PageInfo.Sort,
        ascending = PageInfo.Ascending,
        //filter = PageFilter?.ToString()
      });
      a.AddCssClass("page-link");

      TagBuilder li = new TagBuilder("li"); //linovi za stranice koje generiramo
      li.AddCssClass("page-item");
      li.InnerHtml.AppendHtml(a);
      return li;
    }

    /// <summary>
    /// Stvara polje za prikaz trenutne stranice i unos željene stranice
    /// </summary>
    /// <param name="page">Broj trenutne stranice</param>
    /// <returns></returns>
    private TagBuilder BuildListItemForCurrentPage(int page)
    {
      IUrlHelper urlHelper = urlHelperFactory.GetUrlHelper(ViewContext);
      TagBuilder input = new TagBuilder("input");
      input.Attributes["type"] = "text";
      input.Attributes["value"] = page.ToString();
      input.Attributes["data-current"] = page.ToString();
      input.Attributes["data-min"] = "1"; //sljedeća tri su za kasnije korištenje JavaS najmanji mogući broj za korištenje
      input.Attributes["data-max"] = PageInfo.TotalPages.ToString(); //najveći mogući
      input.Attributes["data-url"] = urlHelper.Action(PageAction, new //za url
      {
        page = -1,
        sort = PageInfo.Sort,
        ascending = PageInfo.Ascending,
        //filter = PageFilter?.ToString()
      });
      input.AddCssClass("page-link");
      input.AddCssClass("pagebox");//da ga se može pronaći i stilizirati

      if (!string.IsNullOrWhiteSpace(PageTitle))
      {
        input.Attributes["title"] = PageTitle;
      }

      TagBuilder li = new TagBuilder("li");
      li.AddCssClass("page-item active");
      li.InnerHtml.AppendHtml(input);

      return li;
    }

  }
}


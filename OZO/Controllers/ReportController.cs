using OZO.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PdfRpt.ColumnsItemsTemplates;
using PdfRpt.Core.Contracts;
using PdfRpt.Core.Helper;
using PdfRpt.FluentInterface;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace OZO.Controllers
{
  public class ReportController : Controller
  {
    private readonly PI09Context ctx;

    public ReportController(PI09Context ctx)
    {
      this.ctx = ctx;
    }

    public IActionResult Index()
    {
      return View();
    }
    

    public async Task<IActionResult> Poslovi()
    {
      string naslov = "Popis nadolazećih poslova";
      var poslovi = await ctx.Poslovi
                            .AsNoTracking()
                            .Where(a => a.VrijemeTrajanja >=  DateTime.Now)
                            .OrderBy(a => a.VrijemeTrajanja)
                            .Select(a => new
                            {
                              a.IdPoslovi,
                              a.Naziv,
                              a.Mjesto,
                              a.VrijemeTrajanja
                            })
                            .Take(10)
                            .ToListAsync();
      PdfReport report = CreateReport(naslov);
      #region Podnožje i zaglavlje
      report.PagesFooter(footer =>
      {
        footer.DefaultFooter(DateTime.Now.ToString("dd.MM.yyyy."));
      })
      .PagesHeader(header =>
      {
        header.CacheHeader(cache: true); // It's a default setting to improve the performance.
        header.DefaultHeader(defaultHeader =>
        {
          defaultHeader.RunDirection(PdfRunDirection.LeftToRight);
          defaultHeader.Message(naslov);
        });
      });
      #endregion
      #region Postavljanje izvora podataka i stupaca
      report.MainTableDataSource(dataSource => dataSource.StronglyTypedList(poslovi));

    
      report.MainTableColumns(columns =>
      {
        columns.AddColumn(column =>
        {
          column.IsRowNumber(true);
          column.CellsHorizontalAlignment(HorizontalAlignment.Right);
          column.IsVisible(true);
          column.Order(0);
          column.Width(1);
          column.HeaderCell("#", horizontalAlignment: HorizontalAlignment.Right);
        });

        columns.AddColumn(column =>
        {
          column.PropertyName<Poslovi>(x => x.IdPoslovi);
          column.CellsHorizontalAlignment(HorizontalAlignment.Center);
          column.IsVisible(true);
          column.Order(1);
          column.Width(1);
          column.HeaderCell("Šifra", horizontalAlignment: HorizontalAlignment.Center);
         
        });

    
        columns.AddColumn(column =>
        {
          column.PropertyName<Poslovi>(x => x.Naziv);
          column.CellsHorizontalAlignment(HorizontalAlignment.Center);
          column.IsVisible(true);
          column.Order(3);
          column.Width(4);
          column.HeaderCell("Naziv posla", horizontalAlignment: HorizontalAlignment.Center);
        });

        columns.AddColumn(column =>
        {
          column.PropertyName<Poslovi>(x => x.Mjesto);
          column.CellsHorizontalAlignment(HorizontalAlignment.Center);
          column.IsVisible(true);
          column.Order(3);
          column.Width(4);
          column.HeaderCell("Mjesto održavanja", horizontalAlignment: HorizontalAlignment.Center);
        });
         columns.AddColumn(column =>
        {
          column.PropertyName<Poslovi>(x => x.VrijemeTrajanja);
          column.CellsHorizontalAlignment(HorizontalAlignment.Center);
          column.IsVisible(true);
          column.Order(3);
          column.Width(4);
          column.HeaderCell("Vrijeme održavanja", horizontalAlignment: HorizontalAlignment.Center);
        });
      });

      #endregion      
      
      byte[] pdf = report.GenerateAsByteArray();

      if (pdf != null)
      {
        Response.Headers.Add("content-disposition", "inline; filename=poslovi.pdf");
        return File(pdf, "application/pdf");
      }
      else
        return NotFound();
    }
   
     public async Task<IActionResult> Usluge()
    {
      string naslov = "Deset najskupljih artikala koji imaju sliku";
      var usluge = await ctx.Usluge
                            .AsNoTracking()
                            .Where(a => a.Cijena != null)
                            .OrderByDescending(a => a.Cijena)
                            .Select(a => new
                            {
                              a.IdUsluge,
                              a.NazivUsluge,
                              a.Cijena,
                              a.Opis
                            })
                            .Take(10)
                            .ToListAsync();
      PdfReport report = CreateReport(naslov);
      #region Podnožje i zaglavlje
      report.PagesFooter(footer =>
      {
        footer.DefaultFooter(DateTime.Now.ToString("dd.MM.yyyy."));
      })
      .PagesHeader(header =>
      {
        header.CacheHeader(cache: true); // It's a default setting to improve the performance.
        header.DefaultHeader(defaultHeader =>
        {
          defaultHeader.RunDirection(PdfRunDirection.LeftToRight);
          defaultHeader.Message(naslov);
        });
      });
      #endregion
      #region Postavljanje izvora podataka i stupaca
      report.MainTableDataSource(dataSource => dataSource.StronglyTypedList(usluge));

      report.MainTableSummarySettings(summarySettings =>
      {
        summarySettings.OverallSummarySettings("Ukupno   ");
      });

      report.MainTableColumns(columns =>
      {
        columns.AddColumn(column =>
        {
          column.IsRowNumber(true);
          column.CellsHorizontalAlignment(HorizontalAlignment.Right);
          column.IsVisible(true);
          column.Order(0);
          column.Width(1);
          column.HeaderCell("#", horizontalAlignment: HorizontalAlignment.Right);
        });

        columns.AddColumn(column =>
        {
          column.PropertyName<Usluge>(x => x.IdUsluge);
          column.CellsHorizontalAlignment(HorizontalAlignment.Center);
          column.IsVisible(true);
          column.Order(3);
          column.Width(4);
          column.HeaderCell("Šifra", horizontalAlignment: HorizontalAlignment.Center);
        });
         columns.AddColumn(column =>
        {
          column.PropertyName<Usluge>(x => x.NazivUsluge);
          column.CellsHorizontalAlignment(HorizontalAlignment.Center);
          column.IsVisible(true);
          column.Order(3);
          column.Width(4);
          column.HeaderCell("Naziv usluge", horizontalAlignment: HorizontalAlignment.Center);
        }); 
        columns.AddColumn(column =>
        {
          column.PropertyName<Usluge>(x => x.Cijena);
          column.CellsHorizontalAlignment(HorizontalAlignment.Center);
          column.IsVisible(true);
          column.Order(4);
          column.Width(1);
          column.HeaderCell("Cijena", horizontalAlignment: HorizontalAlignment.Center);
          column.ColumnItemsTemplate(template =>
          {
            template.TextBlock();
            template.DisplayFormatFormula(obj => obj == null || string.IsNullOrEmpty(obj.ToString())
                                                ? string.Empty : string.Format("{0:C2}", obj));
          });
          column.AggregateFunction(aggregateFunction =>
          {
            aggregateFunction.NumericAggregateFunction(AggregateFunction.Sum);
            aggregateFunction.DisplayFormatFormula(obj => obj == null || string.IsNullOrEmpty(obj.ToString())
                                                ? string.Empty : string.Format("{0:C2}", obj));
          });
          columns.AddColumn(column =>
        {
          column.PropertyName<Usluge>(x => x.Opis);
          column.CellsHorizontalAlignment(HorizontalAlignment.Center);
          column.IsVisible(true);
          column.Order(3);
          column.Width(4);
          column.HeaderCell("Opis ", horizontalAlignment: HorizontalAlignment.Center);
        });

        });
      });

      #endregion      
      byte[] pdf = report.GenerateAsByteArray();

      if (pdf != null)
      {
        Response.Headers.Add("content-disposition", "inline; filename=usluge.pdf");
        return File(pdf, "application/pdf");
      }
      else
        return NotFound();
    }

    
    public async Task<IActionResult> Oprema()
    {
      string naslov = $"Popis dostupne opreme";
      var oprema = await ctx.Oprema                            
                            .AsNoTracking()
                            .Where( a => a.Dostupnost == true)
                            .OrderBy(s => s.IdOprema)
                            .ThenBy(s => s.Naziv)
                            .ToListAsync();
  
      PdfReport report = CreateReport(naslov);
      #region Podnožje i zaglavlje
      report.PagesFooter(footer =>
      {
        footer.DefaultFooter(DateTime.Now.ToString("dd.MM.yyyy."));
      })
      .PagesHeader(header =>
      {
        header.CacheHeader(cache: true); // It's a default setting to improve the performance. !!!!!!!!!!!!
        header.DefaultHeader(defaultHeader =>
        {
          defaultHeader.RunDirection(PdfRunDirection.LeftToRight);
          defaultHeader.Message(naslov);
        });
      });
      #endregion
      #region Postavljanje izvora podataka i stupaca
      report.MainTableDataSource(dataSource => dataSource.StronglyTypedList(oprema));

      report.MainTableSummarySettings(summarySettings =>
      {
        summarySettings.OverallSummarySettings("Ukupno");
      });

      report.MainTableColumns(columns =>
      {
        
        columns.AddColumn(column =>
        {
          column.IsRowNumber(true);
          column.CellsHorizontalAlignment(HorizontalAlignment.Right);
          column.IsVisible(true);        
          column.Width(1);
          column.HeaderCell("#", horizontalAlignment: HorizontalAlignment.Right);
             column.AggregateFunction(aggregateFunction =>
          {
            aggregateFunction.NumericAggregateFunction(AggregateFunction.Sum);
            aggregateFunction.DisplayFormatFormula(obj => obj == null || string.IsNullOrEmpty(obj.ToString())
                                                ? string.Empty : string.Format("{0:C2}", obj));
          });    
        });
        columns.AddColumn(column =>
        {
          column.PropertyName<Oprema>(x => x.IdOprema);
          column.CellsHorizontalAlignment(HorizontalAlignment.Center);
          column.IsVisible(true);         
          column.Width(4);
          column.HeaderCell("Šifra", horizontalAlignment: HorizontalAlignment.Center);
        });

        columns.AddColumn(column =>
        {
          column.PropertyName<Oprema>(x => x.Naziv);
          column.CellsHorizontalAlignment(HorizontalAlignment.Right);
          column.IsVisible(true);         
          column.Width(1);
          column.HeaderCell("Naziv", horizontalAlignment: HorizontalAlignment.Center);
        });  
        columns.AddColumn(column =>
        {
          column.PropertyName<Oprema>(x => x.Status);
          column.CellsHorizontalAlignment(HorizontalAlignment.Right);
          column.IsVisible(true);         
          column.Width(1);
          column.HeaderCell("Opis", horizontalAlignment: HorizontalAlignment.Center);
        });

        });

      #endregion      
      byte[] pdf = report.GenerateAsByteArray();

      if (pdf != null)
      {
        Response.Headers.Add("content-disposition", "inline; filename=artikli.pdf");
        return File(pdf, "application/pdf");
      }
      else
        return NotFound();
    }

    // #region Master-detail header
    // public class MasterDetailsHeaders : IPageHeader
    // {
    //   private string naslov;
    //   public MasterDetailsHeaders(string naslov)
    //   {
    //     this.naslov = naslov;
    //   }
    //   public IPdfFont PdfRptFont { set; get; }

    

    //   public PdfGrid RenderingReportHeader(Document pdfDoc, PdfWriter pdfWriter, IList<SummaryCellData> summaryData)
    //   {
    //     var table = new PdfGrid(numColumns: 1) { WidthPercentage = 100 };
    //     table.AddSimpleRow(
    //        (cellData, cellProperties) =>
    //        {
    //          cellData.Value = naslov;
    //          cellProperties.PdfFont = PdfRptFont;
    //          cellProperties.PdfFontStyle = DocumentFontStyle.Bold;
    //          cellProperties.HorizontalAlignment = HorizontalAlignment.Center;
    //        });
    //     return table.AddBorderToTable();
    //   }
    // }
    // #endregion


   #region Private methods
    private PdfReport CreateReport(string naslov)
    {
      var pdf = new PdfReport();

      pdf.DocumentPreferences(doc =>
      {
        doc.Orientation(PageOrientation.Portrait);
        doc.PageSize(PdfPageSize.A4);
        doc.DocumentMetadata(new DocumentMetadata
        {
          Author = "FSRE",
          Application = "OZO Core",
          Title = naslov
        });
        doc.Compression(new CompressionSettings
        {
          EnableCompression = true,
          EnableFullCompression = true
        });
      })
      .MainTableTemplate(template =>
      {
        template.BasicTemplate(BasicTemplate.ProfessionalTemplate);
      })
      .MainTablePreferences(table =>
      {
        table.ColumnsWidthsType(TableColumnWidthType.Relative);
        //table.NumberOfDataRowsPerPage(20);
        table.GroupsPreferences(new GroupsPreferences
        {
          GroupType = GroupType.HideGroupingColumns,
          RepeatHeaderRowPerGroup = true,
          ShowOneGroupPerPage = true,
          SpacingBeforeAllGroupsSummary = 5f,
          NewGroupAvailableSpacingThreshold = 150,
          SpacingAfterAllGroupsSummary = 5f
        });
        table.SpacingAfter(4f);
      });

      return pdf;
    }
    #endregion
  

  }
}
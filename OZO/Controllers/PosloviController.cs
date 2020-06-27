using Microsoft.AspNetCore.Mvc;
using OZO.Extensions;
using OZO.Models;
using OZO.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace OZO.Controllers
{
    public class PosloviController : Controller
    {
        // GET: /<controller>/
        private readonly PI09Context ctx;
        private readonly AppSettings appSettings;
        private readonly ILogger<PosloviController> logger;

        public PosloviController (PI09Context ctx, IOptionsSnapshot<AppSettings> optionsSnapshot, ILogger<PosloviController> logger) //shapshot ako se konfigurancijska dat. promijeni pri novom istanciranju promijenit će se i ovo-povezana s appsetting.json natjestamo u Startup
        {
            this.ctx = ctx;
            appSettings=optionsSnapshot.Value;
            this.logger = logger;  
        }
        public IActionResult Index(int page = 1, int sort = 1, bool ascending = true){ //preslikva link koji pokazuje kada ucitamo određenu stranicu, ako nedostaje neki od podataka ovaj se broj uzima
             int pagesize = appSettings.PageSize;
              var query = ctx.Poslovi //upit na bazu
                    .AsNoTracking();
                int count = query.Count(); //broj zapisa
                  if (count == 0)
                {
                    logger.LogInformation("Ne postoji nijedna država");
                    TempData[Constants.Message] = "Ne postoji niti jedna država.";
                    TempData[Constants.ErrorOccurred] = false;
                   return RedirectToAction(nameof(Create));
                }
                
                    
            
             System.Linq.Expressions.Expression<Func<Poslovi, object>> orderSelector = null;
                switch (sort)
                {   
                    case 1:
                    orderSelector = d => d.IdPoslovi;
                    break;
                    case 2:
                    orderSelector = d => d.Naziv;
                    break;
                    case 3:
                    orderSelector = d => d.Mjesto;
                    break;
                    case 4:
                    orderSelector = d => d.IdNatječajiNavigation.Naziv;
                    break;
                    case 5:
                    orderSelector = d => d.IdUslugeNavigation.NazivUsluge;
                    break;
                    case 6:
                    orderSelector = d => d.VrijemeTrajanja;
                    break;
                }
             if (orderSelector != null)
                {
                    query = ascending ? //određuje je li order bi uzlazno ili silazno
                        query.OrderBy(orderSelector) :
                        query.OrderByDescending(orderSelector);
                }
             var poslovi =  query
                         .Select(m => new PosaoViewModel
                  {
                    IdPoslovi = m.IdPoslovi,
                    Naziv = m.Naziv,
                    Mjesto = m.Mjesto,
                    NazivNatječaja= m.IdNatječajiNavigation.Naziv,
                    NazivUsluge = m.IdUslugeNavigation.NazivUsluge,
                    VrijemeTrajanja = m.VrijemeTrajanja
                  })
                        .Skip((page - 1) * pagesize ) //  koliko podataka preskočiti, na 7.str. preskočit ćemo 6*vel.stranice
                        .Take(pagesize) //dohvaćamo elemente
                        .ToList(); //dobijemo listu
             var pagingInfo = new PagingInfo //ujedinjujemo sve informacije koje smo primili sa strane 
                {
                    CurrentPage = page,  
                    Sort = sort,
                    Ascending = ascending,
                    ItemsPerPage = pagesize,
                    TotalItems = count
                };
                if (page < 1)
                    {
                        page = 1;
                    }
                else if (page > pagingInfo.TotalPages) //kada korisnik dode do zadnje stranice, radimo redirekcija na neku akciju-referencirat ćemo se na imena metoda nameof nam osigurava da se promjenom imena ne naruši ova stranica (prilikom kompajliranja javlja grešku ili otkrije stranicu)
                    {
                         return RedirectToAction(nameof(Index), new { page = pagingInfo.TotalPages, sort, ascending }); //new-anonimna klasa, formira se link na novu akciju 
                     }
                 var model = new PosloviViewModel
                    {
                        Poslovi = poslovi,
                        PagingInfo = pagingInfo
                    };

                    return View(model);
            }
            
            private void PrepareDropDownLists()
            {
            var natječaji = ctx.Natječaji                    
                            .OrderBy(d => d.IdNatječaji)
                            .Select(d => new { d.IdNatječaji, d.Naziv })
                            .ToList();     
            var usluge = ctx.Usluge                    
                            .OrderBy(d => d.IdUsluge )
                            .Select(d => new { d.IdUsluge , d.NazivUsluge })
                            .ToList();    
            ViewBag.Natječaji = new SelectList(natječaji, nameof(Natječaji.IdNatječaji), nameof(Natječaji.Naziv));
            ViewBag.Usluge = new SelectList(usluge, nameof(Usluge.IdUsluge), nameof(Usluge.NazivUsluge));
            }
            [HttpGet]
            public IActionResult Create()
            {
            PrepareDropDownLists();
            return View();
            }
            public IActionResult Row(int id)
              {
                    var poslovi = ctx.Poslovi                      
                                    .Where(m => m.IdPoslovi == id)
                                    .Select(m => new PosaoViewModel
                                    {
                                        IdPoslovi = m.IdPoslovi,
                                        Naziv = m.Naziv,
                                        Mjesto = m.Mjesto,
                                        NazivNatječaja = m.IdNatječajiNavigation.Naziv,
                                        NazivUsluge = m.IdUslugeNavigation.NazivUsluge,
                                        VrijemeTrajanja=m.VrijemeTrajanja
                                    })
                                    .SingleOrDefault();
                    if (poslovi != null)
                    {
                        return PartialView(poslovi);
                    }
                    else
                    {
                        //vratiti prazan sadržaj?
                        return NoContent();
                    }
                    }
            [HttpPost]
            [ValidateAntiForgeryToken]
            public IActionResult Create(Poslovi poslovi)
            {
            logger.LogTrace(JsonSerializer.Serialize(poslovi), new JsonSerializerOptions{ IgnoreNullValues=true});
            if (ModelState.IsValid) //ako je model valjan može raditi jer smo u leyout dodali jqueryvalidate i validation unbotrusive
            {
                try
                {
                ctx.Add(poslovi); //kontekst
                ctx.SaveChanges(); //dodajemo u bazu

                logger.LogInformation(new EventId(1000), $"Posao {poslovi.Naziv} dodana.");

                TempData[Constants.Message] = $"Posao {poslovi.Naziv} uspješno dodana."; //pohranjuje podatke u session i brišu se nakon korištenja/stvorili Constants.cs za ove
                TempData[Constants.ErrorOccurred] = false;

                return RedirectToAction(nameof(Index));
                }
                catch (Exception exc)
                {
                logger.LogError("Pogreška prilikom dodavanje novog posla: {0}", exc.CompleteExceptionMessage());
                ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                PrepareDropDownLists();
                return View(poslovi);
                }
            }
            else{
                PrepareDropDownLists();
                return View(poslovi);
                }
            }
            [HttpPost]
            [ValidateAntiForgeryToken]
            public IActionResult Delete(int IdPoslovi, int page = 1, int sort = 1, bool ascending = true)
            {
                var poslovi = ctx.Poslovi
                                .AsNoTracking() //ima utjecaj samo za Update, za brisanje možemo staviti AsNoTracking
                                .Where(m => m.IdPoslovi == IdPoslovi)
                                .SingleOrDefault();
                if (poslovi != null)
                {
                    try
                    {
                    string naziv = poslovi.Naziv;
                    ctx.Remove(poslovi);          
                    ctx.SaveChanges();
                    logger.LogInformation($"Posao {naziv} uspješno obrisana");
                    TempData[Constants.Message] = $"Posao {naziv} uspješno obrisana";
                    TempData[Constants.ErrorOccurred] = false;
                    }
                    catch (Exception exc)
                        {
                        TempData[Constants.Message] = "Pogreška prilikom brisanja posla: " + exc.CompleteExceptionMessage();
                        TempData[Constants.ErrorOccurred] = true;

                        logger.LogError("Pogreška prilikom brisanja posla: " + exc.CompleteExceptionMessage());
                        }
                }
                return RedirectToAction(nameof(Index), new { page = page, sort = sort, ascending = ascending });
            }

        [HttpGet]
        public IActionResult Edit(int id, int page = 1, int sort = 1, bool ascending = true)
        {
        var poslovi = ctx.Poslovi.AsNoTracking().Where(d => d.IdPoslovi == id).SingleOrDefault();
        if (poslovi == null)
        {
            logger.LogWarning("Ne postoji posao s oznakom: {0} ", id);
            return NotFound("Ne postoji posao s oznakom: " + id);
        }
        else
        {
            ViewBag.Page = page;
            ViewBag.Sort = sort;
            ViewBag.Ascending = ascending;
            PrepareDropDownLists();
            return View(poslovi);
        }
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, int page = 1, int sort = 1, bool ascending = true)
        {
        //za različite mogućnosti ažuriranja pogledati
        //attach, update, samo id, ...
        //https://docs.microsoft.com/en-us/aspnet/core/data/ef-mvc/crud#update-the-edit-page

        try
        {
            Poslovi poslovi = await ctx.Poslovi.FindAsync(id);                          
            if (poslovi == null)
            {
            return NotFound("Neispravna oznaka posla: " + id);
            }
            PrepareDropDownLists();
            if (await TryUpdateModelAsync<Poslovi>(poslovi, "", //čeka do se ne izvriši ne ide dalje, a drugi ide dalje
                d => d.Naziv, d => d.Mjesto, d => d.IdNatječaji, d => d.IdUsluge, d => d.VrijemeTrajanja
            ))
            {
            ViewBag.Page = page;
            ViewBag.Sort = sort;
            ViewBag.Ascending = ascending;
            try
            {
                await ctx.SaveChangesAsync();
                TempData[Constants.Message] = "Posao ažuriran.";
                TempData[Constants.ErrorOccurred] = false;
                return RedirectToAction(nameof(Index), new { page, sort, ascending });
            }
            catch (Exception exc)
            {
                ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                return View(poslovi);
            }
            }
            else
            {
            ModelState.AddModelError(string.Empty, "Podatke o poslu nije moguće povezati s forme");
            return View(poslovi);
            }
        }
        catch (Exception exc)
        {
            TempData[Constants.Message] = exc.CompleteExceptionMessage();
            TempData[Constants.ErrorOccurred] = true;
            return RedirectToAction(nameof(Edit), id);
        }}
        
    }
}

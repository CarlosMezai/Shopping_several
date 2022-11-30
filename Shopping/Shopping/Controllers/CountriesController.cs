using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Shopping.Data;
using Shopping.Data.Entities;
using Shopping.Models;

namespace Shopping.Controllers
{
    public class CountriesController : Controller
    {
        private readonly DataContext _context;

        public CountriesController(DataContext context)
        {
            _context = context;
        }

        // GET: Countries

        public async Task<IActionResult> Index(
            string sortOrder,
            string currentFilter,
            string searchString,
            int? pageNumber
            )
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = string.IsNullOrEmpty(sortOrder) ? "Name_desc" : "";
            

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;

            var countries = from c in _context.Countries
                            .Include(c => c.States)
                            select c;
            if (!String.IsNullOrEmpty(searchString))
            {
                countries = countries.Where(s => s.Name.Contains(searchString));       

            }

            switch (sortOrder)
            {
                case "Name_desc":
                    countries = countries.OrderByDescending(c => c.Name);
                    break;
                default:
                    countries = countries.OrderBy(c => c.Name);
                    break;
            }
            int pageSize = 10;
            return View(await PaginatedList<Country>.CreateAsync(countries.AsNoTracking(), pageNumber ?? 1, pageSize));


        }


        // GET: Countries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Countries == null)
            {
                return NotFound();
            }

            var country = await _context.Countries
                //incluye los estados por country
                .Include(c => c.States)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (country == null)
            {
                return NotFound();
            }

            return View(country);
        }

        
        public IActionResult Create()
        {
            Country country = new() { States = new List<State>() };
            return View(country);
        }

  
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Country country)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(country);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(String.Empty, "Ya existe un país con el mismo nombre");
                    }

                    else 
                    {
                        ModelState.AddModelError(String.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(String.Empty, exception.Message);
                }
                
            }
            return View(country);
        }


        //Creación de un estado

        public async Task<IActionResult> AddState(int? id)
        {
            if (id == null){
                return NotFound();
            }

            Country country = await _context.Countries.FindAsync(id);
            if (country == null) 
            {
                return NotFound();
            }
            StateViewModel model = new()
            {
                CountryId = country.Id,
            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddState(StateViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    State state = new()
                    {
                        Cities = new List<City>(),
                        Country = await _context.Countries.FindAsync(model.CountryId),
                        Name = model.Name,
                    };

                    _context.Add(state);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Details), new {Id = model.CountryId});
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(String.Empty, "Ya existe un departamento/Estado con el mismo nombre en este país");
                    }

                    else
                    {
                        ModelState.AddModelError(String.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(String.Empty, exception.Message);
                }

            }
            return View(model);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Countries == null)
            {
                return NotFound();
            }

            var country = await _context.Countries.FindAsync(id);
            if (country == null)
            {
                return NotFound();
            }
            return View(country);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Country country)
        {
            if (id != country.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(country);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }

                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(String.Empty, "Ya existe un país con el mismo nombre");
                    }

                    else
                    {
                        ModelState.AddModelError(String.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(String.Empty, exception.Message);
                }

            }
            return View(country);
        }


        //EDIT STATE
        public async Task<IActionResult> EditState(int? id)
        {
            if (id == null || _context.States == null)
            {
                return NotFound();
            }

            var state = await _context.States
                .Include(s => s.Country)
                .FirstOrDefaultAsync(s => s.Id == id);
            if (state == null)
            {
                return NotFound();
            }

            StateViewModel model = new()
            {
                CountryId = state.Country.Id,
                Id = state.Id,
                Name = state.Name,

            };
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditState(int id, StateViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    State state = new()
                    {
                        Id = model.Id,
                        Name = model.Name,
                    };

                    _context.Update(state);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Details), new {Id = model.CountryId});
                }

                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(String.Empty, "Ya existe un Departamento/Estado con el mismo nombre en este país");
                    }

                    else
                    {
                        ModelState.AddModelError(String.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(String.Empty, exception.Message);
                }

            }
            return View();
        }

        // GET: Countries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Countries == null)
            {
                return NotFound();
            }

            var country = await _context.Countries
                .Include(c => c.States)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (country == null)
            {
                return NotFound();
            }

            return View(country);
        }

        // POST: Countries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Countries == null)
            {
                return Problem("Entity set 'DataContext.Countries'  is null.");
            }
            var country = await _context.Countries.FindAsync(id);
            if (country != null)
            {
                _context.Countries.Remove(country);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}




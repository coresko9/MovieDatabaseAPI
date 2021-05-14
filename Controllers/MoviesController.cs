using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieDataBase.Data;
using MovieDataBase.Models;
using System.Linq;
using System.Threading.Tasks;

namespace MovieDataBase.Controllers
{
    public class MoviesController : Controller
    {
        private const int PageSize = 6;
        private readonly MovieDataBaseContext _context;
        private static int _currPage = 1;
        private string totalItems;
        private string _itemsOnPage;



        public MoviesController(MovieDataBaseContext context)
        {
            _context = context;
            totalItems = _context.Movie.Count().ToString();
            ViewBag.isTrue = false;

        }
        // GET: Movies
        [HttpGet]
        public IActionResult Index()
        {

            if (_currPage * PageSize >= _context.Movie.Count())
            {
                _itemsOnPage = $"{_context.Movie.Count()}";
            }
            else
            {
                _itemsOnPage = $"{_currPage * PageSize}";
            }
            ViewBag.onlyShowNextButton = "true";
            if (_context.Movie.Count() <= PageSize)
            {
                ViewBag.onlyShowNextButton = "none";
            }
            return View(new MovieListViewModel
            {
                ViewString = $"{(_currPage * PageSize) - (PageSize - 1)} - {_itemsOnPage} / {totalItems}",
                Movies = _context.Movie
                .OrderBy(p => p.imdbID)
                .Skip((_currPage - 1) * PageSize)
                .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = _currPage,
                    ItemsPerPage = PageSize,
                    TotalItems = _context.Movie.Count()
                }
            }); ;
        }
        [HttpPost]
        public IActionResult Index(string pageDirection)
        {
            if (_context.Movie.Count() <= PageSize)
            {
                ViewBag.onlyShowNextButton = "none";
            }
            else if (pageDirection == "Prev")
            {
                if (_currPage == 2)
                {
                    ViewBag.onlyShowNextButton = "true";

                }
                _currPage--;
            }
            else if (pageDirection == "Next")
            {
                if (PageSize * (_currPage + 1) >= _context.Movie.Count())
                {
                    ViewBag.onlyShowNextButton = "false";
                }
                _currPage++;

            }
            if (_currPage * PageSize >= _context.Movie.Count())
            {
                _itemsOnPage = $"{_context.Movie.Count()}";
            }
            else
            {
                _itemsOnPage = $"{_currPage * PageSize}";

            }
            return View(new MovieListViewModel
            {
                ViewString = $"{(_currPage * PageSize) - (PageSize - 1)} - {_itemsOnPage} / {totalItems}",
                Movies = _context.Movie
                           .OrderBy(p => p.imdbID)
                           .Skip((_currPage - 1) * PageSize)
                           .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = _currPage,
                    ItemsPerPage = PageSize,
                    TotalItems = _context.Movie.Count()
                }
            });
        }
        public IActionResult IndexAll()
        {
            return View(_context.Movie.ToList());
        }
        // GET: Movies/Details/5
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .FirstOrDefaultAsync(m => m.imdbID == id);
            if (movie == null)
            {
                return NotFound();
            }



            return View(movie);
        }

        // GET: Movies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,ReleaseDate")] Movie movie)
        {
            if (ModelState.IsValid)
            {
                _context.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Title,ReleaseDate")] Movie movie)
        {
            if (id != movie.imdbID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.imdbID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .FirstOrDefaultAsync(m => m.imdbID == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var movie = await _context.Movie.FindAsync(id);
            _context.Movie.Remove(movie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(string id)
        {
            return _context.Movie.Any(e => e.imdbID == id);
        }
       
    }
}

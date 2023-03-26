using Microsoft.AspNetCore.Mvc;
using MusicStore.Data;
using MusicStore.Models;
using MusicStore.ViewModels;
using System.Diagnostics;

namespace MusicStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MusicStoreEntities _context;

        public HomeController(ILogger<HomeController> logger,MusicStoreEntities context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {           
            SampleData data = new SampleData(_context);
            data.AddGenres();
            data.AddArtist();
            data.AddAlbums();

            AlbumEncokSatanlarViewModel ornek = new AlbumEncokSatanlarViewModel();
            var albums = GetTopSellingAlbums(5);
            ornek.EnCokSatanAlbumler = albums;
            //genre tablosundan tüm verileri getirecek ve liste olarak döndürecek kod.
            var muzikTurleri=_context.Genres.ToList();
            ornek.MuzikTurleri = muzikTurleri;
            return View(ornek);
        }
        private List<Album> GetTopSellingAlbums(int count)
        {
            // Group the order details by album and return
            // the albums with the highest count
            return _context.Albums
                .OrderByDescending(a => a.OrderDetails.Count())
                .Take(count)
                .ToList();
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    
    }
}
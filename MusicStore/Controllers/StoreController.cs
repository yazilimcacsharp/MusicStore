using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicStore.Data;
using MusicStore.Models;
using System.Web;

namespace MusicStore.Controllers
{
    public class StoreController : Controller
    {
        private readonly MusicStoreEntities _context;

        public StoreController(MusicStoreEntities context)
        {
            _context = context;
        }


        public IActionResult Index()
        {
            //hard coded: manuel olarak tek tek yazma işlemi
            //var genres = new List<Genre>
            //{
            //   new Genre { Name = "Disco"},
            //   new Genre { Name = "Jazz"},
            //   new Genre { Name = "Rock"}
            //};
            var genres = _context.Genres.ToList();

            return View(genres);
        }
        //
        // GET: /Store/Browse?genre=Disco
        //Store Controller yapısının içerisindeki Browse metodu için genre parametresine değer atamak istersek 
        //https://localhost:7285/Store/Browse?GENRE=pop
        // /ControllerAdi/MetotAdi?parametreAdi=kontrolEdilecekDeger
        public IActionResult Browse(string genre)
        {
            //    string message = HttpUtility.HtmlEncode("Store.Browse, Genre = "
            //+ genre);

            //    return message;

            var genreModel = _context.Genres.Include("Albums").Single(g => g.Name == genre);
        
            return View(genreModel);
        }

        public IActionResult Details(int id)
        {
            //var album = new Album { Title = "Album " + id };
            //return View(album);

            //1.yol
            //var album = _context.Albums.Include(p=>p.Genre).Include(p=>p.Artist).Where(p=>p.AlbumId==id).FirstOrDefault();

            //2.yol
            var album = _context.Albums.Include("Genre").Include("Artist").Where(p => p.AlbumId == id).FirstOrDefault();
     
            return View(album);
        }
        public IActionResult GenreMenu()
        {
            var genres = _context.Genres.ToList();
            return PartialView(genres);
        }
    }
}

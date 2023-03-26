using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MusicStore.Data;
using MusicStore.Models;

namespace MusicStore.Controllers
{
    public class StoreManagerController : Controller
    {
        private readonly MusicStoreEntities _context;

        public StoreManagerController(MusicStoreEntities context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var albums = _context.Albums.Where(p=>p.IsActive==true).Include(a => a.Genre).Include(a => a.Artist);
            return View(albums.ToList());
        }

        public IActionResult Details(int id)
        {
            Album album = _context.Albums.Find(id);// tablodan id bilgisini kullanarak ilgili albumu bulur ve bu albümü geriye döndürür.
            return View(album);
        }

        //Create linkine tıklanınca buradaki Create metodu yardımıyla yeni bir albüm oluşturulacak.
        //
        // GET: /StoreManager/Create
        public ActionResult Create()
        {
            ViewBag.GenreId = new SelectList(_context.Genres, "GenreId", "Name");
            ViewBag.ArtistId = new SelectList(_context.Artists, "ArtistId", "Name");
            return View();
        }

        //Create işleminin backende yansıması için
        //
        // POST: /StoreManager/Create
        [HttpPost]
        public IActionResult Create(Album album)
        {
            //genreid ve artistid bilgilerini kullanarak bunların tablodaki karşılıklarını bulduk.
            album.Artist = _context.Artists.Where(p => p.ArtistId == album.ArtistId).FirstOrDefault();
            album.Genre = _context.Genres.Where(p => p.GenreId == album.GenreId).FirstOrDefault();

            var album1=_context.Albums.Where(p => p.ArtistId == album.ArtistId && p.Title == album.Title).FirstOrDefault();
            if (album1 == null) {
               //album.IsActive = true;
                _context.Albums.Add(album);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            else
                return View("Hata");

            //ViewBag.GenreId = new SelectList(_context.Genres, "GenreId", "Name", album.GenreId);
            //ViewBag.ArtistId = new SelectList(_context.Artists, "ArtistId", "Name", album.ArtistId);
            //return View(album);
        }


        //
        // GET: /StoreManager/Edit/5
        public IActionResult Edit(int id)
        {
            Album album = _context.Albums.Where(k=>k.IsActive==true && k.AlbumId==id).FirstOrDefault();
            if (album != null)
            {
                ViewBag.GenreId = new SelectList(_context.Genres, "GenreId", "Name", album.GenreId);
                ViewBag.ArtistId = new SelectList(_context.Artists, "ArtistId", "Name", album.ArtistId);
                return View(album);
            }
            return View("Hata");
        }

        //
        // POST: /StoreManager/Edit/5
        [HttpPost]
        public IActionResult Edit(Album album)
        {
            _context.Entry(album).State = EntityState.Modified;
            _context.SaveChanges();
            return RedirectToAction("Index");
           
            //ViewBag.GenreId = new SelectList(db.Genres, "GenreId", "Name", album.GenreId);
            //ViewBag.ArtistId = new SelectList(db.Artists, "ArtistId", "Name", album.ArtistId);
            //return View(album);
        }


        //
        // GET: /StoreManager/Delete/5

        public IActionResult Delete(int id)
        {
            Album album = _context.Albums.Find(id);
            return View(album);
        }

        //[HttpPost, ActionName("Delete")]
        //public IActionResult DeleteConfirmed(int id)
        //{
        //    Album album = _context.Albums.Find(id);
        //    _context.Albums.Remove(album);
        //    _context.SaveChanges();
        //    return RedirectToAction("Index"); //silme işlemi sonrası Index yani tüm albümlerin listelendiği sayfaya dönecek.
        //}

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            Album album = _context.Albums.Find(id);
            album.IsActive = false;            
            _context.SaveChanges();
            return RedirectToAction("Index"); //silme işlemi sonrası Index yani tüm albümlerin listelendiği sayfaya dönecek.
        }


    }
}

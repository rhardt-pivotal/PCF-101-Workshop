using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using springmusicdotnetcore.Models;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace springmusicdotnetcore.Controllers
{
    [Route("api/[controller]")]
    public class AlbumsController : Controller
    {

        private readonly AlbumContext _context;

        public AlbumsController(AlbumContext context){
            _context = context;
            if(_context.Albums.Count() == 0){
                loadAlbums();
            }
        }

        private void loadAlbums() {
            using (StreamReader r = new StreamReader("albums.json"))
            {
                string json = r.ReadToEnd();
                List<Album> albums = JsonConvert.DeserializeObject<List<Album>>(json);
                foreach (Album a in albums) {
                    _context.Albums.Add(a);
                }
                _context.SaveChanges();
            }
        }

        // GET: api/albums
        [HttpGet]
        public IEnumerable<Album> Get()
        {
            return _context.Albums.ToList();
        }

        // GET api/albums/5
        [HttpGet("{id}", Name ="GetAlbum")]
        public IActionResult GetById(int id)
        {
            var album = _context.Albums.FirstOrDefault(t => t.Id == id);
            if (album == null) {
                return NotFound();
            }
            return new ObjectResult(album);
        }

        // POST api/albums
        [HttpPost]
        public IActionResult Create([FromBody]Album newAlbum)
        {
            if (newAlbum == null) {
                return BadRequest();
            }
            if (newAlbum.Id > 0) {
                return _updateAlbum((int)newAlbum.Id, newAlbum);
            }
            _context.Albums.Add(newAlbum);
            _context.SaveChanges();
            return CreatedAtRoute("GetAlbum", new { id = newAlbum.Id }, newAlbum);
        }

        private IActionResult _updateAlbum(int id, Album updatedAlbum) {
            if (updatedAlbum == null || updatedAlbum.Id != id)
            {
                return BadRequest();
            }
            var album = _context.Albums.FirstOrDefault(a => a.Id == id);
            if (album == null)
            {
                return NotFound();
            }
            album.artist = updatedAlbum.artist;
            album.genre = updatedAlbum.genre;
            album.releaseYear = updatedAlbum.releaseYear;
            album.title = updatedAlbum.title;
            _context.Update(album);
            _context.SaveChanges();
            return new NoContentResult();
        }

        // PUT api/albums/5
        [HttpPut("{id}")]
        public IActionResult UpdateAlbum(int id, [FromBody]Album updatedAlbum)
        {
            return _updateAlbum(id, updatedAlbum);
        }

        // DELETE api/albums/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var album = _context.Albums.FirstOrDefault(a => a.Id == id);
            if (album == null) {
                return NotFound();
            }
            _context.Albums.Remove(album);
            _context.SaveChanges();
            return new NoContentResult();
        }
    }
}

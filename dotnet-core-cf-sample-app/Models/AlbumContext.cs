using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using springmusicdotnetcore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;
namespace springmusicdotnetcore.Models
{
    public class AlbumContext : DbContext
    {
        public AlbumContext(DbContextOptions<AlbumContext> options)
            : base(options)
        {
            if(!Database.IsInMemory()){
                try{
                    Albums.Count();    
                }
                catch(Exception){
                    ((RelationalDatabaseCreator)Database.GetService<IDatabaseCreator>()).CreateTables();
                }
            }
            loadAlbums();
        }

        private void loadAlbums()
        {
            if (Albums.Count() > 0) {
                return;
            }
            using (StreamReader r = new StreamReader("albums.json"))
            {
                string json = r.ReadToEnd();
                List<Album> albums = JsonConvert.DeserializeObject<List<Album>>(json);
                foreach (Album a in albums)
                {
                    Albums.Add(a);
                }
                SaveChanges();
            }
        }

        public DbSet<Album> Albums { get; set; }



    }
}

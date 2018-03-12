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
        }

        public DbSet<Album> Albums { get; set; }



    }
}

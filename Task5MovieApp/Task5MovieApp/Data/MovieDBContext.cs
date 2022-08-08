using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Task5MovieApp.Models;

namespace Task5MovieApp.Data
{
    public class MovieDBContext : DbContext
    {
        public MovieDBContext() : base() { }
        public MovieDBContext (DbContextOptions<MovieDBContext> options)
            : base(options)
        {
        }

        public DbSet<Genre> Genre { get; set; } = default!;
        public DbSet<Role> Role { get; set; } = default!;
        public DbSet<UserData> UserData { get; set; } = default!;
        public DbSet<UserProfile> UserProfile { get; set; } = default!;
    }
}

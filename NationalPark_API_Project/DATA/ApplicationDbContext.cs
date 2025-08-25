using Microsoft.EntityFrameworkCore;
using NationalPark_API_Project.Models;
using System.Collections.Generic;

namespace NationalPark_API_Project.DATA
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options):base(options)
        {
            
        }
        public DbSet<NationalPark> NationalParks { get; set; }
        public DbSet<Trail> Trails { get; set; }
        public DbSet<User> Users { get; set; }
    }
}

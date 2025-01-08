using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Project_PicturesGalleryPlatform.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Member> Members { get; set; }
    }
}

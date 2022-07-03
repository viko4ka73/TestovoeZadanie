using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace TestovoeImage.Models
{
    public class ImageDbContext:DbContext
    {
        public ImageDbContext(DbContextOptions<ImageDbContext> options):base(options)
        {

        }
        public DbSet<ImageModel> Image { get; set; }
    }
}

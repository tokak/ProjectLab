using Microsoft.EntityFrameworkCore;
using ProjectLab.WebUI.Models;

namespace ProjectLab.WebUI.Infrastructures
{
    public class AppDataContext : DbContext
    {

        public AppDataContext(DbContextOptions<AppDataContext> options) : base(options)
        {
        }

        public DbSet<Person> Persons { get; set; }
    }


}

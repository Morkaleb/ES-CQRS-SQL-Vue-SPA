using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Ops.DBContext
{
    public class HubContext: DbContext
    {
        public DbSet<Location> Locations { get; set; }

        public HubContext(DbContextOptions<HubContext> options) : base(options)
        {
            // Database.EnsureCreated();
        }
    }

    [Table("Locations", Schema = "Ws")]
    public class Location
    {
        public int LocationId { get; set; }
    }
}

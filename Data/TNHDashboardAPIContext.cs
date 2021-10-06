using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TNHDashboardAPI.Models;

namespace TNHDashboardAPI.Data
{
    public class TNHDashboardAPIContext : DbContext
    {
        public TNHDashboardAPIContext (DbContextOptions<TNHDashboardAPIContext> options) : base(options)
        {
        }

        public DbSet<ScoreEntry> ScoreEntry { get; set; }
        public DbSet<MapData> MapData { get; set; }
    }
}

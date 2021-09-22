using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TNHDashboardAPI.Data;
using TNHDashboardAPI.Utilities;

namespace TNHDashboardAPI.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new TNHDashboardAPIContext(serviceProvider.GetRequiredService<DbContextOptions<TNHDashboardAPIContext>>()))
            {
                try
                {
                    foreach (ScoreEntry entry in context.ScoreEntry)
                    {
                        if (string.IsNullOrEmpty(entry.HoldActions))
                        {
                            context.ScoreEntry.Remove(entry);
                            Debug.WriteLine("Removing entry from database");
                        }
                    }

                    context.SaveChanges();

                    if (context.ScoreEntry.Any())
                    {
                        Debug.WriteLine("Database is already seeded");
                        return;   // DB has been seeded
                    }
                }
                catch
                {
                    Debug.WriteLine("Migrating Database");
                    context.Database.Migrate();
                    context.SaveChanges();
                }


                Debug.WriteLine("Seeding Database");

                context.ScoreEntry.AddRange(ScoreGeneratingUtils.GenerateRandomScoreEntries(1000));

                Debug.WriteLine("Saving seeded");
                context.SaveChanges();
            }
        }
    }
}

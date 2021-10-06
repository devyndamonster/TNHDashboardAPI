using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TNHDashboardAPI.Models
{
    public record MapData
    {
        public int Id { get; set; }
        public string MapName { get; set; }
        public string HoldPointLocations { get; set; }
        public string SupplyPointLocations { get; set; }
    }
}

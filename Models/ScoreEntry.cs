using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TNHDashboardAPI.Models
{
    public record ScoreEntry
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Score { get; set; }
        public string Character { get; set; }
        public string Map { get; set; }
        public string HealthMode { get; set; }
        public string EquipmentMode { get; set; }
        public string GameLength { get; set; }
        public string HoldActions { get; set; }
        public string HoldStats { get; set; }
    }
}

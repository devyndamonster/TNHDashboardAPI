using Bogus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TNHDashboardAPI.Models;

namespace TNHDashboardAPI.Utilities
{
    public static class ScoreGeneratingUtils
    {

        public static List<ScoreEntry> GenerateRandomScoreEntries(int count)
        {
            string[] maps = { "Default", "Winter Wasteland" };
            string[] equipment_modes = { "Limited", "Spawnlock" };
            string[] game_lengths = { "5-Hold", "3-Hold", "Endless" };
            string[] health_modes = { "Standard", "One-Hit" };
            string[] weapons = { "AKM", "Makarov", "1911", "M16", "Stoner 63", "PPBizon", "P90", "AWM", "P250" };

            var randomActions = new Faker<List<List<string>>>()
                .CustomInstantiator(f =>
                {
                    List<List<string>> actions = new List<List<string>>();

                    for(int i = 0; i < 5; i++)
                    {
                        List<string> holdActions = new List<string>();

                        if (i == 0) holdActions.Add($"Spawned At Supply {f.Random.Number(0, 10)}");

                        holdActions.Add($"Entered Supply {f.Random.Number(0, 10)}");
                        holdActions.Add($"Purchased {f.PickRandom(weapons)}");
                        holdActions.Add($"Entered Supply {f.Random.Number(0, 10)}");
                        holdActions.Add($"Recycled {f.PickRandom(weapons)}");
                        holdActions.Add($"Entered Hold {f.Random.Number(0, 10)}");

                        if(f.Random.Float() > 0.8)
                        {
                            holdActions.Add("Died");
                            actions.Add(holdActions);
                            return actions;
                        }

                        if(i == 4) holdActions.Add("Victory");

                        actions.Add(holdActions);
                    }

                    return actions;
                });

            var randomStats = new Faker<HoldStats>()
                .RuleFor(s => s.SosigsKilled, f => f.Random.Number(0, 30))
                .RuleFor(s => s.MeleeKills, f => f.Random.Number(0, 30))
                .RuleFor(s => s.Headshots, f => f.Random.Number(0, 30))
                .RuleFor(s => s.TokensSpent, f => f.Random.Number(0, 5))
                .RuleFor(s => s.GunsRecycled, f => f.Random.Number(0, 1))
                .RuleFor(s => s.AmmoSpent, f => f.Random.Number(10, 999));


            var randomScores = new Faker<ScoreEntry>()
                .RuleFor(s => s.Name, f => f.Name.FirstName())
                .RuleFor(s => s.Score, f => f.Random.Number(1000000, 9999990))
                .RuleFor(s => s.Map, f => f.PickRandom(maps))
                .RuleFor(s => s.EquipmentMode, f => f.PickRandom(equipment_modes))
                .RuleFor(s => s.GameLength, f => f.PickRandom(game_lengths))
                .RuleFor(s => s.HealthMode, f => f.PickRandom(health_modes))
                .FinishWith((f,s) =>
                {
                    List<List<string>> actions = randomActions.Generate();
                    List<HoldStats> stats = randomStats.Generate(actions.Count());

                    s.HoldActions = JsonConvert.SerializeObject(actions);
                    s.HoldStats = JsonConvert.SerializeObject(stats);
                });


            return randomScores.Generate(count);
        }


    }
}

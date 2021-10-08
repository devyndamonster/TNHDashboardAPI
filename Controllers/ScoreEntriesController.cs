using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TNHDashboardAPI.Data;
using TNHDashboardAPI.Models;

namespace TNHDashboardAPI.Controllers
{

    [Route("api/scores")]
    [ApiController]
    public class ScoreEntriesController : ControllerBase
    {
        private readonly TNHDashboardAPIContext _context;

        public ScoreEntriesController(TNHDashboardAPIContext context)
        {
            _context = context;
        }


        [HttpGet("ping")]
        public ActionResult Ping()
        {
            return Ok("Pong");
        }

        // GET: api/scores
        [HttpGet]
        public ActionResult GetScores([FromQuery] string character, [FromQuery]string map, [FromQuery]string health, [FromQuery]string equipment, [FromQuery]string length, [FromQuery]int startingIndex, [FromQuery]int count)
        {
            try
            {
                var scores = from s in _context.ScoreEntry
                             where s.Character.Equals(character)
                             where s.Map.Equals(map)
                             where s.HealthMode.Equals(health)
                             where s.EquipmentMode.Equals(equipment)
                             where s.GameLength.Equals(length)
                             orderby s.Score descending
                             select s;

                if (scores.Count() == 0)
                {
                    return Ok(scores);
                }

                List<ScoreEntry> selectedScores = scores.Skip(startingIndex).Take(count).ToList();

                int rank = startingIndex;
                selectedScores.ForEach(o =>
                {
                    o.Rank = rank;
                    rank += 1;
                });

                return Ok(selectedScores);
            }
            catch(Exception e)
            {
                return Problem("Something bad happened: " + e.ToString());
            }
        }

        [HttpGet("search")]
        public ActionResult SearchScores([FromQuery] string character, [FromQuery] string map, [FromQuery] string health, [FromQuery] string equipment, [FromQuery] string length, [FromQuery] string name, [FromQuery] int num_before = 0, [FromQuery] int num_after = 0)
        {
            try
            {
                var scores = from s in _context.ScoreEntry
                             where s.Character.Equals(character)
                             where s.Map.Equals(map)
                             where s.HealthMode.Equals(health)
                             where s.EquipmentMode.Equals(equipment)
                             where s.GameLength.Equals(length)
                             orderby s.Score descending
                             select s;

                //First, get index of the person we're searching for
                List<ScoreEntry> scoreList = scores.ToList();
                int scoreIndex = scoreList.FindIndex(o => o.Name == name);

                //If the index is negative, we could not find the desired score in this selection
                if (scoreIndex < 0) return NotFound();

                //Now we want this list to only contain the selected number of scores
                num_before = Math.Min(num_before, scoreIndex);
                scoreList = scoreList.Skip(scoreIndex - num_before).Take(num_before + num_after + 1).ToList();

                //Go through the list and apply rankings
                int rank = scoreIndex - num_before;
                scoreList.ForEach(o =>
                {
                    o.Rank = rank;
                    rank += 1;
                });
                
                //Now we take the desired scores
                return Ok(scoreList);
            }
            catch (Exception e)
            {
                return Problem("Something bad happened: " + e.ToString());
            }
        }

        // GET: api/scores/count
        [HttpGet("count")]
        public ActionResult GetNumScores([FromQuery] string character, [FromQuery] string map, [FromQuery] string health, [FromQuery] string equipment, [FromQuery] string length)
        {
            try
            {
                var scores = from s in _context.ScoreEntry
                             where s.Character.Equals(character)
                             where s.Map.Equals(map)
                             where s.HealthMode.Equals(health)
                             where s.EquipmentMode.Equals(equipment)
                             where s.GameLength.Equals(length)
                             select s;

                return Ok(scores.Count());
            }
            catch (Exception e)
            {
                return Problem("Something bad happened : " + e.ToString());
            }
        }


        [HttpGet("take")]
        public ActionResult GetScores([FromQuery] int count)
        {
            try
            {
                var scores = from s in _context.ScoreEntry
                             orderby s.Score descending
                             select s;

                List<ScoreEntry> selectedScores = scores.Take(count).ToList();

                return Ok(selectedScores);
            }
            catch (Exception e)
            {
                return Problem("Something bad happened: " + e.ToString());
            }

        }


        // POST: ScoreEntries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPut]
        public ActionResult UpdateScore([Bind("Id,Name,Score,Character,Map,HealthMode,EquipmentMode,GameLength,Rank")] ScoreEntry scoreEntry)
        {
            try
            {
                //Gets the score that matches the sent score
                var scores = from s in _context.ScoreEntry
                             where s.Name.Equals(scoreEntry.Name)
                             where s.Character.Equals(scoreEntry.Character)
                             where s.Map.Equals(scoreEntry.Map)
                             where s.HealthMode.Equals(scoreEntry.HealthMode)
                             where s.EquipmentMode.Equals(scoreEntry.EquipmentMode)
                             where s.GameLength.Equals(scoreEntry.GameLength)
                             select s;

                if (scores.Count() == 0)
                {
                    _context.ScoreEntry.Add(scoreEntry);
                }

                else
                {
                    ScoreEntry entry = scores.First();

                    //If the score has not increased, don't edit the entry
                    if (entry.Score >= scoreEntry.Score) return NoContent();

                    entry.Score = scoreEntry.Score;
                    entry.HoldActions = scoreEntry.HoldActions;
                    entry.HoldStats = scoreEntry.HoldStats;

                    _context.Entry(entry).State = EntityState.Modified;
                }

                _context.SaveChanges();

                return NoContent();
            }
            catch(Exception e)
            {
                return Problem("Something bad happened: " + e.ToString());
            }
            
        }
    }
}

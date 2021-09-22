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
        public ActionResult GetScores([FromQuery]string map, [FromQuery]string health, [FromQuery]string equipment, [FromQuery]string length, [FromQuery]int startingIndex, [FromQuery]int count)
        {
            try
            {
                var scores = from s in _context.ScoreEntry
                             where s.Map.Equals(map)
                             where s.HealthMode.Equals(health)
                             where s.EquipmentMode.Equals(equipment)
                             where s.GameLength.Equals(length)
                             orderby s.Score descending
                             select s;

                if (scores.Count() == 0)
                {
                    return Ok(scores.ToList());
                }

                List<ScoreEntry> selectedScores = scores.Skip(startingIndex).Take(count).ToList();

                return Ok(selectedScores);
            }
            catch(Exception e)
            {
                return Problem("Something bad happened: " + e.ToString());
            }
        }

        [HttpGet("search")]
        public ActionResult SearchScores([FromQuery] string map, [FromQuery] string health, [FromQuery] string equipment, [FromQuery] string length, [FromQuery] string name)
        {
            try
            {
                var scores = from s in _context.ScoreEntry
                             where s.Map.Equals(map)
                             where s.HealthMode.Equals(health)
                             where s.EquipmentMode.Equals(equipment)
                             where s.GameLength.Equals(length)
                             where s.Name.Equals(name)
                             select s;

                return Ok(scores);
            }
            catch (Exception e)
            {
                return Problem("Something bad happened: " + e.ToString());
            }
        }


        // GET: api/scores/count
        [HttpGet("count")]
        public ActionResult GetNumScores([FromQuery] string map, [FromQuery] string health, [FromQuery] string equipment, [FromQuery] string length)
        {
            try
            {
                var scores = from s in _context.ScoreEntry
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
        public ActionResult UpdateScore([Bind("Id,Name,Score,Map,HealthMode,EquipmentMode,GameLength")] ScoreEntry scoreEntry)
        {
            try
            {
                //Gets the score that matches the sent score
                var scores = from s in _context.ScoreEntry
                             where s.Name.Equals(scoreEntry.Name)
                             where s.Map.Equals(scoreEntry.Map)
                             where s.HealthMode.Equals(scoreEntry.HealthMode)
                             where s.EquipmentMode.Equals(scoreEntry.EquipmentMode)
                             where s.GameLength.Equals(scoreEntry.GameLength)
                             select s;

                if (scores.Count() == 0)
                {
                    _context.Add(scoreEntry);
                }

                else
                {
                    ScoreEntry entry = scores.First();

                    entry.Score = scoreEntry.Score;

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

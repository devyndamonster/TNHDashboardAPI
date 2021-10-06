using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TNHDashboardAPI.Data;
using TNHDashboardAPI.Models;

namespace TNHDashboardAPI.Controllers
{

    [Route("api/maps")]
    [ApiController]
    public class MapController : ControllerBase
    {
        private readonly TNHDashboardAPIContext _context;

        public MapController(TNHDashboardAPIContext context)
        {
            _context = context;
        }


        [HttpGet]
        public ActionResult GetMapData([FromQuery] string name)
        {
            try
            {
                var mapData = from s in _context.MapData
                              where s.MapName.Equals(name)
                              select s;

                if(mapData.Count() <= 0)
                {
                    return NotFound();
                }

                return Ok(mapData.First());
            }

            catch (Exception e)
            {
                return Problem("Something bad happened: " + e.ToString());
            }
        }



        [HttpPut]
        public ActionResult UpdateMapData([Bind("Id,MapName,HoldPointLocations,SupplyPointLocations")] MapData mapData)
        {
            try
            {
                //Gets the score that matches the sent score
                var mapList = from s in _context.MapData
                              where s.MapName.Equals(mapData.MapName)
                              select s;


                if (mapList.Count() == 0)
                {
                    _context.MapData.Add(mapData);
                }

                else
                {
                    MapData entry = mapList.First();

                    entry.HoldPointLocations = mapData.HoldPointLocations;
                    entry.SupplyPointLocations = mapData.SupplyPointLocations;

                    _context.Entry(entry).State = EntityState.Modified;
                }

                _context.SaveChanges();

                return NoContent();
            }
            catch (Exception e)
            {
                return Problem("Something bad happened: " + e.ToString());
            }
        }
    }
}

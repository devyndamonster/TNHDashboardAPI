using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TNHDashboardAPI.Data;

namespace TNHDashboardAPI.Controllers
{
    [Route("api/content")]
    [ApiController]
    public class UserContentController : ControllerBase
    {
        private readonly TNHDashboardAPIContext _context;

        public UserContentController(TNHDashboardAPIContext context)
        {
            _context = context;
        }


        [HttpGet]
        public ActionResult GetUserContentOptions()
        {
            try
            {
                var uniqueModdedCharacters = (from s in _context.ScoreEntry
                                              where !VanillaSelectionOptions.Character.Contains(s.Character)
                                              select s.Character).Distinct().OrderBy(o => o);

                var uniqueModdedMaps = (from s in _context.ScoreEntry
                                        where !VanillaSelectionOptions.Maps.Contains(s.Map)
                                        select s.Map).Distinct().OrderBy(o => o);

                List<List<string>> result = new List<List<string>>();
                result.Add(uniqueModdedCharacters.ToList());
                result.Add(uniqueModdedMaps.ToList());

                return Ok(result);
            }
            catch (Exception e)
            {
                return Problem("Something bad happened: " + e.ToString());
            }
        }
    }
}

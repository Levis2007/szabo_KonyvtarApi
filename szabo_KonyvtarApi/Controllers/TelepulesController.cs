using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using szabo_KonyvtarApi.DTOs;
using szabo_KonyvtarApi.Models;

namespace szabo_KonyvtarApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TelepulesController : ControllerBase
    {
        // dependency injection tervezési minta
        private KonyvtarakContext _context;

        public TelepulesController(KonyvtarakContext context)
        {
            _context = context;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll() 
        {
            try
            {
               return Ok(_context.Telepuleseks.ToList());
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Something went wrong: " + ex.Message);
            }
        }

        [HttpGet("GetById/{id}")]
        public IActionResult GetById(int id) 
        {
            try
            {
                Telepulesek telepules = _context.Telepuleseks.FirstOrDefault(x => x.Id == id);
                if (telepules == null)
                {
                    return NotFound($"Nem található az {id} idval ellátott település.");
                }
                else 
                {
                    return Ok(telepules);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Something went wrong: " + ex.Message);
            }
        }

        [HttpPost("Post")]
        public IActionResult PostTelepulesek(Telepulesek ujTelepules) 
        {
            try
            {
                _context.Telepuleseks.Add(ujTelepules);
                _context.SaveChanges();
                return Ok("Sikeres mentés!");
            }
            catch (Exception ex) 
            {
                return StatusCode(500, "Error: " + ex.Message);
            }
        }

        [HttpPut("Put/{id}")]
        public IActionResult PutTelepulesek(Telepulesek ujTelepules, int id)
        {
            try
            {
                var toUpdate = _context.Telepuleseks.FirstOrDefault(t => t.Id == id);
                _context.ChangeTracker.Clear();
                if (toUpdate == null)
                {
                    return NotFound("Hiányzó település!");
                }
                _context.Telepuleseks.Update(ujTelepules);
                _context.SaveChanges();
                return Ok("Sikeres módosítás!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error: " + ex.Message);
            }
        }

        [HttpDelete("Delete/{id}")]
        public IActionResult DeleteTelepulesek(int id)
        {
            try
            {
                var toDelete = _context.Telepuleseks.FirstOrDefault(t => t.Id == id);
                _context.ChangeTracker.Clear();
                if (toDelete == null)
                {
                    return NotFound("Hiányzó település!");
                }
                _context.Telepuleseks.Remove(new Telepulesek { Id = id });
                _context.SaveChanges();
                return Ok("Sikeres törlés!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error: " + ex.Message);
            }
        }

        [HttpGet("GetByNevResz/{nevResz}")]
        public IActionResult GetByNevResz(string nevResz)
        {
            try
            {
                List<Telepulesek> telepules = _context.Telepuleseks.Where(x => x.TelepNev.Contains(nevResz)).ToList();
                if (telepules == null)
                {
                    return NotFound($"Nem található a {nevResz} nevű település.");
                }
                else
                {
                    return Ok(telepules);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Something went wrong: " + ex.Message);
            }
        }

        [HttpGet("TelepKonyvtar")]
        public IActionResult TelepKonyvtar()
        {
            try
            {
                return Ok(_context.Telepuleseks.Include(t => t.Konyvtaraks).ToList());
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Something went wrong: " + ex.Message);
            }
        }

        //Adott településhez jelenítse meg a település nevét és az ott lévő könyvtrak nevét és a megyéjét, név alapján keresve
        [HttpGet("TelepulesKonyvtarai/{telepNev}")]
        public IActionResult TelepulesKonyvtarai(string telepNev)
        {
            try
            {
                var result = _context.Telepuleseks
                    .Where(x => x.TelepNev == telepNev)
                    .Include(x => x.Konyvtaraks)
                    .Include(x => x.Megye)
                    .SelectMany(t => t.Konyvtaraks, (t, k) => new TelepulesKonyvtarai
                    {
                        TelepNev = t.TelepNev,
                        MegyeNev = t.Megye.MegyeNev,
                        KonyvtarNev = k.KonyvtarNev 
                    })
                    .ToList();
        
                if (result == null || !result.Any())
                {
                    return NotFound($"Nem található a(z) '{telepNev}' nevű település vagy nincsenek hozzá tartozó könyvtárak.");
                }
        
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Hiba: " + ex.Message);
            }
        }


        //aszinkron végpontok
        [HttpGet("GetAllAsync")]
        public async Task<IActionResult> GetAllAsync() 
        {
            try
            {
                var result = await _context.Telepuleseks.ToListAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Something went wrong: " + ex.Message);
            }
        }

        [HttpGet("GetByIdAsync/{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                Telepulesek telepules = await _context.Telepuleseks.FirstOrDefaultAsync(x => x.Id == id);
                if (telepules == null)
                {
                    return NotFound($"Nem található az {id} idval ellátott település.");
                }
                else
                {
                    return Ok(telepules);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Something went wrong: " + ex.Message);
            }
        }

        [HttpPost("PostAsync")]
        public async Task<IActionResult> PostTelepulesekAsync(Telepulesek ujTelepules)
        {
            try
            {
                await _context.Telepuleseks.AddAsync(ujTelepules);
                await _context.SaveChangesAsync();
                return Ok("Sikeres mentés!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error: " + ex.Message);
            }
        }

        [HttpPut("PutAsync/{id}")]
        public async Task<IActionResult> PutTelepulesekAsync(Telepulesek ujTelepules, int id)
        {
            try
            {
                var toUpdate = await _context.Telepuleseks.FirstOrDefaultAsync(t => t.Id == id);
                _context.ChangeTracker.Clear();
                if (toUpdate == null)
                {
                    return NotFound("Hiányzó település!");
                }
                _context.Telepuleseks.Update(ujTelepules);
                await _context.SaveChangesAsync();
                return Ok("Sikeres módosítás!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error: " + ex.Message);
            }
        }

        [HttpDelete("DeleteAsync/{id}")]
        public async Task<IActionResult> DeleteTelepulesekAsync(int id)
        {
            try
            {
                var toDelete = await _context.Telepuleseks.FirstOrDefaultAsync(t => t.Id == id);
                _context.ChangeTracker.Clear();
                if (toDelete == null)
                {
                    return NotFound("Hiányzó település!");
                }
                _context.Telepuleseks.Remove(new Telepulesek { Id = id });
                await _context.SaveChangesAsync();
                return Ok("Sikeres törlés!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error: " + ex.Message);
            }
        }
    }
}

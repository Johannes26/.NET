using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TodoApi.Models;
namespace TodoApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CarController: ControllerBase
    {
        private readonly ILogger<CarController> _logger;
        private readonly ApplicationDbContext _context;

        public CarController(ILogger<CarController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        
        [HttpGet]
        public async Task<IEnumerable<Car>> GetCarsAsync()
        {
            return await _context.Cars
            .Include(x=>x.Brand)
            .ToListAsync();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> addCar([FromBody] Car entity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _context.Cars.Add(entity);
            await _context.SaveChangesAsync();
            _logger.LogInformation("carro agregado correctamente");
            return Ok(entity);
        }
    }
}
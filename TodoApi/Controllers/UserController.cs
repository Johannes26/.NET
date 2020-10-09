using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {


        private readonly ILogger<UsuarioController> _logger;

        private readonly ApplicationDbContext _context;

        private readonly IConfiguration _config;

        public UsuarioController(ILogger<UsuarioController> logger, ApplicationDbContext context,IConfiguration config)
        {
            _config=config;
            _context=context;
            _logger = logger;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Registro([FromBody] Usuario entity){
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            var user = new Usuario { UserName = entity.UserName, Email = entity.Email};
            user.BirthDate=entity.BirthDate;
            user.Name=entity.Name;
            user.LastName=user.LastName;
            _context.Users.Add(entity);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Usuario Agregado Correctamente");
            return Ok(new{
                entity.Name,
                entity.LastName,
                entity.Email
            });
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login([FromBody] Usuario entity){
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(x=>x.UserName== entity.UserName);

            var userRole= await _context.UserRoles
                .SingleOrDefaultAsync(x=>x.UserId==user.Id);

            var role= userRole !=null ? await _context.Roles
                .SingleOrDefaultAsync(x=>x.Id==userRole.RoleId):null;

            if(user==null) return NotFound();//404

            var claims = new[]{
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.GivenName, user.Name)
            };
            var key = Encoding.ASCII.GetBytes(_config["IssuerKey"]);

            var token = new JwtSecurityToken(_config["Issuer"],
                null,
                claims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256Signature));
            
            var tokenHandler = new JwtSecurityTokenHandler().WriteToken(token);
            
            return Ok(new{
                access_token = tokenHandler,
                expires_in =(int)(token.ValidTo - DateTime.UtcNow).TotalSeconds,
                token_type = "Bearer",
            });
            
        }
    }
}

using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
namespace TodoApi.Models
{
    public class Usuario : IdentityUser
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; }
        [StringLength(200)]
        public string LastName { get; set; }
        [StringLength(200)]
        [Required]
        public DateTime BirthDate { get; set; }
    }
}
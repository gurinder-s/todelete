using System;
using System.ComponentModel.DataAnnotations;

namespace UserManagement.Web.Models.Users
{
    public class EditUserViewModel
    {
        public long Id { get; set; }
        [Required]
        public required string Forename { get; set; }
        [Required]
        public required string Surname { get; set; }
        [Required]
        public required string Email { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        
    }
}

using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QanooniRishta.Models
{
    public enum Gender
    {
        Male,
        Female,
        Others
    }

    public class MatchRealtion
    {

        [Ignore]
        public string? Index { get; set; }
        [Ignore]

        public bool IsFormEditable { get; set; } = true;
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters.")]
        public string FirstName { get; set; }= string.Empty;

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters.")]
        public string LastName { get; set; } = string.Empty;

        public int Age { get; set; } = 0;

        [Required(ErrorMessage = "Gender is required.")]
        public Gender Gender { get; set; }=Gender.Male;


        [Required(ErrorMessage = "Address is required.")]
        [StringLength(300, ErrorMessage = "Address cannot exceed 300 characters.")]
        public string Address { get; set; } = string.Empty;


        [StringLength(100, ErrorMessage = "Profession cannot exceed 100 characters.")]
        public string? Profession { get; set; }

        [StringLength(100, ErrorMessage = "Education details cannot exceed 100 characters.")]
        public string? Education { get; set; }

        [Phone(ErrorMessage = "Invalid phone number format.")]
        public string? ContactNumber { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string? Email { get; set; }

        [StringLength(300, ErrorMessage = "Bio cannot exceed 300 characters.")]
        public string? Bio { get; set; }

        [StringLength(100, ErrorMessage = "City name cannot exceed 100 characters.")]
        public string? City { get; set; }

        [StringLength(100, ErrorMessage = "Country name cannot exceed 100 characters.")]
        public string? Country { get; set; }

        [Required(ErrorMessage = "Caste is required.")]
        [StringLength(100, ErrorMessage = "Caste cannot exceed 100 characters.")]
        public string Caste { get; set; } = string.Empty;

    }

}

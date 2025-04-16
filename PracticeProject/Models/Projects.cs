using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace PracticeProject.Models
{
    public class Projects
    {
        public int ProjectId { get; set; }

        [Required]
        public string ProjectName { get; set; }

        [Required]
        public DateTime? ProjectDate { get; set; }

        public string ProjectDesc { get; set; }

        public string? ProjectImage { get; set; }  // Stores image path in DB

        public IFormFile? ImageFile { get; set; }  // Used for file upload
    }
}

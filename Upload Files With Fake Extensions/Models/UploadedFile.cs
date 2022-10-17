using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Upload_Files_With_Fake_Extensions.Models
{
    public class UploadedFile
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "FileName is requried")]
        [Display(Name = "FileName")]
        public string? FileName { get; set; }


        //save fake name to server
        public string? StoredFileName { get; set; }

        // content type
        public string? ContentType { get; set; }
    }
}

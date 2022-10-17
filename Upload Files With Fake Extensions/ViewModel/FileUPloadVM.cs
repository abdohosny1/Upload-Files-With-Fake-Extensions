using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Upload_Files_With_Fake_Extensions.ViewModel
{
    public class FileUPloadVM
    {
        [Required(ErrorMessage = "Upload file is requried")]
        [Display(Name = "Files")]
        public List<IFormFile> Files { get; set; }

    }
}

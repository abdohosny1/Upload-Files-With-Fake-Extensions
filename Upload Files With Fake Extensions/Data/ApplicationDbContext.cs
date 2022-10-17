using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Upload_Files_With_Fake_Extensions.Models;

namespace Upload_Files_With_Fake_Extensions.Data
{
    public class ApplicationDbContext :DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :base(options) { }


        public DbSet<UploadedFile> uploadedFiles { get; set; }

    }
}

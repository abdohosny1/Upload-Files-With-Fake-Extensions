using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Upload_Files_With_Fake_Extensions.Data;
using Upload_Files_With_Fake_Extensions.Models;
using Upload_Files_With_Fake_Extensions.ViewModel;

namespace Upload_Files_With_Fake_Extensions.Controllers
{
    public class FilesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;



        public FilesController(ApplicationDbContext context , IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;

        }

        public IActionResult Index()
        {
            var files = _context.uploadedFiles.ToList();
            return View(files);
        }

        public IActionResult Upload()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult UploadFiles(FileUPloadVM model)
        {
            if (!ModelState.IsValid)
            {
                return View("Upload");
            }
            else
            {
                var allowedExtensions = new[] { ".pdf", ".docx" };
                List<UploadedFile> uploadedFiles = new();

                foreach (var file in model.Files)
                {
                    var fakeFileName = Path.GetRandomFileName();
                    var fileExt = System.IO.Path.GetExtension(file.FileName).Substring(1);

                        UploadedFile uploadedFile = new()
                        {
                            FileName = file.FileName,
                            ContentType = file.ContentType,
                            StoredFileName = fakeFileName
                        };

                        var path = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", fakeFileName);

                        using FileStream fileStream = new(path, FileMode.Create);
                        file.CopyTo(fileStream);

                        uploadedFiles.Add(uploadedFile);
                        _context.AddRange(uploadedFiles);
                        _context.SaveChanges();

                 
                }
                return RedirectToAction(nameof(Index));



            }

        }
        [HttpGet]
        public IActionResult DownloadFile(string fileName)
        {
            var uploadFile = _context.uploadedFiles.SingleOrDefault(u => u.StoredFileName == fileName);

            if (uploadFile is null) return NotFound();

            var path = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", fileName);

            MemoryStream memoryStream = new MemoryStream();
            using FileStream fileStream = new FileStream(path, FileMode.Open);
            fileStream.CopyTo(memoryStream);

            memoryStream.Position = 0;

            return File(memoryStream, uploadFile.ContentType, uploadFile.FileName);



        }

    }
}

/*
 * public bool FileExtensionValidation(ref FileUpload fileupload, string ValidationExpression)
{
    string FileName = fileupload.PostedFile.FileName;
    string FileExtension = System.IO.Path.GetExtension(fileupload.PostedFile.FileName);
    if (FileExtension == "" & FileName != "")
        return false;
    if (FileName != "" & ValidationExpression != "")
    {
        Regex regex = new Regex(ValidationExpression);
        Match match = regex.Match(FileName);
        if (match.Success == false)
            return false;
        else
        {
            NASDataSource db = new NASDataSource();
            // Dim name() As String
            // name = FileExtension.Split(".").ToString
            string Extension;
            string formats;
            Extension = db.Hits_GetDCCaption("AllowedFileExtensions", FileExtension.Replace(".", ""), "E");
            formats = db.Hits_GetDCCaption("FileUploadRegex", "FileUploadRegexAll", "E");



            if (string.IsNullOrEmpty(Extension))
                return true;
            string[] ExtensionLst;
            ExtensionLst = Extension.Split(";");
            if (formats.Contains(";" + FileExtension.Replace(".", "") + ";"))
            {
                var fs = new BinaryReader(fileupload.PostedFile.InputStream());
                var bytes = new byte[20] { };
                fs.Read(bytes, 0, 20);
                byte[] fileHead = bytes;
                // Dim value As Byte() = New Byte(7) {}
                // Array.Reverse(bytes)
                // Array.Copy(bytes, value, 6)
                // Dim result As ULong = BitConverter.ToUInt32(value, 0)
                // Dim results = sniffer.Match(fileHead)



                StringBuilder strTemp = new StringBuilder(fileHead.Length * 2);



                foreach (byte b in fileHead)
                    strTemp.Append(Conversion.Hex(b));
                if (ExtensionLst.Contains(strTemp.ToString()) == false)
                    return false;




                return true;
            }
            else
            {
                var sniffer = new Sniffer();
                var supportedFiles = new List<Record>();



                for (var i = 0; i <= ExtensionLst.Length - 1; i++)
                    supportedFiles.Add(new Record(FileExtension.Replace(".", ""), ExtensionLst[i]));



                sniffer.Populate(supportedFiles);



                var fs = new BinaryReader(fileupload.PostedFile.InputStream());
                var bytes = new byte[20] { };
                fs.Read(bytes, 0, 20);
                byte[] fileHead = bytes;
                var results = sniffer.Match(fileHead);
                if (results.Count() == 0)
                    return false;
            }
        }
    }
    return true;
}

 */

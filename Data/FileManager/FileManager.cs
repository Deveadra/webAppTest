using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Data.FileManager
{
    public class FileManager : IFileManager
    {
        private string _imagePath;

        public FileManager(IConfiguration config)
        {
            _imagePath = config["Path:Images"];
        }

        public FileStream ImageStream(string image)
        {
            return new FileStream(Path.Combine(_imagePath, image), FileMode.Open, FileAccess.Read);
        }

        public async Task<string> SaveImage(IFormFile image)
        {
            try
            {

                //use this method so as not to individually call in file paths, which can produce errors
                var save_path = Path.Combine(_imagePath);
                if (!Directory.Exists(save_path))
                {
                    Directory.CreateDirectory(save_path);
                }

                //Internet Explorer Error will include the full file path, instead of just img.jpg
                //var fileName = image.FileName;

                //the LastIndexOf('.') is how you get the .jpeg portion (or .whatever file type)
                //How to only accept certain filetypes??? an if statement only passing through acceptable types, else supplies error message with no redirection?
                var extension = image.FileName.Substring(image.FileName.LastIndexOf('.'));
                var fileName = $"img_{DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss")}{extension}";

                //limits an IDisposable object to this scope, it gets created and when it exits, it gets disposed
                using (var fileStream = new FileStream(Path.Combine(save_path, fileName), FileMode.Create))
                {
                    await image.CopyToAsync(fileStream);
                }

                return fileName;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);

                return "Error";
            }
        }
    }
}

using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Final.util
{
    public class ImageUtil
    {

        public ImageUtil()
        {
            
        }
        // ImagePath là cái path không tuyệt đối dùng cho webpage  , imageFullPath là path lưu ảnh 
        public string SaveImage(IFormFile imageFile , string webRootPath )
        {
            if (imageFile == null)
            {
                return null;
            }

            string relativePath = "ProductImages";
            string currentTime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            string imageFileName = currentTime + Path.GetExtension(imageFile.FileName);
            string imageFullPath = Path.Combine(webRootPath, relativePath, imageFileName);

            using (var stream = File.Create(imageFullPath))
            {
                imageFile.CopyTo(stream);
            }

            return Path.Combine(Path.DirectorySeparatorChar.ToString(), relativePath, imageFileName);  // Return the full path if needed
        }
    }
}

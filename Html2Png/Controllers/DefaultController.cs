using System;
using System.Diagnostics;
using System.Net;
using System.Web.Mvc;

namespace Html2Png.Controllers
{
    using IOFile = System.IO.File;

    public class DefaultController : Controller
    {
        [HttpPost, ValidateInput(false)]
        public ActionResult Html2Png(int width, int height, string html)
        {
            // fit to max 1920 x 1080.
            if (width > 1920)
            {
                var scale = 1920.0 / width;
                width = 1920;
                height = (int)(height * scale);
            }
            if (height > 1080)
            {
                var scale = 1080.0 / height;
                width = (int)(width * scale);
                height = 1080;
            }

            // Realize html string to file.
            var htmlFilePath = Server.MapPath("~/App_Data/" + Guid.NewGuid().ToString("N") + ".html");
            IOFile.WriteAllText(htmlFilePath, html);

            // Prepare for save image file.
            var imageFilePath = Server.MapPath("~/App_Data/" + Guid.NewGuid().ToString("N") + ".png");
            var imageBytes = default(byte[]);
            
            try
            {
                // Launch PhantomJS and take a screen shot into image file.
                var procInfo = new ProcessStartInfo
                {
                    FileName = Server.MapPath("~/bin/phantomjs.exe"),
                    Arguments = string.Format("\"{0}\" {1} {2} \"{3}\" \"{4}\"",
                        Server.MapPath("~/bin/take-screen-shot.js"),
                        width,
                        height,
                        htmlFilePath,
                        imageFilePath)
                };
                var proc = Process.Start(procInfo);
                proc.WaitForExit();
            }
            finally
            {
                // Sweep the html file.
                if (IOFile.Exists(htmlFilePath))
                {
                    try { IOFile.Delete(htmlFilePath); }
                    catch (Exception)
                    {
                        // TODO: Report
                    }
                }
                
                // Read image file into memory and sweep the image file.
                if (IOFile.Exists(imageFilePath))
                {
                    try
                    {
                        imageBytes = IOFile.ReadAllBytes(imageFilePath);
                        IOFile.Delete(imageFilePath);
                    }
                    catch (Exception)
                    {
                        // TODO: Report
                    }
                }
            }

            // Respond to client.
            if (imageBytes != null)
            {
                return new FileContentResult(imageBytes, "image/png");
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }
    }
}
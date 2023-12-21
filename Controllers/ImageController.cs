using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Imagegallery_ui.Controllers
{
    public class ImageController : Controller
    {
        string imagegalaryapi = "http://localhost:5068";
        private readonly string rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Uploads");
        public async Task<IActionResult> Index()
        {
            List<string> images = new List<string>();
            using (var client = new HttpClient())
            {
              
              
                client.BaseAddress = new Uri(imagegalaryapi);
                client.DefaultRequestHeaders.Clear();
               
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
             
                HttpResponseMessage Res = await client.GetAsync("api/Images/GetALL?rootpath=" + rootPath);
             
                if (Res.IsSuccessStatusCode)
                {
                   
                    var EmpResponse = Res.Content.ReadAsStringAsync().Result;
                 
                    images = JsonConvert.DeserializeObject<List<string>>(EmpResponse);

                }
                
            }

            return View(images);
        }
        [HttpPost]
        public async Task<IActionResult> Index(IFormFile file)
        { 

           if (file != null) 
            {
                var path = "\\" + Path.Combine(rootPath, Guid.NewGuid() + Path.GetExtension(file.FileName));
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(imagegalaryapi);
                    client.DefaultRequestHeaders.Clear();

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("multipart/form-data"));

                    var myContent = JsonConvert.SerializeObject(file);
                    var httpContent = new StringContent(myContent);
                    HttpResponseMessage Res = await client.PostAsync("api/Images/UploadImage?path=" + path , httpContent);
                   

                    if (Res.IsSuccessStatusCode)
                    { 
                        return RedirectToAction("Index");
                    }
                }
          
                return RedirectToAction("Index");
            }
            return View();
        }
        public async Task<IActionResult> Delete(string imageName)
        {
            if (!string.IsNullOrEmpty(imageName))
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(imagegalaryapi);
                    client.DefaultRequestHeaders.Clear();

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage Res = await client.GetAsync("api/Images/DeleteImage?imageName=" + rootPath);

                    if (Res.IsSuccessStatusCode)
                    {

                        return RedirectToAction("Index");

                    }
                }
            }
            return View();
        }
        public async Task<IActionResult> SearcchImages(string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(imagegalaryapi);
                    client.DefaultRequestHeaders.Clear();

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage Res = await client.GetAsync("api/Images/GetImageByFilePath?imageName=" + searchString);

                    if (Res.IsSuccessStatusCode)
                    {

                        return RedirectToAction(Res.Content.ToString());

                    }
                }
            }

            return View();
        }
    }
}

using BlogProject.Areas.Admin.Models;
using Business.Abstract;
using Entities.Concrate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogProject.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize]
public class ImageController : Controller
{
    private readonly IImageService _imagesService;

    public ImageController(IImageService imagesService)
    {
        _imagesService = imagesService;
    }

    public IActionResult ImageList()
    {
        var values = _imagesService.GetAll();
        return View(values);
    }
    
    [HttpGet]
    public IActionResult AddImage()
    {
        return View();
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddImage(ImageViewModel model)
    {
        string imagePath = null;

        if (model?.Image != null)
        {
            var originalFileName = Path.GetFileNameWithoutExtension(model.Image.FileName);
            var extensions = Path.GetExtension(model.Image.FileName);
            var imageName = originalFileName + extensions;
            
            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/upload");
            var location = Path.Combine(uploadPath, imageName);
            
            int counter = 1;
            while (System.IO.File.Exists(location))
            {
                imageName = $"{originalFileName}_{counter}{extensions}";
                location = Path.Combine(uploadPath, imageName);
                counter++;
            }
            
            using var stream = new FileStream(location, FileMode.Create);
            model.Image.CopyTo(stream);

            imagePath = "/images/upload/" + imageName;
        }
        var imageEntity = new Image
        {
            ImageUrl = imagePath
        };

        _imagesService.Insert(imageEntity);
        return RedirectToAction("ImageList");
    }
    
    [HttpGet]
    public IActionResult Update(int id)
    {
        var value = _imagesService.GetById(id);
        if (value == null)
        {
            return RedirectToAction("ImageList");
        }

        var model = new ImageViewModel()
        {
            ImagesId = value.ImageId,
            ImagePath = value.ImageUrl
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Update(ImageViewModel model)
    {
        if (ModelState.IsValid)
        {
            string imagePath = model.ImagePath; 

            if (model?.Image != null)
            {
                var originalFileName = Path.GetFileNameWithoutExtension(model.Image.FileName);
                var extension = Path.GetExtension(model.Image.FileName);
                var imageName = originalFileName + extension;
                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/upload");
                var location = Path.Combine(uploadPath, imageName);

                int counter = 1;
                while (System.IO.File.Exists(location))
                {
                    imageName = $"{originalFileName}_{counter}{extension}";
                    location = Path.Combine(uploadPath, imageName);
                    counter++;
                }

                using var stream = new FileStream(location, FileMode.Create);
                model.Image.CopyTo(stream);

                imagePath = "/images/upload/" + imageName;
            }

            var imageEntity = new Image()
            {
                ImageId = model.ImagesId,
                ImageUrl = imagePath
            };

            _imagesService.Update(imageEntity);
            return RedirectToAction("ImageList");
        }
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
        var value = _imagesService.GetById(id);
        if (value != null)
        {
            _imagesService.Delete(value);
        }
        return RedirectToAction("ImageList");
    }
}
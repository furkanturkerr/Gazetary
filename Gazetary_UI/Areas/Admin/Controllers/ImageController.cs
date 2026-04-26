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
    private readonly IWebHostEnvironment _env;

    public ImageController(IImageService imagesService, IWebHostEnvironment env)
    {
        _imagesService = imagesService;
        _env = env;
    }

    public IActionResult ImageList(int page = 1)
    {
        int pageSize = 8;

        var allImages = _imagesService.GetAll()
            .OrderByDescending(x => x.CreatedDate);

        var totalCount = allImages.Count();

        var images = allImages
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = (int)Math.Ceiling((double)totalCount / pageSize);

        return View(images);
    }

    [HttpGet]
    public IActionResult AddImage()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddImage(ImageViewModel model)
    {
        try
        {
            if (model?.Image == null)
            {
                TempData["Error"] = "Dosya seçilmedi.";
                return View(model);
            }

            var uploadPath = Path.Combine(_env.WebRootPath, "images", "upload");

            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            var originalFileName = Path.GetFileName(model.Image.FileName);
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(originalFileName);
            var extension = Path.GetExtension(originalFileName).ToLowerInvariant();

            var imageName = originalFileName;
            var location = Path.Combine(uploadPath, imageName);

            int count = 1;
            while (System.IO.File.Exists(location))
            {
                imageName = $"{fileNameWithoutExtension}({count}){extension}";
                location = Path.Combine(uploadPath, imageName);
                count++;
            }

            using (var stream = new FileStream(location, FileMode.Create))
            {
                await model.Image.CopyToAsync(stream);
            }

            var imagePath = "/images/upload/" + imageName;

            _imagesService.Insert(new Image
            {
                ImageUrl = imagePath
            });

            return RedirectToAction("ImageList");
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"HATA: {ex.Message}";
            return View(model);
        }
    }

    [HttpGet]
    public IActionResult Update(int id)
    {
        var value = _imagesService.GetById(id);
        if (value == null)
            return RedirectToAction("ImageList");

        var model = new ImageViewModel
        {
            ImagesId = value.ImageId,
            ImagePath = value.ImageUrl
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(ImageViewModel model)
    {
        try
        {
            if (!ModelState.IsValid)
                return View(model);

            string imagePath = model.ImagePath;

            if (model?.Image != null)
            {
                var uploadPath = Path.Combine(_env.WebRootPath, "images", "upload");

                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                var originalFileName = Path.GetFileName(model.Image.FileName);
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(originalFileName);
                var extension = Path.GetExtension(originalFileName).ToLowerInvariant();

                var imageName = originalFileName;
                var location = Path.Combine(uploadPath, imageName);

                int count = 1;
                while (System.IO.File.Exists(location))
                {
                    imageName = $"{fileNameWithoutExtension}({count}){extension}";
                    location = Path.Combine(uploadPath, imageName);
                    count++;
                }

                using (var stream = new FileStream(location, FileMode.Create))
                {
                    await model.Image.CopyToAsync(stream);
                }

                imagePath = "/images/upload/" + imageName;
            }

            _imagesService.Update(new Image
            {
                ImageId = model.ImagesId,
                ImageUrl = imagePath
            });

            return RedirectToAction("ImageList");
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"HATA: {ex.Message}";
            return View(model);
        }
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
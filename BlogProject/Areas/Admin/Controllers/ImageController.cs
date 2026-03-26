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
    public async Task<IActionResult> AddImage(ImageViewModel model)
    {
        try
        {
            string imagePath = null;

            if (model?.Image == null)
            {
                TempData["Error"] = "HATA: Model.Image NULL geldi. Dosya seçilmedi veya form düzgün gönderilmedi.";
                return View(model);
            }

            var extension = Path.GetExtension(model.Image.FileName).ToLowerInvariant();
            var imageName = Guid.NewGuid().ToString() + extension;
            var uploadPath = Path.Combine(_env.WebRootPath, "images", "upload");

            TempData["Debug"] = $"WebRootPath: {_env.WebRootPath} | UploadPath: {uploadPath} | KlasörVarMı: {Directory.Exists(uploadPath)} | Dosya: {imageName}";

            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
                TempData["Debug"] += " | Klasör OLUŞTURULDU";
            }

            var location = Path.Combine(uploadPath, imageName);

            using var stream = new FileStream(location, FileMode.Create);
            await model.Image.CopyToAsync(stream);
            await stream.FlushAsync();

            imagePath = "/images/upload/" + imageName;

            _imagesService.Insert(new Image { ImageUrl = imagePath });
            return RedirectToAction("ImageList");
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"HATA: {ex.Message} | Inner: {ex.InnerException?.Message} | Stack: {ex.StackTrace}";
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
                var extension = Path.GetExtension(model.Image.FileName).ToLowerInvariant();
                var imageName = Guid.NewGuid().ToString() + extension;
                var uploadPath = Path.Combine(_env.WebRootPath, "images", "upload");

                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);

                var location = Path.Combine(uploadPath, imageName);

                using var stream = new FileStream(location, FileMode.Create);
                await model.Image.CopyToAsync(stream);
                await stream.FlushAsync();

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
            TempData["Error"] = $"HATA: {ex.Message} | Inner: {ex.InnerException?.Message} | Stack: {ex.StackTrace}";
            return View(model);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
        var value = _imagesService.GetById(id);
        if (value != null)
            _imagesService.Delete(value);

        return RedirectToAction("ImageList");
    }
}
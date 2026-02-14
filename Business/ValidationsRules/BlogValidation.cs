using Entities.Concrate;
using FluentValidation;

namespace Business.ValidationsRules;

public class BlogValidation : AbstractValidator<BlogPost>
{
    public BlogValidation()
    {
        RuleFor(blogPost => blogPost.Title).NotEmpty().WithMessage("Başlık boş olamaz");
        RuleFor(blogPost => blogPost.CategoryId).NotEmpty().WithMessage("Kategori boş olamaz");
        RuleFor(blogPost => blogPost.Content).NotEmpty().WithMessage("Boş olamaz");
        RuleFor(blogPost => blogPost.ImageUrl).NotEmpty().WithMessage("Görsel zorunludur");
        RuleFor(blogPost => blogPost.ImageDescription).NotEmpty().WithMessage("Görsel açıklaması zorunludur");
        RuleFor(blogPost => blogPost.Description).NotEmpty().WithMessage("Açıklama boş bırakılamaz");
        RuleFor(blogPost => blogPost.Slug).NotEmpty().WithMessage("Kısa ad boş bırakılamaz");
    }
}
using Entities.Concrate;
using FluentValidation;

namespace Business.ValidationsRules;

public class CategoryValidation : AbstractValidator<Category>
{
    public CategoryValidation()
    {
        RuleFor(x=>x.CategoryName).NotEmpty().WithMessage("Kategori adı boş olamaz");
        RuleFor(x=>x.CategorySlug).NotEmpty().WithMessage("Kategori kısa adı boş olamaz");
    }
}
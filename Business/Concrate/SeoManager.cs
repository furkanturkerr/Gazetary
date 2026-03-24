using Business.Abstract;
using Entities.Concrate;

namespace Business.Concrate;

public class SeoManager : ISeoService
{
    private const string BaseUrl = "https://gazetary.com";

    public void SetPostSeo(IDictionary<string, object?> viewData, BlogPost post)
    {
        viewData["MetaTitle"]      = !string.IsNullOrEmpty(post.MetaTitle)       ? post.MetaTitle       : post.Title;
        viewData["MetaDesc"]       = !string.IsNullOrEmpty(post.MetaDescription) ? post.MetaDescription : post.Description;
        viewData["MetaKeywords"]   = !string.IsNullOrEmpty(post.MetaKeywords)    ? post.MetaKeywords    : post.Category.CategoryName;
        viewData["CanonicalUrl"]   = $"{BaseUrl}/{post.Category.CategorySlug}/{post.Slug}";
        viewData["OgImage"]        = post.ImageUrl;
        viewData["OgType"]         = "article";
        viewData["PublishedTime"]  = post.CreatedDate.ToString("yyyy-MM-ddTHH:mm:ssZ");
        viewData["ArticleSection"] = post.Category.CategoryName;
    }

    public void SetCategorySeo(IDictionary<string, object?> viewData, Category category, string? ogImage = null)
    {
        viewData["MetaTitle"]    = $"{category.CategoryName} Haberleri — Gazetary";
        viewData["MetaDesc"]     = $"Gazetary'de {category.CategoryName} kategorisindeki en güncel haberler.";
        viewData["MetaKeywords"] = $"{category.CategoryName.ToLower()}, {category.CategoryName.ToLower()} haberleri, gazetary";
        viewData["CanonicalUrl"] = $"{BaseUrl}/{category.CategorySlug}";
        viewData["OgImage"]      = ogImage ?? $"{BaseUrl}/img/og-image.jpg";
        viewData["OgType"]       = "website";
    }

    public void SetCategoryListSeo(IDictionary<string, object?> viewData)
    {
        viewData["MetaTitle"]    = "Tüm Kategoriler — Gazetary";
        viewData["MetaDesc"]     = "Gazetary'deki tüm haber kategorilerini keşfedin.";
        viewData["MetaKeywords"] = "gazetary kategoriler, teknoloji, yazılım, ekonomi, spor";
        viewData["CanonicalUrl"] = $"{BaseUrl}/kategoriler";
        viewData["OgType"]       = "website";
    }
}
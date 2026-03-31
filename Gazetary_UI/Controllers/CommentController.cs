using System.Text;
using Business.Abstract;
using Entities.Concrate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Newtonsoft.Json;

namespace BlogProject.Controllers;

[Authorize]
public class CommentController : Controller
{
    private readonly ICommentService _commentService;
    private readonly ICommentLikeService _commentLikeService;
    private readonly UserManager<AppUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<CommentController> _logger;


    public CommentController(
        ICommentService commentService,
        ICommentLikeService commentLikeService,
        UserManager<AppUser> userManager,
        IConfiguration configuration,
        IHttpClientFactory httpClientFactory,
        ILogger<CommentController> logger)
    {
        _commentService     = commentService;
        _commentLikeService = commentLikeService;
        _userManager        = userManager;
        _configuration = configuration;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    [HttpPost]
    [IgnoreAntiforgeryToken]
    [EnableRateLimiting("comment-limit")]
    public async Task<IActionResult> AddComment(Comment comment)
    {
        if (string.IsNullOrWhiteSpace(comment.Content))
            return Json(new { success = false, message = "Yorum boş olamaz." });

        if (comment.Content.Length > 1000)
            return Json(new { success = false, message = "Yorum en fazla 1000 karakter olabilir." });

        if (comment.BlogPostId <= 0)
            return Json(new { success = false, message = "Geçersiz yazı." });

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return Json(new { success = false, message = "Giriş yapmanız gerekiyor." });



        comment.Content     = comment.Content.Trim();
        comment.CreatedDate = DateTime.Now;
        comment.Name        = user.NameSurname;
        comment.AppUserId   = user.Id;
        comment.IsStatus    = true;
        
        var sentiment = await AnalyzeSentiment(comment.Content);
        comment.Analysis = sentiment;

        if (sentiment == "RED")
        {
            return Json(new
            {
                success = false,
                message = "Yorumunuz yayın politikalarına uygun değil."
            });
        }

        _commentService.Insert(comment);

        return Json(new
        {
            success   = true,
            commentId = comment.CommentId,
            name      = comment.Name,
            content   = comment.Content,
            date      = comment.CreatedDate.ToString("dd.MM.yyyy HH:mm"),
            userId    = comment.AppUserId
        });
    }

    [HttpPost]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> DeleteComment(int commentId)
    {
        if (commentId <= 0)
            return Json(new { success = false, message = "Geçersiz yorum." });

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return Json(new { success = false, message = "Giriş yapmanız gerekiyor." });

        var comment = _commentService.GetById(commentId);
        if (comment == null)
            return Json(new { success = false, message = "Yorum bulunamadı." });

        if (comment.AppUserId != user.Id)
            return Json(new { success = false, message = "Bu yorum size ait değil." });

        var likes = _commentLikeService.GetByCommentId(commentId);

        foreach (var like in likes)
            _commentLikeService.Delete(like);

        _commentService.Delete(comment);
        return Json(new { success = true });
    }

    [HttpPost]
    [IgnoreAntiforgeryToken]
    [EnableRateLimiting("like-limit")]
    public async Task<IActionResult> ToggleLike(int commentId)
    {
        if (commentId <= 0)
            return Json(new { success = false, message = "Geçersiz yorum." });

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return Json(new { success = false, message = "Giriş yapmanız gerekiyor." });

        var comment = _commentService.GetById(commentId);
        if (comment == null)
            return Json(new { success = false, message = "Yorum bulunamadı." });

        var existing = _commentLikeService.GetByCommentAndUser(commentId, user.Id);

        if (existing != null)
        {
            _commentLikeService.Delete(existing);
            var count = _commentLikeService.GetLikeCount(commentId);
            return Json(new { success = true, liked = false, count });
        }

        _commentLikeService.Insert(new CommentLike
        {
            CommentId = commentId,
            AppUserId = user.Id
        });

        var newCount = _commentLikeService.GetLikeCount(commentId);
        return Json(new { success = true, liked = true, count = newCount });
    }
    
    private async Task<string> AnalyzeSentiment(string text)
    {
        try
        {
            var key = _configuration["Api:Url"];
            if (string.IsNullOrEmpty(key))
            {
                _logger.LogWarning("OpenAI API key is not configured. Skipping sentiment analysis.");
                return "ONAY"; // Fallback: approve if API not configured
            }

            var client = _httpClientFactory.CreateClient();
            client.Timeout = TimeSpan.FromSeconds(10);
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {key}");

            var requestBody = new
            {
                model = "gpt-4o-mini",
                messages = new[]
                {
                    new
                    {
                        role = "system",
                        content = @"Sen bir yorum moderasyon sistemisin.
Görevin:
Kullanıcıdan gelen yorumu analiz etmek ve aşağıdaki kurallara göre karar vermek.

RED (sil):
- Küfür, hakaret, ağır argo
- Nefret söylemi (ırk, din, cinsiyet vb.)
- Şiddet veya tehdit içeren ifadeler
- Spam, reklam veya anlamsız içerik
- Tamamen boş veya çok kısa (örn: 'aaa', '123') yorumlar

ONAY (yayınla):
- Normal, saygılı, anlamlı yorumlar

KURALLAR:
- SADECE TEK KELİME DÖN
- AÇIKLAMA YAZMA
- NOKTALAMA KULLANMA
- SADECE ŞUNLARDAN BİRİNİ DÖN:

ONAY
RED

Başka hiçbir şey yazma."
                    },
                    new
                    {
                        role = "user",
                        content = text
                    }
                }
            };

            string json = JsonConvert.SerializeObject(requestBody);
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(8));
            HttpResponseMessage response = await client.PostAsync(
                "https://api.openai.com/v1/chat/completions",
                content,
                cts.Token
            );

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<dynamic>(responseBody);
                string sentiment = result?.choices?[0]?.message?.content?.ToString()?.Trim().ToUpper();

                return sentiment == "RED" || sentiment == "ONAY" ? sentiment : "ONAY";
            }

            _logger.LogWarning("OpenAI API returned non-success status: {StatusCode}", response.StatusCode);
            return "ONAY"; // Fallback: approve if API fails
        }
        catch (TaskCanceledException ex)
        {
            _logger.LogError(ex, "OpenAI API request timed out for comment analysis");
            return "ONAY"; // Fallback: approve on timeout
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request failed during sentiment analysis");
            return "ONAY"; // Fallback: approve on network error
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during sentiment analysis");
            return "ONAY"; // Fallback: approve on any error
        }
    }
}
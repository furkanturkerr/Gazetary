using System.Xml.Linq;
using Business.Abstract;
using Dtos;

namespace Business.Concrate;

public class RssManager : IRssService
{
    private readonly HttpClient _httpClient;

    public RssManager(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public List<RssSourceDto> GetRssSources()
{
    return new List<RssSourceDto>
    {
        new RssSourceDto(1, "NTV", "Gündem", "https://www.ntv.com.tr/gundem.rss"),
        new RssSourceDto(2, "NTV", "Türkiye", "https://www.ntv.com.tr/turkiye.rss"),
        new RssSourceDto(3, "NTV", "Dünya", "https://www.ntv.com.tr/dunya.rss"),
        new RssSourceDto(4, "NTV", "Ekonomi", "https://www.ntv.com.tr/ekonomi.rss"),
        new RssSourceDto(5, "NTV", "Spor", "https://www.ntv.com.tr/spor.rss"),
        new RssSourceDto(6, "NTV", "Teknoloji", "https://www.ntv.com.tr/teknoloji.rss"),

        new RssSourceDto(7, "AA", "Güncel", "https://www.aa.com.tr/tr/rss/default?cat=guncel"),

        new RssSourceDto(8, "Cumhuriyet", "Son Dakika", "http://www.cumhuriyet.com.tr/rss/son_dakika.xml"),

        new RssSourceDto(9, "Habertürk", "Genel", "http://www.haberturk.com/rss"),

        new RssSourceDto(10, "Hürriyet", "Anasayfa", "http://www.hurriyet.com.tr/rss/anasayfa"),
        new RssSourceDto(11, "Hürriyet", "Gündem", "http://www.hurriyet.com.tr/rss/gundem"),
        new RssSourceDto(12, "Hürriyet", "Ekonomi", "http://www.hurriyet.com.tr/rss/ekonomi"),
        new RssSourceDto(13, "Hürriyet", "Dünya", "http://www.hurriyet.com.tr/rss/dunya"),
        new RssSourceDto(14, "Hürriyet", "Spor", "http://www.hurriyet.com.tr/rss/spor"),
        new RssSourceDto(15, "Hürriyet", "Teknoloji", "http://www.hurriyet.com.tr/rss/teknoloji"),

        new RssSourceDto(16, "Milliyet", "Gündem", "http://www.milliyet.com.tr/rss/rssNew/gundemRss.xml"),
        new RssSourceDto(17, "Milliyet", "Dünya", "http://www.milliyet.com.tr/rss/rssNew/dunyaRss.xml"),
        new RssSourceDto(18, "Milliyet", "Ekonomi", "http://www.milliyet.com.tr/rss/rssNew/ekonomiRss.xml"),
        new RssSourceDto(19, "Milliyet", "Siyaset", "http://www.milliyet.com.tr/rss/rssNew/siyasetRss.xml"),
        new RssSourceDto(20, "Milliyet", "Teknoloji", "http://www.milliyet.com.tr/rss/rssNew/teknolojiRss.xml"),
        new RssSourceDto(21, "Milliyet", "Son Dakika", "http://www.milliyet.com.tr/rss/rssNew/SonDakikaRss.xml"),

        new RssSourceDto(22, "Sabah", "Anasayfa", "https://www.sabah.com.tr/rss/anasayfa.xml"),
        new RssSourceDto(23, "Sabah", "Son Dakika", "https://www.sabah.com.tr/rss/sondakika.xml"),
        new RssSourceDto(24, "Sabah", "Gündem", "https://www.sabah.com.tr/rss/gundem.xml"),
        new RssSourceDto(25, "Sabah", "Dünya", "https://www.sabah.com.tr/rss/dunya.xml"),
        new RssSourceDto(26, "Sabah", "Ekonomi", "https://www.sabah.com.tr/rss/ekonomi.xml"),
        new RssSourceDto(27, "Sabah", "Spor", "https://www.sabah.com.tr/rss/spor.xml"),
        new RssSourceDto(28, "Sabah", "Teknoloji", "https://www.sabah.com.tr/rss/teknoloji.xml"),

        new RssSourceDto(29, "CNN Türk", "Tüm Haberler", "https://www.cnnturk.com/feed/rss/all/news"),
        new RssSourceDto(30, "CNN Türk", "Türkiye", "https://www.cnnturk.com/feed/rss/turkiye/news"),
        new RssSourceDto(31, "CNN Türk", "Dünya", "https://www.cnnturk.com/feed/rss/dunya/news"),
        new RssSourceDto(32, "CNN Türk", "Ekonomi", "https://www.cnnturk.com/feed/rss/ekonomi/news"),
        new RssSourceDto(33, "CNN Türk", "Spor", "https://www.cnnturk.com/feed/rss/spor/news"),
        new RssSourceDto(34, "CNN Türk", "Bilim Teknoloji", "https://www.cnnturk.com/feed/rss/bilim-teknoloji/news"),

        new RssSourceDto(35, "A Haber", "Anasayfa", "https://www.ahaber.com.tr/rss/anasayfa.xml"),
        new RssSourceDto(36, "A Haber", "Gündem", "https://www.ahaber.com.tr/rss/gundem.xml"),
        new RssSourceDto(37, "A Haber", "Ekonomi", "https://www.ahaber.com.tr/rss/ekonomi.xml"),
        new RssSourceDto(38, "A Haber", "Dünya", "https://www.ahaber.com.tr/rss/dunya.xml"),
        new RssSourceDto(39, "A Haber", "Teknoloji", "https://www.ahaber.com.tr/rss/teknoloji.xml"),

        new RssSourceDto(40, "TRT Haber", "Son Dakika", "http://www.trthaber.com/sondakika.rss"),

        new RssSourceDto(41, "BBC Türkçe", "Genel", "http://feeds.bbci.co.uk/turkce/rss.xml"),
        new RssSourceDto(42, "DW Türkçe", "Genel", "http://rss.dw.com/rdf/rss-tur-all"),

        new RssSourceDto(43, "Ensonhaber", "Genel", "http://www.ensonhaber.com/rss/ensonhaber.xml"),

        new RssSourceDto(44, "Mynet", "Son Dakika", "http://www.mynet.com/haber/rss/sondakika"),
        new RssSourceDto(45, "Mynet", "Teknoloji", "http://www.mynet.com/haber/rss/kategori/teknoloji/"),
        new RssSourceDto(46, "Mynet", "Dünya", "http://www.mynet.com/haber/rss/kategori/dunya/"),

        new RssSourceDto(47, "TOBB", "Haberler", "https://www.tobb.org.tr/Sayfalar/RssFeeder.php?List=Haberler"),
        new RssSourceDto(48, "TOBB", "Duyurular", "https://www.tobb.org.tr/Sayfalar/RssFeeder.php?List=DuyurularListesi"),

        new RssSourceDto(49, "Bigpara", "Ekonomi", "http://bigpara.hurriyet.com.tr/rss/"),
        new RssSourceDto(50, "Ekosayir", "Piyasalar", "http://www.ekoseyir.com/rss/piyasalar/248.xml")
    };
}

    public async Task<List<RssNewsDto>> GetNewsFromFeedAsync(string rssUrl)
    {
        var result = new List<RssNewsDto>();

        try
        {
            var xmlContent = await _httpClient.GetStringAsync(rssUrl);
            var doc = XDocument.Parse(xmlContent);

            XNamespace media = "http://search.yahoo.com/mrss/";

            var items = doc.Descendants("item").ToList();

            foreach (var item in items)
            {
                var title = item.Element("title")?.Value?.Trim();
                var link = item.Element("link")?.Value?.Trim();
                var description = item.Element("description")?.Value?.Trim();
                var pubDateText = item.Element("pubDate")?.Value?.Trim();
                var category = item.Element("category")?.Value?.Trim();

                DateTime? publishDate = null;
                if (DateTime.TryParse(pubDateText, out var parsedDate))
                    publishDate = parsedDate;

                string imageUrl = null;

                // media:content
                imageUrl = item.Elements(media + "content")
                    .Attributes("url")
                    .Select(x => x.Value)
                    .FirstOrDefault();

                // media:thumbnail
                if (string.IsNullOrEmpty(imageUrl))
                {
                    imageUrl = item.Elements(media + "thumbnail")
                        .Attributes("url")
                        .Select(x => x.Value)
                        .FirstOrDefault();
                }

                // enclosure
                if (string.IsNullOrEmpty(imageUrl))
                {
                    imageUrl = item.Elements("enclosure")
                        .Where(x => x.Attribute("type")?.Value?.StartsWith("image") == true)
                        .Attributes("url")
                        .Select(x => x.Value)
                        .FirstOrDefault();
                }

                result.Add(new RssNewsDto
                {
                    Title = title,
                    Link = link,
                    Description = description,
                    ImageUrl = imageUrl,
                    PublishDate = publishDate,
                    Source = rssUrl,
                    Category = category
                });
            }
        }
        catch
        {
            return new List<RssNewsDto>();
        }

        return result
            .Where(x => !string.IsNullOrWhiteSpace(x.Title))
            .OrderByDescending(x => x.PublishDate)
            .ToList();
    }
}
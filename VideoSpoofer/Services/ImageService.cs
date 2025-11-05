namespace VideoSpoofer.Services;

public class ImageService
{
    private readonly HttpClient _httpClient;

    public ImageService()
    {
        _httpClient = new HttpClient();
    }

    /// <summary>
    /// Will download a random image and save it in the current directory as bg.jpg
    /// </summary>
    /// <returns></returns>
    public async Task<string> GetImageAsync()
    {
        var stream = await _httpClient.GetStreamAsync("https://picsum.photos/1080/1920");
        var filePath = Path.Join(Directory.GetCurrentDirectory(), "bg.jpg");
        await using var fs = File.Create(filePath);
        await stream.CopyToAsync(fs);
        return filePath;
    }
}
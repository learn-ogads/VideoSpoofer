using Spectre.Console;

namespace VideoSpoofer.Services;

public class ImageService
{
    private readonly HttpClient _httpClient;

    public ImageService()
    {
        _httpClient = new HttpClient();
    }

    public async Task GetImageAsync()
    {
        AnsiConsole.MarkupLine("[green]Getting image...[/]");
        try
        {
            // var stream = await _httpClient.GetStreamAsync("https://random.imagecdn.app/1080/1920");
            var stream = await _httpClient.GetStreamAsync("https://source.unsplash.com/random/1080x1920?sig=");
            var filePath = Path.Join(Directory.GetCurrentDirectory(), "bg.jpg");
            AnsiConsole.MarkupLine($"[green]Downloaded image to: {filePath}[/]");
            await using var fs = File.Create(filePath);
            await stream.CopyToAsync(fs);
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]{ex.Message}[/]");
            AnsiConsole.MarkupLine("[red]Failed to fetch image[/]");
            AnsiConsole.MarkupLine("[red]Skipping creating this video[/]");
        }
    }
}
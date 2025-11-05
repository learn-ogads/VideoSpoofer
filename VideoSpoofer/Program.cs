using Spectre.Console;
using VideoSpoofer.Models;
using VideoSpoofer.Services;

AnsiConsole.Write(
    new FigletText("VideoSpoofer")
        .LeftJustified()
        .Color(Color.Gold1));

AnsiConsole.MarkupLine("[red3_1]This program only works on Windows[/]");

/**
 * Install ffmpeg if it doesn't exist
 */
var ffmpegDownload = new FFmpegDownloadService();
var installed = await ffmpegDownload.InstallAsync();

if (!installed)
{
    AnsiConsole.MarkupLine("[red3_1]Failed to install ffmpeg. Are you on Windows?[/]");
}

/**
 * Get the input video path.
 * If it doesn't exist, close the application.
 */
AnsiConsole.MarkupLine("[red]Make sure your video dimensions are 1080x1920[/]");
var fileName = AnsiConsole.Ask<string>("What's the video filename (ex: test.mp4)?");
var videoPath = Path.Join(Directory.GetCurrentDirectory(), fileName);
if (!File.Exists(videoPath))
{
    AnsiConsole.MarkupLine("[red]The provided video file name does not exist![/]");
    Console.WriteLine("Press any key to continue...");
    Console.ReadKey();
    return;
}

/**
 * Get the total amount of videos we want to create
 */
var videoCount = AnsiConsole.Ask<int>("How many videos do you want to create? (ex: 10)");

/**
 * Loop through and create all videos
 */
var imgService = new ImageService();
for (var i = 0; i < videoCount; i++)
{
    AnsiConsole.MarkupLine("[yellow]Creating a new video...[/]");

    try
    {
        await imgService.GetImageAsync();
    }
    catch (Exception ex)
    {
        AnsiConsole.MarkupLine($"[red]{ex.Message}[/]");
        AnsiConsole.MarkupLine("[red]Failed to fetch image[/]");
        AnsiConsole.MarkupLine("[red]Skipping creating this video[/]");
        continue;
    }

    /**
     * Create the video details
     */
    var details = new VideoDetails
    {
        VideoPath = videoPath,
        ImagePath = Path.Join(Directory.GetCurrentDirectory(), "bg.jpg")
    };

    /**
     * Create the video using ffmpeg
     */
    try
    {
        var success = await FFmpegService.CreateVideoAsync(details);
        if (!success)
        {
            AnsiConsole.MarkupLine("[red]Failed to create the video using ffmpeg[/]");
            continue;
        }
    }
    catch (Exception ex)
    {
        AnsiConsole.MarkupLine($"[red]{ex.Message}[/]");
        AnsiConsole.MarkupLine("[red]Failed to create the video using ffmpeg... Trying the next video[/]");
        continue;
    }

    AnsiConsole.MarkupLine("[green]Created a video successfully![/]");
}

AnsiConsole.MarkupLine("[green]Finished creating all videos[/]");
Console.WriteLine("Press any key to continue...");
Console.ReadKey();
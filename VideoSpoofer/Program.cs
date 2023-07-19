using Spectre.Console;
using VideoSpoofer.Models;
using VideoSpoofer.Services;

AnsiConsole.Write(
    new FigletText("VideoSpoofer")
        .LeftJustified()
        .Color(Color.Gold1));

var ffmpegDownload = new FFmpegDownloadService();
await ffmpegDownload.InstallAsync();

AnsiConsole.MarkupLine("[red3_1]This program only works on Windows[/]");

// Get the video path
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

var videoCount = AnsiConsole.Ask<int>("How many videos do you want to create? (ex: 10)");
// Loop and create each video one by one
for (var i = 0; i < videoCount; i++)
{
    var imgService = new ImageService();
    await imgService.GetImageAsync();

    var details = new VideoDetails
    {
        VideoPath = videoPath,
        ImagePath = Path.Join(Directory.GetCurrentDirectory(), "bg.jpg")
    };
    var ffmpeg = new FFmpegService();
    await ffmpeg.CreateVideoAsync(details);
}

AnsiConsole.MarkupLine("[green]Finished creating all videos[/]");
Console.WriteLine("Press any key to continue...");
Console.ReadKey();
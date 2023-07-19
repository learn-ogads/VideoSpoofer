using System.Diagnostics;
using Spectre.Console;
using VideoSpoofer.Models;

namespace VideoSpoofer.Services;

public class FFmpegService
{

    public static string GetExecutablePath() =>
        Path.Join(Path.GetPathRoot(Environment.SystemDirectory), "ffmpeg", "bin", "ffmpeg.exe");

    /// <summary>
    /// CreateArguments will create the arguments for FFMpeg and create the directories required
    /// </summary>
    private static string CreateArguments(VideoDetails videoDetails)
    {
        Directory.CreateDirectory("output");
        var outputPath = Path.Join(Directory.GetCurrentDirectory(), "output", $"{Guid.NewGuid()}.mp4");
        return $"-i \"{videoDetails.VideoPath}\" " +
               $"-i \"{videoDetails.ImagePath}\" " +
               "-filter_complex \"[0]crop=900:1600:70:0[vid];[1][vid]overlay=(main_w-overlay_w)/2:(main_h-overlay_h)/2\" " +
               "-c:a copy " +
               $"\"{outputPath}\"";
    }
    
    public async Task CreateVideoAsync(VideoDetails videoDetails)
    {
        var arguments = CreateArguments(videoDetails);
        var startInfo = new ProcessStartInfo
        {
            CreateNoWindow = false,
            UseShellExecute = false,
            FileName = GetExecutablePath(),
            Arguments = arguments,
            WindowStyle = ProcessWindowStyle.Hidden,
            RedirectStandardOutput = true
        };

        try
        {
            AnsiConsole.MarkupLine("[green]Creating video...[/]");
            using var process = Process.Start(startInfo);
            if (process == null)
            {
                AnsiConsole.MarkupLine("[red]Failed to start ffmpeg... Closing[/]");
                Environment.Exit(1);
            }
            while (!process.StandardOutput.EndOfStream)
            {
                var line = await process.StandardOutput.ReadLineAsync();
                if (line != null)
                    Console.WriteLine(line.Trim());
            }
            await process.WaitForExitAsync();
            AnsiConsole.MarkupLine("[green]Finished creating video[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]{ex.Message}[/]");
        }
    }
}
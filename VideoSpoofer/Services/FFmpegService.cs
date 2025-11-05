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
               "-filter_complex \"[0:v]scale=720:1280:force_original_aspect_ratio=decrease[v];[1:v]scale=1080:1920[bg];[bg][v]overlay=(W-w)/2:(H-h)/2\" " +
               "-c:a copy " +
               $"\"{outputPath}\"";
    }
    
    /// <summary>
    /// Will create a video using ffmpeg.
    /// This expects the video to have the dimensions of 1080x1920.
    /// </summary>
    public static async Task<bool> CreateVideoAsync(VideoDetails videoDetails)
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

        /**
         * Start the process
         */
        using var process = Process.Start(startInfo);

        /**
         * If the process is null, return false.
         * This means ffmpeg failed to start
         */
        if (process == null)
        {
            return false;
        }

        /**
         * Read the standard output until the end of the stream.
         * Once we reach the end, output the message
         */
        while (!process.StandardOutput.EndOfStream)
        {
            var line = await process.StandardOutput.ReadLineAsync();
            if (line != null)
                Console.WriteLine(line.Trim());
        }
        
        /**
         * Wait for ffmpeg to close
         */
        await process.WaitForExitAsync();
        return true;
    }
}
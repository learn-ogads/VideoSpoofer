using System.IO.Compression;
using System.Runtime.InteropServices;
using SharpCompress.Archives;
using SharpCompress.Common;
using Spectre.Console;

namespace VideoSpoofer.Services;

public class FFmpegDownloadService
{
    private readonly HttpClient _httpClient;

    public FFmpegDownloadService()
    {
        _httpClient = new HttpClient();
    }
    
    private async Task<Stream> DownloadAsync()
    {
        AnsiConsole.MarkupLine("[yellow]Downloading ffmpeg...[/]");
        var resp = await _httpClient.GetAsync("https://www.gyan.dev/ffmpeg/builds/ffmpeg-git-full.7z");
        if (!resp.IsSuccessStatusCode)
        {
            AnsiConsole.MarkupLine("[red]Failed to download FFmpeg[/]");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            Environment.Exit(1);
        }
        AnsiConsole.MarkupLine("[yellow]Finished downloading ffmpeg...[/]");

        return await resp.Content.ReadAsStreamAsync();
    }

    /// <summary>
    /// Extracts the zip file and returns the root folder name
    /// </summary>
    /// <param name="stream"></param>
    private static string Extract7Zip(Stream stream)
    {
        AnsiConsole.MarkupLine("[yellow]Extracting ffmpeg...[/]");
        using var archive = ArchiveFactory.Open(stream);
        var entries = archive.Entries.ToList();
        var rootFolder = entries.First().Key;
        foreach (var entry in entries)
        {
            entry.WriteToDirectory(Path.Join(GetRootDrive()), new ExtractionOptions
            {
                ExtractFullPath = true,
                Overwrite = true
            });
            AnsiConsole.MarkupLine($"[yellow]Extracted file {entry.Key}[/]");
        }
        AnsiConsole.MarkupLine("[yellow]Finished extracting ffmpeg...[/]");
        return rootFolder;
    }
    
    
    /// <summary>
    /// Exists will check if the FFmpeg executable exists
    /// </summary>
    private static bool Exists()
    {
        AnsiConsole.MarkupLine("[yellow]Checking if FFmpeg exists...[/]");
        var drive = GetRootDrive();
        var ffmpegPath = Path.Join(drive, "ffmpeg");
        return Directory.Exists(ffmpegPath) && File.Exists(Path.Join(ffmpegPath, "bin", "ffmpeg.exe"));
    }
    
    /// <summary>
    /// GetRootDrive will return the root drive for the current operating system.
    /// Most of the time this will be <c>C:</c>
    /// </summary>
    /// <returns></returns>
    private static string GetRootDrive()
    {
        var drive = Path.GetPathRoot(Environment.SystemDirectory);
        if (drive != null) return drive;
        AnsiConsole.MarkupLine("[red]Failed to find your root drive[/]");
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
        Environment.Exit(1);
        return drive;
    }
    
    /// <summary>
    /// InstallAsync will download/install FFmpeg if it doesn't exist.
    /// Currently this is a Windows only feature
    /// </summary>
    public async Task InstallAsync()
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            return;
        if (Exists())
        {
            AnsiConsole.MarkupLine("[yellow]FFmpeg exists![/]");
            return;
        }
        
        var zipStream = await DownloadAsync();
        var rootFolder = Extract7Zip(zipStream);
        ChangeFolderName(rootFolder);
    }

    private static void ChangeFolderName(string rootFolder)
    {
        var currentPath = Path.Join(GetRootDrive(), rootFolder);
        var newPath = Path.Join(GetRootDrive(), "ffmpeg");
        Directory.Move(currentPath, newPath);
    }
}
# VideoSpoofer
Are you wanting to mass produce videos? Introducing the VideoSpoofer.

### How Does It Work?
It works by combining a video with an image. This will resize the video and place it on top of a randomly downloaded image.

![VideoSpoofer output example](https://i.imgur.com/8BsoDO8.png)

### How Is This Helpful?
If you're needing to mass post on social media platforms this will help you make videos that are "unique".

### What's Required?
- You'll need a video in the vertical format `1080x1920`
- You'll need ffmpeg. This will be downloaded automatically through the VideoSpoofer
- You'll need an internet connection
- Windows

# Running The Application
Have everything required? We recommend running the VideoSpoofer executable through your terminal. If you receive an error, copy the logs and open an issue.

First, open your terminal. Then change to your downloads folder (or wherever the exe is located):
```
cd Downloads
```

Your path should now look like the following: `C:\Users\username\Downloads`

Now you'll run the executable:
```
VideoSpoofer.exe
```

This will download ffmpeg to your main drive. For example, `C:`.

The full path for ffmpeg will be as follows:
`C:\ffmpeg\bin\ffmpeg.exe`

***Having problems downloading ffmpeg automatically?*** Try downloading it manually from [here](https://www.gyan.dev/ffmpeg/builds/ffmpeg-git-full.7z). You'll need to make sure it has the same full path as listed above for the VideoSpoofer.

After ffmpeg is downloaded, you'll be asked to provide your file path/name.

```
What's the video filename (ex: test.mp4)?
```

***We highly recommend having the video in the same folder as the VideoSpoofer executable.*** You'll simply enter in the filename or path and then press enter to continue.

If the file is found you'll be asked how many videos you'd like to create.

Enter in a number such as `10`. Then press on enter.

You'll then wait for the VideoSpoofer to mass create your videos.


# Help! It's Not Working
Please copy the output from your terminal and open an issue. You can also try getting support through the official OGAds discord server.

# Windows Is Blocking The File
This is because the executable is not signed. You can allow the application to run. Don't trust it? You can download the source code and build it yourself.

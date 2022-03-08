using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace UltrawideHelper.Update;

public static class HttpHelper
{
    private static readonly HttpClient HttpClient = new();

    public static async Task DownloadFileAsync(string uri, string outputPath)
    {
        if (!Uri.TryCreate(uri, UriKind.Absolute, out _))
            throw new InvalidOperationException("URI is invalid.");
        
        if (string.IsNullOrEmpty(outputPath))
            throw new ArgumentNullException(nameof(outputPath));

        if (File.Exists(outputPath))
            File.Delete(outputPath);

        await File.Create(outputPath).DisposeAsync();

        var fileBytes = await HttpClient.GetByteArrayAsync(uri);
        await File.WriteAllBytesAsync(outputPath, fileBytes);
    }
}
using Octokit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using UltrawideHelper.Configuration;
using Application = System.Windows.Application;

namespace UltrawideHelper.Update;

public class UpdateManager
{
    private readonly string owner;
    private readonly string repositoryName;
    private readonly GitHubClient gitHubClient;

    private const string UpdateFileName = "update.zip";
    private const string UpdateScriptFileName = "update.ps1";

    private const string UpdateScript =
        @"Start-Sleep -s 1
Set-Location -Path $PSScriptRoot
Expand-Archive -Force -Path update.zip -DestinationPath .\
Remove-Item update.zip -Force
Remove-Item -LiteralPath $MyInvocation.MyCommand.Path -Force
Start-Process -FilePath UltrawideHelper.exe";

    public UpdateManager(string owner, string repositoryName)
    {
        this.owner = owner;
        this.repositoryName = repositoryName;
        gitHubClient = new GitHubClient(new ProductHeaderValue(repositoryName));
    }

    public async void CheckForNewVersion()
    {
        IReadOnlyList<Release> releases;

        try
        {
            releases = await gitHubClient.Repository.Release.GetAll(owner, repositoryName);
        }
        catch (RateLimitExceededException)
        {
            return;
        }

        if (releases == null || releases.Count == 0)
        {
            return;
        }

        var newestRelease = releases.OrderByDescending(r => new Version(r.TagName)).First();
        var newestVersion = new Version(newestRelease.TagName);
        var currentVersion = Application.ResourceAssembly.GetName().Version;

        if (newestVersion <= currentVersion)
        {
            return;
        }

        var directory = Path.GetDirectoryName(Application.ResourceAssembly.Location);
        var updateFile = Path.Combine(directory ?? throw new InvalidOperationException(), UpdateFileName);
        
        await HttpHelper.DownloadFileAsync(newestRelease.Assets[0].BrowserDownloadUrl, updateFile);

        await using (var updateFileStream = new FileStream(updateFile, System.IO.FileMode.Open)) 
            using (var archive = new ZipArchive(updateFileStream, ZipArchiveMode.Update))
            {
                var configEntry = archive.Entries.SingleOrDefault(e => e.Name.Equals(ConfigurationManager.FileName));
                configEntry?.Delete();
            }

        var updateScriptFile = Path.Combine(directory, UpdateScriptFileName);
        await File.WriteAllTextAsync(updateScriptFile, UpdateScript);

        // Start script and exit application
        var startInfo = new ProcessStartInfo()
        {
            FileName = "powershell.exe",
            Arguments = $"-NoProfile -ExecutionPolicy unrestricted \"& \'{updateScriptFile}\'\"",
            UseShellExecute = false
        };

        Process.Start(startInfo);
        Application.Current.Shutdown();
    }
}
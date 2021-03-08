using Octokit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using UltrawideHelper.Configuration;
using Application = System.Windows.Application;

namespace UltrawideHelper.Update
{
    public class UpdateManager
    {
        private string owner;
        private string repositoryName;
        private GitHubClient client;

        private const string UpdateFileName = "update.zip";
        private const string UpdateScriptFileName = "update.ps1";
        private const string UpdateScript = 
@"Start-Sleep -s 1
Expand-Archive -Force -Path update.zip -DestinationPath .\
Remove-Item update.zip -Force
Remove-Item -LiteralPath $MyInvocation.MyCommand.Path -Force
Start-Process -FilePath UltrawideHelper.exe";

        public UpdateManager(string owner, string repositoryName)
        {
            this.owner = owner;
            this.repositoryName = repositoryName;
            client = new GitHubClient(new ProductHeaderValue(repositoryName));
        }

        public async void CheckForNewVersion()
        {
            IReadOnlyList<Release> releases = null;

            try
            {
                releases = await client.Repository.Release.GetAll(owner, repositoryName);
            }
            catch (RateLimitExceededException)
            {
                return;
            }

            if (releases == null || releases.Count == 0)
            {
                return;
            }

            var newestRelease = releases?.OrderByDescending(r => new Version(r.TagName)).FirstOrDefault();
            var newestVersion = new Version(newestRelease.TagName);
            var currentVersion = Application.ResourceAssembly.GetName().Version;

            if (newestVersion > currentVersion)
            {
                var directory = Path.GetDirectoryName(Application.ResourceAssembly.Location);
                var updateFile = Path.Combine(directory, UpdateFileName);

                using (var client = new WebClient())
                {
                    client.DownloadFile(newestRelease.Assets.First().BrowserDownloadUrl, updateFile);
                }

                using (FileStream updateFileStream = new FileStream(updateFile, System.IO.FileMode.Open))
                using (ZipArchive archive = new ZipArchive(updateFileStream, ZipArchiveMode.Update))
                {
                    var configEntry = archive.Entries.SingleOrDefault(e => e.Name.Equals(ConfigurationManager.FileName));
                    configEntry?.Delete();
                }

                var updateScriptFile = Path.Combine(directory, UpdateScriptFileName);
                File.WriteAllText(updateScriptFile, UpdateScript);

                // Start script and exit application
                var startInfo = new ProcessStartInfo()
                {
                    FileName = "powershell.exe",
                    Arguments = $"-NoProfile -ExecutionPolicy unrestricted \"{updateScriptFile}\"",
                    UseShellExecute = false
                };
                Process.Start(startInfo);
                Application.Current.Shutdown();
            }
        }
    }
}

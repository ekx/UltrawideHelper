using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using UltrawideHelper.Data;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace UltrawideHelper.Configuration
{
    public class ConfigurationManager : IDisposable
    {
        public ConfigurationFile ConfigFile { get; private set; }

        public event ConfigurationChangedEventHandler Changed;

        public static readonly string FileName = "config.yaml";
        public static readonly string FileDirectory = Path.GetDirectoryName(Application.ResourceAssembly.Location);
        public static readonly string FilePath = Path.Combine(FileDirectory, FileName);

        private IDeserializer deserializer;
        private FileSystemWatcher fileSystemWatcher;

        private const int RetryDelayMilliseconds = 10;
        private const int RetryCount = 10;

        public ConfigurationManager()
        {
            deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            ConfigFile = deserializer.Deserialize<ConfigurationFile>(File.ReadAllText(FilePath));

            fileSystemWatcher = new FileSystemWatcher();
            fileSystemWatcher.Path = FileDirectory;
            fileSystemWatcher.NotifyFilter = NotifyFilters.LastWrite;
            fileSystemWatcher.Filter = FileName;
            fileSystemWatcher.Changed += FileSystemWatcher_Changed;
            fileSystemWatcher.EnableRaisingEvents = true;
        }

        private async void FileSystemWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            fileSystemWatcher.EnableRaisingEvents = false;

            ConfigFile = deserializer.Deserialize<ConfigurationFile>(await ReadConfigFile());
            Application.Current.Dispatcher.Invoke(new Action(() => { Changed.Invoke(ConfigFile); }));

            fileSystemWatcher.EnableRaisingEvents = true;
        }

        private async Task<string> ReadConfigFile()
        {
            await Task.Delay(RetryDelayMilliseconds);

            for (int i = 0; i < RetryCount; i++)
            {
                try
                {
                    using (var fileStream = new FileStream(FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    using (var textReader = new StreamReader(fileStream))
                    {
                        return await textReader.ReadToEndAsync();
                    }
                }
                catch (IOException)
                {
                    if (i == RetryCount - 1)
                    {
                        throw;
                    }

                    await Task.Delay(RetryDelayMilliseconds);
                    continue;
                }
            }

            return null;
        }

        public void Dispose()
        {
            fileSystemWatcher.Dispose();
        }
    }
}
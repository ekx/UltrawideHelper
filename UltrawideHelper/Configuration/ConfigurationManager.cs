﻿using System;
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

        private IDeserializer deserializer;
        private FileSystemWatcher fileSystemWatcher;

        private const string FileName = "config.yaml";

        public ConfigurationManager()
        {
            deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            ConfigFile = deserializer.Deserialize<ConfigurationFile>(File.ReadAllText(FileName));

            fileSystemWatcher = new FileSystemWatcher();
            fileSystemWatcher.Path = Directory.GetCurrentDirectory();
            fileSystemWatcher.NotifyFilter = NotifyFilters.LastWrite;
            fileSystemWatcher.Filter = FileName;
            fileSystemWatcher.Changed += FileSystemWatcher_Changed;
            fileSystemWatcher.EnableRaisingEvents = true;
        }

        private async void FileSystemWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            fileSystemWatcher.EnableRaisingEvents = false;

            await Task.Delay(10);

            ConfigFile = deserializer.Deserialize<ConfigurationFile>(await File.ReadAllTextAsync(FileName));
            Application.Current.Dispatcher.Invoke(new Action(() => { Changed.Invoke(ConfigFile); }));

            fileSystemWatcher.EnableRaisingEvents = true;
        }

        public void Dispose()
        {
            fileSystemWatcher.Dispose();
        }
    }
}
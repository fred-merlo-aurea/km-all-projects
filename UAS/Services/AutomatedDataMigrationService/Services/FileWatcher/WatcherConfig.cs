﻿using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Core.ADMS;

namespace ADMS.Services.FileWatcher
{
    public class WatcherConfig : ServiceBase
    {
        private readonly string WatcherConfigPath = BaseDirs.createDirectory(BaseDirs.getConfigDir(), Core.ADMS.Settings.WatcherConfig);

        public List<string> GetOutsideDirectoriesToWatch()
        {
            FWDirectory DirectoryList;
            List<string> clientData = new List<string>();

            XmlSerializer serializer = new XmlSerializer(typeof(FWDirectory));
            StreamReader reader = new StreamReader(WatcherConfigPath);
            DirectoryList = (FWDirectory)serializer.Deserialize(reader);
            foreach (FWPath p in DirectoryList.Paths)
            {
                clientData.Add(p.Path);
            }

            return clientData;
        }

        public string GetClientForOutsideDirectory(string directory)
        {
            FWDirectory DirectoryList;
            string clientName = "";

            XmlSerializer serializer = new XmlSerializer(typeof(FWDirectory));
            StreamReader reader = new StreamReader(WatcherConfigPath);
            DirectoryList = (FWDirectory)serializer.Deserialize(reader);
            foreach (FWPath p in DirectoryList.Paths)
            {
                if (p.Path == directory)
                {
                    clientName = p.Client;
                    break;
                }
            }
            reader.Close();
            return clientName;
        }
    }

    [XmlType(AnonymousType = true)]
    [XmlRoot("FileWatchDirectory")]
    public class FWDirectory
    {
        [XmlElement("Directory")]
        public List<FWPath> Paths { get; set; }
    }

    public class FWPath
    {
        public FWPath() { }

        [XmlElement("Path")]
        public string Path { get; set; }
        [XmlElement("Client")]
        public string Client { get; set; }
    }
}

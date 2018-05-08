﻿using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace ADMS.Services.FileMover
{
    public class MoverConfig : ServiceBase 
    {
        public Dictionary<string, string> FileMovingConfigurations(string client)
        {
            FMFiles FilesList;
            Dictionary<string, string> clientData = new Dictionary<string, string>();

            string path = Core.ADMS.Settings.MoverConfigPath;
            string file = Core.ADMS.Settings.MoverConfigFile;
            string pathToFile = path + client + "\\" + file;

            XmlSerializer serializer = new XmlSerializer(typeof(FMFiles));
            if (File.Exists(pathToFile))
            {
                StreamReader reader = new StreamReader(pathToFile);
                FilesList = (FMFiles)serializer.Deserialize(reader);
                foreach (FMFile f in FilesList.Files)
                {
                    //Check if value exists
                    if (clientData.ContainsKey(f.Schema))
                        clientData[f.Schema] = "ERROR";
                    else
                        clientData.Add(f.Schema, f.StagingDirectory);

                }
            }

            return clientData;
        }

        public string GetLocationToMoveFile(string clientName, string fileName)
        {
            Dictionary<string, string> movingData = FileMovingConfigurations(clientName);
            if (movingData.ContainsKey(fileName) &&
                (movingData[fileName] != "Default" || movingData[fileName] != "ERROR"))
                return movingData[fileName];
            else
                return "Default";

        }
    }

    [XmlType(AnonymousType = true)]
    [XmlRoot("Files")]
    public class FMFiles
    {
        [XmlElement("File")]
        public List<FMFile> Files { get; set; }
    }

    public class FMFile
    {
        public FMFile() { }

        [XmlElement("FileName")]
        public string FileName { get; set; }
        [XmlElement("Schema")]
        public string Schema { get; set; }
        [XmlElement("StagingDirectory")]
        public string StagingDirectory { get; set; }
    }
}

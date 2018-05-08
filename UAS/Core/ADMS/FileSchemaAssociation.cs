using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Core.ADMS
{
    public class FileSchemaAssociation
    {
        public Dictionary<string, string> CreateClientFileSchemaAssociationDictionary(string client)
        {
            ClientFileSchemaAssociation ClientList;
            Dictionary<string, string> clientData = new Dictionary<string, string>();

            string path = Settings.FileSchemaAssociation;
            if (!File.Exists(path))
            {
                throw new FileNotFoundException(path + ": Does Not Exist");
            }
            XmlSerializer serializer = new XmlSerializer(typeof(ClientFileSchemaAssociation));
            StreamReader reader = new StreamReader(path);
            ClientList = (ClientFileSchemaAssociation)serializer.Deserialize(reader);
            foreach (Client c in ClientList.Clients)
                if (c.Name.Equals(client, StringComparison.CurrentCultureIgnoreCase))
                    foreach (ClientFile f in c.Files)
                    {
                        //Check if value exists
                        if (clientData.ContainsKey(f.FileName))
                            clientData[f.FileName] = "ERROR";
                        else
                            clientData.Add(f.FileName, f.Schema);

                    }

            return clientData;
        }

        public Dictionary<string, string> GetIfFileIsIgnored(string client)
        {
            ClientFileSchemaAssociation ClientList;
            Dictionary<string, string> clientData = new Dictionary<string, string>();
            clientData.Clear();

            string path = Settings.FileSchemaAssociation;
            XmlSerializer serializer = new XmlSerializer(typeof(ClientFileSchemaAssociation));
            StreamReader reader = new StreamReader(path);
            ClientList = (ClientFileSchemaAssociation)serializer.Deserialize(reader);
            foreach (Client c in ClientList.Clients)
                if (c.Name.Equals(client, StringComparison.CurrentCultureIgnoreCase))
                    foreach (ClientFile f in c.Files)
                    {
                        //Check if value exists
                        if (f.Ignore != null)
                            clientData.Add(f.FileName, "Ignore");
                        else
                            clientData.Add(f.FileName, "Process");

                    }

            return clientData;
        }

        public bool IgnoreUnknownFiles(string client)
        {
            ClientFileSchemaAssociation ClientList;
            bool processUnknowns = true;

            string path = Settings.FileSchemaAssociation;
            XmlSerializer serializer = new XmlSerializer(typeof(ClientFileSchemaAssociation));
            StreamReader reader = new StreamReader(path);
            ClientList = (ClientFileSchemaAssociation)serializer.Deserialize(reader);
            foreach (Client c in ClientList.Clients)
                if (c.Name.Equals(client, StringComparison.CurrentCultureIgnoreCase))
                {
                    if (c.IgnoreUnknownFiles != null)
                        processUnknowns = false;

                }

            return processUnknowns;
        }
    }

    [XmlType(AnonymousType = true)]
    [XmlRoot("Clients")]
    [ExcludeFromCodeCoverage]
    public class ClientFileSchemaAssociation
    {
        [XmlElement("Client")]
        public List<Client> Clients { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class Client
    {
        public Client() { }

        [XmlElement("Name")]
        public string Name { get; set; }
        [XmlElement("IgnoreUnknownFiles", IsNullable = false)]
        public string IgnoreUnknownFiles { get; set; }
        [XmlArray("Files"), XmlArrayItem("File")]
        public List<ClientFile> Files { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class ClientFile
    {
        public ClientFile() { }

        [XmlElement("FileName")]
        public string FileName { get; set; }
        [XmlElement("Schema")]
        public string Schema { get; set; }
        [XmlElement("Ignore", IsNullable = false)]
        public string Ignore { get; set; }

    }

}
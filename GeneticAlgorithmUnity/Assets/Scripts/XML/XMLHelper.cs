using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace Game
{
    public static class XMLHelper
    {
        public static void SerializeData<DataType>(string path, DataType data, bool append = true)
        {
            FileStream fileStream = new FileStream(path, FileMode.Create);

            XmlWriterSettings settings = new XmlWriterSettings()
            {
                Indent = true,
                IndentChars = "\t",
            };

            XmlWriter writer = XmlWriter.Create(fileStream, settings);

            DataContractSerializer serializer = new DataContractSerializer(typeof(DataType));

            serializer.WriteObject(writer, data);

            writer.Close();
        }
    }
}

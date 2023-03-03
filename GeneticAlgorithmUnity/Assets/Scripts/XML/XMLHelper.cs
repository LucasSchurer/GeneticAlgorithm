using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace Game
{
    public static class XMLHelper
    {
        public static void SerializeData<DataType>(string path, DataType data, bool append = true)
        {
            XmlWriterSettings settings = new XmlWriterSettings()
            {
                Indent = true,
                IndentChars = "\t",
            };

            DataContractSerializer serializer = new DataContractSerializer(typeof(DataType));

            using (FileStream fileStream = new FileStream(path, FileMode.Create))
            {
                using (XmlWriter writer = XmlWriter.Create(fileStream, settings))
                {
                    serializer.WriteObject(writer, data);
                }
            }
        }
    }
}

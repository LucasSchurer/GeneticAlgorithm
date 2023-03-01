using Game.GA;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

namespace Game
{
    public static class XMLHelper
    {
        public static void SerializeData<DataType>(string path, DataType data, bool append = true)
        {
            StreamWriter writer = new StreamWriter(path, append);

            XmlSerializer xmls = new XmlSerializer(typeof(DataType));

            xmls.Serialize(writer.BaseStream, data);

            writer.Close();
        }
    }
}

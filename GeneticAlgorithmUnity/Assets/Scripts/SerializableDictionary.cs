using System.Collections.Generic;
using System.Xml.Serialization;

namespace Game
{
    [XmlRoot("Dictionary")]
    public class SerializableDictionary<Key, Value>
    {
        [XmlElement("Key")]
        public Key key;
        [XmlElement("Value")]
        public Value value;

        public SerializableDictionary(Key key, Value value)
        {
            this.key = key;
            this.value = value;
        }

        public SerializableDictionary() { }

        public static List<SerializableDictionary<Key, Value>> BuildListFromDictionary(Dictionary<Key, Value> dictionary)
        {
            List<SerializableDictionary<Key, Value>> list = new List<SerializableDictionary<Key, Value>>();

            foreach (KeyValuePair<Key, Value> keyValue in dictionary)
            {
                list.Add(new SerializableDictionary<Key, Value>(keyValue.Key, keyValue.Value));
            }

            return list;
        }
    } 
}

using System.Collections.Generic;
using System.Xml.Serialization;

namespace Game.GA
{
    [XmlRoot("Generation")]
    public class GenerationData
    {
        [XmlAttribute]
        public int number;
        [XmlIgnore]
        public Dictionary<int, CreatureData> creatures;
        [XmlArray("Creatures")]
        [XmlArrayItem("Creature")]
        public List<SerializableDictionary<int, CreatureData>> serializableCreatures => SerializableDictionary<int, CreatureData>.BuildListFromDictionary(creatures);
        public float generationFitness;

        public GenerationData()
        {
            creatures = new Dictionary<int, CreatureData>();
        }

        public void AddCreatureData(CreatureData data)
        {
            creatures.TryAdd(data.id, data);
        }
    } 
}

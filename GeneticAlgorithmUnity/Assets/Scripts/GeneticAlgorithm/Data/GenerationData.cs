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

        public GenerationData()
        {
            creatures = new Dictionary<int, CreatureData>();
        }

        public void AddCreature(CreatureController creature)
        {
            creatures.TryAdd(creature.data.id, creature.data);
        }
    } 
}

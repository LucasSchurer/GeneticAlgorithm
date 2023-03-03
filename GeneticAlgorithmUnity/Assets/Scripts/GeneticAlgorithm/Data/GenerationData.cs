using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Game.GA
{
    [DataContract(Name = "Generation")]
    public class GenerationData
    {
        [DataMember()]
        public int number;
        public Dictionary<int, CreatureData> creatures;
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

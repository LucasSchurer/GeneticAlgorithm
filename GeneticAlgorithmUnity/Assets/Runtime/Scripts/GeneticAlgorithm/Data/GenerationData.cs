using Game.Traits;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Game.GA
{
    [DataContract(Name = "Generation", Namespace = "")]
    public class GenerationData
    {
        [DataMember(Name = "Number", Order = 0)]
        private int _number;
        [DataMember(Name = "GenerationFitness", Order = 1)]
        private float _generationFitness = 0f;
        private Dictionary<int, CreatureData> _creatures;
        public int Number { get => _number; set => _number = value >= 0 ? value : 0; }
        public float GenerationFitness { get => _generationFitness; set => _generationFitness = value >= 0 ? value : 0; }
        public Dictionary<int, CreatureData> Creatures { get => _creatures; private set => _creatures = value; }
        [DataMember(Name = "Creatures", Order = 99)]
        private List<CreatureData> CreaturesList => _creatures.Values.ToList();
        [DataMember(Name = "TraitWeights")]
        private List<SerializableDictionary<TraitIdentifier, float>> _serializableDictionary;
        private List<SerializableDictionary<TraitIdentifier, float>> SerializableDictionary => _serializableDictionary;

        public GenerationData()
        {
            _creatures = new Dictionary<int, CreatureData>();
        }

        public void SetTraitWeights(Traits.TraitWeights weights)
        {
            _serializableDictionary = SerializableDictionary<TraitIdentifier, float>.BuildListFromDictionary(weights.GetTraitWeightsDictionary());
        }

        public void AddCreatureData(CreatureData data)
        {
            _creatures.TryAdd(data.Id, data);
        }

        public void RemoveCreatureData(CreatureData data)
        {
            _creatures.Remove(data.Id);
        }
    }
}

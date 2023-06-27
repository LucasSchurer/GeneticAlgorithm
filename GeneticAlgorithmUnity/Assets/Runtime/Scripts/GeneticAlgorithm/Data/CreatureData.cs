using Game.Traits;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Game.GA
{
    [DataContract(Name = "Creature", Namespace = "")]
    public class CreatureData
    {
        [DataMember(Name = "Id", Order = 0)]
        private int id;
        [DataMember(Name = "Generation", Order = 1)]
        private int _generation;
        [DataMember(Name = "Fitness", Order = 2)]
        private Fitness _fitness;
        [DataMember(Name = "Chromosome", Order = 97)]
        private BaseEnemyChromosome _chromosome;
        private CreatureData[] _parents;
        private List<CreatureData> _children;
        [DataMember(Name = "IsDead", Order = 3)]
        private bool _isDead = false;

        public int Id { get => id; set => id = value; }
        public int Generation { get => _generation; set => _generation = value; }
        public Fitness Fitness { get => _fitness; set => _fitness = value; }        
        public BaseEnemyChromosome Chromosome { get => _chromosome; set => _chromosome = value; }
        public CreatureData[] Parents { get => _parents; set => _parents = value; }
        public bool IsDead { get => _isDead; set => _isDead = value; }
        public List<CreatureData> Children => _children;
        [DataMember(Name = "Parents", Order = 98)]
        private List<int> ParentsID => _parents != null ? _parents.Select(p => p.Id).ToList() : new List<int>();
        [DataMember(Name = "Children", Order = 99)]
        private List<int> ChildrenID => _children.Select(c => c.Id).ToList();

        public CreatureData(GeneticAlgorithmController controller)
        {
            _children = new List<CreatureData>();
            _fitness = new Fitness(controller);
        }
    }
}
